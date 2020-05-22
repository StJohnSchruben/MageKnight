using MKViewModel;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace MKView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            this.bg.Visibility = Visibility.Visible;
            this.bg.PreviewMouseWheel += Bg_PreviewMouseWheel;
        }

        private List<MouseWheelEventArgs> _reentrantList = new List<MouseWheelEventArgs>();
        private void Bg_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                e.Handled = true;
                UserControl_MouseWheel(sender, e);
            }
                //if (Keyboard.IsKeyDown(Key.LeftCtrl))
            //{
            //    var scrollControl = sender as ScrollViewer;

            //    if (!e.Handled && sender != null)

            //    {



            //        bool cancelScrolling = false;



            //        if ((e.Delta > 0 && scrollControl.VerticalOffset == 0)

            //            || (e.Delta <= 0 && scrollControl.VerticalOffset >= scrollControl.ExtentHeight - scrollControl.ViewportHeight))

            //        {

            //            e.Handled = true;

            //            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);

            //            eventArg.RoutedEvent = UIElement.MouseWheelEvent;

            //            eventArg.Source = sender;

            //            var parent = ((Control)sender).Parent as UIElement;

            //            parent.RaiseEvent(eventArg);

            //        }

            //    }
            //}
        }

        private void mainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.mm.Visibility = Visibility.Visible;
        }

        private void CollapsViews()
        {
            this.mm.Visibility = Visibility.Hidden;
            this.ab.Visibility = Visibility.Hidden;
            this.bg.Visibility = Visibility.Hidden;
            this.mkg.Visibility = Visibility.Hidden;
        }

        private void ArmyBuilder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.ab.Visibility = Visibility.Visible;
        }

        private void BattleGround_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.bg.Visibility = Visibility.Visible;
        }

        private void Database_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.mkg.Visibility = Visibility.Visible;
        }

        private void UnderConstruction_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void AdjustScroll(Point centerPoint, double scale)
        {
                Point poi = Mouse.GetPosition(bg);

                double Xmove = (bg.ActualHeight) / 2 - poi.X;
                double Ymove = (bg.ActualWidth) / 2 - poi.Y;

                bg.ScrollToHorizontalOffset((bg.HorizontalOffset - Xmove));
                bg.ScrollToVerticalOffset((bg.VerticalOffset - Ymove));    
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                try
                {
                    Point p = e.GetPosition(this);
                    this.AdjustScroll(p, e.Delta);
                    this.vb.Height += e.Delta/2;
                    this.vb.Width += this.vb.Height;
                }
                catch
                {
                    ;
                }
            }
        }
    }
}
