using MKModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MKView.Views
{
    public class StatToDoubleReturnHighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
           return  (double)Math.Max(speed, range);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
