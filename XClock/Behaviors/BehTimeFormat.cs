using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace XClock
{
    /// <summary>
    /// Adds "0" digit in front of string if string is one digit long to reflect the data format
    /// </summary>
    class BehTimeFormat : Behavior<TextBox>
    {
        private readonly RoutedEventHandler _onLostFocusHandler = (o, e) =>
        {
            if ((o as TextBox).Text.Length == 1)
            {
                (o as TextBox).Text = "0" + (o as TextBox).Text;
            }
        };

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.LostFocus += _onLostFocusHandler;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LostFocus -= _onLostFocusHandler;
        }
    }
}
