// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Docs.Build
{
    internal class SourceInfo : IEquatable<SourceInfo>, IComparable<SourceInfo>
    {
        /// <summary>
        /// Path to the source file.
        /// </summary>
        public FilePath File { get; }

        /// <summary>
        /// A one based start line value.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// A one based start column value.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// A one based end line value.
        /// </summary>
        public int EndLine { get; }

        /// <summary>
        /// A one based end column value.
        /// </summary>
        public int EndColumn { get; }

        /// <summary>
        /// A special storage for source info of the JObject property key
        /// if this is a JObject property value.
        /// </summary>
        public SourceInfo? KeySourceInfo { get; }

        public SourceInfo(FilePath file)
            : this(file, 0, 0, 0, 0)
        { }

        public SourceInfo(FilePath file, int line, int column, SourceInfo? keySourceInfo = null)
            : this(file, line, column, line, column, keySourceInfo)
        { }

        public SourceInfo(
            FilePath file, int startLine, int startColumn, int endLine, int endColumn, SourceInfo? keySourceInfo = null)
        {
            File = file;
            Line = startLine;
            Column = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
            KeySourceInfo = keySourceInfo;
        }

        public SourceInfo WithFile(FilePath file)
        {
            return file == File ? this : new SourceInfo(file, Line, Column, EndLine, EndColumn, KeySourceInfo);
        }

        public SourceInfo WithKeySourceInfo(SourceInfo? keySourceInfo)
        {
            return new SourceInfo(File, Line, Column, EndLine, EndColumn, keySourceInfo);
        }

        public static bool operator ==(SourceInfo? a, SourceInfo? b) => Equals(a, b);

        public static bool operator !=(SourceInfo? a, SourceInfo? b) => !Equals(a, b);

        public override bool Equals(object? obj)
        {
            return Equals(obj as SourceInfo);
        }

        public bool Equals(SourceInfo? other)
        {
            return other != null &&
                   File.Equals(other.File) &&
                   Line.Equals(other.Line) &&
                   Column.Equals(other.Column) &&
                   EndLine.Equals(other.EndLine) &&
                   EndColumn.Equals(other.EndColumn);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(File, Line, Column, EndLine, EndColumn);
        }

        public override string ToString()
        {
            return Line <= 1 && Column <= 1 ? File.ToString() : $"{File}({Line},{Column})";
        }

        public int CompareTo(SourceInfo? other)
        {
            if (other is null)
            {
                return 1;
            }

            var result = File.CompareTo(other.File);
            if (result == 0)
            {
                result = Line - other.Line;
            }

            if (result == 0)
            {
                result = Column - other.Column;
            }

            if (result == 0)
            {
                result = EndLine - other.EndLine;
            }

            if (result == 0)
            {
                result = EndColumn - other.EndColumn;
            }

            return result;
        }
    }
}
