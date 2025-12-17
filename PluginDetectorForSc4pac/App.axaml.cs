using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using PluginDetectorForSc4pac.Services;
using PluginDetectorForSc4pac.ViewModels;
using PluginDetectorForSc4pac.Views;

namespace PluginDetectorForSc4pac;

public partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        var config = ConfigManager.Load();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            var mw = new MainWindow();
            var fps = new FolderPickerService(mw);
            var dbpf = new DbpfService();
            DataContext = new MainViewModel(fps, dbpf, config);
            mw.DataContext = new MainViewModel(fps, dbpf, config);
            desktop.MainWindow = mw;
            desktop.Exit += (sender, e) => {
                ConfigManager.Save(config); // Hook into Exit event to save config
            };

        } else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
            var mw = new MainWindow();
            var fps = new FolderPickerService(mw);
            var dbpf = new DbpfService();
            DataContext = new MainViewModel(fps, dbpf, config);
            mw.DataContext = new MainViewModel(fps, dbpf, config);
            singleViewPlatform.MainView = mw;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
