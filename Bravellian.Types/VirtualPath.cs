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

namespace Bravellian;

[DebuggerDisplay("{ToString()}")]
[JsonConverter(typeof(VirtualPathJsonConverter))]
public readonly record struct VirtualPath
        : IComparable,
          IComparable<VirtualPath>
{
    public static readonly VirtualPath Root = new("/");
    public static readonly char DefaultDirectorySeperator = '/';

    private readonly List<string> segments = [];

    public VirtualPath(string value)
        : this(value, DefaultDirectorySeperator)
    {
    }

    public VirtualPath(string value, char directorySeparator)
    {
        this.segments = value.Split(directorySeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        this.DirectorySeperator = directorySeparator;
    }

    public VirtualPath(IReadOnlyList<string> segments)
        : this(segments, DefaultDirectorySeperator)
    {
    }

    public VirtualPath(IEnumerable<string> segments, char directorySeperator)
    {
        this.segments = segments?.ToList() ?? new List<string>();
        this.DirectorySeperator = directorySeperator;
    }

    public IReadOnlyList<string> Segments => this.segments;

    public char DirectorySeperator { get; }

    public override string ToString() => string.Join(this.DirectorySeperator, this.segments);

    public string ToString(char overrideSeperator) => string.Join(overrideSeperator, this.segments);

    public int CompareTo(VirtualPath other)
    {
        return string.CompareOrdinal(this.ToString('¦'), other.ToString('¦'));
    }

    public int CompareTo(object? obj)
    {
        if (obj is VirtualPath id)
        {
            return string.CompareOrdinal(this.ToString('¦'), id.ToString('¦'));
        }

        return string.Compare(this.ToString('¦'), obj?.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public bool Equals(VirtualPath other)
    {
        if (this.segments is null && other.segments is null)
        {
            return true;
        }

        if (this.segments is null || other.segments is null)
        {
            return false;
        }

        var equal = this.DirectorySeperator.Equals(other.DirectorySeperator) && this.segments.Count.Equals(other.segments.Count);
        if (!equal)
        {
            return false;
        }

#pragma warning disable S2589
        for (int i = 0; i < this.segments.Count && equal; i++)
#pragma warning restore S2589
        {
            equal = this.segments[i].Equals(other.segments[i], StringComparison.Ordinal);

            if (!equal)
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode(StringComparison.Ordinal);
    }

    public static VirtualPath Parse(string s)
    {
        return new VirtualPath(s);
    }

    public static VirtualPath Parse(string s, char directorySeparator)
    {
        return new VirtualPath(s, directorySeparator);
    }

    public static VirtualPath? TryParse(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return new VirtualPath(value);
        }

        return null;
    }

    public static bool TryParse(string value, out VirtualPath id)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            id = new VirtualPath(value);
            return true;
        }

        id = default;
        return false;
    }

    public class VirtualPathJsonConverter : JsonConverter<VirtualPath>
    {
        public override VirtualPath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var val = reader.GetString();
            if (!string.IsNullOrWhiteSpace(val))
            {
                return new VirtualPath(val);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, VirtualPath value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}

public static class VirtualPathExtensions
{
    public static string ToSystemPath(this VirtualPath source)
    {
        return source.ToString(Path.DirectorySeparatorChar);
    }

    public static string GetFileName(this VirtualPath source)
    {
        return Path.GetFileName(source.ToSystemPath());
    }

    public static string GetFileNameWithoutExtension(this VirtualPath source)
    {
        return Path.GetFileNameWithoutExtension(source.ToSystemPath());
    }

    public static string GetExtension(this VirtualPath source)
    {
        return Path.GetExtension(source.ToSystemPath());
    }

    public static bool HasExtension(this VirtualPath source)
    {
        return Path.HasExtension(source.ToSystemPath());
    }

    public static VirtualPath ChangeExtension(this VirtualPath source, string? extension)
    {
        var newPath = Path.ChangeExtension(source.ToSystemPath(), extension);
        return new VirtualPath(newPath.Split(Path.DirectorySeparatorChar), source.DirectorySeperator);
    }

    public static VirtualPath? Combine(this VirtualPath source, VirtualPath other)
    {
        IEnumerable<string> allSegments = source.Segments.Concat(other.Segments);
        return new VirtualPath(allSegments, source.DirectorySeperator);
    }

    public static VirtualPath? Combine(this VirtualPath source, string other)
    {
        VirtualPath? otherPath = VirtualPath.TryParse(other);
        if (otherPath is null)
        {
            return source;
        }

        IEnumerable<string> allSegments = source.Segments.Concat(otherPath.Value.Segments);
        return new VirtualPath(allSegments, source.DirectorySeperator);
    }
}
