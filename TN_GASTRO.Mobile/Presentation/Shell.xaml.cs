//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using TN_GASTRO.Mobile.Presentation.Views.Login;

//namespace TN_GASTRO.Mobile.Presentation;

//public sealed partial class Shell : Page
//{
//    public Shell()
//    {
//        InitializeComponent();
//        Loaded += OnLoaded;
//    }

//    private void OnLoaded(object sender, RoutedEventArgs e)
//    {
//        // Ẩn splash ngay lập tức (để chắc chắn không che UI)
//        Splash.Visibility = Visibility.Collapsed;

//        // Điều hướng vào LoginPage
//        RootFrame.Navigate(typeof(LoginPage));
//    }
//}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TN_GASTRO.Mobile.Presentation.Views.Login;

namespace TN_GASTRO.Mobile.Presentation;

public sealed partial class Shell : UserControl
{
    public Shell()
    {
        InitializeComponent();
        Loaded += Shell_Loaded;
    }

    private void Shell_Loaded(object sender, RoutedEventArgs e)
    {
        RootFrame.Navigate(typeof(TN_GASTRO.Mobile.Presentation.Views.Login.LoginPage));
        Splash.Visibility = Visibility.Collapsed;

    }
}
