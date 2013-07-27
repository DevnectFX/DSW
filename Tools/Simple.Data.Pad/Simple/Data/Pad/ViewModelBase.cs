namespace Simple.Data.Pad
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged.Raise(this, propertyName);
        }

        protected bool Set<T>(ref T field, T value, string propertyName)
        {
            if (object.Equals((T) field, value))
            {
                return false;
            }
            field = value;
            this.PropertyChanged.Raise(this, propertyName);
            return true;
        }
    }
}

