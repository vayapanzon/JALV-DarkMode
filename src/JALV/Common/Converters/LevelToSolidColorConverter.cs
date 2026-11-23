using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace JALV.Common.Converters
{
    public class LevelToSolidColorConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
                return Brushes.Transparent;

            int levelIndex = (int)value;
            switch (levelIndex)
            {
                case 1:
                    return GetLevelBrush("DebugLevelColor");
                case 2:
                    return GetLevelBrush("InfoLevelColor");
                case 3:
                    return GetLevelBrush("WarnLevelColor");
                case 4:
                    return GetLevelBrush("ErrorLevelColor");
                case 5:
                    return GetLevelBrush("FatalLevelColor");
                default:
                    return Brushes.Transparent;
            }
        }

        private static Brush GetLevelBrush(string key)
        {
            return Application.Current.TryFindResource(key) as Brush ?? Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
