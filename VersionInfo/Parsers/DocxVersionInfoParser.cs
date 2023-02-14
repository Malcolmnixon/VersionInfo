using System;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace VersionInfo.Parsers
{
    /// <summary>
    /// Docx version information parser
    /// </summary>
    public static class DocxVersionInfoParser
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
                // Open docx as zip file
                using var docx = ZipFile.OpenRead(info.FullPath);

                // Find the core entry
                var coreEntry = docx.GetEntry("docProps/core.xml");
                if (coreEntry == null)
                    return;

                // Parse the coreXml
                var coreXml = XDocument.Load(coreEntry.Open());

                // Get the raw fields
                var title = coreXml.Descendants().FirstOrDefault(e => e.Name.LocalName == "title")?.Value;
                var version = coreXml.Descendants().FirstOrDefault(e => e.Name.LocalName == "revision")?.Value;
                var author = coreXml.Descendants().FirstOrDefault(e => e.Name.LocalName == "creator")?.Value;

                // Return version information
                info.Title = title?.Trim();
                info.Version = version?.Trim();
                info.Author = author?.Trim();
            }
            catch
            {
                // Unable to parse Nuget version information
            }
        }
    }
}
