using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PluginDetectorForSc4pac.Services {
    public static class ConfigManager {
        private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PluginDetectorForSc4pac", "config.json");

        public static UserConfig Load() {
            if (File.Exists(ConfigPath)) {
                var json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<UserConfig>(json) ?? new UserConfig();
            }
            return new UserConfig();
        }

        public static void Save(UserConfig config) {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }
    }


    public class UserConfig {
        public string PluginsPath { get; set; } = string.Empty;
        public DateTime LastScanned { get; set; }
        public string PluginsChecksum { get; set; } = string.Empty;
    }
}
