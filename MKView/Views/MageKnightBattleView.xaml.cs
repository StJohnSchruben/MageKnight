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
    /// Interaction logic for MageKnightBattleView.xaml
    /// </summary>
    public partial class MageKnightBattleView : UserControl
    {
        private Canvas canvas;
        public MageKnightBattleView()
        {
            InitializeComponent();
            this.Loaded += MageKnightBattleView_Loaded;
        }

        private void MageKnightBattleView_Loaded(object sender, RoutedEventArgs e)
        {
           this.canvas = this.FindAncestor<Canvas>();
            IMageKnightBattleViewModel data = this.DataContext as IMageKnightBattleViewModel;
        }

        public Point GetPoint(Visual element)
        {
            var positionTransform = element.TransformToAncestor(canvas);
            var areaPosition = positionTransform.Transform(new Point(0, 0));
            return areaPosition;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = this.DataContext as IMageKnightBattleViewModel;
            data.IsSelected = !data.IsSelected;
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var data = this.DataContext as IMageKnightBattleViewModel;
            data.ToggleRangeView = !data.ToggleRangeView;
        }
    }
}
