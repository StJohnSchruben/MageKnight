using MKModel;
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
            this.IsVisibleChanged += Dial_IsVisibleChanged;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(Control_MouseMove);

        }

        private void Dial_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //var dial = this.DataContext as IDial;
            //Canvas canvas = this.FindAncestor<Canvas>();
            //var draggableControl = this.FindAncestor<MageKnightBattleView>();
            //var transform = draggableControl.RenderTransform as TranslateTransform;
            //if (transform == null)
            //{
            //    transform = new TranslateTransform();
            //    draggableControl.RenderTransform = transform;
            //}
            //double offsetX = draggableControl.ActualWidth - this.ActualWidth;
            //double offsetY = draggableControl.ActualHeight - this.ActualHeight;
            //transform.X = 1850;
            //transform.Y = 200;
        }
        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            var draggableControl = sender as UserControl;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
            e.Handled = true;
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = this.FindAncestor<MageKnightBattleView>();

            if (isDragging && draggableControl != null)
            {
                Canvas canvas = this.FindAncestor<Canvas>();
                Point currentCanvasPosition = e.GetPosition(canvas as UIElement);
                Point currentControlPosition = e.GetPosition(draggableControl as UIElement);
                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                double offsetX = draggableControl.ActualWidth - this.ActualWidth;
                double offsetY = draggableControl.ActualHeight - this.ActualHeight;
                double xOffset = currentCanvasPosition.X - clickPosition.X - (offsetX / 2.0);
                double yOffset = currentCanvasPosition.Y - clickPosition.Y - (offsetY / 2.0);
                //double top = draggableControl.RenderTransform.Value.OffsetY;
                //double left = draggableControl.RenderTransform.Value.OffsetX;
                double top = currentCanvasPosition.Y + clickPosition.Y -50;//+ draggableControl.ActualHeight/2 - 50;
                double left = draggableControl.RenderTransform.Value.OffsetX;
                if (/*!(top <= 3750 ) && */!(top <= 200))
                {
                    Console.WriteLine(yOffset);

                    transform.Y = yOffset;
                }
                transform.X = xOffset;
                //if (!(left <= 3750) && !(left > 200))
                //{
                    
                //}
            }
        }
    }
}
