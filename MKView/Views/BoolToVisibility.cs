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
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AbilityToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = value as string;
            return (v != string.Empty &&
                v != "Toughness" &&
                v != "Magic Enhancement" &&
                v != "Magic Immunity" &&
                v != "Berserk" &&
                v != "Defend" &&
                v != "Aquatic" &&
                v != "Demoralized" &&
                v != "Regeneration" &&
                v != "Vampirism" &&
                v != "Invulnerability" &&
                v != "Pole Arm" &&
                v != "Battle Armor" &&
                v != "Battle Fury") ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
