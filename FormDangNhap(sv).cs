using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Project_Final_Semester__NT106_
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormMoDau ChonChucNang = new FormMoDau();
            this.Visible = false;
            ChonChucNang.Show();
        }
        public delegate void ChuyenTK(TextBox text);
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress newip;
            int newport;
           
            try
            {
               
                if (IPAddress.TryParse(textBox1.Text, out newip) == false || int.TryParse(textBox2.Text, out newport) == false)
                {

                }
                else
                {
                    FormDienThongTin dtt = new FormDienThongTin(newip, newport);
                    dtt.Show();
                    this.Visible = false;
                }
                //IPAddress newip = IPAddress.Parse(textBox1.Text);
                //int newport = int.Parse(textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);        
            }
            
        }
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            textBox1.Text = GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            if (string.IsNullOrEmpty(textBox1.Text))
            {
               textBox1.Text = GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }
    }
}
