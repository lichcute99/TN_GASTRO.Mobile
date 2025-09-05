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
using TN_GASTRO.Mobile.Presentation.Views.Login;

namespace TN_GASTRO.Mobile;

public partial class App : Application
{
    public App() => InitializeComponent();

    private Window? _window;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new Window();

        var frame = new Frame();
        frame.Navigate(typeof(LoginPage));   // hoặc typeof(Presentation.Shell) khi Shell ổn
        _window.Content = frame;

        _window.Activate();
    }
}

