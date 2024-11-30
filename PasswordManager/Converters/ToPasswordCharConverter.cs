using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Converters
{
    internal class ToPasswordCharConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value?.GetType() != typeof(string)) throw new InvalidOperationException("The value must be a string");
                var str = (string)value;
                return new String('*', str.Length);


            }
            else
            {
                throw new InvalidOperationException("The target must be a string");
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
