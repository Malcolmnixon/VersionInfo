// <copyright file="Sha2HashParser.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Parse file for SHA-2 Hash</summary>

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace VersionInfo.Parsers
{
    /// <summary>
    /// SHA-2 Hash information parser
    /// </summary>
    public static class Sha2HashParser
    {
        /// <summary>
        /// Parse hash information
        /// </summary>
        /// <param name="info">FileInformation to populate</param>
        /// <param name="options">Additional parse options</param>
        public static void Parse(FileInformation info, Options options)
        {
            try
            {
                // Calculate the SHA2 of the file
                using var sha2 = SHA256.Create();
                using var stream = File.OpenRead(info.FullPath);
                info.Sha2 = sha2.ComputeHash(stream).Select(b => b.ToString("x2")).Aggregate((cur, nxt) => cur + nxt);
            }
            catch
            {
                // Acceptable if we can't calculate hash
            }
        }

        /// <summary>
        /// Calculate summary hash of files
        /// </summary>
        /// <param name="files">List of files</param>
        /// <param name="summary">Summary</param>
        public static void Summary(IList<FileInformation> files, FileInformation summary)
        {
            // Calculate hash of the relevant files information
            using var sha2 = SHA256.Create();

            // Add the name and hash of every file into the MD5
            foreach (var file in files)
            {
                var name = Encoding.UTF8.GetBytes(file.Name);
                var hash = Encoding.UTF8.GetBytes(file.Sha2 ?? "");
                sha2.TransformBlock(name, 0, name.Length, name, 0);
                sha2.TransformBlock(hash, 0, hash.Length, hash, 0);
            }

            // Finalize the hash
            var empty = new byte[1];
            sha2.TransformFinalBlock(empty, 0, 0);

            // Write the summary hash
            summary.Sha2 = sha2.Hash.Select(b => b.ToString("x2")).Aggregate((cur, nxt) => cur + nxt);
        }
    }
}
