using MyDBMS.MyDB;
using MyDBMS.SQLReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDBMS
{
    public partial class Form1 : Form
    {
        public static Form1 fmain = null;
        public Form1()
        {
            InitializeComponent();
            bindTableData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    TableF tableF = TableF.getTableF();//获得DataF或者TableF的实例
            //    Table table = new Table("table1");//新建表实例
            //    Field field = new Field("col1", Field.Type.nChar, 6);//新建字段实例
            //    table.addField(field);//为表添加字段
            //    field = new Field("col2", Field.Type.Int);
            //    table.addField(field);
            //    tableF.addTable(table);//表文件中增加刚新建的表。
            //    DataTable dt = table.getEmptyTable();
            //    DataRow dr = dt.NewRow();
            //    dr[0] = "你好";
            //    dr[1] = 23333;
            //    dt.Rows.Add(dr);
            //    DataF df = DataF.getDataF();
            //    df.changeData("table1", dt);
            //}
            //catch (TableEditException te)
            //{
            //    MessageBox.Show(te.Message);

            //}
            //SQLreader.readsql("select a from b where c=d and e=f");


        }

        private void btnSQL_Click(object sender, EventArgs e)
        {
            SQLForm sqlForm = new SQLForm();
            fmain = this;
            sqlForm.Show();
            Hide();
        }

        private void bindTableData()
        {
            lbxTable.Items.Clear();
            TableF tableF = TableF.getTableF();
            for(int i = 0; i < tableF.tables.Count; i++)
            {
                lbxTable.Items.Add(tableF.tables[i].TableName);
            }
        }
        public void refreshAll()
        {
            bindTableData();
            lbxTable.SelectedIndex = -1;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (lbxTable.SelectedIndex == -1)
            {
                MessageBox.Show("请选择表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bindTableDetailData();
                bindValueData();
            }
        }
        private void bindValueData()
        {
            DataF dataF = DataF.getDataF();
            TableF table = TableF.getTableF();
            dgvValue.DataSource = dataF.datas[lbxTable.SelectedIndex].getData(table.tables[lbxTable.SelectedIndex]);
        }
        private void bindTableDetailData()
        {
            TableF tableF = TableF.getTableF();
            DataTable dt = getFieldTable();
            foreach(var field in tableF.tables[lbxTable.SelectedIndex].fields)
            {
                DataRow dr = dt.NewRow();
                dr[0] = field.FieldName;
                switch (field.type)
                {
                    case Field.Type.Bit:
                        dr[1] = "Bit";
                        break;
                    case Field.Type.Int:
                        dr[1] = "Int";
                        break;
                    case Field.Type.nChar:
                        dr[1] = "nChar[" + field.length + "]";
                        break;
                    case Field.Type.Real:
                        dr[1] = "Real";
                        break;
                }
                dr[2] = field.isNullable;
                dr[3] = field.isKey;
                dt.Rows.Add(dr);
            }
            dgvTableDetial.DataSource = dt;
            
            

        }
        private void btnDropTable_Click(object sender, EventArgs e)
        {
            if (lbxTable.SelectedIndex == -1)
            {
                MessageBox.Show("请选择表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TableF tableF = TableF.getTableF();
                tableF.deleteTable(lbxTable.SelectedItem.ToString());
                refreshAll();
            }
        }

        private void btnSaveValue_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvValue.DataSource;

            if (lbxTable.SelectedIndex == -1)
            {
                //MessageBox.Show("请选择表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataF dataF = DataF.getDataF();
                try
                {
                    dataF.changeData(lbxTable.SelectedItem.ToString(), dt);
                    refreshAll();
                }
                catch(DataEditException dataE)
            {
                MessageBox.Show(dataE.Message, "数据操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
            
        }
        private static DataTable getFieldTable()
        {
            DataTable dt = new DataTable();
            #region 设置表格式
            DataColumn dc = new DataColumn("列名");
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);
            dc = new DataColumn("类型");
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);
            dc = new DataColumn("允许空值");
            dc.DataType = typeof(bool);
            dt.Columns.Add(dc);
            dc = new DataColumn("是否主键");
            dc.DataType = typeof(bool);
            dt.Columns.Add(dc);
            #endregion
            return dt;
        }

        private void btnSaveTable_Click(object sender, EventArgs e)
        {
            fmain = this;
            if (lbxTable.SelectedIndex == -1)
            {
                NameForm nameform = new NameForm();
                nameform.Show();
            }
            else
            {
                
            }
        }
        public void saveNewTable(string name)
        {

        }
        //private void saveTable(DataTable dt,string name)
        //{
        //    bool hasKey = false;
        //    Table table = new Table(name);
        //    for(int i = 0; i < dt.Rows.Count; i++) {
        //        string fname = (string)dt.Rows[i][0];
        //        string types = (string)dt.Rows[i][1];
        //        types = types.ToLower();
        //        switch (types[0])
        //        {
        //            case 'i':
        //                break;
        //            case 'n':
        //                break;
        //            case 'r':
        //                break;
        //            case 'b':
        //                break;
        //        }
        //        Field field = new Field();
        //    }
        //}
    }
}
