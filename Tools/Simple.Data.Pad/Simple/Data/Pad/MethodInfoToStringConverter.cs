using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
namespace Simple.Data.Pad
{
    public class MethodInfoToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MethodInfo methodInfo = value as MethodInfo;
            if (methodInfo == null)
            {
                return string.Empty;
            }
            return string.Format("{0}({1})", methodInfo.Name, string.Join(", ",
                from p in methodInfo.GetParameters()
                select p.Name));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
