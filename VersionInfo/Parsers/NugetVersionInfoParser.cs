// <copyright file="NugetVersionInfoParser.cs" company="DEMA Consulting">
// Copyright © 2022 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Nuget version information parser</summary>

using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace VersionInfo.Parsers
{
    public static class NugetVersionInfoParser
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
                // Open Nuget as zip file
                using var zip = ZipFile.OpenRead(info.FullPath);

                // Find the nuspec entry
                var nuspecEntry = zip.Entries.FirstOrDefault(e => e.Name.EndsWith(".nuspec"));
                if (nuspecEntry == null)
                    return;

                // Parse the nuspec
                var nuspec = XDocument.Load(nuspecEntry.Open());

                // Get the raw fields
                var title = nuspec.Descendants().FirstOrDefault(e => e.Name.LocalName == "description")?.Value;
                var version = nuspec.Descendants().FirstOrDefault(e => e.Name.LocalName == "version")?.Value;
                var product = nuspec.Descendants().FirstOrDefault(e => e.Name.LocalName == "id")?.Value;
                var author = nuspec.Descendants().FirstOrDefault(e => e.Name.LocalName == "authors")?.Value;
                var copyright = nuspec.Descendants().FirstOrDefault(e => e.Name.LocalName == "copyright")?.Value;

                // Return version information
                info.Title = title?.Trim();
                info.Version = version?.Trim();
                info.Product = product?.Trim();
                info.Author = author?.Trim();
                info.Copyright = copyright?.Trim();
            }
            catch
            {
                // Unable to parse Nuget version information
            }
        }
    }
}
