using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Exam_InputHooker_Karvatyuk
{
    // BackConverter from string to string array
    [ValueConversion(typeof(string[]), typeof(string))]
    public class StringToArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            return text != null ? text.Split(new[] { ' ', ',' }) : DependencyProperty.UnsetValue;
        }
    }
}
