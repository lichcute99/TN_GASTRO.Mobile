//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Input;
//using TN_GASTRO.Mobile.Presentation.ViewModels.Login;
//using TN_GASTRO.Mobile.Services.Device;
//using TN_GASTRO.Mobile.Services.Registration;

//namespace TN_GASTRO.Mobile.Presentation.Views.Login;

//public sealed partial class LoginPage : Page
//{
//    // ✅ Property cho x:Bind
//    public LoginModel ViewModel { get; } 

//    private readonly IRegistrationService _regService = new DummyRegistrationService();
//    private readonly IDeviceRegistry _device = new LocalSettingsDeviceRegistry();

//    public LoginPage()
//    {
//        InitializeComponent();
//        ViewModel = new LoginModel();
//        // Nếu bạn vẫn dùng Binding thông thường trong XAML, DataContext sẽ dùng VM này
//        DataContext = ViewModel;

//        // Gắn callback UI cho VM (nếu bạn dùng)
//        ViewModel.ShowRegisterDialog = async () => await ShowRegisterDialogAsync();
//        ViewModel.ShowLanguageDialog = async () => await ShowLanguageDialogAsync();
//        ViewModel.ShowAlert = async (t, m) =>
//            await new ContentDialog
//            {
//                XamlRoot = this.XamlRoot,
//                Title = t,
//                Content = m,
//                CloseButtonText = "OK"
//            }.ShowAsync();

//        // Chỉ mở popup sau khi trang đã Loaded (tránh treo splash)
//        Loaded += async (_, __) =>
//        {
//            if (!_device.IsRegistered())
//                await ShowRegisterDialogAsync();
//        };
//    }
//    private async void Register_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
//    {
//        await ShowRegisterDialogAsync();
//    }

//    // mở popup Chọn ngôn ngữ
//    private async void Language_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
//    {
//        await ShowLanguageDialogAsync();
//    }
//    // ========= các hàm ShowRegisterDialogAsync / ShowLanguageDialogAsync =========
//    private async Task ShowRegisterDialogAsync()
//    {
//        var stack = new StackPanel { Spacing = 8 };

//        var pwd = new PasswordBox { PlaceholderText = "Nhập mật khẩu" };
//        var ip = new TextBox { PlaceholderText = "IP" };
//        var name = new TextBox { PlaceholderText = "Tên thiết bị (chỉ số)" };
//        name.InputScope = new InputScope { Names = { new InputScopeName(InputScopeNameValue.Number) } };

//        stack.Children.Add(pwd);
//        stack.Children.Add(ip);
//        stack.Children.Add(name);

//        var dlg = new ContentDialog
//        {
//            XamlRoot = this.XamlRoot,
//            Title = "Đăng ký thiết bị",
//            PrimaryButtonText = "Đăng ký",
//            CloseButtonText = "Thoát",
//            DefaultButton = ContentDialogButton.Primary,
//            Content = stack
//        };

//        var res = await dlg.ShowAsync();
//        if (res != ContentDialogResult.Primary) return;

//        var ok = await _regService.ValidateAsync(ip.Text?.Trim() ?? "", pwd.Password ?? "");
//        if (!ok)
//        {
//            await new ContentDialog
//            {
//                XamlRoot = this.XamlRoot,
//                Title = "Sai thông tin đăng ký",
//                CloseButtonText = "Đóng",
//                Content = "Mật khẩu hoặc IP không hợp lệ."
//            }.ShowAsync();
//            return;
//        }

//        var deviceName = (name.Text ?? "").Trim();
//        if (string.IsNullOrEmpty(deviceName) || !deviceName.All(char.IsDigit))
//        {
//            await new ContentDialog
//            {
//                XamlRoot = this.XamlRoot,
//                Title = "Tên thiết bị không hợp lệ",
//                CloseButtonText = "Đóng",
//                Content = "Vui lòng nhập số (ví dụ: 1, 2, 101…)."
//            }.ShowAsync();
//            return;
//        }

//        ViewModel.SaveRegisteredDevice(deviceName, ip.Text!.Trim());

//        await new ContentDialog
//        {
//            XamlRoot = this.XamlRoot,
//            Title = "Thành công",
//            CloseButtonText = "OK",
//            Content = "Thiết bị đã được đăng ký. Từ lần sau chỉ cần nhập mật khẩu."
//        }.ShowAsync();
//    }

//    private async Task ShowLanguageDialogAsync()
//    {
//        var opts = new StackPanel { Spacing = 8 };
//        var vi = new RadioButton { Content = "Vietnamese", Tag = "vi" };
//        var en = new RadioButton { Content = "English", Tag = "en" };
//        var de = new RadioButton { Content = "German", Tag = "de" };
//        opts.Children.Add(vi); opts.Children.Add(en); opts.Children.Add(de);

//        var current = new LocalSettingsDeviceRegistry().GetLanguage() ?? "vi";
//        (current switch { "en" => en, "de" => de, _ => vi }).IsChecked = true;

