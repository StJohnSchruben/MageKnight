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
    /// Interaction logic for SpeedBoundary.xaml
    /// </summary>
    public partial class SpeedBoundary : UserControl
    {
        public SpeedBoundary()
        {
            InitializeComponent();
            this.Loaded += SpeedBoundary_Loaded;
        }

        private void SpeedBoundary_Loaded(object sender, RoutedEventArgs e)
        {
            IMageKnightBattleViewModel data = this.DataContext as IMageKnightBattleViewModel;
            if (data.IsMovingBorder)
            {
                this.speedBorder.Visibility = Visibility.Visible;
                this.IsHitTestVisible = false;
            }
        }
    }
}
