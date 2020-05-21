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
    /// Interaction logic for ArcControl.xaml
    /// </summary>
    public partial class ArcControl : UserControl
    {
        protected bool isDragging;
        double mouseDownAngle;
        public ArcControl()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(Control_MouseMove);
        }

        private void ArcControl_MouseLeave(object sender, MouseEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var data = this.DataContext as MKViewModel.IMageKnightBattleViewModel;
            if (!data.IsSelected)
            {
                return;
            }

            isDragging = true;
            var draggableControl = this.FindAncestor<MageKnightBattleView>();
            Point currentLocation = e.GetPosition(draggableControl as UIElement);
            this.mouseDownAngle = this.GetAngle(currentLocation);
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }
        private double DegreeToRadian(double angle)
        {
            double a = Math.PI * angle / 180.0;
            return a;
        }

        private double RadianToDegree(double angle)
        {
            double a = angle * (180.0 / Math.PI);
            return a;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
          
            // this.Angle.Angle = 90.0;
            var draggableControl = this.FindAncestor<MageKnightBattleView>();

            if (isDragging && draggableControl != null)
            {
                // Get the current mouse position relative to the volume control
                Point currentLocation = e.GetPosition(draggableControl as UIElement);
                double mouseMovedAngle = this.GetAngle(currentLocation);
                double newAngle = mouseDownAngle - mouseMovedAngle;

                this.Angle.Angle = -newAngle;
            }
        }

        private double GetAngle(Point currentLocation)
        {
            double mouseX = currentLocation.X;
            double mouseY = currentLocation.Y;
            double centerX = (this.ActualWidth / 2.0);
            double centerY = (this.ActualHeight / 2.0);
            double myX, myY;
            if (mouseX > centerX)
            {
                myX = mouseX - centerX;
            }
            else
            {
                myX = centerX - mouseX;
                myX *= -1.0;
            }

            if (mouseY > centerY)
            {
                myY = mouseY - centerY;
                myY *= -1.0;
            }
            else
            {
                myY = centerY - mouseY;
            }



            double a = myX * myX;

            double b = myY * myY;

            double h = Math.Sqrt(a + b);
            double angle = 0;
            if (myX < 0 && myY < 0)
            {
                //3
                return angle = (90.0 + RadianToDegree(Math.Atan((double)(-myY / myX))));
            }
            else if (myX > 0 && myY > 0)
            {
                //1
                return angle = -(90.0 + RadianToDegree(Math.Atan((double)(myY / myX))));
            }
            else if (myX > 0 && myY < 0)
            {
                //4
                return angle = -(RadianToDegree(Math.Acos((double)(-myX / h))) - 90.0);
            }
            else
            {
                //2
                return angle = (90.0 + RadianToDegree(Math.Acos((double)(Math.Abs(myX) / h))));
            }
        }
    }
}
