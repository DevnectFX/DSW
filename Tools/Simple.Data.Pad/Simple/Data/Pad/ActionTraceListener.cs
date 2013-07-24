namespace Simple.Data.Pad
{
    using System;
    using System.Diagnostics;

    internal class ActionTraceListener : TraceListener
    {
        private readonly Action<string> _action;

        public ActionTraceListener(Action<string> action)
        {
            this._action = action;
        }

        public override void Write(string message)
        {
            this._action(message);
        }

        public override void WriteLine(string message)
        {
            this._action(message + Environment.NewLine);
        }
    }
}

