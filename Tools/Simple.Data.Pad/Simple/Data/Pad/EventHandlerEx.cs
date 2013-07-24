namespace Simple.Data.Pad
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal static class EventHandlerEx
    {
        public static void Raise(this PropertyChangedEventHandler handler, object sender, string propertyName)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

