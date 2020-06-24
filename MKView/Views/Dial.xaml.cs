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
    /// Interaction logic for Dial.xaml
    /// </summary>
    public partial class Dial : UserControl
    {
        protected bool isDragging;
        private Point clickPosition;

        public Dial()
        {
            InitializeComponent();
            this.Loaded += Dial_Loaded;
        }
       
        private void Dial_Loaded(object sender, RoutedEventArgs e)
        {
            var mage = this.FindAncestor<MageKnightBattleView>();
            IMageKnightBattleViewModel data = mage.DataContext as IMageKnightBattleViewModel;
            if (data.IsMovingBorder)
            {
                this.grid.Visibility = Visibility.Hidden;
            }
        }
    }
}
