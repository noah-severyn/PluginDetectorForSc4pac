using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using PluginDetectorForSc4pac.Services;

namespace PluginDetectorForSc4pac.ViewModels {
    public class DesignMainViewModel : MainViewModel {

        // Parameterless constructor for design-time
        public DesignMainViewModel() : base(
            new DesignFolderPickerService(),
            new DesignDbpfService(),
            new DesignUserConfig()) {

            PluginsFolder = @"C:\Users\Administrator\Documents\SimCity 4\Plugins";
            FileCount = 200;
            FilesScanned = 150;
            Results = [
                new PackageGroup(null, "b62:safeway-60s-retro-grocery", [
                    "\\075-my-plugins\\B62-Safeway 60's Retro\\B62-CS$$_Safeway Retro 1960s v 2.0-0x6534284a-0xd3a3e650-0x5639d97a.SC4Desc",
                    "\\075-my-plugins\\B62-Safeway 60's Retro\\CS$$2_6x6_B62-CS$$_Safeway Retro 1960s v 2.0_8639d990.SC4Lot",
                    "\\075-my-plugins\\B62-Safeway 60's Retro\\PLOP_6x6_B62-PLOP_Safeway Retro 1960s v 2.0_1639da1b.SC4Lot"])
            ];
        }

        // Mock services for design-time only
        private class DesignFolderPickerService : IFolderPickerService {
            public Task<string?> PickFolderAsync() => Task.FromResult<string?>(null);

            Task<IStorageFolder?> IFolderPickerService.PickFolderAsync() {
                throw new NotImplementedException();
            }
        }

        private class DesignDbpfService : IDbpfService {
            // Implement required interface members with no-op or mock behavior
            // Add the methods your IDbpfService interface requires
            public Task<IEnumerable<ScanResult>> ScanAsync(string folder, IProgress<ScanProgress>? progress = null) {
                throw new NotImplementedException();
            }
        }

        private class DesignUserConfig : UserConfig {
            public DesignUserConfig() : base() {
                // Initialize with default/mock values if needed
            }
        }
    }
}