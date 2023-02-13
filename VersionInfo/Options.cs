// <copyright file="Options.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Program Options class</summary>

using System.Collections.Generic;

namespace VersionInfo
{
    /// <summary>
    /// Options class
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Print help option
        /// </summary>
        public bool PrintHelp { get; set; }

        /// <summary>
        /// Print version option
        /// </summary>
        public bool PrintVersion { get; set; }

        /// <summary>
        /// Get file size option
        /// </summary>
        public bool GetFileSize { get; set; } = true;

        /// <summary>
        /// Get file time option
        /// </summary>
        public bool GetFileTime { get; set; } = true;

        /// <summary>
        /// Search root
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        /// Calculate CRC32 option
        /// </summary>
        public bool CalculateCrc32 { get; set; }

        /// <summary>
        /// Calculate MD5 option
        /// </summary>
        public bool CalculateMd5 { get; set; }

        /// <summary>
        /// Calculate SHA-1 option
        /// </summary>
        public bool CalculateSha1 { get; set; }

        /// <summary>
        /// Calculate SHA-2 option
        /// </summary>
        public bool CalculateSha2 { get; set; }

        /// <summary>
        /// Summary mode
        /// </summary>
        public Summary Summary { get; set; } = Summary.None;

        /// <summary>
        /// Search Pattern options
        /// </summary>
        public List<string> Patterns { get; } = new();
    }
}
