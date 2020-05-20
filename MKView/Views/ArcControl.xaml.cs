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
        private Point clickPosition;
        private Point _lastMouseOverPoint;

        public ArcControl()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(Control_MouseMove);
        }

        private void Dial_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           // var dial = this.DataContext as IDial;
        }
        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //object _selectedObject = sender;
            isDragging = true;
            //var draggableControl = sender as UserControl;
            //clickPosition = e.GetPosition(this);
            //draggableControl.CaptureMouse();
            //var xCenter = (clickPosition.Point2.X - _selectedObject.Point1.X) / 2 + _selectedObject.Point1.X
            //var yCenter = (_selectedObject.Point2.Y - _selectedObject.Point1.Y) / 2 + _selectedObject.Point1.Y
            //_selectedObjectCenterPoint = new Point((double)xCenter, (double)yCenter);

            ////init set of last mouse over step with the mouse click point
            //var clickPoint = eventargs.GetPosition(source);
            //_lastMouseOverPoint = new Point(clickPosition.X, clickPosition.Y);
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
        
            Console.WriteLine(a);
            return a;
        }

        private double RadianToDegree(double angle)
        {
            double a = angle * (180.0 / Math.PI);
            Console.WriteLine(a);
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
                    angle = (90.0 + RadianToDegree(Math.Atan((double)(-myY / myX))));
                }
                else if (myX > 0 && myY > 0)
                {
                    //1
                    angle = -(90.0 + RadianToDegree(Math.Atan((double)(myY / myX))));
                }
                else if (myX > 0 && myY < 0)
                {
                    //4
                    angle = -(RadianToDegree(Math.Acos((double)(-myX / h))) - 90.0);
                }
                else if (myX < 0 && myY > 0)
                {
                    //2
                    angle = (90.0 + RadianToDegree(Math.Acos((double)(Math.Abs(myX) / h))));
                }

                this.Angle.Angle =- (angle -this.Angle.Angle) ;
                // double dot = currentLocation.X * (this.ActualWidth / 2.0) + currentLocation.Y * (-this.ActualHeight / 2.0);

                //double angle = Math.Atan((double)(Math.Abs(myX)/h));
                // double angle = Math.Asin((double)(Math.Abs(myY)/ Math.Abs(myX))); // positive x?
                //double angle = Math.Atan((double)(Math.Abs(myY)/ Math.Abs(myX))); //usual
                //double angle = Math.Acos((double)(Math.Abs(myY)/ Math.Abs(myX)));
                // double angle = Math.Sin((double)(Math.Abs(myY)/ Math.Abs(myX))); // positive x
                //double angle = Math.Cos((double)(Math.Abs(myY)/ Math.Abs(myX))); 
                //double angle = Math.Tan((double)(Math.Abs(myY)/ Math.Abs(myX)));


                //double angle = Math.Tan((double)(Math.Abs(myX) / Math.Abs(myY)));
                //double angle = Math.Tan((double)(Math.Abs(myX) / Math.Abs(myY)));
                //double angle = Math.Tan((double)(Math.Abs(myX) / Math.Abs(myY)));
                //double angle = Math.Tan((double)(Math.Abs(myX) / Math.Abs(myY)));
                //double angle = Math.Tan((double)(Math.Abs(myX) / Math.Abs(myY)));

                //double angle = Math.Atan((double)(Math.Abs(myX)/h));
                //double angle = Math.Atan((double)(Math.Abs(myX)/h));
                //double angle = Math.Atan((double)(Math.Abs(myX)/h));
                //double angle = Math.Atan((double)(Math.Abs(myX)/h));
                //double angle = Math.Atan((double)(Math.Abs(myX) / h));

                //double angle = Math.Atan((double)(Math.Abs(myY) / h));
                //double angle = Math.Atan((double)(Math.Abs(myY) / h));
                //double angle = Math.Atan((double)(Math.Abs(myY) / h));
                //double angle = Math.Atan((double)(Math.Abs(myY) / h));
                //double angle = Math.Atan((double)(Math.Abs(myY) / h));

                //double angle = Math.Atan((double)(h / Math.Abs(myX)));
                //double angle = Math.Atan((double)(h / Math.Abs(myX)));
                //double angle = Math.Atan((double)(h / Math.Abs(myX)));
                //double angle = Math.Atan((double)(h / Math.Abs(myX)));
                //double angle = Math.Atan((double)(h / Math.Abs(myX)));

                //double angle = Math.Atan((double)( h/ Math.Abs(myY)));
                //double angle = Math.Atan((double)( h/ Math.Abs(myY)));
                //double angle = Math.Atan((double)( h/ Math.Abs(myY)));
                //double angle = Math.Atan((double)(h / Math.Abs(myY)));
                //double angle = Math.Atan((double)( h/ Math.Abs(myY)));

                //this.Angle.Angle = -(270.0 + RadianToDegree(angle));
                //// We want to rotate around the center of the knob, not the top corner
                //Point knobCenter = new Point(this.ActualHeight / 2, this.ActualWidth / 2);

                //// Calculate an angle
                //double radians = Math.Atan((currentLocation.Y - knobCenter.Y) /
                //                           (currentLocation.X - knobCenter.X));
                //this.Angle = radians * 180 / Math.PI;

                // Apply a 180 degree shift when X is negative so that we can rotate
                // all of the way around
                //if (currentLocation.X - knobCenter.X < 0)
                //{
                //    this.Angle += 180;
                //}
            }
        }
    }
}
