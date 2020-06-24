using MKModel;
using MKService;
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
using ZoomAndPan;

namespace MKView.Views
{

    public enum MouseHandlingMode
    {
        None,

        DraggingRectangles,

        Panning,

        Zooming,

        DragZooming,
    }

    /// <summary>
    /// Interaction logic for ZoomControl.xaml
    /// </summary>
    public partial class ZoomControl : UserControl
    {
        private MouseHandlingMode mouseHandlingMode = MouseHandlingMode.None;

        private Point origZoomAndPanControlMouseDownPoint;

        private Point origContentMouseDownPoint;

        private MouseButton mouseButtonDown;

        private Rect prevZoomRect;

        private double prevZoomScale;

        private bool prevZoomRectSet = false;
        public ZoomControl()
        {
            InitializeComponent();
            this.zoomAndPanControl.Visibility = Visibility.Visible;
            this.Loaded += ZoomControl_Loaded;
        }

        private void ZoomControl_Loaded(object sender, RoutedEventArgs e)
        {
            zoomAndPanControl.AnimatedScaleToFit();
        }

        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }


        private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }


        private void JumpBackToPrevZoom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            JumpBackToPrevZoom();
        }

        private void JumpBackToPrevZoom_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = prevZoomRectSet;
        }


        private void Fill_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            zoomAndPanControl.AnimatedScaleToFit();
        }


        private void OneHundredPercent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            zoomAndPanControl.AnimatedZoomTo(1.0);
        }


        private void JumpBackToPrevZoom()
        {
            zoomAndPanControl.AnimatedZoomTo(prevZoomScale, prevZoomRect);

            ClearPrevZoomRect();
        }


        private void ZoomOut(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale - 0.1, contentZoomCenter);
        }

        private void ZoomIn(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale + 0.1, contentZoomCenter);
        }

        private void InitDragZoomRect(Point pt1, Point pt2)
        {
            SetDragZoomRect(pt1, pt2);

            dragZoomCanvas.Visibility = Visibility.Visible;
            dragZoomBorder.Opacity = 0.5;
        }

        private void SetDragZoomRect(Point pt1, Point pt2)
        {
            double x, y, width, height;
            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            Canvas.SetLeft(dragZoomBorder, x);
            Canvas.SetTop(dragZoomBorder, y);
            dragZoomBorder.Width = width;
            dragZoomBorder.Height = height;
        }

        private void ApplyDragZoomRect()
        {
            SavePrevZoomRect();

            double contentX = Canvas.GetLeft(dragZoomBorder);
            double contentY = Canvas.GetTop(dragZoomBorder);
            double contentWidth = dragZoomBorder.Width;
            double contentHeight = dragZoomBorder.Height;
            zoomAndPanControl.AnimatedZoomTo(new Rect(contentX, contentY, contentWidth, contentHeight));

            FadeOutDragZoomRect();
        }

        private void FadeOutDragZoomRect()
        {
            AnimationHelper.StartAnimation(dragZoomBorder, Border.OpacityProperty, 0.0, 0.1,
                delegate (object sender, EventArgs e)
                {
                    dragZoomCanvas.Visibility = Visibility.Collapsed;
                });
        }

        private void SavePrevZoomRect()
        {
            prevZoomRect = new Rect(zoomAndPanControl.ContentOffsetX, zoomAndPanControl.ContentOffsetY, zoomAndPanControl.ContentViewportWidth, zoomAndPanControl.ContentViewportHeight);
            prevZoomScale = zoomAndPanControl.ContentScale;
            prevZoomRectSet = true;
        }

        /// <summary>
        /// Clear the memory of the previous zoom level.
        /// </summary>
        private void ClearPrevZoomRect()
        {
            prevZoomRectSet = false;
        }



        private void zoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                Point curContentMousePoint = e.GetPosition(this.content);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                Point curContentMousePoint = e.GetPosition(this.content);
                ZoomOut(curContentMousePoint);
            }
        }
        private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseHandlingMode == MouseHandlingMode.Panning)
            {
                Point curContentMousePoint = e.GetPosition(this.content);
                Vector dragOffset = curContentMousePoint - origContentMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= dragOffset.X;
                zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.Zooming)
            {
                Point curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
                Vector dragOffset = curZoomAndPanControlMousePoint - origZoomAndPanControlMouseDownPoint;
                double dragThreshold = 10;
                if (mouseButtonDown == MouseButton.Left &&
                    (Math.Abs(dragOffset.X) > dragThreshold ||
                     Math.Abs(dragOffset.Y) > dragThreshold))
                {
                    mouseHandlingMode = MouseHandlingMode.DragZooming;
                    Point curContentMousePoint = e.GetPosition(this.content);
                    InitDragZoomRect(origContentMouseDownPoint, curContentMousePoint);
                }

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
            {
                Point curContentMousePoint = e.GetPosition(this.content);
                SetDragZoomRect(origContentMouseDownPoint, curContentMousePoint);

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.None)
            {
                if (selectionBorder != null)
                {
                    Point curContentMousePoint = e.GetPosition(this.content);

                    if (curContentMousePoint.X <= selectionPoint.X && curContentMousePoint.Y <= selectionPoint.Y)
                    {
                        selectionBorder.Height = selectionPoint.Y - curContentMousePoint.Y;
                        selectionBorder.Width = selectionPoint.X - curContentMousePoint.X;
                        selectionBorder.SetValue(Canvas.LeftProperty, curContentMousePoint.X);
                        selectionBorder.SetValue(Canvas.TopProperty, curContentMousePoint.Y);
                        return;
                    }
                    else if (curContentMousePoint.Y <= selectionPoint.Y)
                    {
                        selectionBorder.Height = selectionPoint.Y - curContentMousePoint.Y;
                        selectionBorder.Width = curContentMousePoint.X - selectionPoint.X;
                        selectionBorder.SetValue(Canvas.LeftProperty, curContentMousePoint.X);
                        selectionBorder.SetValue(Canvas.TopProperty, curContentMousePoint.Y);
                        return;
                    }
                    else if (curContentMousePoint.X <= selectionPoint.X)
                    {
                        selectionBorder.Height = curContentMousePoint.Y - selectionPoint.Y;
                        selectionBorder.Width = selectionPoint.X - curContentMousePoint.X;
                        selectionBorder.SetValue(Canvas.LeftProperty, curContentMousePoint.X);
                        selectionBorder.SetValue(Canvas.TopProperty, curContentMousePoint.Y);
                        return;
                    }

                    try
                    {

                        selectionBorder.Height = curContentMousePoint.Y - selectionPoint.Y;
                        selectionBorder.Width = curContentMousePoint.X - selectionPoint.X;
                    }
                    catch
                    {
                        ;
                    }
                }
            }
        }

        private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.None)
            {

                if (mouseHandlingMode == MouseHandlingMode.Zooming)
                {
                    if (mouseButtonDown == MouseButton.Left)
                    {
                        ZoomIn(origContentMouseDownPoint);
                    }
                    else if (mouseButtonDown == MouseButton.Right)
                    {
                        ZoomOut(origContentMouseDownPoint);
                    }
                }
                else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
                {
                    ApplyDragZoomRect();
                }

                zoomAndPanControl.ReleaseMouseCapture();
                mouseHandlingMode = MouseHandlingMode.None;
                e.Handled = true;
            }
        }
        Point selectionPoint;
        Border selectionBorder;
        private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.content.Focus();
            Keyboard.Focus(this.content);

            mouseButtonDown = e.ChangedButton;
            origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
            origContentMouseDownPoint = e.GetPosition(this.content);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
                (e.ChangedButton == MouseButton.Left ||
                 e.ChangedButton == MouseButton.Right))
            {
                // Shift + left- or right-down initiates zooming mode.
                mouseHandlingMode = MouseHandlingMode.Zooming;
            }
            else if (mouseButtonDown == MouseButton.Right)
            {
                // Just a plain old left-down initiates panning mode.
                mouseHandlingMode = MouseHandlingMode.Panning;
            }

            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                e.Handled = true;
            }
        }
    }
}
