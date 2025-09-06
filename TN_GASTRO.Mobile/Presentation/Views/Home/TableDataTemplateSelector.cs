using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TN_GASTRO.Mobile.Presentation.ViewModels.Home;

namespace TN_GASTRO.Mobile.Presentation.Views.Home;

public sealed class TableDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? EmptyTemplate { get; set; }
    public DataTemplate? OccupiedTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is TableItemVm vm)
        {
            return vm.IsOccupied || vm.IsTakeaway
                ? (OccupiedTemplate ?? base.SelectTemplateCore(item, container))
                : (EmptyTemplate ?? base.SelectTemplateCore(item, container));
        }

        return base.SelectTemplateCore(item, container);
    }
}
