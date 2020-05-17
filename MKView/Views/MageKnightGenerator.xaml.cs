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
    /// Interaction logic for MageKnightGenerator.xaml
    /// </summary>
    public partial class MageKnightGenerator : UserControl
    {
        private IMageKnightGenerator mk;
        public MageKnightGenerator()
        {
            InitializeComponent();
            
            
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mk = this.DataContext as IMageKnightGenerator;
            mk.SelectedMageKnight = dg.SelectedItem as IMageKnightModel;
        }
    }
}
