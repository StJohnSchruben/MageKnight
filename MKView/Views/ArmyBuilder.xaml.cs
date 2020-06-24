using MKModel;
using MKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MKView.Views
{
    /// <summary>
    /// Interaction logic for ArmyBuilder.xaml
    /// </summary>
    public partial class ArmyBuilder : UserControl
    {
        private IArmyBuilder ab;
        public ArmyBuilder()
        {
            InitializeComponent();
            this.Loaded += ArmyBuilder_Loaded;
        }

        private void ArmyBuilder_Loaded(object sender, RoutedEventArgs e)
        {
            ab = this.DataContext as IArmyBuilder;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ab = this.DataContext as IArmyBuilder;
            //ab.SelectedArmy = lba.SelectedItem as IArmy;
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ab = this.DataContext as IArmyBuilder;
            //ab.SelectedMageKnight = lbm.SelectedItem as IMageKnightModel;
        }

        private void lbm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ab = this.DataContext as IArmyBuilder;
          //  this.lbm.ItemsSource = ab.User.MageKnights;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Collapsed;
        }

        private void Games_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
