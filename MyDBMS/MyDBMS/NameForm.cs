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
    public partial class NameForm : Form
    {
        public NameForm()
        {
            InitializeComponent();
        }

        private void NameForm_Load(object sender, EventArgs e)
        {
            Form1.fmain.Enabled = false;
        }

        private void NameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.fmain.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Form1.fmain.saveNewTable(txtbName.Text);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
