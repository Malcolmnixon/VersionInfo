// <copyright file="Program.cs" company="DEMA Consulting">
// Copyright © 2018 All Rights Reserved
// </copyright>
// <author>Malcolm Nixon</author>
// <summary>VersionInfo Application</summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VersionInfo.Emitters;
using VersionInfo.Parsers;

namespace VersionInfo
{
    /// <summary>
    /// VersionInfo Program Class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Get the full path of a path
        /// </summary>
        /// <param name="path">Path string</param>
        /// <returns>Full path string</returns>
        private static string GetFullPath(string path)
        {
            // Treat empty string as current directory
            if (string.IsNullOrEmpty(path))
                path = ".";

            // Convert to absolute path
            path = Path.GetFullPath(path);

            // Fix path element-by-element
            var fixedPath = string.Empty;
            foreach (var pathElem in path.Split(Path.DirectorySeparatorChar))
            {
                if (string.IsNullOrEmpty(fixedPath))
                {
                    fixedPath = pathElem + Path.DirectorySeparatorChar;
                    continue;
                }
                fixedPath = Directory.GetFileSystemEntries(fixedPath, pathElem)[0];
            }

            // Return fixed path
            return fixedPath;
        }

        /// <summary>
        /// Find files matching the given search pattern
        /// </summary>
        /// <param name="pattern">Search pattern</param>
        /// <param name="options">Options</param>
        /// <returns>Found files</returns>
        private static IEnumerable<FileInformation> Find(string pattern, Options options)
        {
            // Build the return list
            var ret = new List<FileInformation>();

            try
            {
                if (Directory.Exists(pattern))
                {
                    pattern = GetFullPath(pattern);

                    // Add all files in the directory
                    ret.AddRange(Directory.EnumerateFiles(pattern).Select(f => new FileInformation { FullPath = f }));

                    // Optionally recurse into child folders
                    if (options.Recursive)
                    {
                        ret.AddRange(Directory.EnumerateDirectories(pattern).SelectMany(d => Find(d, options)));
                    }
                }
                else
                {
                    // Split the pattern into directory and file
                    var patternDir = GetFullPath(Path.GetDirectoryName(pattern));
                    var patternFile = Path.GetFileName(pattern) ?? string.Empty;

                    // Add all matching files
                    ret.AddRange(Directory.EnumerateFiles(patternDir, patternFile).Select(f => new FileInformation { FullPath = f }));
                }
            }
            catch
            {
                // Errors can happen if user does not have permissions
            }

            // Return matching FileInformation
            return ret;
        }

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">Program arguments</param>
        public static void Main(string[] args)
        {
            // Get our version
            var version = Assembly.GetEntryAssembly().GetName().Version;

            // Options
            var options = new Options();
            var badArguments = false;
            IEmitter emitter = new TextEmitter();

            // Handle arguments from file
            var arguments = args.ToList();
            if (args.Length == 1 && args[0].StartsWith("@"))
            {
                try
                {
                    arguments = File.ReadAllText(args[0][1..])
                        .Split('\r', '\n')
                        .Select(a => a.Trim())
                        .Where(a => !string.IsNullOrEmpty(a))
                        .ToList();
                }
                catch
                {
                    // Report error
                    Console.WriteLine($"Unable to process {args[0][1..]}");
                    badArguments = true;
                }
            }

            // Process the arguments
            for (var i = 0; i < arguments.Count; ++i)
            {
                // Get argument (and optional parameter)
                var arg = arguments[i];
                var param = string.Empty;
                if (arg.StartsWith("--") && arg.Contains('='))
                {
                    // Argument is of type --arg=param
                    var sep = arg.IndexOf('=');
                    param = arg.Substring(sep + 1);
                    arg = arg.Substring(0, sep);
                }

                switch (arg)
                {
                    // Help
                    case "-h":
                    case "-?":
                    case "--help":
                        options.PrintHelp = true;
                        break;

                    // Version query
                    case "-v":
                    case "--version":
                        options.PrintVersion = true;
                        break;

                    // Recursive
                    case "-r":
                    case "--recurse":
                        options.Recursive = param != "off";
                        break;

                    // File size
                    case "-s":
                    case "--size":
                        options.GetFileSize = param != "off";
                        break;

                    // File time
                    case "-t":
                    case "--time":
                        options.GetFileTime = param != "off";
                        break;

                    // CRC32
                    case "-c":
                    case "--crc32":
                        options.CalculateCrc32 = param != "off";
                        break;
                        
                    // MD5
                    case "-m":
                    case "--md5":
                        options.CalculateMd5 = param != "off";
                        break;

                    // SHA-1
                    case "-s1":
                    case "--sha-1":
                        options.CalculateSha1 = param != "off";
                        break;

                    // SHA-2
                    case "-s2":
                    case "--sha-2":
                        options.CalculateSha2 = param != "off";
                        break;

                    case "--summary":
                        switch (param)
                        {
                            case "none":
                            case "off":
                                options.Summary = Summary.None;
                                break;

                            case "on":
                                options.Summary = Summary.On;
                                break;

                            case "only":
                                options.Summary = Summary.Only;
                                break;

                            default:
                                badArguments = true;
                                break;
                        }

                        break;

                    case "--emit-text": // Legacy style
                        emitter = new TextEmitter();
                        break;

                    case "--emit-verbose": // Legacy style
                        emitter = new VerboseEmitter();
                        break;

                    case "--emit-csv": // Legacy style
                        emitter = new CsvEmitter();
                        break;

                    case "--emit":
                        switch (param)
                        {
                            case "text":
                                emitter = new TextEmitter();
                                break;

                            case "verbose":
                                emitter = new VerboseEmitter();
                                break;

                            case "csv":
                                emitter = new CsvEmitter();
                                break;

                            default:
                                badArguments = true;
                                break;
                        }
                        break;

                    // End of options, start of patterns
                    case "--":
                        options.Patterns.AddRange(arguments.Skip(i + 1));
                        i = arguments.Count;
                        break;

                    // Unknown, possibly pattern
                    default:
                        // Inspect argument
                        if (arguments[i].StartsWith("-"))
                        {
                            // Unknown argument. If this were a pattern then use '--'
                            badArguments = true;
                        }
                        else
                        {
                            // Looks like a pattern
                            options.Patterns.Add(arguments[i]);
                        }
                        break;
                }
            }

            // Print version if requested
            if (options.PrintVersion)
            {
                Console.WriteLine(version);
                return;
            }

            // Print help if desired
            if (badArguments || options.PrintHelp || options.Patterns.Count == 0)
            {
                Console.WriteLine("VersionInfo {0}", version);
                Console.WriteLine("Reports version information for files matching a pattern");
                Console.WriteLine();
                Console.WriteLine("VersionInfo [options] <patterns>");
                Console.WriteLine();
                Console.WriteLine("-h -? --help           Print help about this program");
                Console.WriteLine("-v --version           Print the version of this program");
                Console.WriteLine("-s --size[=on/off]     Include/exclude file-size");
                Console.WriteLine("-t --time[=on/off]     Include/exclude file-time");
                Console.WriteLine("-r --recurse[=on/off]  Enable/disable recurse into folders");
                Console.WriteLine("-c --crc32[=on/off]    Enable/disable file CRC32");
                Console.WriteLine("-m --md5[=on/off]      Enable/disable file MD5");
                Console.WriteLine("-s1 --sha-1[=on/off]   Enable/disable file SHA-1");
                Console.WriteLine("-s2 --sha-2[=on/off]   Enable/disable file SHA-2");
                Console.WriteLine("--summary=on           Add summary to report");
                Console.WriteLine("--summary=only         Only show summary");
                Console.WriteLine("--emit=text            Emit Text report");
                Console.WriteLine("--emit=verbose         Emit Verbose report");
                Console.WriteLine("--emit=csv             Emit CSV report");
                Console.WriteLine("--                     End of options");
                Console.WriteLine();
                Console.WriteLine("@<options-file>        Provide command line arguments from file");
                Console.WriteLine();

                // Report error
                if (badArguments)
                {
                    Console.WriteLine("Error: Unsupported options or arguments");
                    Environment.Exit(1);
                }

                // Done
                return;
            }

            // Find all files matching the patterns and sort them
            var foundFiles = options.Patterns.SelectMany(p => Find(p, options)).OrderBy(f => f.FullPath).ToList();

            // Find the common portion of the file path and build a relative name
            if (foundFiles.Count != 0)
            {
                // Start with a guess of the first files folder
                var commonDir = Path.GetDirectoryName(foundFiles[0].FullPath);

                // Analyze all other files and shrink commonDir if necessary
                foreach (var file in foundFiles)
                {
                    // While commonDir is too specific, remove trailing elements from commonDir
                    while (!file.FullPath.StartsWith(commonDir))
                    {
                        commonDir = Path.GetDirectoryName(commonDir);
                    }
                }

                // Strip the common portion from the paths
                var stripLen = string.IsNullOrEmpty(commonDir) ? 0 : commonDir.Length + 1;
                foreach (var file in foundFiles)
                {
                    file.Name = file.FullPath.Substring(stripLen);
                }
            }

            // Begin the emitter
            emitter.Begin(options);

            // Parse all the files
            var files = new List<FileInformation>();
            foreach (var file in foundFiles)
            {
                try
                {
                    FileParser.Parse(file, options);
                    files.Add(file);
                    emitter.Entry(file);
                }
                catch
                {
                    // Skip files we can't parse
                }
            }

            // Create virtual summary and populate hashes
            var summary = new FileInformation {Name = "Summary", Size = files.Count};
            if (options.CalculateCrc32) Crc32HashParser.Summary(files, summary);
            if (options.CalculateMd5) Md5HashParser.Summary(files, summary);
            if (options.CalculateSha1) Sha1HashParser.Summary(files, summary);
            if (options.CalculateSha2) Sha2HashParser.Summary(files, summary);

            // Emit all data
            emitter.End(files, summary);
        }
    }
}
