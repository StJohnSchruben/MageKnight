using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using MKViewModel;
using MKModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MKView.Views
{
    public class DraggableExtender : DependencyObject
    {
        public static readonly DependencyProperty CanDragProperty =
            DependencyProperty.RegisterAttached("CanDrag",
            typeof(bool),
            typeof(DraggableExtender),
            new UIPropertyMetadata(false, OnChangeCanDragProperty));

        public static void SetCanDrag(UIElement element, bool o)
        {
            element.SetValue(CanDragProperty, o);
        }

        public static bool GetCanDrag(UIElement element)
        {
            return (bool)element.GetValue(CanDragProperty);
        }

        private static void OnChangeCanDragProperty(DependencyObject d,
                  DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if (element == null) return;

            if (e.NewValue != e.OldValue)
            {
                if ((bool)e.NewValue)
                {
                    if (canvas == null)
                    {
                        canvas = element.FindAncestor<Canvas>();
                        canvas.PreviewMouseMove += Canvas_PreviewMouseMove;
                    }

                    element.MouseLeftButtonDown += element_PreviewMouseDown;
                    element.MouseLeftButtonUp += element_PreviewMouseUp;
                }
                else
                {
                    element.MouseLeftButtonDown -= element_PreviewMouseDown;
                    element.MouseLeftButtonUp -= element_PreviewMouseUp;
                }
            }
        }

        private static void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift)) return;
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;
            FrameworkElement element = sender as FrameworkElement;
            if (draggingDial == null) return;

            var mageKnightBattleView = FindFirstGenericChild.FindFirstChild<MageKnightBattleView>(draggingDial);
            IMageKnightBattleViewModel myBattleViewData = mageKnightBattleView.DataContext as IMageKnightBattleViewModel;

            if (myBattleViewData.ActionMode == ActionMode.Move || myBattleViewData.ActionMode == ActionMode.MoveFormation)
            {
                Point mouseDownPoint = canvasMouseDownOffset;
                Point mouseDragPoint = e.GetPosition(canvas);
                Vector dragDiff = mouseDragPoint - mouseDownPoint;
                Point centerOfMyDial = myBattleViewData.CenterPoint;
                Point newCenterOfDial = mouseDragPoint;


                Point centerOfMoveBoundary = new Point(myBattleViewData.MoveStartingPoint.X, myBattleViewData.MoveStartingPoint.Y);
                Vector centerDifference = mouseDragPoint - centerOfMoveBoundary;

                bool moveLimitReached = false;
                bool isTouchingAtLeastOneOtherMage = false;
                if (Math.Abs(centerDifference.Length) > myBattleViewData.Model.Dial.Click.Speed.Value * 100.0)
                {
                    moveLimitReached = true;
                }

                foreach (ContentPresenter mage in canvas.Children)
                {
                    var view = FindFirstGenericChild.FindFirstChild<MageKnightBattleView>(mage);
                    IMageKnightBattleViewModel data = view.DataContext as IMageKnightBattleViewModel;
                    if (data.Name.StartsWith("temp") || data.Model.InstantiatedId == myBattleViewData.Model.InstantiatedId)
                    {
                        continue;
                    }


                    Vector diff = newCenterOfDial - data.CenterPoint;

                    if (Math.Abs(diff.Length) < 100 || moveLimitReached)
                    {
                        isTouchingAtLeastOneOtherMage = true;
                        data.IsInRange = true;
                    }

                    if (Math.Abs(diff.Length) >= 100 && Math.Abs(diff.Length) < myBattleViewData.Range * 100)
                    {
                        data.IsInRange = true;
                    }
                    else
                    {
                        data.IsInRange = false;
                    }
                }



                if (!moveLimitReached && !isTouchingAtLeastOneOtherMage)
                {
                    draggingDial.SetValue(Canvas.LeftProperty, mouseDragPoint.X - myBattleViewData.Size);
                    draggingDial.SetValue(Canvas.TopProperty, mouseDragPoint.Y - myBattleViewData.Size);
                }
            }
        }

        
        private static Canvas canvas;
        private static bool _isDragging = false;
        private static Point _offset;
        private static Point canvasMouseDownOffset;

        static void element_PreviewMouseDown(object sender,
                System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift)) return;
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            draggingDial = element as ContentPresenter;
            _offset = e.GetPosition(element);
            canvasMouseDownOffset = e.GetPosition(canvas);
            var zIndex =
                ((Canvas)canvas)
                .Children.Cast<UIElement>()
                .Max(child => Canvas.GetZIndex(child))
                ;

            zIndex++;

            Canvas.SetZIndex(element, zIndex);
        }

        private static ContentPresenter draggingDial;
        private static IMageKnightBattleViewModel selectedMageViewModel;

        private static void element_PreviewMouseUp(object sender,
                MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
            {
                return;
            }

            draggingDial = null;
        }

    }
}
