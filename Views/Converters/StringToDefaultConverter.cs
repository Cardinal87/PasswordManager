using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Views.Converters
{
    internal class StringToDefaultConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value is string str)
                {
                    if (parameter is string def)
                    {
                        if (string.IsNullOrEmpty(str))
                            return def;
                        else return str;
                    }
                    else throw new ArgumentException("parameter must be string");
                }
                else throw new ArgumentException("value must be string");
            }
            else throw new InvalidOperationException("The target must be a string");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
