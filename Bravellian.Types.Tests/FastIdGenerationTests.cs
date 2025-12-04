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

namespace Bravellian.Types.Tests;
using System;
using System.Collections.Generic;
using Xunit;

// Assuming the FastId struct and its dependencies are in a project referenced by this test project.
// using YourNamespace; 

public class FastIdGenerationTests
{
    // Replicate constants from FastId to make tests independent and clear.
    private const int TimestampBits = 34;
    private const int RandomBits = 30; // 64 - TimestampBits

    // The epoch as defined in your FastId class.
    private static readonly DateTimeOffset CustomEpoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

    // The maximum timestamp for our tests, as specified in your request.
    private readonly DateTimeOffset _maxTimestamp = new DateTimeOffset(2025, 07, 15, 0, 0, 0, TimeSpan.Zero);

    #region Helper Method

    /// <summary>
    /// Helper method to reverse the FastId generation process and extract the DateTimeOffset.
    /// This is crucial for verifying the timestamp constraints.
    /// </summary>
    private static DateTimeOffset GetTimestampFromFastId(FastId id)
    {
        // To get the original timestamp and random parts, we must first unshuffle the bits.
        // The provided FastId class does not expose the un-shuffled value directly, 
        // so we must call the public UnshuffleBits method.
        long unshuffledValue = FastId.LongBitShuffler.UnshuffleBits(id.Value);

        // The timestamp is stored in the most significant bits.
        // We shift the value right to isolate the timestamp part.
        ulong timestampSeconds = (ulong)unshuffledValue >> RandomBits;

        // Convert the seconds back into a DateTimeOffset by adding them to the custom epoch.
        return CustomEpoch.AddSeconds(timestampSeconds);
    }

    #endregion

    #region FromGuidWithinTimestampRange Tests

    [Fact]
    public void FromGuid_WithFixedGuid_IsDeterministic()
    {
        // Arrange
        var guid = new Guid("1a721394-6a63-4a55-89e5-8246c63749d2");

        // Act
        var id1 = FastId.FromGuidWithinTimestampRange(guid, _maxTimestamp);
        var id2 = FastId.FromGuidWithinTimestampRange(guid, _maxTimestamp);

        // Assert
        Assert.Equal(id1.Value, id2.Value);
        Assert.Equal(id1.Encoded, id2.Encoded);
    }

    [Theory]
    [InlineData("1a721394-6a63-4a55-89e5-8246c63749d2")]
    [InlineData("00000000-0000-0000-0000-000000000000")] // Guid.Empty
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")] // Max Guid
    public void FromGuid_WithValidInputs_GeneratesTimestambvithinRange(string guidString)
    {
        // Arrange
        var guid = new Guid(guidString);

        // Act
        var fastId = FastId.FromGuidWithinTimestampRange(guid, _maxTimestamp);
        var generatedTimestamp = GetTimestampFromFastId(fastId);

        // Assert
        Assert.True(generatedTimestamp >= CustomEpoch, "Generated timestamp should not be before the epoch.");
        Assert.True(generatedTimestamp <= _maxTimestamp, "Generated timestamp should not be after the specified maximum.");
    }

    [Fact]
    public void FromGuid_WhenMaxTimestampIsEpoch_GeneratesTimestampExactlyAtEpoch()
    {
        // Arrange
        var guid = Guid.NewGuid();
        // Set the max timestamp to be the epoch itself. The modulo operation should result in 0.
        var maxTimestampAsEpoch = CustomEpoch;

        // Act
        var fastId = FastId.FromGuidWithinTimestampRange(guid, maxTimestampAsEpoch);
        var generatedTimestamp = GetTimestampFromFastId(fastId);

        // Assert
        // The timestamp value must be 0 seconds past the epoch.
        Assert.Equal(CustomEpoch, generatedTimestamp);
    }

    [Fact]
    public void FromGuid_WhenMaxTimestampIsBeforeEpoch_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var invalidMaxTimestamp = CustomEpoch.AddSeconds(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            FastId.FromGuidWithinTimestampRange(guid, invalidMaxTimestamp)
        );

        Assert.Equal("maxTimestamp", exception.ParamName);
    }

    #endregion

    #region FromStringWithinTimestampRange Tests

    [Fact]
    public void FromString_WithFixedString_IsDeterministic()
    {
        // Arrange
        var sourceString = "test-string-123";

        // Act
        var id1 = FastId.FromStringWithinTimestampRange(sourceString, _maxTimestamp);
        var id2 = FastId.FromStringWithinTimestampRange(sourceString, _maxTimestamp);

        // Assert
        Assert.Equal(id1.Value, id2.Value);
        Assert.Equal(id1.Encoded, id2.Encoded);
    }

    [Theory]
    [InlineData("my-unique-identifier")]
    [InlineData("user@example.com")]
    [InlineData("Another ID")]
    [InlineData("12345")]
    [InlineData("Complex_String-With/Special$Chars!@#%^&*()")] // Test with various characters
    [InlineData("こんにちは世界")] // Test with Unicode characters
    public void FromString_WithValidInputs_GeneratesTimestambvithinRange(string sourceString)
    {
        // Arrange & Act
        var fastId = FastId.FromStringWithinTimestampRange(sourceString, _maxTimestamp);
        var generatedTimestamp = GetTimestampFromFastId(fastId);

        // Assert
        Assert.True(generatedTimestamp >= CustomEpoch, "Generated timestamp should not be before the epoch.");
        Assert.True(generatedTimestamp <= _maxTimestamp, "Generated timestamp should not be after the specified maximum.");
    }

    [Fact]
    public void FromString_WhenMaxTimestampIsEpoch_GeneratesTimestampExactlyAtEpoch()
    {
        // Arrange
        var sourceString = "any-string-will-do";
        // Set the max timestamp to be the epoch itself. The modulo operation should result in 0.
        var maxTimestampAsEpoch = CustomEpoch;

        // Act
        var fastId = FastId.FromStringWithinTimestampRange(sourceString, maxTimestampAsEpoch);
        var generatedTimestamp = GetTimestampFromFastId(fastId);

        // Assert
        // The timestamp value must be 0 seconds past the epoch.
        Assert.Equal(CustomEpoch, generatedTimestamp);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void FromString_WithNullOrEmpty_ThrowsArgumentException(string invalidString)
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            FastId.FromStringWithinTimestampRange(invalidString, _maxTimestamp)
        );

        Assert.Equal("sourceString", exception.ParamName);
    }

    [Fact]
    public void FromString_WhenMaxTimestampIsBeforeEpoch_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var sourceString = "test-string";
        var invalidMaxTimestamp = CustomEpoch.AddSeconds(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            FastId.FromStringWithinTimestampRange(sourceString, invalidMaxTimestamp)
        );

        Assert.Equal("maxTimestamp", exception.ParamName);
    }

    #endregion
}