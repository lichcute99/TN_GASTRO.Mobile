using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Globalization;
using Windows.Storage;

namespace TN_GASTRO.Mobile.Presentation.Views.Login
{
    public sealed partial class LoginPage : Page
    {
        private string _pin = string.Empty;

        public LoginPage()
        {
            // Áp dụng ngôn ngữ đã lưu TRƯỚC khi load XAML
            var saved = (ApplicationData.Current.LocalSettings.Values["lang"] as string) ?? "vi";
            ApplicationLanguages.PrimaryLanguageOverride = saved;

            InitializeComponent();

            CheckRadio(saved);
            UpdateDisplay();
        }

        private void CheckRadio(string code)
        {
            if (rbVi is not null) rbVi.IsChecked = code == "vi";
            if (rbEn is not null) rbEn.IsChecked = code == "en";
            if (rbDe is not null) rbDe.IsChecked = code == "de";
        }

        // Không giới hạn số ký tự
        private void OnNumClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Content is string s && !string.IsNullOrEmpty(s))
            {
                _pin += s;
                UpdateDisplay();
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (_pin.Length > 0)
            {
                _pin = _pin[..^1];
                UpdateDisplay();
            }
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            // TODO: Validate _pin / điều hướng
            System.Diagnostics.Debug.WriteLine($"Login PIN length = {_pin.Length}");
        }

        private void UpdateDisplay()
        {
            if (PinDisplay is null) return;
            PinDisplay.Text = new string('*', _pin.Length);
        }

        // Đổi ngôn ngữ từ popup
        private void OnLanguageRadioChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag is string code && !string.IsNullOrWhiteSpace(code))
            {
                // Lưu + áp dụng
                ApplicationData.Current.LocalSettings.Values["lang"] = code;
                ApplicationLanguages.PrimaryLanguageOverride = code;

                // Đóng popup
                (BtnLang?.Flyout as Flyout)?.Hide();

                // Reload lại chính trang này để strings x:Uid được áp dụng
                ReloadPage();
            }
        }

        private void ReloadPage()
        {
            var frame = this.Frame;

            // Nếu trang đang nằm trong Frame thì refresh chính nó
            if (frame is not null)
            {
                frame.Navigate(typeof(LoginPage));
                if (frame.CanGoBack)
                    frame.BackStack.RemoveAt(frame.BackStackDepth - 1);
            }
            else
            {
                // Fallback: thử lấy Frame ở Window hiện tại (nếu có)
                if (Microsoft.UI.Xaml.Window.Current?.Content is Frame rootFrame)
                {
                    rootFrame.Navigate(typeof(LoginPage));
                    if (rootFrame.CanGoBack)
                        rootFrame.BackStack.RemoveAt(rootFrame.BackStackDepth - 1);
                }
            }
        }
    }
}
