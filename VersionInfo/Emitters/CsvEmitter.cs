// <copyright file="CsvEmitter.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Emitter for CSV report</summary>

using System;
using System.Collections.Generic;

namespace VersionInfo.Emitters
{
    /// <summary>
    /// Emitter for CSV report
    /// </summary>
    public class CsvEmitter : IEmitter
    {
        /// <summary>
        /// Emit options
        /// </summary>
        private Options _options;

        /// <summary>
        /// CSV-escape the text of an object
        /// </summary>
        /// <param name="obj">Object to escape</param>
        /// <returns>Escaped text</returns>
        private static string Esc(object obj)
        {
            // Get text
            var text = obj.ToString();

            // Perform escaping if necessary
            if (text.Contains(',') || text.Contains('"') || text.Contains('\n'))
            {
                text = text.Replace("\"", "\"\"");
                text = "\"" + text + "\"";
            }

            // Return text
            return text;
        }

        public void Begin(Options options)
        {
            _options = options;

            // Print header
            Console.WriteLine("Name,Size,Time,CRC32,MD5,SHA1,SHA2,Title,Version,Product,Author,Copyright");
        }

        public void Entry(FileInformation file)
        {
            // Skip if we only want summary
            if (_options.Summary == Summary.Only)
                return;

            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                Esc(file.Name),
                file.Size?.ToString() ?? "-",
                file.ModifyTime?.ToString("yyyy/MM/dd HH:mm:ss") ?? string.Empty,
                file.Crc32?.ToString("x8") ?? string.Empty,
                file.Md5 ?? string.Empty,
                file.Sha1 ?? string.Empty,
                file.Sha2 ?? string.Empty,
                Esc(file.Title ?? string.Empty),
                Esc(file.Version ?? string.Empty),
                Esc(file.Product ?? string.Empty),
                Esc(file.Author ?? string.Empty),
                Esc(file.Copyright ?? string.Empty));
        }

        public void End(IList<FileInformation> files, FileInformation summary)
        {
            // Skip if summary is disabled
            if (_options.Summary == Summary.None)
                return;

            Console.WriteLine("Summary,{0},,{1},{2},{3},{4},,,,,",
                summary.Size ?? 0,
                summary.Crc32?.ToString("x8") ?? string.Empty,
                summary.Md5 ?? string.Empty,
                summary.Sha1 ?? string.Empty,
                summary.Sha2 ?? string.Empty);
        }
    }
}
