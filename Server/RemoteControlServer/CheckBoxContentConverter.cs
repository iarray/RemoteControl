using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace RemoteControlServer
{
    [ValueConversion(typeof(bool), typeof(string))]

    class CheckBoxContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b = (bool)value;
            if(b)
            {
                return "启用";
            }
            else
            {
                return "未启用";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;
            if (strValue == "启用")
            {
                return true;
            }
            if (strValue == "未启用")
            {
                return false;
            }
            return DependencyProperty.UnsetValue;

        }
    }
}
