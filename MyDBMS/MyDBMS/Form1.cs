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
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
