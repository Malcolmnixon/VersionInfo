// <copyright file="JarVersionInfoParser.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Parse Jar files for version information</summary>

using System.IO;
using System.IO.Compression;
using System.Linq;

namespace VersionInfo.Parsers
{
    /// <summary>
    /// Jar version information parser
    /// </summary>
    public static class JarVersionInfoParser
    {
        /// <summary>
        /// Parse version information
        /// </summary>
        /// <param name="info">FileInformation to populate</param>
        /// <param name="options">Additional parse options</param>
        public static void Parse(FileInformation info, Options options)
        {
            try
            {
                // Open JAR as zip file
                using var zip = ZipFile.OpenRead(info.FullPath);

                // Find the manifest entry
                var manifestEntry = zip.GetEntry("META-INF/MANIFEST.MF");
                if (manifestEntry == null)
                    return;

                // Read the manifest
                using var reader = new StreamReader(manifestEntry.Open());

                // Get manifest file lines
                var lines = reader.ReadToEnd().Split('\n').Select(line => line.Trim()).ToList();

                // Find title
                var titleLine = lines.FirstOrDefault(line => line.StartsWith("Implementation-Title:")) ??
                                lines.FirstOrDefault(line => line.StartsWith("Specification-Title:"));
                if (titleLine != null)
                {
                    info.Title = titleLine.Split(new[] {':'}, 2)[1].Trim();
                }

                // Find version
                var versionLine = lines.FirstOrDefault(line => line.StartsWith("Implementation-Version:")) ??
                                  lines.FirstOrDefault(line => line.StartsWith("Bundle-Version:")) ??
                                  lines.FirstOrDefault(line => line.StartsWith("Specification-Version:"));
                if (versionLine != null)
                {
                    info.Version = versionLine.Split(new[] {':'}, 2)[1].Trim();
                }

                // Find product
                var productLine = lines.FirstOrDefault(line => line.StartsWith("Bundle-Name:"));
                if (productLine != null)
                {
                    info.Product = productLine.Split(new[] {':'}, 2)[1].Trim();
                }

                // Find author
                var authorLine = lines.FirstOrDefault(line => line.StartsWith("Implementation-Vendor:")) ??
                                 lines.FirstOrDefault(line => line.StartsWith("Bundle-Vendor:")) ??
                                 lines.FirstOrDefault(line => line.StartsWith("Created-By:")) ??
                                 lines.FirstOrDefault(line => line.StartsWith("Specification-Vendor:"));
                if (authorLine != null)
                {
                    info.Author = authorLine.Split(new[] {':'}, 2)[1].Trim();
                }

                // Find license
                var licenseLine = lines.FirstOrDefault(line => line.StartsWith("Implementation-License:")) ??
                                  lines.FirstOrDefault(line => line.StartsWith("Bundle-License:"));
                if (licenseLine != null)
                {
                    info.Copyright = licenseLine.Split(new[] {':'}, 2)[1].Trim();
                }
            }
            catch
            {
                // Acceptable if we can't get version information
            }
        }
    }
}
