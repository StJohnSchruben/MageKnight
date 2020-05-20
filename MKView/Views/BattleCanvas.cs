using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MKView.Views
{
    public class BattleCanvas : Canvas
    {
        public BattleCanvas()
        {
            this.MouseDown += BattleCanvas_MouseDown;
            this.PreviewMouseDown += BattleCanvas_PreviewMouseDown;
        }

        private void BattleCanvas_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ;
        }

        private void BattleCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ;
        }

        //public bool State
        //{
        //    get { return (Boolean)this.GetValue(StateProperty); }
        //    set { this.SetValue(StateProperty, value); }
        //}

        //public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
        //  "State", typeof(bool), typeof(BattleCanvas), new PropertyMetadata(false));
    }
}
