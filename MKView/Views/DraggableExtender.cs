using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace MKView.Views
{
    public static class FindGenericAncestor 
    {
        public static T FindAncestor<T>(this DependencyObject obj)
    where T : DependencyObject
        {
            DependencyObject tmp = VisualTreeHelper.GetParent(obj);
            while (tmp != null && !(tmp is T))
            {
                tmp = VisualTreeHelper.GetParent(tmp);
            }
            return tmp as T;
        }
    }

    public class DraggableExtender : DependencyObject
    {
        // This is the dependency property we're exposing - we'll 
        // access this as DraggableExtender.CanDrag="true"/"false"
        public static readonly DependencyProperty CanDragProperty =
            DependencyProperty.RegisterAttached("CanDrag",
            typeof(bool),
            typeof(DraggableExtender),
            new UIPropertyMetadata(false, OnChangeCanDragProperty));

        // The expected static setter
        public static void SetCanDrag(UIElement element, bool o)
        {
            element.SetValue(CanDragProperty, o);
        }

        // the expected static getter
        public static bool GetCanDrag(UIElement element)
        {
            return (bool)element.GetValue(CanDragProperty);
        }

        // This is triggered when the CanDrag property is set. We'll
        // simply check the element is a UI element and that it is
        // within a canvas. If it is, we'll hook into the mouse events
        private static void OnChangeCanDragProperty(DependencyObject d,
                  DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if (element == null) return;

            if (e.NewValue != e.OldValue)
            {
                if ((bool)e.NewValue)
                {
                    element.PreviewMouseDown += element_PreviewMouseDown;
                    element.PreviewMouseUp += element_PreviewMouseUp;
                    element.PreviewMouseMove += element_PreviewMouseMove;
                }
                else
                {
                    element.PreviewMouseDown -= element_PreviewMouseDown;
                    element.PreviewMouseUp -= element_PreviewMouseUp;
                    element.PreviewMouseMove -= element_PreviewMouseMove;
                }
            }
        }

        // Determine if we're presently dragging
        private static bool _isDragging = false;

        // The offset from the top, left of the item being dragged 
        // and the original mouse down
        private static Point _offset;

        // This is triggered when the mouse button is pressed 
        // on the element being hooked
        static void element_PreviewMouseDown(object sender,
                System.Windows.Input.MouseButtonEventArgs e)
        {
            // Ensure it's a framework element as we'll need to 
            // get access to the visual tree
            if (Keyboard.IsKeyDown(Key.LeftShift)) return;
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // start dragging and get the offset of the mouse 
            // relative to the element
            // _isDragging = true;
            element.Tag = "true";
             Canvas canvas = element.FindAncestor<Canvas>();
            if (canvas == null) return;
            _offset = e.GetPosition(element);
            var zIndex =
                ((Canvas)canvas)
                .Children.Cast<UIElement>()
                .Max(child => Canvas.GetZIndex(child))
                ;

            zIndex++;

            Canvas.SetZIndex(element, zIndex);
        }

        // This is triggered when the mouse is moved over the element
        private static void element_PreviewMouseMove(object sender,
                  MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift)) return;
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            if (element.Tag == null) return;
            if (element.Tag.ToString() == "false") return;

            Canvas canvas = element.FindAncestor<Canvas>();
            if (canvas == null) return;

            // Get the position of the mouse relative to the canvas
            Point mousePoint = e.GetPosition(canvas);

            // Offset the mouse position by the original offset position
            mousePoint.Offset(-_offset.X, -_offset.Y);

            // Move the element on the canvas
            if (mousePoint.X < 0 || mousePoint.Y < 0) return;
            element.SetValue(Canvas.LeftProperty, mousePoint.X);
            element.SetValue(Canvas.TopProperty, mousePoint.Y);
        }

        // this is triggered when the mouse is released
        private static void element_PreviewMouseUp(object sender,
                MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
            {
                return;
            }

            element.Tag = "false";
        }

    }
}
