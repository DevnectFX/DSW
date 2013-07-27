using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
namespace Simple.Data.Pad
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class MainWindow : Window, IComponentConnector
    {
        private static readonly string[] NewlineSplitArg = new string[]
		{
			Environment.NewLine
		};
        private readonly Typeface _typeface;
        private readonly MainViewModel _viewModel;
        private bool _deferKeyUp;
        internal TextBox QueryTextBox;
        internal Popup AutoCompletePopup;
        internal ListBox AutoCompleteListBox;
        private bool _contentLoaded;
        public MainWindow()
        {
            this.InitializeComponent();
            base.DataContext = (this._viewModel = new MainViewModel());
            this._viewModel.PropertyChanged += new PropertyChangedEventHandler(this.ViewModelPropertyChanged);
            this._typeface = new Typeface(this.QueryTextBox.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            this.AutoCompletePopup.Opened += new EventHandler(this.AutoCompletePopupOpened);
            this.AutoCompletePopup.Closed += new EventHandler(this.AutoCompletePopupClosed);
        }
        private void AutoCompletePopupClosed(object sender, EventArgs e)
        {
            this.QueryTextBox.PreviewKeyUp -= new KeyEventHandler(this.MainWindowPreviewKeyUp);
            this.QueryTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.QueryTextBoxPreviewTextInput);
        }
        private void AutoCompletePopupOpened(object sender, EventArgs e)
        {
            this._deferKeyUp = true;
            this.QueryTextBox.PreviewKeyUp += new KeyEventHandler(this.MainWindowPreviewKeyUp);
            this.QueryTextBox.PreviewTextInput += new TextCompositionEventHandler(this.QueryTextBoxPreviewTextInput);
        }
        private void QueryTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "." || e.Text == "(")
            {
                this.SelectAutoCompleteText(e.Text);
                e.Handled = true;
            }
        }
        private void MainWindowPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (this._deferKeyUp)
            {
                this._deferKeyUp = false;
                return;
            }
            if (e.Key == Key.Down)
            {
                if (this.AutoCompleteListBox.SelectedIndex + 1 < this.AutoCompleteListBox.Items.Count)
                {
                    this.AutoCompleteListBox.SelectedIndex++;
                }
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {
                if (this.AutoCompleteListBox.SelectedIndex > 0)
                {
                    this.AutoCompleteListBox.SelectedIndex--;
                }
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Return || e.Key == Key.Tab)
            {
                this.SelectAutoCompleteText("");
                e.Handled = true;
            }
        }
        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AutoCompleteOptions")
            {
                if (!this._viewModel.AutoCompleteOptions.Any<string>())
                {
                    this.AutoCompletePopup.IsOpen = false;
                    return;
                }
                this.OpenPopup();
            }
        }
        private void OpenPopup()
        {
            bool wasOpen = this.AutoCompletePopup.IsOpen;
            string[] options = this._viewModel.AutoCompleteOptions.ToArray<string>();
            this.AutoCompletePopup.IsOpen = true;
            this.AutoCompletePopup.Width = (double)((
                from s in options
                select s.Length * 8).Max() + 8);
            this.AutoCompletePopup.Height = (double)(Math.Min(options.Length * 16 + 16, 200) + 8);
            string text = this._viewModel.QueryTextToCursor;
            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, this._typeface, this.QueryTextBox.FontSize, Brushes.Black);
            this.AutoCompletePopup.VerticalOffset = formattedText.Height + 4.0;
            text = text.Split(MainWindow.NewlineSplitArg, StringSplitOptions.None).Last<string>();
            formattedText = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, this._typeface, this.QueryTextBox.FontSize, Brushes.Black);
            this.AutoCompletePopup.HorizontalOffset = formattedText.Width;
            if (!wasOpen)
            {
                this.AutoCompleteListBox.SelectedIndex = 0;
            }
        }
        private void MainWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                this._viewModel.Run.Execute(null);
            }
        }
        private void QueryTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            this._viewModel.CursorPosition = this.QueryTextBox.CaretIndex;
        }
        private void QueryTextBoxTextChanged(object sender, RoutedEventArgs e)
        {
            this._viewModel.CursorPosition = this.QueryTextBox.CaretIndex;
        }
        private void ListBoxMouseUp(object sender, RoutedEventArgs e)
        {
            this.SelectAutoCompleteText("");
        }
        private void SelectAutoCompleteText(string andAppend = "")
        {
            object selected = this.AutoCompleteListBox.SelectedItem;
            while (this.QueryTextBox.SelectedText.FirstOrDefault<char>() != '.')
            {
                this.QueryTextBox.SelectionStart--;
                this.QueryTextBox.SelectionLength++;
            }
            this.QueryTextBox.SelectedText = "." + selected + andAppend;
            this.QueryTextBox.SelectionStart += this.QueryTextBox.SelectionLength;
            this.QueryTextBox.SelectionLength = 0;
            this.QueryTextBox.CaretIndex = this.QueryTextBox.SelectionStart;
            this._viewModel.CursorPosition = this.QueryTextBox.CaretIndex;
            if (andAppend != ".")
            {
                this.AutoCompletePopup.IsOpen = false;
                return;
            }
            this._viewModel.ForceAutoCompleteOptionsUpdate();
        }
        private void AutoCompleteListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AutoCompleteListBox.SelectedItem == null)
            {
                this.AutoCompleteListBox.SelectedIndex = 0;
                return;
            }
            this.AutoCompleteListBox.ScrollIntoView(this.AutoCompleteListBox.SelectedItem);
        }
        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
            {
                return;
            }
            this._contentLoaded = true;
            Uri resourceLocater = new Uri("/Simple.Data.Pad;component/mainwindow.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocater);
        }
        [DebuggerNonUserCode]
        internal Delegate _CreateDelegate(Type delegateType, string handler)
        {
            return Delegate.CreateDelegate(delegateType, this, handler);
        }
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((MainWindow)target).KeyUp += new KeyEventHandler(this.MainWindowKeyUp);
                    return;
                case 2:
                    this.QueryTextBox = (TextBox)target;
                    this.QueryTextBox.TextChanged += new TextChangedEventHandler(this.QueryTextBoxTextChanged);
                    this.QueryTextBox.SelectionChanged += new RoutedEventHandler(this.QueryTextBoxSelectionChanged);
                    return;
                case 3:
                    this.AutoCompletePopup = (Popup)target;
                    return;
                case 4:
                    this.AutoCompleteListBox = (ListBox)target;
                    this.AutoCompleteListBox.MouseUp += new MouseButtonEventHandler(this.ListBoxMouseUp);
                    this.AutoCompleteListBox.SelectionChanged += new SelectionChangedEventHandler(this.AutoCompleteListBox_OnSelectionChanged);
                    return;
                default:
                    this._contentLoaded = true;
                    return;
            }
        }
    }
}
