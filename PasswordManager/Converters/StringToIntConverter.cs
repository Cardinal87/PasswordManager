using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Converters
{
    internal class StringToIntConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (targetType == typeof(int?))
            {
                if (value is string || value is null)
                {
                    var str = value as string;
                    if (String.IsNullOrEmpty(str)) 
                        return null;
                    else
                        return int.Parse(str);
                }
                else throw new ArgumentException("value must be string or null");
            }
            else throw new InvalidOperationException("The target must be a int");
        }
    }
}
