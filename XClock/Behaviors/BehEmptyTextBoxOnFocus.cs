using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace XClock
{
    /// <summary>
    /// Behavior that makes textbox empty on focus and restores previous text on lost focus in case if new text is not implemented
    /// </summary>
    class BehEmptyTextBoxOnFocus : Behavior<TextBox>
    {
        private string previousText;

        private void AssociatedObjectGetFocusHandler(object o, RoutedEventArgs e)
        {
            previousText = (o as TextBox).Text;
            (o as TextBox).Text = string.Empty;
        }

        private void AssociatedObjectLostFocusHandler(object o, RoutedEventArgs e)
        {
            if ((o as TextBox).Text.Length == 0)
            {
                (o as TextBox).Text = previousText;
            }
        }

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.GotFocus += AssociatedObjectGetFocusHandler;
                AssociatedObject.LostFocus += AssociatedObjectLostFocusHandler;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.GotFocus -= AssociatedObjectGetFocusHandler;
                AssociatedObject.LostFocus -= AssociatedObjectLostFocusHandler;
            }
        }
    }
}
