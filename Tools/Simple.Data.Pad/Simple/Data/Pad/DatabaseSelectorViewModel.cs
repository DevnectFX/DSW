using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Simple.Data.Pad
{
    public class DatabaseSelectorViewModel : ViewModelBase
    {
        private static readonly MethodInfo[] DatabaseOpenerMethods = DatabaseSelectorViewModel.GetDatabaseOpenerMethods();
        private MethodInfo _selectedMethod;
        private string _parameter1;
        private string _parameter2;
        private bool _hasParameter1;
        private bool _hasParameter2;
        public IEnumerable<MethodInfo> Methods
        {
            get
            {
                return DatabaseSelectorViewModel.DatabaseOpenerMethods;
            }
        }
        public MethodInfo SelectedMethod
        {
            get
            {
                return this._selectedMethod;
            }
            set
            {
                if (base.Set<MethodInfo>(ref this._selectedMethod, value, "SelectedMethod"))
                {
                    this.SetParameterVisibility();
                }
            }
        }
        public string Parameter1
        {
            get
            {
                return this._parameter1;
            }
            set
            {
                base.Set<string>(ref this._parameter1, value, "Parameter1");
            }
        }
        public string Parameter2
        {
            get
            {
                return this._parameter2;
            }
            set
            {
                base.Set<string>(ref this._parameter2, value, "Parameter2");
            }
        }
        public bool HasParameter1
        {
            get
            {
                return this._hasParameter1;
            }
            set
            {
                base.Set<bool>(ref this._hasParameter1, value, "HasParameter1");
            }
        }
        public bool HasParameter2
        {
            get
            {
                return this._hasParameter2;
            }
            set
            {
                base.Set<bool>(ref this._hasParameter2, value, "HasParameter2");
            }
        }
        private static MethodInfo[] GetDatabaseOpenerMethods()
        {
            return typeof(IDatabaseOpener).GetMethods().Where(delegate(MethodInfo m)
            {
                if (m.Name.StartsWith("Open"))
                {
                    return m.GetParameters().All((ParameterInfo p) => p.ParameterType == typeof(string));
                }
                return false;
            }).ToArray<MethodInfo>();
        }
        private void SetParameterVisibility()
        {
            if (this.SelectedMethod != null)
            {
                ParameterInfo[] parameters = this.SelectedMethod.GetParameters();
                this.HasParameter1 = (parameters.Length > 0);
                if (this.HasParameter1)
                {
                    this.Parameter1 = parameters[0].Name;
                }
                this.HasParameter2 = (parameters.Length > 1);
                if (this.HasParameter2)
                {
                    this.Parameter2 = parameters[1].Name;
                }
            }
        }
        public DatabaseSelectorViewModel()
        {
            this.SelectedMethod = (
                from m in DatabaseSelectorViewModel.DatabaseOpenerMethods
                where m.Name.Equals("OpenConnection")
                orderby m.GetParameters().Length
                select m).FirstOrDefault<MethodInfo>();
        }
    }
}
