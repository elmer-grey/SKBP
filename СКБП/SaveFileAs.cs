using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace СКБП
{
    public partial class SaveFileAs : Form
    {
        public SaveFileAs()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Data.Check0 = CheckKoef0.Checked;
            Data.Check1 = CheckKoef1.Checked;
            Data.Check2 = CheckKoef2.Checked;
            Data.Check3 = CheckKoef3.Checked;
            Close();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            CheckKoef0.Checked = false;
            CheckKoef1.Checked = false;
            CheckKoef2.Checked = false;
            CheckKoef3.Checked = false;
        }      
    }
}
