using MKModel;
using MKService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MKView.Views
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ScaleToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)(int)((double)value * 100.0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 100.0;
        }
    }

    public class AbilityToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = value as string;
            switch (v) 
            {
                case "Flight":
                case "Toughness":
                case "Battle Fury":
                case "Flame Lightning":
                    return new SolidColorBrush(Colors.Orange);

                case "Charge":
                case "Battle Armor":
                case "Healing":
                    return new SolidColorBrush(Colors.Green);

                case "Wheapon Master":
                case "Pole Arm":
                case "Quickness":
                case "Berserk":
                    return new SolidColorBrush(Colors.Red);

                case "Magic Levitation":
                case "Magic Enhancement":
                case "Magic Blast":
                case "Magic Immunity":
                    return new SolidColorBrush(Colors.Blue);

                case "Shockwave":
                case "Defend":
                case "Aquatic":
                case "Demoralized":
                    return new SolidColorBrush(Colors.Yellow);

                case "Command":
                case "Magic Healing":
                case "Invulnerability":
                case "Bound":
                    return new SolidColorBrush(Colors.Gray);

                case "Regeneration":
                case "Stealth":
                case "Necromancy":
                case "Vampirism":
                    return new SolidColorBrush(Colors.Black);
            }

            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AbilityToTextColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = value as string;
            switch (v)
            {
                case "Charge":
                case "Battle Armor":
                case "Healing":
                case "Magic Levitation":
                case "Magic Enhancment":
                case "Magic Blast":
                case "Magic Immunity":
                case "Regeneration":
                case "Stealth":
                case "Necromancy":
                case "Vampirism":
                    return new SolidColorBrush(Colors.White);

                case "Wheapon Master":
                case "Pole Arm":
                case "Quickness":
                case "Berserk":
                case "Shockwave":
                case "Defend":
                case "Aquatic":
                case "Demoralized":
                case "Command":
                case "Magic Healing":
                case "Invulnerability":
                case "Bound":
                case "Flight":
                case "Toughness":
                case "Battle Fury":
                case "Flame Lightning":
                    return new SolidColorBrush(Colors.Black);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GameIdToDisplayString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IGameModel gameModel = value as IGameModel;
            var user = ServiceTypeProvider.Instance.UserCollection.Users.FirstOrDefault(x => x.Id == gameModel.User1Id);
            var user2 = ServiceTypeProvider.Instance.UserCollection.Users.FirstOrDefault(x => x.Id == gameModel.User2Id);
            if (user != null && user2 != null)
            {
                return $"User {user.UserName} Chanllenges {user2.UserName}";
            }
            else if (user != null)
            {
                return $"User {user.UserName} Chanllenges";
            }

            return "Say What?";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class HalfDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatToDoubleReturnHighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as IMageKnightModel;

            double speed = data.Dial.Click.Speed.Value;
            double range = data.Range;
            return (double)Math.Max(speed, range);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class FindGenericAncestor
    {
        public static T FindAncestor<T>(this DependencyObject obj)
    where T : DependencyObject
        {
            DependencyObject tmp = VisualTreeHelper.GetParent(obj);
            while (tmp != null && !(tmp is T))
            {
                tmp = VisualTreeHelper.GetParent(tmp);
            }
            return tmp as T;
        }
    }

    public static class FindFirstGenericChild
    {
        public static T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);
            var children = new FrameworkElement[childrenCount];
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                children[i] = child;
                if (child is T)
                {
                    return (T)child;
                }

            }

            for (int i = 0; i < childrenCount; i++)
            {
                if (children[i] != null)
                {
                    var subChild = FindFirstChild<T>(children[i]);
                    if (children[i] != null)
                    {
                        return subChild;
                    }
                }
            }

            return null;
        }
    }

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


            double rangeHalf = 100 * data.Range;
            double maxHalf = 100 * (double)Math.Max(data.Dial.Click.Speed.Value, data.Range); ;
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
            p.X = 100 * (double)Math.Max(speed, range);
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
