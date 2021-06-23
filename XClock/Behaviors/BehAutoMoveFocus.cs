using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace XClock
{
    /// <summary>
    /// Automaticly move focus if user cannot input more digits to set a time
    /// </summary>
    class BehAutoMoveFocus : Behavior<TextBox>
    {
        public int MaxFirstDigit { get; set; }

        private void AssociatedObjectChangedHandler(object o, TextChangedEventArgs e)
        {
            if ((o as TextBox).Text.Length == 1)
            {
                if (int.Parse((o as TextBox).Text) > MaxFirstDigit)
                {
                    (o as TextBox).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
            if ((o as TextBox).Text.Length == 2)
            {
                (o as TextBox).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.TextChanged += AssociatedObjectChangedHandler;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.TextChanged += AssociatedObjectChangedHandler;
            }
        }
    }
}
