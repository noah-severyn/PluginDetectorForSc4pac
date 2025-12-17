using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace PluginDetectorForSc4pac.Services {
    internal class FolderPickerService(Window w) : IFolderPickerService {

        private readonly Window _window = w;

        public async Task<IStorageFolder?> PickFolderAsync() {
            var storage = _window.StorageProvider;
            if (!storage.CanPickFolder) {
                return null;
            }

            var folder = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions {
                Title = "Select plugins folder",
                AllowMultiple = false
            });

            return folder?.FirstOrDefault();
        }
    }
}
