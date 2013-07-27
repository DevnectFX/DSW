using Simple.Data.Interop;
using Simple.Data.Pad.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
namespace Simple.Data.Pad
{
	public class MainViewModel : ViewModelBase
	{
		private readonly ICommand _runCommand;
		private readonly Timer _timer;
        private AutoCompleter _autoCompleter;
		private string _queryText = "";
		private readonly DatabaseSelectorViewModel _databaseSelectorViewModel;
		private string _resultText;
		private Color _resultColor;
		private bool _dataAvailable;
		private object _data;
		private string _traceOutput;
		public string WindowTitle
		{
			get
			{
				return string.Format("Simple.Data.Pad {0}", Assembly.GetAssembly(typeof(Database)).GetName().Version.ToString(3));
			}
		}
		public ICommand Run
		{
			get
			{
				return this._runCommand;
			}
		}
		public DatabaseSelectorViewModel DatabaseSelectorViewModel
		{
			get
			{
				return this._databaseSelectorViewModel;
			}
		}
		public int CursorPosition
		{
			get;
			set;
		}
		public string QueryText
		{
			get
			{
				return this._queryText;
			}
			set
			{
				if (base.Set<string>(ref this._queryText, value, "QueryText"))
				{
					base.RaisePropertyChanged("AutoCompleteOptions");
				}
			}
		}
		public string TraceOutput
		{
			get
			{
				return this._traceOutput;
			}
			set
			{
				base.Set<string>(ref this._traceOutput, value, "TraceOutput");
			}
		}
		public string ResultText
		{
			get
			{
				return this._resultText;
			}
			set
			{
				base.Set<string>(ref this._resultText, value, "ResultText");
			}
		}
		public Color ResultColor
		{
			get
			{
				return this._resultColor;
			}
			set
			{
				base.Set<Color>(ref this._resultColor, value, "ResultColor");
			}
		}
		public bool DataAvailable
		{
			get
			{
				return this._dataAvailable;
			}
			set
			{
				base.Set<bool>(ref this._dataAvailable, value, "DataAvailable");
			}
		}
		public object Data
		{
			get
			{
				return this._data;
			}
			set
			{
				base.Set<object>(ref this._data, value, "Data");
			}
		}
		public string QueryTextToCursor
		{
			get
			{
				return this.QueryText.Substring(0, Math.Min(this.CursorPosition + 1, this.QueryText.Length));
			}
		}
		public IEnumerable<string> AutoCompleteOptions
		{
			get
			{
				if (this.QueryText.Length >= this.CursorPosition)
				{
                    if (_autoCompleter == null)
                        return Enumerable.Empty<string>();

					return this._autoCompleter.GetOptions(this.QueryTextToCursor);
				}
				return Enumerable.Empty<string>();
			}
		}
		public MainViewModel()
		{
			this._databaseSelectorViewModel = new DatabaseSelectorViewModel();
			this.LoadSettings();
			this._databaseSelectorViewModel.PropertyChanged += new PropertyChangedEventHandler(this.DatabaseSelectorViewModelPropertyChanged);
			this._runCommand = new ActionCommand(new Action(this.RunImpl));
			this._timer = new Timer(500.0)
			{
				AutoReset = false
			};
			this._timer.Elapsed += new ElapsedEventHandler(this.TimerElapsed);
			Trace.Listeners.Add(new ActionTraceListener(delegate(string message)
			{
				this.TraceOutput += message;
			}));
		}
		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				this._autoCompleter = new AutoCompleter(this.CreateDatabase());
			}
			catch (Exception)
			{
				Trace.WriteLine("Failed to open database.");
			}
		}
		private void DatabaseSelectorViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this._timer.Stop();
			this._timer.Start();
		}
		private void LoadSettings()
		{
			if (Settings.Default.UpgradeRequired)
			{
				Settings.Default.Upgrade();
				Settings.Default.Save();
			}
			this._databaseSelectorViewModel.SelectedMethod = this._databaseSelectorViewModel.Methods.FirstOrDefault((MethodInfo m) => m.Name.Equals(Settings.Default.OpenMethod) && m.GetParameters().Length == Settings.Default.OpenMethodParameterCount);
			this._databaseSelectorViewModel.Parameter1 = Settings.Default.OpenMethodParameter1;
			this._databaseSelectorViewModel.Parameter2 = Settings.Default.OpenMethodParameter2;
			try
			{
				this._autoCompleter = new AutoCompleter(this.CreateDatabase());
			}
			catch (Exception)
			{
				Trace.WriteLine("Failed to open database.");
			}
		}
		private void SaveSettings()
		{
			Settings.Default.OpenMethod = this._databaseSelectorViewModel.SelectedMethod.Name;
			Settings.Default.OpenMethodParameterCount = this._databaseSelectorViewModel.SelectedMethod.GetParameters().Length;
			Settings.Default.OpenMethodParameter1 = this._databaseSelectorViewModel.Parameter1;
			Settings.Default.OpenMethodParameter2 = this._databaseSelectorViewModel.Parameter2;
			Settings.Default.Save();
		}
		public void ForceAutoCompleteOptionsUpdate()
		{
			base.RaisePropertyChanged("AutoCompleteOptions");
		}
		private void RunImpl()
		{
			this.SaveSettings();
			this.TraceOutput = string.Empty;
			Database database = this.CreateDatabase();
            if (database == null)
                return;
			QueryExecutor executor = new QueryExecutor(this._queryText);
			object result;
			this.DataAvailable = executor.CompileAndRun(database, out result);
			this.ResultColor = (this.DataAvailable ? Colors.Black : Colors.Red);
			this.Data = MainViewModel.FormatResult(result);
		}
		private Database CreateDatabase()
		{
            try
            {
                MethodInfo method = this.DatabaseSelectorViewModel.SelectedMethod;
                string[] parameters = this.BuildParameters(method);
                return method.Invoke(Database.Opener, parameters) as Database;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to open database. : " + e.Message);
                return null;
            }
		}
		private string[] BuildParameters(MethodInfo method)
		{
			string[] parameters;
			if (method.GetParameters().Length == 1)
			{
				parameters = new string[]
				{
					this.DatabaseSelectorViewModel.Parameter1
				};
			}
			else
			{
				if (method.GetParameters().Length == 2)
				{
					parameters = new string[]
					{
						this.DatabaseSelectorViewModel.Parameter1,
						this.DatabaseSelectorViewModel.Parameter2
					};
				}
				else
				{
					parameters = new string[0];
				}
			}
			return parameters;
		}
		private static object FormatResult(object result)
		{
			if (result == null)
			{
				return "No results found.";
			}
			if (result is SimpleRecord)
			{
				return MainViewModel.FormatDictionary(result as IDictionary<string, object>);
			}
			if (result is SimpleQuery)
			{
				return MainViewModel.FormatQuery(result as SimpleQuery);
			}
			return result.ToString();
		}
		private static object FormatQuery(SimpleQuery simpleQuery)
		{
			IList<object> list = simpleQuery.ToList();
			if (list.Count == 0)
			{
				return "No matching records.";
			}
			IDictionary<string, object> firstRow = list.FirstOrDefault<object>() as IDictionary<string, object>;
			if (firstRow == null)
			{
				throw new InvalidOperationException();
			}
			DataTable table = new DataTable();
			foreach (KeyValuePair<string, object> kvp in firstRow)
			{
				table.Columns.Add(kvp.Key);
			}
			foreach (IDictionary<string, object> row in list.Cast<IDictionary<string, object>>())
			{
				table.Rows.Add(row.Values.ToArray<object>());
			}
			return table.DefaultView;
		}
		private static object FormatDictionary(IEnumerable<KeyValuePair<string, object>> dictionary)
		{
			DataTable table = new DataTable();
			table.Columns.Add("Property");
			table.Columns.Add("Value");
			foreach (KeyValuePair<string, object> kvp in dictionary)
			{
				table.Rows.Add(new object[]
				{
					kvp.Key,
					kvp.Value
				});
			}
			return table.DefaultView;
		}
	}
}
