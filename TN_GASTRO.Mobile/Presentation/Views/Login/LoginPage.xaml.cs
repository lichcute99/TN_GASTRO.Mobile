//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Windows.Globalization;
//using Windows.Storage; // ApplicationData

//namespace TN_GASTRO.Mobile.Presentation.Views.Login
//{
//    public sealed partial class LoginPage : Page
//    {
//        private string _pin = string.Empty;

//        public LoginPage()
//        {
//            InitializeComponent();
//            UpdateDisplay();
//        }

//        // ====== PIN keypad ======

//        private void OnNumClick(object sender, RoutedEventArgs e)
//        {
//            if (sender is Button b && b.Content is string s)
//            {
//                _pin += s;                  // không giới hạn độ dài
//                UpdateDisplay();
//            }
//        }

//        private void OnDeleteClick(object sender, RoutedEventArgs e)
//        {
//            if (_pin.Length > 0)
//            {
//                _pin = _pin[..^1];
//                UpdateDisplay();
//            }
//        }

//        private void OnLoginClick(object sender, RoutedEventArgs e)
//        {
//            // TODO: validate _pin / điều hướng
//        }

//        private void UpdateDisplay()
//        {
//            // hiện số lượng * tương ứng độ dài PIN (không giới hạn)
//            PinDisplay.Text = new string('*', _pin.Length);
//        }

//        // ====== Language dialog (ở giữa màn hình) ======

//        private async void OnLanguageClick(object sender, RoutedEventArgs e)
//        {
//            await ShowLanguageDialogAsync();
//        }

//        private async Task ShowLanguageDialogAsync()
//        {
//            var vi = new RadioButton { Content = "Tiếng Việt", Tag = "vi", Margin = new Thickness(0, 6, 0, 6) };
//            var en = new RadioButton { Content = "English", Tag = "en", Margin = new Thickness(0, 6, 0, 6) };
//            var de = new RadioButton { Content = "Deutsch", Tag = "de", Margin = new Thickness(0, 6, 0, 6) };

//            // lấy ngôn ngữ đang lưu (nếu có)
//#pragma warning disable CA1416
//            var current = (ApplicationData.Current.LocalSettings.Values["lang"] as string) ?? "vi";
//#pragma warning restore CA1416
//            (current switch { "en" => en, "de" => de, _ => vi }).IsChecked = true;

//            var content = new StackPanel { Spacing = 8 };
//            content.Children.Add(vi);
//            content.Children.Add(en);
//            content.Children.Add(de);

//            var dlg = new ContentDialog
//            {
//                XamlRoot = this.XamlRoot,
//                Title = "Chọn ngôn ngữ",
//                PrimaryButtonText = "Áp dụng",
//                CloseButtonText = "Hủy",
//                DefaultButton = ContentDialogButton.Primary,
//                Content = new Grid { Width = 300, Children = { content } } // canh giữa mặc định
//            };

//            var res = await dlg.ShowAsync();
//            if (res != ContentDialogResult.Primary) return;

//            var chosen = new[] { vi, en, de }
//                .FirstOrDefault(x => x.IsChecked == true)?.Tag?.ToString() ?? "vi";

//#pragma warning disable CA1416
//            ApplicationData.Current.LocalSettings.Values["lang"] = chosen;
//#pragma warning restore CA1416
//            ApplicationLanguages.PrimaryLanguageOverride = chosen;

//            // reload trang để áp dụng x:Uid
//            Frame?.Navigate(typeof(LoginPage));
//        }
//    }
//}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TN_GASTRO.Mobile.lib.database;
using TN_GASTRO.Mobile.Services.UserServices;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Storage;
using Uno.Extensions.Navigation;                        // <-- thêm
using TN_GASTRO.Mobile.Presentation.Views.Home;

namespace TN_GASTRO.Mobile.Presentation.Views.Login
{
    public sealed partial class LoginPage : Page
    {
        private readonly ResourceLoader _resources = ResourceLoader.GetForCurrentView();
        private string _pin = string.Empty;

