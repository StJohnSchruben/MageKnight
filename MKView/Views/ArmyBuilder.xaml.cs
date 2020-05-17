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
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ab = this.DataContext as IArmyBuilder;
            ab.SelectedArmy = lba.SelectedItem as IArmy;
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ab = this.DataContext as IArmyBuilder;
            ab.SelectedMageKnight = lbm.SelectedItem as IMageKnightModel;
        }
    }
}
