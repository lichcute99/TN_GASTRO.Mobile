using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace TN_GASTRO.Mobile.Presentation.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var flag = value is bool b && b;
            if (Invert) flag = !flag;
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => (value is Visibility v && v == Visibility.Visible) ^ Invert;
    }
}
