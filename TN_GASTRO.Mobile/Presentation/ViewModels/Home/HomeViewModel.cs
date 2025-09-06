using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TN_GASTRO.Mobile.Models;
using TN_GASTRO.Mobile.Services.Tables;

namespace TN_GASTRO.Mobile.Presentation.ViewModels.Home;

public sealed partial class TableItemVm : ObservableObject
{
    public Table Model { get; }

    public TableItemVm(Table model) => Model = model;

    public string Id => Model.Id;
    public string? GuestName => Model.GuestName;
    public string? AmountText => Model.Amount?.ToString("0.##");
    public string? ElapsedText => Model.Elapsed?.ToString(@"hh\:mm");

    public bool IsOccupied => Model.Status == TableStatus.Occupied;
    public bool IsTakeaway => Model.Status == TableStatus.Takeaway;
}

public sealed partial class HomeViewModel : ObservableObject
{
    private readonly ITableService _svc;

    [ObservableProperty] private string areaTitle = "Khu vực: Nhà hàng";
    [ObservableProperty] private bool isBusy;

    public ObservableCollection<TableItemVm> Tables { get; } = new();

    public HomeViewModel(ITableService svc) => _svc = svc;

    [RelayCommand]
    private async Task LoadAsync(CancellationToken ct)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Tables.Clear();

            // Lấy dữ liệu từ service tạm (Dummy) – sau này thay API
            var data = await _svc.GetTablesAsync("restaurant", ct);

            foreach (var t in data.Select(x => new TableItemVm(x)))
                Tables.Add(t);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private Task TableClickAsync(TableItemVm? item, CancellationToken ct)
    {
        if (item is null) return Task.CompletedTask;

        System.Diagnostics.Debug.WriteLine(
            $"Click table {item.Id} - Occupied: {item.IsOccupied} - Takeaway: {item.IsTakeaway}");

        // TODO: Điều hướng tới chi tiết bàn khi có yêu cầu
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenAreaPickerAsync(CancellationToken ct)
    {
        // TODO: chọn khu vực
        System.Diagnostics.Debug.WriteLine("Area bar clicked");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenMenuAsync(CancellationToken ct)
    {
        // TODO: menu trái
        System.Diagnostics.Debug.WriteLine("Menu clicked");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenNotificationsAsync(CancellationToken ct)
    {
        // TODO: thông báo
        System.Diagnostics.Debug.WriteLine("Bell clicked");
        return Task.CompletedTask;
    }
}
