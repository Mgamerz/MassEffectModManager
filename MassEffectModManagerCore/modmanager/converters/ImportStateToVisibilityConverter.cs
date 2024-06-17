using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using LegendaryExplorerCore.Gammtek.Extensions;

namespace ME3TweaksModManager.modmanager.converters
{
    internal class ImportStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is Enum e && parameter is string valueStr)
            {
                var splitparms = valueStr.Split('_');

                // Go by pairings
                for (int i = 0; i < splitparms.Length; i++)
                {
                    bool isMin = splitparms[i] == @"Min";
                    bool isMax = splitparms[i] == @"Max";
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

                        if (isMax && myIndex < testIndex)
                            continue; // OK
                        if (isMin && myIndex > testIndex)
                            continue; // OK
                    }

                    // One of the above conditions did not register as true.
                    return Visibility.Collapsed;
                }

                // We are OK
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}