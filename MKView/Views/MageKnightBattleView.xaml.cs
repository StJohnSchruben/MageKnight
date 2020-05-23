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
        public MageKnightBattleView()
        {
            InitializeComponent();
            this.dial.MouseDown += Dial_MouseDown;
        }

        private void Dial_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int index = (int)this.GetValue(Canvas.ZIndexProperty);
            Canvas.SetZIndex(this, ++index);
        }


        private void Dial_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dial = this.DataContext as IDial;
            this.SetValue(Canvas.LeftProperty, 1800);
            this.SetValue(Canvas.TopProperty, 200);
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = this.DataContext as IMageKnightBattleViewModel;
            data.ToggleRangeView = !data.ToggleRangeView;
        }
    }
}
