﻿//
// FileUtil.cs
//
// Author:
//       Mikayla Hutchinson <m.j.hutchinson@gmail.com>
//
// Copyright (c) 2013 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;

namespace Mono.TextTemplating
{
	static class FileUtil
	{
		//from MonoDevelop.Core.FileService, copied here so Mono.TextTemplating can be used w/o MD dependency
		public static string AbsoluteToRelativePath (string baseDirectoryPath, string absPath)
		{
			var fileUri = new Uri(absPath);
			var referenceUri = new Uri(baseDirectoryPath);
			return Uri.UnescapeDataString(referenceUri.MakeRelativeUri(fileUri).ToString()).Replace('/', Path.DirectorySeparatorChar);
		}

		static bool IsSeparator (char ch)
		{
			return ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar || ch == Path.VolumeSeparatorChar;
		}

		static string GetFullPath (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			if (!isWindows || path.IndexOf ('*') == -1)
				return Path.GetFullPath (path);
			else {
				// On Windows, GetFullPath doesn't work if the path contains wildcards.
				path = path.Replace ("*", wildcardMarker);
				path = Path.GetFullPath (path);
				return path.Replace (wildcardMarker, "*");
			}
		}

		static readonly string wildcardMarker = "_" + Guid.NewGuid () + "_";
		static readonly bool isWindows = Path.DirectorySeparatorChar == '\\';
	}
}
