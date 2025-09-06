//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.Extensions.Logging;
//using Uno.Extensions;
//using Uno.Extensions.Hosting;
//using Uno.Extensions.Navigation.Toolkit;

//namespace TN_GASTRO.Mobile;

//public partial class App : Application
//{
//    public App() => InitializeComponent();

//    protected Window? MainWindow { get; private set; }

//    protected override void OnLaunched(LaunchActivatedEventArgs args)
//    {
//        var builder = this.CreateBuilder(args)
//            .UseToolkitNavigation()
//            .Configure(host => host
//#if DEBUG
//                .UseEnvironment(Environments.Development)
//#endif
//                .UseLogging((ctx, log) =>
//                {
//                    log.SetMinimumLevel(
//                        ctx.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning)
//                       .CoreLogLevel(LogLevel.Warning);
//                }, enableUnoLogging: true)
//            );

//        MainWindow = builder.Window;    

//        // 🔎 SMOKE TEST: hiển thị text đơn giản để chắc chắn cửa sổ hoạt động
//        MainWindow.Content = new TextBlock
//        {
//            Text = "Boot OK",
//            FontSize = 28,
//            HorizontalAlignment = HorizontalAlignment.Center,
//            VerticalAlignment = VerticalAlignment.Center
//        };

//        //MainWindow.Activate(); // 👈 thêm Activate để chắc chắn kích hoạt cửa sổ
//        MainWindow.Content = new TN_GASTRO.Mobile.Presentation.Shell();
//        MainWindow.Activate();
//    }
//}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Extensions.Hosting;
using Uno.Extensions.Navigation.Toolkit;

namespace TN_GASTRO.Mobile;

public partial class App : Application
{
    public App() => InitializeComponent();

    protected Window? MainWindow { get; private set; }

    private async Task DebugListAssetsAsync()
    {
        try
        {
            var installFolder = Package.Current.InstalledLocation;
            var assetsFolder = await installFolder.GetFolderAsync("Assets");
            var files = await assetsFolder.GetFilesAsync();

            foreach (var f in files)
            {
                System.Diagnostics.Debug.WriteLine($"[ASSET] {f.Name}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Lỗi khi duyệt Assets: " + ex.Message);
        }
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {

        _ = DebugListAssetsAsync();


        SQLitePCL.Batteries_V2.Init();

        var builder = this.CreateBuilder(args)
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging((ctx, log) =>
                {
                    log.SetMinimumLevel(
                        ctx.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning)
                       .CoreLogLevel(LogLevel.Warning);
                }, enableUnoLogging: true)
            );

        MainWindow = builder.Window;

        // 🔎 SMOKE TEST: hiển thị text đơn giản để chắc chắn cửa sổ hoạt động
        MainWindow.Content = new TextBlock
        {
            Text = "Boot OK",
            FontSize = 28,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        //MainWindow.Activate(); // 👈 thêm Activate để chắc chắn kích hoạt cửa sổ
        MainWindow.Content = new TN_GASTRO.Mobile.Presentation.Shell();
        MainWindow.Activate();
    }
}

