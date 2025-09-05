using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using TN_GASTRO.Mobile.Services.Tables;

namespace TN_GASTRO.Mobile.Presentation.ViewModels.Home;

public sealed partial class TableItemVm : ObservableObject
{
    public string Id { get; }
    public TableStatus Status { get; }
    public string? GuestName { get; }
    public string? AmountText { get; }
    public string? ElapsedText { get; }

    public bool IsEmpty => Status == TableStatus.Empty;

    public TableItemVm(Table t)
    {
        Id = t.Id;
        Status = t.Status;
        GuestName = t.GuestName;
        AmountText = t.Amount.HasValue ? t.Amount.Value.ToString("0.##") : null;
        if (t.Elapsed is TimeSpan el)
            ElapsedText = $"{(int)el.TotalHours:00}:{el.Minutes:00}";
    }
}

public sealed partial class HomeViewModel : ObservableObject
{
    private readonly ITableService _svc;

    [ObservableProperty] private string areaTitle = "Khu vực: Nhà hàng";
    [ObservableProperty] private bool isBusy;

    public ObservableCollection<TableItemVm> Tables { get; } = new();

    public HomeViewModel(ITableService svc)
    {
        _svc = svc;
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken ct)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Tables.Clear();
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

        // TODO: điều hướng sang trang chi tiết bàn, hoặc mở popup…
        System.Diagnostics.Debug.WriteLine($"Click table {item.Id} - Status: {item.Status}");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task GroupTablesAsync(CancellationToken ct)
    {
        // TODO: xử lý gộp bàn sau này
        System.Diagnostics.Debug.WriteLine("Group tables (+) clicked");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenAreaPickerAsync(CancellationToken ct)
    {
        // TODO: chọn khu vực (nhà hàng/sân vườn/tầng…)
        System.Diagnostics.Debug.WriteLine("Area bar clicked");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenMenuAsync(CancellationToken ct)
    {
        // TODO: mở menu dạng grid (icon trái)
        System.Diagnostics.Debug.WriteLine("Menu icon clicked");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenNotificationsAsync(CancellationToken ct)
    {
        // TODO: mở bell notifications
        System.Diagnostics.Debug.WriteLine("Bell icon clicked");
        return Task.CompletedTask;
    }
}
