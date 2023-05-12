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
    public partial class FormDapAn : Form
    {
        public FormDapAn()
        {
            InitializeComponent();
        }
        public void NhanDiem(TextBox txtFormlb)
        { textBox1.Text = txtFormlb.Text; }
        public void NhanCauDaLam(TextBox txtFormlb)
        { textBox3.Text = txtFormlb.Text;} 
        public void NhanTDiem(TextBox txtFormlb)
        { textBox5.Text = txtFormlb.Text; }
        public void NhanXLoai(string text)
        { textBox4.Text = text; }

        private void FormDapAn_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
