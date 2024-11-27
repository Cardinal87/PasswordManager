using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Converters
{
    class BoolToImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var dict = App.Current?.Resources;
            if (dict != null)
            {
                if (value is bool b && b)
                    return dict["favourite_icon"];
                else return dict["favourite_off_icon"];
            }
            return dict["favourite_off_icon"];
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
