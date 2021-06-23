using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace XClock.ViewModel
{
    public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        protected Action<object> execute = null;

        protected static readonly DependencyProperty captionProperty = DependencyProperty.Register(
            "Caption", typeof(string), typeof(DialogBox), new PropertyMetadata(""));

        public string Caption
        {
            get { return (string)GetValue(captionProperty); }
            set { SetValue(captionProperty, value); }
        }

        protected ICommand show;

        public virtual ICommand Show
        {
            get
            {
                if (show == null)
                {
                    show = new RelayCommand(execute);
                }
                return show;
            }
        }
    }

    public abstract class CommandDialogBox : DialogBox
    {
        public override ICommand Show
        {
            get
            {
                if (show == null)
                {
                    show = new RelayCommand(o =>
                    {
                        executeCommand(CommandBefore, CommandParameter);
                        execute(o);
                        executeCommand(CommandAfter, CommandParameter);
                    },
                    o => CanExecuteShow);
                }
                return show;
            }
        }

        protected static readonly DependencyProperty commandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(CommandDialogBox));

        public object CommandParameter
        {
            get { return GetValue(commandParameterProperty); }
            set { SetValue(commandParameterProperty, value); }
        }

        protected static void executeCommand(ICommand command, object commandParameter)
        {
            if (command != null)
            {
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        protected static readonly DependencyProperty commandBeforeProperty = DependencyProperty.Register(
            "CommandBefore", typeof(ICommand), typeof(CommandDialogBox));

        public ICommand CommandBefore
        {
            get { return (ICommand)GetValue(commandBeforeProperty); }
            set { SetValue(commandBeforeProperty, value); }
        }

        public static DependencyProperty commandAfterProperty = DependencyProperty.Register(
            "CommandAfter", typeof(ICommand), typeof(CommandDialogBox));

        public ICommand CommandAfter
        {
            get { return (ICommand)GetValue(commandAfterProperty); }
            set { SetValue(commandAfterProperty, value); }
        }

        public static DependencyProperty canExecuteShowProperty = DependencyProperty.Register(
            "CanExecuteShow", typeof(bool), typeof(CommandDialogBox));

        public bool CanExecuteShow
        {
            get { return (bool)GetValue(canExecuteShowProperty); }
            set { SetValue(canExecuteShowProperty, value); }
        }
    }

    public class MessageDialogBox : CommandDialogBox
    {
        public MessageBoxResult? LastResult { get; protected set; }
        public MessageBoxButton Buttons { get; set; } = MessageBoxButton.OK;
        public MessageBoxImage Icon { get; set; } = MessageBoxImage.None;

        public bool IsLastResultYes
        {
            get
            {
                if (!LastResult.HasValue)
                {
                    return false;
                }
                else
                {
                    return LastResult.Value == MessageBoxResult.Yes;
                }
            }
        }

        public bool IsLastResultNo
        {
            get
            {
                if (!LastResult.HasValue)
                {
                    return false;
                }
                else
                {
                    return LastResult.Value == MessageBoxResult.No;
                }
            }
        }

        public bool IsLastResultCancel
        {
            get
            {
                if (!LastResult.HasValue)
                {
                    return false;
                }
                else
                {
                    return LastResult.Value == MessageBoxResult.Cancel;
                }
            }
        }

        public bool IsLastResultOK
        {
            get
            {
                if (!LastResult.HasValue)
                {
                    return false;
                }
                else
                {
                    return LastResult.Value == MessageBoxResult.OK;
                }
            }
        }

        public MessageDialogBox()
        {
            execute = o =>
            {
                LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
                OnPropertyChanged("LastResult");
                switch (LastResult)
                {
                    case MessageBoxResult.Yes:
                        OnPropertyChanged("IsLastResultYes");
                        executeCommand(CommandYes, CommandParameter);
                        break;
                    case MessageBoxResult.No:
                        OnPropertyChanged("IsLastResultNo");
                        executeCommand(CommandNo, CommandParameter);
                        break;
                    case MessageBoxResult.Cancel:
                        OnPropertyChanged("IsLastResultCancel");
                        executeCommand(CommandCancel, CommandParameter);
                        break;
                    case MessageBoxResult.OK:
                        OnPropertyChanged("IsLastResultOK");
                        executeCommand(CommandOK, CommandParameter);
                        break;
                }
            };
        }

        protected static readonly DependencyProperty commandYesProperty = DependencyProperty.Register(
            "CommandYes", typeof(ICommand), typeof(MessageDialogBox));
        protected static readonly DependencyProperty commandNoProperty = DependencyProperty.Register(
            "CommandNo", typeof(ICommand), typeof(MessageDialogBox));
        protected static readonly DependencyProperty commandCancelProperty = DependencyProperty.Register(
            "CommandCancel", typeof(ICommand), typeof(MessageDialogBox));
        protected static readonly DependencyProperty commandOKProperty = DependencyProperty.Register(
            "CommandOK", typeof(ICommand), typeof(MessageDialogBox));

        public ICommand CommandYes
        {
            get { return (ICommand)GetValue(commandYesProperty); }
            set { SetValue(commandYesProperty, value); }
        }

        public ICommand CommandNo
        {
            get { return (ICommand)GetValue(commandNoProperty); }
            set { SetValue(commandNoProperty, value); }
        }

        public ICommand CommandCancel
        {
            get { return (ICommand)GetValue(commandCancelProperty); }
            set { SetValue(commandCancelProperty, value); }
        }

        public ICommand CommandOK
        {
            get { return (ICommand)GetValue(commandOKProperty); }
            set { SetValue(commandOKProperty, value); }
        }
    }

    public class ConditionalMessageDialogBox : MessageDialogBox
    {
        public static DependencyProperty IsDialogBypassedProperty = DependencyProperty.Register(
            "IsDialogBypassed", typeof(bool), typeof(ConditionalMessageDialogBox));

        public bool IsDialogBypassed
        {
            get { return (bool)GetValue(IsDialogBypassedProperty); }
            set { SetValue(IsDialogBypassedProperty, value); }
        }

        public MessageBoxResult DialogBypassButton { get; set; } = MessageBoxResult.None;

        public ConditionalMessageDialogBox()
        {
            execute = o =>
            {
                MessageBoxResult result;
                if (!IsDialogBypassed)
                {
                    LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
                    OnPropertyChanged("LastResult");
                    result = LastResult.Value;
                }
                else
                {
                    result = DialogBypassButton;
                }
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (!IsDialogBypassed)
                        {
                            OnPropertyChanged("IsLastResultYes");
                            executeCommand(CommandYes, CommandParameter);
                        }
                        break;
                    case MessageBoxResult.No:
                        if (!IsDialogBypassed)
                        {
                            OnPropertyChanged("IsLastResultNo");
                            executeCommand(CommandNo, CommandParameter);
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        if (!IsDialogBypassed)
                        {
                            OnPropertyChanged("IsLastResultCancel");
                            executeCommand(CommandCancel, CommandParameter);
                        }
                        break;
                    case MessageBoxResult.OK:
                        if (!IsDialogBypassed)
                        {
                            OnPropertyChanged("IsLastResultOK");
                            executeCommand(CommandOK, CommandParameter);
                        }
                        break;
                }
            };
        }
    }

    public abstract class FileDialogBox : CommandDialogBox
    {
        public bool? FileDialogResult { get; protected set; }

        protected static readonly DependencyProperty filePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(FileDialogBox));
        protected static readonly DependencyProperty filterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(FileDialogBox));
        protected static readonly DependencyProperty filterIndexProperty = DependencyProperty.Register("filterIndex", typeof(int), typeof(FileDialogBox));
        protected static readonly DependencyProperty defaultExtensionProperty = DependencyProperty.Register("DefaultExtension", typeof(string), typeof(FileDialogBox));

        public string FilePath
        {
            get { return (string)GetValue(filePathProperty); }
            set { SetValue(filePathProperty, value); }
        }
        public string Filter
        {
            get { return (string)GetValue(filterProperty); }
            set { SetValue(filterProperty, value); }
        }
        public int FilterIndex
        {
            get { return (int)GetValue(filterIndexProperty); }
            set { SetValue(filterIndexProperty, value); }
        }
        public string DefaultExtension
        {
            get { return (string)GetValue(defaultExtensionProperty); }
            set { SetValue(defaultExtensionProperty, value); }
        }

        protected Microsoft.Win32.FileDialog fileDialog = null;

        protected FileDialogBox()
        {
            execute = o =>
            {
                fileDialog.Title = Caption;
                fileDialog.Filter = Filter;
                fileDialog.FilterIndex = FilterIndex;
                fileDialog.DefaultExt = DefaultExtension;
                string filePath = "";
                if (FilePath != null)
                {
                    filePath = FilePath;
                }
                else
                {
                    FilePath = "";
                }
                if (o != null)
                {
                    filePath = (string)o;
                }
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    fileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(filePath);
                    fileDialog.FileName = System.IO.Path.GetFileName(filePath);
                }
                FileDialogResult = fileDialog.ShowDialog();
                OnPropertyChanged("FileDialogResult");
                if (FileDialogResult.HasValue && FileDialogResult.Value)
                {
                    FilePath = fileDialog.FileName;
                    OnPropertyChanged("FilePath");
                    object commandParameter = CommandParameter;
                    if (commandParameter == null)
                    {
                        commandParameter = FilePath;
                    }
                    executeCommand(CommandFileOK, commandParameter);
                };
            };
        }

        protected static readonly DependencyProperty CommandFileOKProperty = DependencyProperty.Register
            ("CommandFileOK", typeof(ICommand), typeof(FileDialogBox));

        public ICommand CommandFileOK
        {
            get { return (ICommand)GetValue(CommandFileOKProperty); }
            set { SetValue(CommandFileOKProperty, value); }
        }
    }

    public class OpenFileDialogBox : FileDialogBox
    {
        public OpenFileDialogBox()
        {
            fileDialog = new Microsoft.Win32.OpenFileDialog();
        }
    }

    public class SaveFileDialogBox : FileDialogBox
    {
        public SaveFileDialogBox()
        {
            fileDialog = new Microsoft.Win32.SaveFileDialog();
        }
    }

    [ContentProperty("WindowContent")]
    public class CustomContentDialogBox : CommandDialogBox
    {
        bool? LastResult;

        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public object WindowContent { get; set; }
        public ResizeMode WindowResizeMode { get; set; } = ResizeMode.NoResize;
        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;

        private static Window window = null;

        public CustomContentDialogBox()
        {
            execute =
                o =>
                {
                    if (window == null)
                    {
                        window = new Window();
                        window.Width = WindowWidth;
                        window.Height = WindowHeight;
                        window.ResizeMode = WindowResizeMode;
                        window.WindowStyle = WindowStyle;
                        window.WindowStartupLocation = WindowStartupLocation;
                        if (Caption != null)
                        {
                            window.Title = Caption;
                        }
                        window.Content = WindowContent;
                        LastResult = window.ShowDialog();
                        OnPropertyChanged("LastResult");
                        window = null;
                        switch (LastResult)
                        {
                            case true:
                                executeCommand(CommandTrue, CommandParameter);
                                break;
                            case false:
                                executeCommand(CommandFalse, CommandParameter);
                                break;
                            case null:
                                executeCommand(CommandNull, CommandParameter);
                                break;
                        }
                    }
                };
        }

        public static bool? GetCustomContentDialogResult(DependencyObject d)
        {
            return (bool?)d.GetValue(DialogResultProperty);
        }

        public static void SetCustomContentDialogResult(DependencyObject d, bool? value)
        {
            d.SetValue(DialogResultProperty, value);
        }

        public static readonly DependencyProperty DialogResultProperty =  DependencyProperty.RegisterAttached(
                "DialogResult", typeof(bool?), typeof(CustomContentDialogBox), new PropertyMetadata(null, DialogResultChanged));

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool? dialogResult = (bool?)e.NewValue;
            if (d is Button)
            {
                Button button = d as Button;
                button.Click += (object sender, RoutedEventArgs _e) => { window.DialogResult = dialogResult; };
            }
        }

        public static DependencyProperty CommandTrueProperty = DependencyProperty.Register(
            "CommandTrue", typeof(ICommand), typeof(CustomContentDialogBox));
        public static DependencyProperty CommandFalseProperty = DependencyProperty.Register(
            "CommandFalse", typeof(ICommand), typeof(CustomContentDialogBox));
        public static DependencyProperty CommandNullProperty = DependencyProperty.Register(
            "CommandNull", typeof(ICommand), typeof(CustomContentDialogBox));

        public ICommand CommandTrue
        {
            get
            {
                return (ICommand)GetValue(CommandTrueProperty);
            }
            set
            {
                SetValue(CommandTrueProperty, value);
            }
        }

        public ICommand CommandFalse
        {
            get
            {
                return (ICommand)GetValue(CommandFalseProperty);
            }
            set
            {
                SetValue(CommandFalseProperty, value);
            }
        }

        public ICommand CommandNull
        {
            get
            {
                return (ICommand)GetValue(CommandNullProperty);
            }
            set
            {
                SetValue(CommandNullProperty, value);
            }
        }
    }
}
