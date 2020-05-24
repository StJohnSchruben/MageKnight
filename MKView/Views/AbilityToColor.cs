using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MKView.Views
{
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
                case "Magic Enhancment":
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
}
