using System.Globalization;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Data;
using LegendaryExplorerCore.Helpers;

namespace ME3TweaksModManager.modmanager.converters
{
    internal class EnumToEnabledConverter : IValueConverter
    {
        public static bool StaticConvert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum e && parameter is string valueStr)
            {
                var splitparms = valueStr.Split('_');

                // Go by pairings
                for (int i = 0; i < splitparms.Length; i++)
                {
                    bool isMin = splitparms[i].CaseInsensitiveEquals(@"min");
                    bool isMax = splitparms[i].CaseInsensitiveEquals(@"max");
                    if (isMin || isMax) i++; // skip to next parm

                    var testParm = splitparms[i];

                    var passedInVal = e.ToString();
                    if (passedInVal == testParm)
                        continue; // OK

                    if (isMin || isMax)
                    {
                        var vals = Enum.GetValues(e.GetType());
                        int myIndex = -1;
                        int testIndex = -1;
                        int j = 0;
                        foreach (var eVal in vals)
                        {
                            var eStr = eVal.ToString();
                            if (eStr == testParm)
                            {
                                testIndex = j;
                                j++;
                                continue;
                            }

                            if (eStr == passedInVal)
                            {
                                myIndex = j;
                                j++;
                                continue;
                            }

                            j++;
                        }

                        if (isMax && myIndex < testIndex && testIndex != -1)
                            continue; // OK
                        if (isMin && myIndex > testIndex && testIndex != -1)
                            continue; // OK
                    }

                    // One of the above conditions did not register as true.
                    return false;
                }

                // We are OK
                return true;
            }

            return false;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StaticConvert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class EnumToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumToEnabledConverter.StaticConvert(value, targetType, parameter, culture) ? Visibility.Visible : Visibility.Collapsed;
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}