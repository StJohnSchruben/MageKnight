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
        protected bool isDragging;
        private Point clickPosition;

        public MageKnightBattleView()
        {
            InitializeComponent();
        //    this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
        //    this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
        //    this.MouseMove += new MouseEventHandler(Control_MouseMove);
        }

        private void Dial_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dial = this.DataContext as IDial;
        }
        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            var draggableControl = sender as UserControl;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UserControl;

            if (isDragging && draggableControl != null)
            {
                Canvas canvas = this.FindAncestor<Canvas>();
                Point currentPosition = e.GetPosition(canvas as UIElement);
                //var xxxxx = currentPosition.X;
                //var yyyyy = currentPosition.Y;
                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = this.DataContext as IMageKnightBattleViewModel;
            data.IsSelected = !data.IsSelected;
        }
    }
}
