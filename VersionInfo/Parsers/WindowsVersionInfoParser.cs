// <copyright file="WindowsVersionInfoParser.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Parse Windows (PE) files for version information</summary>

using System.Diagnostics;

namespace VersionInfo.Parsers
{
    /// <summary>
    /// Windows version information parser
    /// </summary>
    public static class WindowsVersionInfoParser
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
                // Get version information
                var verInfo = FileVersionInfo.GetVersionInfo(info.FullPath);

                // Save information
                info.Title = verInfo.FileDescription;
                info.Version = verInfo.FileVersion;
                info.Product = verInfo.ProductName;
                info.Author = verInfo.CompanyName;
                info.Copyright = verInfo.LegalCopyright;
            }
            catch
            {
                // Acceptable if we can't get version information
            }
        }
    }
}
