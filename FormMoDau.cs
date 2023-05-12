using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Final_Semester__NT106_
{
    public partial class FormMoDau : Form
    {
        public FormMoDau()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("THÔNG TIN NGƯỜI THỰC HIỆN: \n Lý Phúc Thành (20521916) \n Cao Anh Khoa (20521462) \n Trần Đại Dương (20521226) \n NT106.M21.MMCL", "Thông tin người thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Form2 HocSinh = new Form2();
          
            HocSinh.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 Admin = new Form6();

            Admin.Show();
        }
    }
}
