// <copyright file="TextEmitter.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Emitter for Text report</summary>

using System;
using System.Collections.Generic;
using System.Linq;

namespace VersionInfo.Emitters
{
    /// <summary>
    /// Emitter for Text report
    /// </summary>
    public class TextEmitter : IEmitter
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
        }

        public void End(IList<FileInformation> files, FileInformation summary)
        {
            // Calculate lengths and visibilities
            var nameLen = 16;
            var sizeLen = Math.Max(4, (summary.Size ?? 0).ToString().Length);
            var timeLen = 19;
            var titleLen = "Title".Length;
            var versionLen = "Version".Length;
            var productLen = "Product".Length;
            var printSize = _options.Summary != Summary.None;
            var printTime = false;
            var printTitle = false;
            var printVersion = false;
            var printProduct = false;

            // Update lengths to include files (if enabled)
            if (files.Any() && _options.Summary != Summary.Only)
            {
                nameLen = Math.Max(nameLen, files.Max(f => f.Name.Length));
                sizeLen = Math.Max(sizeLen, files.Max(f => f.Size ?? 0).ToString().Length);
                titleLen = Math.Max(titleLen, Math.Min(32, files.Max(f => f.Title?.Length ?? 0)));
                versionLen = Math.Max(versionLen, Math.Min(32, files.Max(f => f.Version?.Length ?? 0)));
                productLen = Math.Max(productLen, Math.Min(32, files.Max(f => f.Product?.Length ?? 0)));
                printSize |= files.Any(f => f.Size != null);
                printTime = files.Any(f => f.ModifyTime != null);
                printTitle = files.Any(f => f.Title != null);
                printVersion = files.Any(f => f.Version != null);
                printProduct = files.Any(f => f.Product != null);
            }

            // Write header
            Console.Write("{0} ", "Name".PadRight(nameLen));
            if (printSize) Console.Write("{0} ", "Size".PadRight(sizeLen));
            if (printTime) Console.Write("{0} ", "Time".PadRight(timeLen));
            if (printTitle) Console.Write("{0} ", "Title".PadRight(titleLen));
            if (printVersion) Console.Write("{0} ", "Version".PadRight(versionLen));
            if (printProduct) Console.Write("{0} ", "Product".PadRight(productLen));
            if (_options.CalculateCrc32) Console.Write("CRC32 ");
            if (_options.CalculateMd5) Console.Write("MD5 ");
            if (_options.CalculateSha1) Console.Write("SHA-1 ");
            if (_options.CalculateSha2) Console.Write("SHA-2 ");
            Console.WriteLine();

            // Print all files
            if (_options.Summary != Summary.Only)
            {
                foreach (var info in files)
                {
                    Console.Write("{0} ", info.Name.PadRight(nameLen));
                    if (printSize) Console.Write("{0} ", (info.Size?.ToString() ?? "-").PadRight(sizeLen));
                    if (printTime) Console.Write("{0} ", (info.ModifyTime?.ToString("yyyy/MM/dd HH:mm:ss") ?? "-").PadRight(timeLen));
                    if (printTitle) Console.Write("{0} ", (info.Title ?? "-").PadRight(titleLen));
                    if (printVersion) Console.Write("{0} ", (info.Version ?? "-").PadRight(versionLen));
                    if (printProduct) Console.Write("{0} ", (info.Product ?? "-").PadRight(productLen));
                    if (_options.CalculateCrc32) Console.Write("{0:x8} ", info.Crc32);
                    if (_options.CalculateMd5) Console.Write("{0} ", info.Md5);
                    if (_options.CalculateSha1) Console.Write("{0} ", info.Sha1);
                    if (_options.CalculateSha2) Console.Write("{0} ", info.Sha2);
                    Console.WriteLine();
                }
            }

            // Skip if summary is disabled
            if (_options.Summary == Summary.None)
                return;

            Console.Write("{0} ", "Summary".PadRight(nameLen));
            Console.Write("{0} ", (summary.Size ?? 0).ToString().PadRight(sizeLen));
            if (printTime) Console.Write("{0} ", "-".PadRight(timeLen));
            if (printTitle) Console.Write("{0} ", "-".PadRight(titleLen));
            if (printVersion) Console.Write("{0} ", "-".PadRight(versionLen));
            if (printProduct) Console.Write("{0} ", "-".PadRight(productLen));
            if (_options.CalculateCrc32) Console.Write("{0:x8} ", summary.Crc32);
            if (_options.CalculateMd5) Console.Write("{0} ", summary.Md5);
            if (_options.CalculateSha1) Console.Write("{0} ", summary.Sha1);
            if (_options.CalculateSha2) Console.Write("{0} ", summary.Sha2);
            Console.WriteLine();
        }
    }
}
