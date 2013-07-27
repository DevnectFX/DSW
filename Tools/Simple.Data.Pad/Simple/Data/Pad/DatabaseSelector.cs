namespace Simple.Data.Pad
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class DatabaseSelector : UserControl, System.Windows.Markup.IComponentConnector
    {
        private bool _contentLoaded;

        public DatabaseSelector()
        {
            this.InitializeComponent();
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Simple.Data.Pad;component/databaseselector.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }
    }
}

