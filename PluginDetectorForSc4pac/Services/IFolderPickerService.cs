using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace PluginDetectorForSc4pac.Services {
    public interface IFolderPickerService {
        Task<IStorageFolder?> PickFolderAsync();
    }
}
