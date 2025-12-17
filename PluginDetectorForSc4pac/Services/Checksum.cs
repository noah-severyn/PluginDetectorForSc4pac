using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PluginDetectorForSc4pac.Services {
    public static class Checksum {
        /// <summary>
        /// Computes a SHA-256 hash representing the contents and metadata of all files and directories within the specified folder and its subdirectories.
        /// </summary>
        /// <remarks>The method processes all files and directories recursively, including their metadata
        /// (size and last modified time), and computes a hash based on this information. The order of entries is
        /// normalized to ensure consistent results.</remarks>
        /// <param name="folderPath">The path to the folder whose contents will be hashed. The folder must exist and be accessible.</param>
        /// <returns>A hexadecimal string representing the SHA-256 hash of the folder's contents and metadata.  The hash is based
        /// on the full path, size, and last modified time of each file and directory.</returns>
        public static string? Compute(string folderPath) {
            var items = Directory.EnumerateFileSystemEntries(folderPath, "*", SearchOption.AllDirectories)
                .OrderBy(e => e);
            //.Select(e => {
            //    var info = new FileInfo(e);
            //    return $"{e}|{info.Length}|{info.LastWriteTimeUtc}";
            //});

            var bytes = Encoding.UTF8.GetBytes(string.Join("\n", items));
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
