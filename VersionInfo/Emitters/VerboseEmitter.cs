// <copyright file="VerboseEmitter.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Emitter for Verbose report</summary>

using System;
using System.Collections.Generic;

namespace VersionInfo.Emitters
{
    /// <summary>
    /// Emitter for Verbose report
    /// </summary>
    public class VerboseEmitter : IEmitter
    {
        /// <summary>
        /// Emit options
        /// </summary>
        private Options _options;

        public void Begin(Options options)
        {
            _options = options;
        }

        public void Entry(FileInformation file)
        {
            // Skip if we only want summary
            if (_options.Summary == Summary.Only)
                return;

            Console.WriteLine("{0}", file.Name);
            if (file.Size != null) Console.WriteLine("  Size: {0}", file.Size);
            if (file.ModifyTime != null) Console.WriteLine("  Time: {0:yyyy/MM/dd HH:mm:ss}", file.ModifyTime);
            if (file.Crc32 != null) Console.WriteLine("  CRC32: {0:x8}", file.Crc32);
            if (file.Md5 != null) Console.WriteLine("  MD5: {0}", file.Md5);
            if (file.Sha1 != null) Console.WriteLine("  SHA-1: {0}", file.Sha1);
            if (file.Sha2 != null) Console.WriteLine("  SHA-2: {0}", file.Sha2);
            if (file.Title != null) Console.WriteLine("  Title: {0}", file.Title);
            if (file.Version != null) Console.WriteLine("  Version: {0}", file.Version);
            if (file.Product != null) Console.WriteLine("  Product: {0}", file.Product);
            if (file.Author != null) Console.WriteLine("  Author: {0}", file.Author);
            if (file.Copyright != null) Console.WriteLine("  Copyright: {0}", file.Copyright);
        }

        public void End(IList<FileInformation> files, FileInformation summary)
        {
            // Skip if summary is disabled
            if (_options.Summary == Summary.None)
                return;

            Console.WriteLine("Summary");
            Console.WriteLine("  Size: {0}", summary.Size ?? 0);
            if (summary.ModifyTime != null) Console.WriteLine("  Time: {0:yyyy/MM/dd HH:mm:ss}", summary.ModifyTime);
            if (summary.Crc32 != null) Console.WriteLine("  CRC32: {0:x8}", summary.Crc32);
            if (summary.Md5 != null) Console.WriteLine("  MD5: {0}", summary.Md5);
            if (summary.Sha1 != null) Console.WriteLine("  SHA-1: {0}", summary.Sha1);
            if (summary.Sha2 != null) Console.WriteLine("  SHA-2: {0}", summary.Sha2);
        }
    }
}
