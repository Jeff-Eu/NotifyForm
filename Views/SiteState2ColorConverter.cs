using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using WebSearcher.Models;
using WindowsFormsApplication1.ViewModels;

namespace WindowsFormsApplication1
{
    class SiteState2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
              System.Globalization.CultureInfo culture)
        {
            SiteStates state = (SiteStates)value;
            SolidColorBrush brush = new SolidColorBrush();
            switch (state)
            {
                case SiteStates.Detected:
                    brush.Color = Colors.Yellow;
                    break;
                case SiteStates.SiteCorrupted:
                    brush.Color = Colors.Pink;
                    break;
                case SiteStates.NotDetected:
                    brush.Color = Colors.Transparent;
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
