using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.Sqlite;

namespace GenModel
{
    public partial class MainForm : Form
    {
        private ISchemaProvider schemaProvider;
        private IList<WrapTable> tableList = new List<WrapTable>();


        public MainForm()
        {
            InitializeComponent();
            SetTitle();
        }

        private void SetTitle()
        {
            Text = Application.ProductName + "(" + Application.ProductVersion + ")";
        }

        private bool ReloadTableList()
        {
            Simple.Data.Database db = Simple.Data.Database.OpenConnection(txtConnectionString.Text);
            var adoAdapter = db.GetAdapter() as AdoAdapter;
            schemaProvider = adoAdapter.SchemaProvider;

            this.tableList.Clear();
            try
            {
                var tableList = schemaProvider.GetTables();
                foreach (var table in tableList)
                    this.tableList.Add(new WrapTable() { Table = table });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void txtConnectionString_Leave(object sender, EventArgs e)
        {
            cmbTable.Items.Clear();

            var result = ReloadTableList();
            if (result == false)
            {
                //cmbTable.DroppedDown = false; //동작하지 않음
                errorProvider.SetError(txtConnectionString, "연결실패");
                return;
            }
            errorProvider.SetError(txtConnectionString, "");
            cmbTable.Items.AddRange(tableList.OrderBy(t => t.Table.ActualName).ToArray());
        }

        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            var result = GenerateModel((cmbTable.SelectedItem as WrapTable).Table);
            txtOutput.Text = result;
        }

        private string GenerateModel(Table table)
        {
            StringBuilder sb = new StringBuilder();
            var columnList = GetColumns(table);

            var className = table.ActualName;
            sb.AppendFormat("public class {0} : Model", className).AppendLine();
            sb.AppendLine("{");
            foreach (var column in columnList)
            {
                var propertyName = GetPrettyName(column.ActualName);
                var propertyType = column.DbType.ToClrType();
                sb.AppendFormat("   public {0} {1} {{ get; set; }}", propertyType, propertyName).AppendLine();
            }
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static String GetPrettyName(string name)
        {
            var sb = new StringBuilder();
            bool upperToggleFlag = true;
            foreach (var c in name)
            {
                if (c == '_')
                {
                    upperToggleFlag = true;
                    continue;
                }

                if (upperToggleFlag == true)
                {
                    upperToggleFlag = false;

                    sb.Append(c.ToString().ToUpper());
                }
                else
                {
                    sb.Append(c.ToString().ToLower());
                }
            }

            return sb.ToString();
        }

        private IEnumerable<Column> GetColumns(Table table)
        {
            return GetSchema("COLUMNS", new[] { null, null, table.ActualName })
                .Select(row =>
                    {
                        var dataType = row.Field<string>("DATA_TYPE");
                        DbType dbType = DbType.Object;
                        // TODO: long혹은 decimal에 해당하는 타입도 변환해줘야 한다.
                        if (dataType == "integer" || dataType == "int")
                            dbType = DbType.Int32;
                        else if (dataType == "varchar" || dataType == "char")
                            dbType = DbType.String;
                        else if (dataType == "date" || dataType == "time" || dataType == "datetime")
                            dbType = DbType.DateTime;

                        var column = new Column(row.Field<string>("COLUMN_NAME"), table, row.Field<bool>("AUTOINCREMENT"), dbType, 1);
                        return column;
                    }
                    );
        }

        private IEnumerable<DataRow> GetSchema(string collectionName, params string[] constraints)
        {
            var sqliteSchemaProvider = schemaProvider as SqliteSchemaProvider;
            using (var cn = sqliteSchemaProvider.ConnectionProvider.CreateConnection())
            {
                cn.Open();

                return cn.GetSchema(collectionName, constraints).AsEnumerable();
            }
        }
    }

    public class WrapTable
    {
        public Table Table { get; set; }

        public override string ToString()
        {
            return Table.ActualName;
        }
    }
}
