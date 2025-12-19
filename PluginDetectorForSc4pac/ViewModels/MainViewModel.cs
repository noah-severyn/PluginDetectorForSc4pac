using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PluginDetectorForSc4pac.Services;

namespace PluginDetectorForSc4pac.ViewModels;

public partial class MainViewModel(IFolderPickerService folderPicker, IDbpfService dbpfService, UserConfig config) : ObservableObject {

    private readonly IFolderPickerService _folderPicker = folderPicker;
    private readonly IDbpfService _dbpfService = dbpfService;
    private readonly UserConfig _config = config;

    /// <summary>
    /// Gets or sets the path to the folder containing plugins.
    /// </summary>
    /// <remarks>Changes to this property will trigger updates to the execution state of the <see cref="ScanPluginsCommand"/> and <see cref="OpenFolderCommand"/> commands.</remarks>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ScanPluginsCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenFolderCommand))]
    private string _pluginsFolder = config.PluginsPath;
    partial void OnPluginsFolderChanged(string value) {
        _config.PluginsPath = value;
    }

    /// <summary>
    /// Gets or sets the total number of files processed during the scan.
    /// </summary>
    /// <remarks>Changes to this property will also update <see cref="ScanProgress"/></remarks>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ScanProgress))]
    private int _fileCount;

    /// <summary>
    /// Gets or sets the number of files that have been scanned.
    /// </summary>
    /// <remarks>Changes to this property will also update <see cref="ScanProgress"/></remarks>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ScanProgress))]
    private int _filesScanned;

    [ObservableProperty]
    private string _lastScanned = "Last scan: " + config.LastScanned.ToString();
    partial void OnLastScannedChanged(string value) {
        _config.LastScanned = DateTime.Now;
    }

    public string ScanProgress => $"Scanning {FilesScanned} / {FileCount} files";



    partial void OnFilesScannedChanged(int value) {
        OnPropertyChanged(nameof(ScanProgress));
    }
    partial void OnFileCountChanged(int value) {
        OnPropertyChanged(nameof(ScanProgress));
    }

    [ObservableProperty]
    private ObservableCollection<PackageGroup> _results = [];

    [RelayCommand]
    private async Task ChooseFolder() {
        var picker = await _folderPicker.PickFolderAsync();
        PluginsFolder = picker?.Path.LocalPath ?? string.Empty;
    }

    [RelayCommand(CanExecute = nameof(PluginsFolderValid))]
    private void OpenFolder() {
        try {
            OpenUrl(PluginsFolder);
        }
        catch (Exception) {
            throw;
        }
    }

    [RelayCommand(CanExecute = nameof(PluginsFolderValid))]
    private async Task ScanPlugins() {
        LastScanned = "Last scan: " + DateTime.Now.ToString();

        var progress = new Progress<ScanProgress>(p => {
            FilesScanned = p.FilesScanned;
            FileCount = p.TotalFiles;
        });

        Results.Clear();
        var results = await _dbpfService.ScanAsync(PluginsFolder, progress);
        var grouped = results.GroupBy(i => i.Package);
        foreach (var g in grouped) {
            var filesList = g.Select(scanresult => scanresult.Path).ToList();
            for (int idx = 0; idx < filesList.Count; idx++) {
                string file = filesList[idx].Replace(PluginsFolder, string.Empty);
                if (!file.StartsWith('\\')) file = "\\" + file;
                filesList[idx] = file;
            }
            Results.Add(new PackageGroup(this, g.Key, filesList));
        }
    }

    private bool PluginsFolderValid() {
        return !string.IsNullOrEmpty(PluginsFolder) && Directory.Exists(PluginsFolder);
    }

    public partial class PackageGroup(MainViewModel viewModel, string package, List<string> files) {
        private readonly MainViewModel _mvm = viewModel;
        
        public string Package { get; } = package;
        public List<string> Files { get; } = files;

        [RelayCommand]
        public void OpenPackageInSc4pac() {
            //A package link looks like: sc4pac:///package?pkg=b62%3Asafeway-60s-retro-grocery
            string uri = "sc4pac:///package?pkg=" + HttpUtility.UrlEncode(Package);
            try {
                OpenUrl(uri);
            }
            catch (Exception) {
                throw;
            }
        }

        /// <summary>
        /// Find the lowest level folder common among all the files and open it.
        /// </summary>
        [RelayCommand]
        public void OpenParentFolder() {
            var min = Files.MinBy(p => p.Length);
            var minFldrs = min.Split(Path.DirectorySeparatorChar);
            foreach (var path in Files) {
                var fldrs = path.Split(Path.DirectorySeparatorChar);
                if (fldrs.Length < minFldrs.Length) {
                    minFldrs = fldrs;
                }
            }
            
            OpenUrl(Path.Combine(_mvm.PluginsFolder, Path.Combine(minFldrs.SkipLast(1).ToArray())));
        }
    }


    private static void OpenUrl(string url) {
        Process.Start(new ProcessStartInfo {
            FileName = url,
            UseShellExecute = true
        });
    }

}
