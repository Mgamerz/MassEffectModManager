using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using LegendaryExplorerCore.Gammtek.Extensions;
using LegendaryExplorerCore.Helpers;

namespace ME3TweaksModManager.modmanager.converters
{
    [Localizable(false)]
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class MEGameToGameNameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MEGame game)
            {
                bool useShortName = parameter is string str && str.CaseInsensitiveEquals("shortname");
                return game.ToGameName(useShortName);
            }

            return "Invalid value provided: " + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {    // Don't need any convert back
            return null;
        }
    }
}

