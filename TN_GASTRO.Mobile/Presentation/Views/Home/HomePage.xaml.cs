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

        // TODO: khi có DI hãy inject ITableService; tạm thời dùng Dummy
        ViewModel = new HomeViewModel(new DummyTableService());
        DataContext = ViewModel;

        Loaded += async (_, __) =>
        {
            using var cts = new CancellationTokenSource();
            await ViewModel.LoadCommand.ExecuteAsync(cts.Token);
        };
    }

    // Header actions
    private async void Menu_Click(object sender, RoutedEventArgs e)
        => await ViewModel.OpenMenuCommand.ExecuteAsync(default);

    private async void Bell_Click(object sender, RoutedEventArgs e)
        => await ViewModel.OpenNotificationsCommand.ExecuteAsync(default);

    private async void Group_Click(object sender, RoutedEventArgs e)
        => await ViewModel.GroupTablesCommand.ExecuteAsync(default);

    private async void Area_Click(object sender, RoutedEventArgs e)
        => await ViewModel.OpenAreaPickerCommand.ExecuteAsync(default);
}
