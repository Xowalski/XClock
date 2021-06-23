using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace XClock
{
    class BehOnlyMaxIntTextBoxInput : Behavior<TextBox>
    {
        public int MaxInt { get; set; }

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
                AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;

                DataObject.AddPastingHandler(AssociatedObject, Pasting);
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
                AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;

                DataObject.RemovePastingHandler(AssociatedObject, Pasting);
            }
        }

        private void Pasting(object o, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pastedText = (string)e.DataObject.GetData(typeof(string));

                if (!this.IsValid(this.GetText(pastedText)))
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.CancelCommand();
                }
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                e.CancelCommand();
            }
        }

        private void AssociatedObjectPreviewKeyDown(object o, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!this.IsValid(this.GetText(" ")))
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObjectPreviewTextInput(object o, TextCompositionEventArgs e)
        {
            if (!this.IsValid(this.GetText(e.Text)))
            {
                System.Media.SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        private string GetText(string input)
        {
            var txt = this.AssociatedObject;

            int selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
                selectionStart = txt.Text.Length;

            int selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
                selectionLength = txt.Text.Length - selectionStart;

            var realtext = txt.Text.Remove(selectionStart, selectionLength);

            int caretIndex = txt.CaretIndex;
            if (realtext.Length < caretIndex)
                caretIndex = realtext.Length;

            var newtext = realtext.Insert(caretIndex, input);

            return newtext;
        }

        private bool IsValid(string input)
        {
            int i;
            return int.TryParse(input, out i) && i >= 0 && i <= MaxInt;
        }
    }
}
