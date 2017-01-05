using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WebSearcher.Models;

namespace WindowsFormsApplication1
{
    class IdToUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
              System.Globalization.CultureInfo culture)
        {
            return TheEntireInternet.Urls[((int)value) - 1];
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