//        var dlg = new ContentDialog
//        {
//            XamlRoot = this.XamlRoot,
//            Title = "Ngôn ngữ",
//            PrimaryButtonText = "Chọn",
//            CloseButtonText = "Đóng",
//            DefaultButton = ContentDialogButton.Primary,
//            Content = opts
//        };

//        var res = await dlg.ShowAsync();
//        if (res == ContentDialogResult.Primary)
//        {
//            var chosen = new[] { vi, en, de }.FirstOrDefault(x => x.IsChecked == true)?.Tag?.ToString() ?? "vi";
//            ViewModel.SetLanguage(chosen);
//        }
//    }
//}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using TN_GASTRO.Mobile.Presentation.ViewModels.Login;
using TN_GASTRO.Mobile.Services.Device;
using TN_GASTRO.Mobile.Services.Registration;

namespace TN_GASTRO.Mobile.Presentation.Views.Login
{
    public sealed partial class LoginPage : Page
    {
        public LoginModel ViewModel { get; }

        private readonly IRegistrationService _regService = new DummyRegistrationService();
        private readonly IDeviceRegistry _device = new LocalSettingsDeviceRegistry();

        public LoginPage()
        {
            InitializeComponent();

            // VM + DataContext
            ViewModel = new LoginModel();
            DataContext = ViewModel;

            // Bridge: VM yêu cầu Page mở dialog
            ViewModel.ShowAlert = async (t, m) => await new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = t,
                Content = m,
                CloseButtonText = "OK"
            }.ShowAsync();

            ViewModel.ShowLanguageDialog = async () => await ShowLanguageDialogAsync();
            ViewModel.ShowRegisterDialog = async () => await ShowRegisterDialogAsync();
        }

        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Lần đầu nếu chưa đăng ký thiết bị thì ép mở dialog đăng ký
            if (!_device.IsRegistered())
            {
                await ShowRegisterDialogAsync();
            }
        }

        private async Task ShowRegisterDialogAsync()
        {
            var stack = new StackPanel { Spacing = 8 };

            var pwd = new PasswordBox { PlaceholderText = "Nhập mật khẩu" };
            var ip = new TextBox { PlaceholderText = "IP" };
            var name = new TextBox { PlaceholderText = "Tên thiết bị (chỉ số)" };
            name.InputScope = new InputScope { Names = { new InputScopeName(InputScopeNameValue.Number) } };

            stack.Children.Add(pwd);
            stack.Children.Add(ip);
            stack.Children.Add(name);

            var dlg = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Đăng ký thiết bị",
                PrimaryButtonText = "Đăng ký",
                CloseButtonText = "Thoát",
                DefaultButton = ContentDialogButton.Primary,
                Content = stack
            };

            var res = await dlg.ShowAsync();
            if (res != ContentDialogResult.Primary) return;

            // validate
            var ok = await _regService.ValidateAsync(ip.Text?.Trim() ?? "", pwd.Password ?? "");
            if (!ok)
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Sai thông tin đăng ký",
                    CloseButtonText = "Đóng",
                    Content = "Mật khẩu hoặc IP không hợp lệ."
                }.ShowAsync();
                return;
            }

            var deviceName = (name.Text ?? "").Trim();
            if (string.IsNullOrEmpty(deviceName) || !deviceName.All(char.IsDigit))
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Tên thiết bị không hợp lệ",
                    CloseButtonText = "Đóng",
                    Content = "Vui lòng nhập số (ví dụ: 1, 2, 101…)."
                }.ShowAsync();
                return;
            }

            // lưu
            ViewModel.SaveRegisteredDevice(deviceName, ip.Text!.Trim());

            await new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Thành công",
                CloseButtonText = "OK",
                Content = "Thiết bị đã được đăng ký. Từ lần sau chỉ cần nhập mật khẩu."
            }.ShowAsync();
        }

        private async Task ShowLanguageDialogAsync()
        {
            var opts = new StackPanel { Spacing = 8 };
            var vi = new RadioButton { Content = "Vietnamese", Tag = "vi" };
            var en = new RadioButton { Content = "English", Tag = "en" };
            var de = new RadioButton { Content = "German", Tag = "de" };
            opts.Children.Add(vi); opts.Children.Add(en); opts.Children.Add(de);

            var current = new LocalSettingsDeviceRegistry().GetLanguage() ?? "vi";
            (current switch { "en" => en, "de" => de, _ => vi }).IsChecked = true;

            var dlg = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Ngôn ngữ",
                PrimaryButtonText = "Chọn",
                CloseButtonText = "Đóng",
                DefaultButton = ContentDialogButton.Primary,
                Content = opts
            };

            var res = await dlg.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                var chosen = new[] { vi, en, de }.FirstOrDefault(x => x.IsChecked == true)?.Tag?.ToString() ?? "vi";
                ViewModel.SetLanguage(chosen);
            }
        }
    }
}
