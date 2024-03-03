using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace paktool
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            _lblTitle.Text = MainForm.Title;
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
