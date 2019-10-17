using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssetsHelper.DBHelper;

namespace BatchTransfer
{
    public partial class Index : Form
    {
        public Index()
        {
            InitializeComponent();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            var test = SqlHelper.GetTest();
            string[] arr = new string[] { "请选择要转移的城市", "北京", "上海", "深圳" };
            for (int i = 0; i < arr.Length; i++)
            {
                this .SourceCityList.Items.Add(arr[i]);
                this.SourceCityList.SelectedIndex = 0;
            }
            AddColumn();

        }

        private void AddColumn()
        {
            DataTable myTable = new DataTable();

            // Add a new DataColumn to the DataTable.
            DataColumn myColumn = new DataColumn("myTextBoxColumn");
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.DefaultValue = "default string";
            myTable.Columns.Add(myColumn);
            // Get the CurrencyManager for the DataTable.
            CurrencyManager cm = (CurrencyManager)this.BindingContext[myTable];
            // Use the CurrencyManager to get the PropertyDescriptor for the new column.
            PropertyDescriptor pd = cm.GetItemProperties()["myTextBoxColumn"];
            DataGridTextBoxColumn myColumnTextColumn;
            // Create the DataGridTextBoxColumn with the PropertyDescriptor.
            myColumnTextColumn = new DataGridTextBoxColumn(pd);
            // Add the new DataGridColumn to the GridColumnsCollection.
            this.SourceTable.DataSource = myTable;
            //this.SourceTable.TableStyles.Add(new DataGridTableStyle());
            //this.SourceTable.TableStyles[0].GridColumnStyles.Add(myColumnTextColumn);
        }
    }
}
