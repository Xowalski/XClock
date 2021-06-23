using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace XClock
{
    public class MovingWindow : Behavior<Window>
    {
        protected override void OnAttached()
        {
            Window window = this.AssociatedObject;
            if (window != null)
            {
                window.KeyDown += Window_KeyDown;
                window.MouseDown += window_MouseDown;
                window.MouseMove += window_MouseMove;
                window.MouseUp += window_MouseUp;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Window window = (Window)sender;
            if (e.Key == Key.Escape)
            {
                window.Close();
            }
        }

        bool whileMoving = false;
        Point cursorStartPoint;

        private void window_MouseDown(object sender, MouseEventArgs e)
        {
            Window window = (Window)sender;
            if (!whileMoving && e.LeftButton == MouseButtonState.Pressed)
            {
                whileMoving = true;
                cursorStartPoint = e.GetPosition(window);
            }
        }

        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            Window window = (Window)sender;
            if (whileMoving)
            {
                Point cursorPoint = e.GetPosition(window);
                Vector movement = cursorPoint - cursorStartPoint;
                window.Left += movement.X;
                window.Top += movement.Y;
            }
        }

        private void window_MouseUp(object sender, MouseEventArgs e)
        {
            Window window = (Window)sender;
            if (e.LeftButton == MouseButtonState.Released)
            {
                whileMoving = false;
            }
        }
    }
}
