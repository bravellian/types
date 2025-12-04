// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


#nullable enable

using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Bravellian;

[JsonConverter(typeof(FastIdJsonConverter))]
[TypeConverter(typeof(FastIdTypeConverter))]
public readonly partial record struct FastId
        : IComparable,
          IComparable<FastId>,
          IEquatable<FastId>,
          IParsable<FastId>,
          ILongBackedType<FastId>
{
    public static readonly DateTimeOffset CustomEpoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private const int TimestampBits = 34;
    private const int RandomBits = 30; // 64 - TimestampBits
    private static readonly ulong MaxRandom = (1UL << RandomBits) - 1;
    private readonly Lazy<string> encoded;

    public static readonly FastId Empty = new FastId(0, "0");

    private FastId(long value, string encodedValue)
    {
        this.Value = value;
        this.encoded = new Lazy<string>(() => encodedValue.ToUpperInvariant());
    }

    public FastId(long value)
    {
        this.Value = value;
        this.encoded = new Lazy<string>(() => CrockfordBase32.Encode(LongBitShuffler.ShuffleBits(value)));
    }

    public long Value { get; init; }

    public string Encoded => this.encoded.Value;

    public override string ToString() => this.Encoded;

    public bool Equals(FastId other)
    {
        return string.Equals(this.Value, other.Value);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    public int CompareTo(FastId other)
    {
        return this.Value.CompareTo(other.Value);
    }

    public int CompareTo(object? obj)
    {
        return obj is FastId id ? this.Value.CompareTo(id.Value) : this.Value.CompareTo(obj);
    }

    public static FastId From(long value) => new FastId(value);
    public static FastId? From(long? value) => value.HasValue ? new FastId(value.Value) : null;

    /// <summary>
    /// Creates a FastId deterministically from a Guid, ensuring its timestamp component
    /// is less than or equal to a specified maximum timestamp.
    /// </summary>
    /// <remarks>
    /// This is the recommended approach for migrating existing Guids. It hashes the Guid to
    /// a 64-bit value and then uses the modulo operator to constrain the timestamp portion,
    /// ensuring the generated ID is localized chronologically.
    /// </remarks>
    /// <param name="guid">The Guid to convert.</param>
    /// <param name="maxTimestamp">The latest possible timestamp the generated FastId can represent.</param>
    /// <returns>A FastId deterministically derived from the Guid within the specified time range.</returns>
    public static FastId FromGuidWithinTimestampRange(Guid guid, DateTimeOffset maxTimestamp)
    {
        // 1. Calculate the maximum allowed timestamp value in seconds since the epoch.
        if (maxTimestamp < CustomEpoch)
        {
            throw new ArgumentOutOfRangeException(nameof(maxTimestamp), "Maximum timestamp cannot be before the custom epoch.");
        }

        ulong maxTimestampSeconds = (ulong)(maxTimestamp - CustomEpoch).TotalSeconds;

        // Ensure the provided max timestamp doesn't exceed what 42 bits can hold.
        // This is a safety check; 2^42 seconds is over 139,000 years.
        ulong maxAllowedTimestampValue = (1UL << TimestampBits) - 1;
        if (maxTimestampSeconds > maxAllowedTimestampValue)
        {
            throw new ArgumentOutOfRangeException(nameof(maxTimestamp), "Maximum timestamp exceeds the 42-bit capacity.");
        }

        // 2. Hash the 128-bit Guid into a 64-bit long by XORing its two halves.
        byte[] guidBytes = guid.ToByteArray();
        long part1 = BitConverter.ToInt64(guidBytes, 0);
        long part2 = BitConverter.ToInt64(guidBytes, 8);
        ulong hashedGuidValue = (ulong)(part1 ^ part2);

        // 3. Extract the timestamp and random parts from the hash.
        ulong hashedTimestampPart = hashedGuidValue >> RandomBits; // The top 42 bits of the hash
        ulong hashedRandomPart = hashedGuidValue & MaxRandom;      // The bottom 22 bits of the hash

        // 4. Constrain the timestamp part to be within our allowed range [0, maxTimestampSeconds].
        //    We add 1 to maxTimestampSeconds because the modulo range is [0, N-1].
        ulong constrainedTimestampSeconds = hashedTimestampPart % (maxTimestampSeconds + 1);

        // 5. Combine the constrained timestamp with the hashed random part.
        ulong finalValue = (constrainedTimestampSeconds << RandomBits) | hashedRandomPart;

        return FastId.From((long)finalValue);
    }

    /// <summary>
    /// Creates a FastId deterministically from a string, ensuring its timestamp component
    /// is less than or equal to a specified maximum timestamp.
    /// </summary>
    /// <remarks>
    /// This function uses the SHA-256 hash of the input string to ensure a deterministic
    /// and well-distributed 64-bit value, which is then constrained to the specified time range.
    /// A consistent encoding (UTF-8) is used to guarantee reliable results.
    /// </remarks>
    /// <param name="sourceString">The string to convert. Must not be null or empty.</param>
    /// <param name="maxTimestamp">The latest possible timestamp the generated FastId can represent.</param>
    /// <returns>A FastId deterministically derived from the string within the specified time range.</returns>
    public static FastId FromStringWithinTimestampRange(string sourceString, DateTimeOffset maxTimestamp)
    {
        // 1. Input validation.
        if (string.IsNullOrEmpty(sourceString))
        {
            throw new ArgumentException("Source string cannot be null or empty.", nameof(sourceString));
        }

        if (maxTimestamp < CustomEpoch)
        {
            throw new ArgumentOutOfRangeException(nameof(maxTimestamp), "Maximum timestamp cannot be before the custom epoch.");
        }

        // 2. Calculate the maximum allowed timestamp value in seconds since the epoch.
        ulong maxTimestampSeconds = (ulong)(maxTimestamp - CustomEpoch).TotalSeconds;
        ulong maxAllowedTimestampValue = (1UL << TimestampBits) - 1;
        if (maxTimestampSeconds > maxAllowedTimestampValue)
        {
            throw new ArgumentOutOfRangeException(nameof(maxTimestamp), "Maximum timestamp exceeds the 42-bit capacity.");
        }

        // 3. Hash the string into a stable 64-bit value using SHA-256.
        byte[] stringBytes = Encoding.UTF8.GetBytes(sourceString);
        byte[] hashBytes = SHA256.HashData(stringBytes); // .NET 5+ shortcut

        // Take the first 8 bytes (64 bits) of the 256-bit hash result.
        ulong hashedValue = BitConverter.ToUInt64(hashBytes, 0);

        // 4. Extract the timestamp and random parts from the hash.
        ulong hashedTimestampPart = hashedValue >> RandomBits; // The top 42 bits
        ulong hashedRandomPart = hashedValue & MaxRandom;      // The bottom 22 bits

        // 5. Constrain the timestamp part to be within our allowed range [0, maxTimestampSeconds].
        ulong constrainedTimestampSeconds = hashedTimestampPart % (maxTimestampSeconds + 1);

        // 6. Combine the constrained timestamp with the hashed random part.
        ulong finalValue = (constrainedTimestampSeconds << RandomBits) | hashedRandomPart;

        return FastId.From((long)finalValue);
    }

    public static bool TryParse([NotNullWhen(true)] string? value, [MaybeNullWhen(false)][NotNullWhen(true)] out FastId result)
    {
        if (value == null)
        {
            result = default;
            return false;
        }

        return TryParse(value.AsSpan(), null, out result);
    }

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out FastId result)
    {
        return TryParse(value == null ? ReadOnlySpan<char>.Empty : value.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> value, [MaybeNullWhen(false)][NotNullWhen(true)] out FastId result) => TryParse(value, null, out result);

    public static bool TryParse(ReadOnlySpan<char> value, IFormatProvider? provider, [MaybeNullWhen(false)][NotNullWhen(true)] out FastId result)
    {
        result = default;

        // 1. Null or Empty/Whitespace Check
        if (value.IsEmpty || value.IsWhiteSpace())
        {
            return false;
        }

        // 2. Length Check (Critical for Base32 of a 64-bit number)
        // Max 13 characters for a 64-bit number (ceil(64/5)). Min 1 character (e.g., "0").
        // The Trim() below is because Crockford spec is often case-insensitive and might ignore hyphens
        // but we'll assume for this basic validator no hyphens are allowed in our canonical form.

        // Attempt to parse as a raw long value first.
        if (long.TryParse(value, out long rawLongValue))
        {
            result = FastId.From(rawLongValue);
            return true;
        }

        if (value.Length == 0 || value.Length > 13)
        {
            return false;
        }

        try
        {
            // 3. Attempt to Decode (This is the main validation step)
            // Your CrockfordBase32.Decode should:
            //  - Handle case-insensitivity and character mappings (I/L->1, O->0).
            //  - Throw ArgumentException for any character not in the valid set (after mapping).
            //  - Throw OverflowException if the decoded value exceeds ulong.MaxValue (ensures it fits 64 bits).
            //    (Note: For a fixed 64-bit target, if length is max 13, overflow is less likely
            //     if the string *only* has valid chars, but a good decoder should check.)

            // We decode to ulong first to get the full 64-bit unsigned pattern.
            // Then cast to long for storage (this is a bitwise cast).
            long longValue = CrockfordBase32.DecodeToInt64(value); // Assuming this method from prior example
            long unshuffled = LongBitShuffler.UnshuffleBits(longValue);

            // If decode succeeds, the string is valid in terms of characters and fitting into 64 bits.
            // We can then re-encode to get the canonical string form if needed, or trust cleanString
            // if your CrockfordBase32.Decode normalizes its input (e.g., to uppercase).
            // For simplicity here, we'll use the cleaned input string for the StringValue,
            // assuming the user wants to preserve the casing they entered if it was valid.
            // A more robust approach might re-encode ulongValue to get a canonical string form.
            // String canonicalString = CrockfordBase32.Encode(longValue);

            result = new FastId(unshuffled, value.ToString()); // Or use canonicalString
            return true;
        }
        catch (ArgumentNullException) // Should be caught by IsNullOrWhiteSpace earlier
        {
            return false;
        }
        catch (ArgumentException) // Invalid character in Base32 string
        {
            return false;
        }
        catch (OverflowException) // Decoded number too large for ulong (i.e., > 64 bits)
        {
            return false;
        }
        catch (Exception) // Catch any other unexpected errors during decode, though specific ones are better
        {
            // Log this unexpected exception
            return false;
        }
    }

    public static FastId? TryParse(string? value)
    {
        if (value == null)
        {
            return null;
        }

        return FastId.TryParse(value.AsSpan());
    }

    public static FastId? TryParse(ReadOnlySpan<char> value)
    {
        if (FastId.TryParse(value, out FastId result))
        {
            return result;
        }

        return null;
    }

    public static FastId Parse(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return Parse(value.AsSpan());
    }

    public static FastId Parse(string value, IFormatProvider? provider)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return Parse(value.AsSpan(), provider);
    }

    public static FastId Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static FastId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (FastId.TryParse(s, provider, out FastId result))
        {
            return result;
        }

        throw new ArgumentException("Unable to parse this value", nameof(s));
    }

    public static FastId New()
    {
        return FastId.From(GenerateInt64SecondsPrecision());
    }

    private static long GenerateInt64SecondsPrecision()
    {
        // Use TotalSeconds
        ulong timestampInSeconds = (ulong)(DateTimeOffset.UtcNow - CustomEpoch).TotalSeconds;

        byte[] randomBytes = new byte[8];
        RandomNumberGenerator.Fill(randomBytes);
        ulong randomNumber = BitConverter.ToUInt64(randomBytes, 0) & MaxRandom;

        // Check if timestampInSeconds exceeds the capacity of TimestampBits
        // 2^42 seconds is a very long time, so this check is mostly for extreme edge cases or incorrect bit allocation.
        if ((timestampInSeconds >> TimestampBits) > 0)
        {
            // This means the number of seconds has exceeded what can be stored in TimestampBits.
            // Handle this error: throw exception, log, or implement a rollover strategy if desired (though with seconds, it's far off).
            throw new OverflowException("Timestamp exceeds allocated bits. Consider a later epoch or more bits for timestamp.");
        }

        ulong id = (timestampInSeconds << RandomBits) | randomNumber;
        return (long)id;
    }

    public class FastIdJsonConverter : JsonConverter<FastId>
    {
        public override FastId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();

            if (!string.IsNullOrEmpty(s) && FastId.TryParse(s, out FastId result))
            {
                return result;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, FastId value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Encoded);

        public override void WriteAsPropertyName(Utf8JsonWriter writer, FastId value, JsonSerializerOptions options) =>
            writer.WritePropertyName(value.Encoded);

        public override FastId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return this.Read(ref reader, typeToConvert, options);
        }
    }

    // TypeConverter for FastId to and from string
    public class FastIdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || sourceType == typeof(long) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
            destinationType == typeof(string) || destinationType == typeof(long) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return FastId.Parse(s);
            }

            if (value is long l)
            {
                return FastId.From(l);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is FastId type)
            {
                if (destinationType == typeof(string))
                {
                    return type.ToString();
                }

                if (destinationType == typeof(long))
                {
                    return type.Value;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public static class CrockfordBase32
    {
        // Crockford's Base32 alphabet: Excludes I, L, O, U
        private const string EncodeAlphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        private static readonly Dictionary<char, byte> DecodeMap = new Dictionary<char, byte>();

        static CrockfordBase32()
        {
            for (int i = 0; i < EncodeAlphabet.Length; i++)
            {
                char c = EncodeAlphabet[i];
                DecodeMap[c] = (byte)i; // Original character (uppercase)
                if (char.IsLetter(c))
                {
                    DecodeMap[char.ToLowerInvariant(c)] = (byte)i; // Lowercase version
                }
            }

            // Map common transcription errors to their canonical equivalents
            // 'O' (letter) or 'o' decodes as '0' (digit)
            DecodeMap['O'] = 0; DecodeMap['o'] = 0;
            // 'I' (letter), 'i', 'L' (letter), or 'l' decodes as '1' (digit)
            DecodeMap['I'] = 1; DecodeMap['i'] = 1;
            DecodeMap['L'] = 1; DecodeMap['l'] = 1;
        }

        /// <summary>
        /// Encodes a 64-bit unsigned integer (ulong) into a Crockford Base32 string.
        /// </summary>
        /// <param name="number">The ulong to encode.</param>
        /// <returns>The Crockford Base32 encoded string.</returns>
        public static string Encode(ulong number)
        {
            if (number == 0)
            {
                return "0";
            }

            var sb = new StringBuilder();
            while (number > 0)
            {
                sb.Append(EncodeAlphabet[(int)(number % 32)]);
                number /= 32;
            }

            // The characters are generated in reverse order, so reverse the string.
            char[] arr = sb.ToString().ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Encodes a 64-bit signed integer (long) into a Crockford Base32 string.
        /// The bit pattern of the long is treated as an unsigned long for encoding.
        /// </summary>
        /// <param name="number">The long to encode.</param>
        /// <returns>The Crockford Base32 encoded string.</returns>
        public static string Encode(long number)
        {
            return Encode((ulong)number);
        }

        /// <summary>
        /// Decodes a Crockford Base32 string into a 64-bit unsigned integer (ulong).
        /// </summary>
        /// <param name="crockfordString">The Crockford Base32 string to decode. Case-insensitive.</param>
        /// <returns>The decoded ulong.</returns>
        /// <exception cref="ArgumentNullException">Thrown if crockfordString is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if crockfordString contains invalid characters.</exception>
        /// <exception cref="OverflowException">Thrown if the decoded number exceeds ulong.MaxValue.</exception>
        public static ulong Decode(string crockfordString)
        {
            if (string.IsNullOrWhiteSpace(crockfordString))
            {
                // Conventionally, an empty string might decode to 0, or you might require "0".
                // For this implementation, let's treat "0" as the only representation for zero.
                // If an empty string is not allowed, throw ArgumentNullException.
                // If you want empty string to mean 0: if (string.IsNullOrEmpty(crockfordString)) return 0;
                throw new ArgumentNullException(nameof(crockfordString));
            }

            return Decode(crockfordString.AsSpan());
        }

        /// <summary>
        /// Decodes a Crockford Base32 string into a 64-bit unsigned integer (ulong).
        /// </summary>
        /// <param name="crockfordString">The Crockford Base32 string to decode. Case-insensitive.</param>
        /// <returns>The decoded ulong.</returns>
        /// <exception cref="ArgumentException">Thrown if crockfordString is empty, contains invalid characters, or decodes to a value exceeding ulong.MaxValue.</exception>
        public static ulong Decode(ReadOnlySpan<char> crockfordString)
        {
            // Trim whitespace
            crockfordString = crockfordString.Trim();

            if (crockfordString.IsEmpty)
            {
                throw new ArgumentException("Crockford Base32 string cannot be empty.", nameof(crockfordString));
            }

            if (crockfordString.SequenceEqual("0"))
            {
                return 0;
            }

            ulong result = 0;

            // Process each character directly - DecodeMap handles both cases
            foreach (char c in crockfordString)
            {
                if (!DecodeMap.TryGetValue(c, out byte value))
                {
                    throw new ArgumentException($"Invalid character '{c}' in Crockford Base32 string.", nameof(crockfordString));
                }

                // Check for overflow before multiplication and addition
                if (result > (ulong.MaxValue - value) / 32)
                {
                    throw new OverflowException("Crockford Base32 string decodes to a number larger than ulong.MaxValue.");
                }

                result = result * 32 + value;
            }

            return result;
        }

        /// <summary>
        /// Decodes a Crockford Base32 string into a 64-bit signed integer (long).
        /// </summary>
        public static long DecodeToInt64(ReadOnlySpan<char> crockfordString)
        {
            return (long)Decode(crockfordString);
        }
    }

    public static class LongBitShuffler
    {
        private static readonly int[] ShufflePermutation = new int[64];
        private static readonly int[] UnshufflePermutation = new int[64];

        static LongBitShuffler()
        {
            // Initialize the permutation arrays for 8-chunk interleaving.
            // Each chunk is 8 bits long. There are 8 chunks.
            // original_bit_index = k_orig * 8 + j_orig
            //   (k_orig = original chunk index [0-7], j_orig = bit index within original chunk [0-7])
            // new_bit_index = j_orig * 8 + k_orig
            //   (The j_orig-th bit from original chunk k_orig goes to this new position)

            for (int k_orig = 0; k_orig < 8; k_orig++) // k_orig is the original chunk index (0-7)
            {
                for (int j_orig = 0; j_orig < 8; j_orig++) // j_orig is the bit index within that original chunk (0-7)
                {
                    int originalBitIndex = k_orig * 8 + j_orig;
                    int newBitIndex = j_orig * 8 + k_orig;

                    ShufflePermutation[originalBitIndex] = newBitIndex;
                    UnshufflePermutation[newBitIndex] = originalBitIndex;
                }
            }
        }

        /// <summary>
        /// Shuffles the bits of an 8-byte long by interleaving eight 8-bit chunks.
        /// </summary>
        /// <param name="value">The long value whose bits are to be shuffled.</param>
        /// <returns>The long value with its bits shuffled.</returns>
        public static long ShuffleBits(long value)
        {
            long shuffledValue = 0L;
            for (int i = 0; i < 64; i++) // i is the original bit position
            {
                // If the i-th bit of the original value is set
                if (((value >> i) & 1L) == 1L)
                {
                    // Set the corresponding bit in the shuffled value at its new permuted position
                    shuffledValue |= (1L << ShufflePermutation[i]);
                }
            }

            return shuffledValue;
        }

        /// <summary>
        /// Unshuffles the bits of an 8-byte long that was shuffled by ShuffleBits,
        /// restoring the original 8-chunk interleaving.
        /// </summary>
        /// <param name="shuffledValue">The long value with shuffled bits.</param>
        /// <returns>The original, de-shuffled long value.</returns>
        public static long UnshuffleBits(long shuffledValue)
        {
            long originalValue = 0L;
            for (int i = 0; i < 64; i++) // i is the shuffled bit position
            {
                // If the i-th bit of the shuffled value is set
                if (((shuffledValue >> i) & 1L) == 1L)
                {
                    // Set the corresponding bit in the original value at its original (unpermuted) position
                    originalValue |= (1L << UnshufflePermutation[i]);
                }
            }

            return originalValue;
        }
    }
}
