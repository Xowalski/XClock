using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XClock
{
    class BehClosingWindowButton : Behavior<Window>
    {
        public static readonly DependencyProperty ButtonProperty = DependencyProperty.Register(
            "Button", typeof(Button), typeof(BehClosingWindowButton), new PropertyMetadata(null, ChangedButton));


        public Button Button
        {
            get { return (Button)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        private static void ChangedButton(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = (d as BehClosingWindowButton).AssociatedObject;
            RoutedEventHandler button_Click = (object sender, RoutedEventArgs _e) => { window.Close(); };
            if (e.OldValue != null)
            {
                ((Button)e.OldValue).Click -= button_Click;
            }
            if (e.NewValue != null)
            {
                ((Button)e.NewValue).Click += button_Click;
            }
        }
    }
}
