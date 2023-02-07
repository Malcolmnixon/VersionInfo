// <copyright file="FileInformation.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>File Information class</summary>

using System;

namespace VersionInfo
{
    /// <summary>
    /// File Information class
    /// </summary>
    public class FileInformation
    {
        #region Basic Information

        /// <summary>
        /// Full path
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File size (or null if unavailable)
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// File modify time (or null if unavailable)
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        #endregion
        
        #region Hash Information

        /// <summary>
        /// File CRC32 (or null if unavailable or not calculated)
        /// </summary>
        public uint? Crc32 { get; set; }

        /// <summary>
        /// File MD5 (or null if unavailable or not calculated)
        /// </summary>
        public string Md5 { get; set; }

        /// <summary>
        /// File SHA1 (or null if unavailable or not calculated)
        /// </summary>
        public string Sha1 { get; set; }

        /// <summary>
        /// File SHA2 (or null if unavailable or not calculated)
        /// </summary>
        public string Sha2 { get; set; }

        #endregion

        #region Version Information

        /// <summary>
        /// File Title (or null if unavailable)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// File Version (or null if unavailable)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Product Name (of null if unavailable)
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// File Author (or null if unavailable)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// File Copyright (or null if unavailable)
        /// </summary>
        public string Copyright { get; set; }

        #endregion
    }
}
