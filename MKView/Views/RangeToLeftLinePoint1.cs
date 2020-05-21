using MKModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MKView.Views
{
    public class RangeForWidthToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            return 200 * data.Range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SpeedForWidthToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            return 200 * (double)speed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TotalWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            return 200 * (double)Math.Max(speed, range);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    

    public class HighStatCenterWithDialOffsetToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;                   //offset to the corner of the dial
            return 100 * (double)Math.Max(speed, range) - 55;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class HighStatCenterOffsetToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            return 100 * (double)Math.Max(speed, range);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class RangeToLeftLinePoint1X : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // return 0;
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            return 100 * (double)Math.Max(speed, range);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RangeToLeftLinePoint1Y : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double range = data.Range;
            return 200 * (double)range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RangeToLeftLinePoint2X : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double range = data.Range;
            return 100 * range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RangeToLeftLinePoint2Y : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double range = data.Range;
            return 100 * range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RangeToRightLinePoint1X : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double range = data.Range;
            return 100 * range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class RangeToRightLinePoint1Y : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double range = data.Range;
            return 100 * range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class RangeToRightLinePoint2X : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double range = data.Range;
            return 200 * (double)range;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RangeToRightLinePoint2Y : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return 0;
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            return 100 * (double)Math.Max(speed, range);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CenterRangePointToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;


            double rangeHalf = 100 *data.Range;
            double maxHalf  = 100 * (double)Math.Max(data.Dial.Click.Speed.Value, data.Range); ;
            return maxHalf - rangeHalf;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HightStatToPointCenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            Point p = new Point();
            p.X = 100* (double)Math.Max(speed, range);
            p.Y = p.X;
            return p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class HightStatToPointEndConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;


            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            double rangeWidth = range * 200;
            double halfRangeWidth = range * 100;
            Point p = new Point();
            p.X = 200 * (double)Math.Max(speed, range);// rangeWidth;
            p.Y = p.X;//halfRangeWidth + (50 * range);
            return p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HighStatToPointStartToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;
            if (data == null)
                return 0;
            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            double halfRangeWidth = range * 100;
            Point p = new Point();
            p.X = 0.0;
            p.Y = 200 * (double)Math.Max(speed, range);//halfRangeWidth + (50 * range);
            return p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
