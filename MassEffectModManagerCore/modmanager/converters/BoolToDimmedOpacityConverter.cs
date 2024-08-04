using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ME3TweaksModManager.modmanager.converters
{
    internal class BoolToDimmedOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool input)
            {
                bool inverted = parameter is string str && str == @"Not";
                if (input ^ inverted) // xor
                    return 1.0f;
                return 0.5f;
            }
         
            return 0f;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