        public LoginPage()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        // ====== PIN keypad ======

        private void OnNumClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Content is string s)
            {
                _pin += s; // Không giới hạn độ dài
                UpdateDisplay();
            }
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(_pin))
            {
                await ShowMessageAsync(_resources.GetString("Language_Vietnamese"));
                return;
            }

            try
            {
                var db = await DatabaseHelper.Instance.GetDatabaseAsync();
                var userService = new UserService(db);

                var user = await userService.LoginAsync(_pin);
                if (user != null)
                {
                    NavigateToHome();      // 👈 ĐIỀU HƯỚNG TẠI ĐÂY
                    return;
                }
                else
                {
                    await ShowMessageAsync("PIN không hợp lệ, vui lòng thử lại");
                    _pin = string.Empty;
                    UpdateDisplay();
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Lỗi khi đăng nhập: " + ex.Message);
            }

        }

        private void NavigateToHome()
        {
            // 1) Nếu page đang ở trong Frame, dùng ngay
            if (this.Frame is Frame f)
            {
                f.Navigate(typeof(HomePage));
                f.BackStack.Clear(); // không cho back về Login
                return;
            }

            // 2) Nếu dùng Shell với RootFrame
            var app = Application.Current as App;
            if (app?.MainWindow?.Content is Shell sh && sh.Content is Frame rf)
            {
                rf.Navigate(typeof(HomePage));
                rf.BackStack.Clear();
                return;
            }

            // 3) Phương án dự phòng: thay thẳng Window.Content
            (Application.Current as App)!.MainWindow!.Content = new HomePage();
        }

        private async Task ShowMessageAsync(string message)
        {
            var dlg = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Thông báo",
                Content = message,
                CloseButtonText = "OK"
            };

            await dlg.ShowAsync();
        }



        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (_pin.Length > 0)
            {
                _pin = _pin[..^1];
                UpdateDisplay();
            }
        }



        private void UpdateDisplay()
        {
            // Hiển thị đúng số lượng * theo _pin
            PinDisplay.Text = new string('*', _pin.Length);
        }

        // ====== Language dialog (ở giữa màn hình) ======

        private async void OnLanguageClick(object sender, RoutedEventArgs e)
        {
            await ShowLanguageDialogAsync();
        }

        private async Task ShowLanguageDialogAsync()
        {
            var vi = new RadioButton { Content = "Tiếng Việt", Tag = "vi", Margin = new Thickness(0, 6, 0, 6) };
            var en = new RadioButton { Content = "English", Tag = "en", Margin = new Thickness(0, 6, 0, 6) };
            var de = new RadioButton { Content = "Deutsch", Tag = "de", Margin = new Thickness(0, 6, 0, 6) };

            // Lấy ngôn ngữ đang lưu (nếu có)
#pragma warning disable CA1416
            var current = (ApplicationData.Current.LocalSettings.Values["lang"] as string) ?? "vi";
#pragma warning restore CA1416
            (current switch { "en" => en, "de" => de, _ => vi }).IsChecked = true;

            var content = new StackPanel { Spacing = 8 };
            content.Children.Add(vi);
            content.Children.Add(en);
            content.Children.Add(de);

            var dlg = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Chọn ngôn ngữ",
                PrimaryButtonText = "Áp dụng",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                // Để dialog gọn và luôn nằm giữa
                Content = new Grid { Width = 300, Children = { content } }
            };

            var res = await dlg.ShowAsync();
            if (res != ContentDialogResult.Primary) return;

            var chosen = new[] { vi, en, de }
                .FirstOrDefault(x => x.IsChecked == true)?.Tag?.ToString() ?? "vi";

#pragma warning disable CA1416
            ApplicationData.Current.LocalSettings.Values["lang"] = chosen;
#pragma warning restore CA1416

            ApplicationLanguages.PrimaryLanguageOverride = chosen;

            // Reload trang để áp dụng text từ x:Uid (Resources.resw)
            Frame?.Navigate(typeof(LoginPage));
        }
    }
}

