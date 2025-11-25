using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace EkandidatoApp
{
    public class BoolToPickText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is bool b && b) ? "Picked" : "Pick";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}