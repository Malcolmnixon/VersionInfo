// <copyright file="MsiVersionInfoParser.cs" company="DEMA Consulting">
// Copyright © 2022 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>Windows MSI version information parser</summary>

using System.Collections.Generic;
using WixToolset.Dtf.WindowsInstaller;

namespace VersionInfo.Parsers
{
    /// <summary>
    /// Windows MSI version information parser
    /// </summary>
    public static class MsiVersionInfoParser
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
                // Get the summary information
                var summary = new SummaryInfo(info.FullPath, false);

                var data = new Dictionary<string, string>();
                using (var database = new Database(info.FullPath, DatabaseOpenMode.ReadOnly))
                {
                    var view = database.OpenView("Select `Property`,`Value` FROM `Property`");
                    view.Execute();
                    var record = view.Fetch();
                    while (record != null)
                    {
                        data[record.GetString(1)] = record.GetString(2);
                        record = view.Fetch();
                    }
                    view.Close();
                }

                // Get the product name
                info.Title = data.TryGetValue("ProductName", out var productName) ? productName : summary.Title;

                // Get the product version
                info.Version = data.TryGetValue("ProductVersion", out var productVersion) ? productVersion : null;

                // Get the manufacturer
                info.Author = data.TryGetValue("Manufacturer", out var manufacturer) ? manufacturer : summary.Author;
            }
            catch
            {
                // Skip if we failed to get the information
            }
        }
    }
}
