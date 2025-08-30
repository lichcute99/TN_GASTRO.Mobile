using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;               // RelayCommand, AsyncRelayCommand
using Microsoft.UI;                               // Colors
using Microsoft.UI.Xaml.Media;                    // Brush, SolidColorBrush
using TN_GASTRO.Mobile.Services.Auth;
using TN_GASTRO.Mobile.Services.Device;

namespace TN_GASTRO.Mobile.Presentation.ViewModels.Login
{
    public class LoginModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IDeviceRegistry _reg = new LocalSettingsDeviceRegistry();
        private readonly IAuthService _auth = new DummyAuthService();

        private string _password = string.Empty;
        private string _deviceName = string.Empty;
        private string _lang = "vi";

        public LoginModel()
        {
            LoadDeviceInfo();

            TapDigitCommand = new RelayCommand<string>(TapDigit);
            BackspaceCommand = new RelayCommand(Backspace);
            LoginCommand = new AsyncRelayCommand(DoLoginAsync, CanLoginFunc);
            OpenRegisterDialogRequested = new RelayCommand(() => ShowRegisterDialog?.Invoke());
            OpenLanguageDialogRequested = new RelayCommand(() => ShowLanguageDialog?.Invoke());

            UpdateLocalizedTexts();
        }

        // ========= Callbacks cho Page (vì VM không có XamlRoot) =========
        public Action? ShowRegisterDialog { get; set; }
        public Action? ShowLanguageDialog { get; set; }
        public Action<string, string>? ShowAlert { get; set; }  // (title, message)

        // ========= Header =========
        public string AppVersion => "V.25454";
        public string DeviceNameDisplay => _reg.IsRegistered() ? _deviceName : string.Empty;
        public Brush DeviceIndicatorBrush =>
            _reg.IsRegistered() ? new SolidColorBrush(Colors.LawnGreen)
                                : new SolidColorBrush(Colors.Transparent);

        // ========= UI text =========
        public string MaskedPassword => new string('*', _password.Length);
        public string LoginText { get; private set; } = "LOGIN";

        // ========= Commands =========
        public IRelayCommand<string> TapDigitCommand { get; }
        public IRelayCommand BackspaceCommand { get; }
        public IAsyncRelayCommand LoginCommand { get; }
        public IRelayCommand OpenRegisterDialogRequested { get; }
        public IRelayCommand OpenLanguageDialogRequested { get; }

        // ========= Public helpers (Page sẽ gọi) =========
        public void SetLanguage(string lang)
        {
            _lang = lang;
            _reg.SaveLanguage(lang);
            UpdateLocalizedTexts();
        }

        public void SaveRegisteredDevice(string name, string ip)
        {
            _reg.SaveRegistration(name, ip);
            _deviceName = name;
            OnPropertyChanged(nameof(DeviceNameDisplay));
            OnPropertyChanged(nameof(DeviceIndicatorBrush));
        }

        // ========= Private logic =========
        private void LoadDeviceInfo()
        {
            _deviceName = _reg.GetDeviceName() ?? string.Empty;
            _lang = _reg.GetLanguage() ?? "vi";
            OnPropertyChanged(nameof(DeviceNameDisplay));
            OnPropertyChanged(nameof(DeviceIndicatorBrush));
        }

        private void TapDigit(string? d)
        {
            if (string.IsNullOrEmpty(d) || !d.All(char.IsDigit)) return;
            if (_password.Length >= 10) return;

            _password += d;
            OnPropertyChanged(nameof(MaskedPassword));
            LoginCommand.NotifyCanExecuteChanged();
        }

        private void Backspace()
        {
            if (_password.Length == 0) return;
            _password = _password[..^1];
            OnPropertyChanged(nameof(MaskedPassword));
            LoginCommand.NotifyCanExecuteChanged();
        }

        private bool CanLoginFunc() => _password.Length > 0 && _password.Length <= 10;

        private async Task DoLoginAsync()
        {
            if (!_reg.IsRegistered())
            {
                ShowRegisterDialog?.Invoke();
                return;
            }

            var ok = await _auth.ValidateAsync(_deviceName, _password);
            if (!ok)
            {
                ShowAlert?.Invoke("Lỗi", "Sai mật khẩu.");
                return;
            }

            _password = string.Empty;
            OnPropertyChanged(nameof(MaskedPassword));
            ShowAlert?.Invoke("OK", "Đăng nhập thành công");
            // TODO: điều hướng sang Home khi bạn gửi UI sau đăng nhập
        }

        private void UpdateLocalizedTexts()
        {
            LoginText = _lang switch
            {
                "de" => "ANMELDEN",
                "en" => "LOGIN",
                _ => "LOGIN" // vi
            };
            OnPropertyChanged(nameof(LoginText));
        }

        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
