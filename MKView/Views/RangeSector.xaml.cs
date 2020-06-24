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
    /// Interaction logic for RangeSector.xaml
    /// </summary>
    public partial class RangeSector : UserControl
    {
        public RangeSector()
        {
            InitializeComponent();
            this.Loaded += RangeSector_Loaded;
        }

        private void RangeSector_Loaded(object sender, RoutedEventArgs e)
        {
            IMageKnightBattleViewModel data = this.DataContext as IMageKnightBattleViewModel;
            if (data.IsMovingBorder)
            {
                this.rangeBorder.Visibility = Visibility.Collapsed;
                this.rangeSector.Visibility = Visibility.Collapsed;
                this.dialProxy.Visibility = Visibility.Collapsed;
            }
        }
    }
}
