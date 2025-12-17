using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginDetectorForSc4pac.ViewModels;

namespace PluginDetectorForSc4pac.Services {
    public interface IDbpfService {
        Task<IEnumerable<ScanResult>> ScanAsync(string folder, IProgress<ScanProgress>? progress = null);
    }

    //public record class ScanResult(string PacPath, string NonPacPath, string Tgi);
    public record class ScanResult(string Path, string FileName, string Package);


    public record struct ScanProgress(int FilesScanned, int TotalFiles);
}
