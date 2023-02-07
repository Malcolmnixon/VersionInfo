// <copyright file="FileParser.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Parse file(s) for all relevant information</summary>

using System.IO;

namespace VersionInfo.Parsers
{
    /// <summary>
    /// File parser class
    /// </summary>
    public static class FileParser
    {
        /// <summary>
        /// Parse file information
        /// </summary>
        /// <param name="info">FileInformation to populate</param>
        /// <param name="options">Additional parse options</param>
        public static void Parse(FileInformation info, Options options)
        {
            // Get the file information
            var fileInfo = new FileInfo(info.FullPath);

            // Save file size if requested
            if (options.GetFileSize)
                info.Size = fileInfo.Length;

            // Save file time if requested
            if (options.GetFileTime)
                info.ModifyTime = fileInfo.LastWriteTime;

            // Add hashes
            if (options.CalculateCrc32) Crc32HashParser.Parse(info, options);
            if (options.CalculateMd5) Md5HashParser.Parse(info, options);
            if (options.CalculateSha1) Sha1HashParser.Parse(info, options);
            if (options.CalculateSha2) Sha2HashParser.Parse(info, options);

            // Process based on extensions
            switch (Path.GetExtension(info.FullPath))
            {
                case ".exe":
                case ".dll":
                case ".ocx":
                case ".drv":
                    WindowsVersionInfoParser.Parse(info, options);
                    break;

                case ".jar":
                    JarVersionInfoParser.Parse(info, options);
                    break;

                case ".msi":
                    MsiVersionInfoParser.Parse(info, options);
                    break;

                case ".nupkg":
                    NugetVersionInfoParser.Parse(info, options);
                    break;
            }
        }
    }
}
