using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO; 
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Project_Final_Semester__NT106_
{
    public partial class Form6 : Form
    {   
        public Form6()
        {
            InitializeComponent();
        }
        // tài khoản là admin , mật khẩu là admin
        private void button2_Click(object sender, EventArgs e)
        {
            FormMoDau ChonChucNang = new FormMoDau();
            this.Visible = false;
            ChonChucNang.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="admin" && textBox2.Text=="admin")
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormDieuLenh fdl = new FormDieuLenh();
                fdl.Show();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Thông tin sai, vui lòng nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (textBox2.PasswordChar == '*')
                {
                    textBox2.PasswordChar = '\0';
                }
            }
            else
            {
                if (textBox2.PasswordChar == '\0')
                {
                    textBox2.PasswordChar = '*';
                }
            }
        }       
        
    }
}
