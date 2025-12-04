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

using System.IO;
using System.Text.Json.Serialization;
using CommunityToolkit.Diagnostics;

public sealed record BvFile
{
    public BvFile(byte[] bytes, string fileName, string contentType)
    {
        this.FileName = fileName;
        this.ContentType = contentType;
        this.Data = BinaryData.FromBytes(bytes);
    }

    [JsonConstructor]
    public BvFile(BinaryData data, string fileName, string contentType)
    {
        this.FileName = fileName;
        this.ContentType = contentType;
        this.Data = data;
    }

    public BvFile(ReadOnlySpan<byte> bytes, string fileName, string contentType)
    {
        this.FileName = fileName;
        this.ContentType = contentType;
        this.Data = BinaryData.FromBytes(bytes.ToArray());
    }

    public BvFile(Stream stream, string fileName, string contentType)
    {
        this.FileName = fileName;
        this.ContentType = contentType;
        this.Data = BinaryData.FromStream(stream);
    }

    public string FileName { get; }

    public string ContentType { get; }

    public BinaryData Data { get; }

    public static BvFile FromBase64(string base64, string fileName, string contentType)
    {
        return new BvFile(Convert.FromBase64String(base64), fileName, contentType);
    }

    public static BvFile FromBase64(string base64, string fileName)
    {
        return new BvFile(Convert.FromBase64String(base64), fileName, "application/octet-stream");
    }

    public static BvFile FromBase64(string base64)
    {
        return new BvFile(Convert.FromBase64String(base64), "file", "application/octet-stream");
    }

    public static BvFile FromPath(string path)
    {
        Guard.IsNotNullOrWhiteSpace(path);

        var bytes = File.ReadAllBytes(path);
        var fileName = Path.GetFileName(path);
        var contentType = MimeKit.MimeTypes.GetMimeType(fileName);
        return new BvFile(bytes, fileName, contentType);
    }

    public static BvFile FromFileInfo(FileInfo fileInfo)
    {
        Guard.IsNotNull(fileInfo);

        var bytes = File.ReadAllBytes(fileInfo.FullName);
        var fileName = fileInfo.Name;
        var contentType = MimeKit.MimeTypes.GetMimeType(fileName);
        return new BvFile(bytes, fileName, contentType);
    }
}
