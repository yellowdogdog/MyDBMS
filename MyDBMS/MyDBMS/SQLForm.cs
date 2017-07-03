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
    public partial class SQLForm : Form
    {
        public SQLForm()
        {
            InitializeComponent();
        }

        private void btnExcute_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable dt = SQLreader.readsql(rtbSQL.Text);
                dataGridView1.DataSource = dt;
                rtbSQL.Text = "";
            }
            catch (TableEditException tableE)
            {
                MessageBox.Show(tableE.Message, "表管理错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(DataEditException dataE)
            {
                MessageBox.Show(dataE.Message, "数据操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbSQL.Text = "";
        }

        private void SQLForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.fmain.Show();
        }
    }
}
