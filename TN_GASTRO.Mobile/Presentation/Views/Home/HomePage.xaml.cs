using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading;
using TN_GASTRO.Mobile.Presentation.ViewModels.Home;
using TN_GASTRO.Mobile.Services.Tables;

namespace TN_GASTRO.Mobile.Presentation.Views.Home;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel { get; }

    public HomePage()
    {
        InitializeComponent();

        // Tạo service tạm (sau này DI/Api thay thế)
        ViewModel = new HomeViewModel(new DummyTableService());
        DataContext = this;

        Loaded += async (_, __) =>
        {
            using var cts = new CancellationTokenSource();
            await ViewModel.LoadCommand.ExecuteAsync(cts.Token);
        };
    }

    private async void Menu_Click(object sender, RoutedEventArgs e)
        => await ViewModel.OpenMenuCommand.ExecuteAsync(default);

    private async void Bell_Click(object sender, RoutedEventArgs e)
        => await ViewModel.OpenNotificationsCommand.ExecuteAsync(default);

    private async void Area_Click(object sender, RoutedEventArgs e)
        => await ViewModel.OpenAreaPickerCommand.ExecuteAsync(default);
}
