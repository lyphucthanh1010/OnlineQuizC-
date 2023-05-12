using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Forms;

namespace Project_Final_Semester__NT106_
{
    public partial class FormDieuLenh : Form
    {
        struct Clientinfo
        {
            public int ipaddr;
            public string pot;
        };
        struct ChonClient
        {
            public int t;
            public string Portclient;
        };
        string conStringCauHoiDoAn = "Data Source=LAPTOP-PHUCTHANH;Initial Catalog=CAUHOIDA;Integrated Security=True";
        string conStringCauHoiNMM = "Data Source=LAPTOP-PHUCTHANH;Initial Catalog=CAUHOINMM;Integrated Security=True";
        string conStringCauHoiKTMT = "Data Source=LAPTOP-PHUCTHANH;Initial Catalog=CAUHOIKTMT;Integrated Security=True";
        string conStringCauHoiKTCT = "Data Source=LAPTOP-PHUCTHANH;Initial Catalog=CAUHOIKTCT;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        string data = "", data2 = "";
        IPEndPoint ipep;
        int fowardingcheck = 0;
        SqlDataAdapter sqlDa;
        DataTable dtbl;
        List<Clientinfo> listclientinfo = new List<Clientinfo>();
        List<Socket> listsocket = new List<Socket>();
        TcpListener serverListener;
        Socket recv;
        IPAddress ipadd;
        int socnum = -1;
        public FormDieuLenh()
        {
            InitializeComponent();
            Connect();
        }
        Clientinfo AddClientinfo(int ipaddress, string ip)
        {
            Clientinfo cliinfo = new Clientinfo();
            cliinfo.ipaddr = ipaddress;
            string portclient = ip.Substring(ip.IndexOf(":") + 1);
            cliinfo.pot = portclient;
            return cliinfo;
        }
        public delegate void SafeCallThreadSafe(Control ctrl, string text);
        public void Connect()
        {
            try
            {
                ipep = new IPEndPoint(IPAddress.Any, 9090);
                serverListener = new TcpListener(ipep);
                Thread thr = new Thread(() =>
                {
                    while (true)
                    {
                        serverListener.Start();
                        socnum++;
                        listsocket.Add(serverListener.AcceptSocket());
                        //IPBOX.Items.Add("Thisinh " + (socnum + 1));
                        UpdateThreadSafeCall(IPBOX, "Thisinh " + (socnum + 1));
                        listclientinfo.Add(AddClientinfo(socnum, listsocket[socnum].RemoteEndPoint.ToString()));
                        //chat.Text = "Chào mừng đến giao diện hỗ trợ ! Xin chào thí sinh " + (socnum + 1);
                        UpdateThreadSafeCall(chat, "Chào mừng đến giao diện hỗ trợ ! Xin chào thí sinh " + (socnum + 1));
                        //chat.Text = "Thí sinh " + socnum + " đã kết nối thành công\n";
                        UpdateThreadSafeCall(chat, "Thí sinh " + (socnum + 1).ToString() + " đã kết nối thành công\n");
                        Send(listsocket[socnum]);
                        Thread thr1 = new Thread(Receive);
                        thr1.IsBackground = true;
                        thr1.Start(listsocket[socnum]);
                    }
                });
                thr.IsBackground = true;
                thr.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateThreadSafeCall(Control ctrl, string v)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(new SafeCallThreadSafe(UpdateThreadSafeCall), new object[] { ctrl, v });
            else
            {
                if (ctrl is ComboBox)
                {
                    IPBOX.Items.Add(v);
                }
                else if (ctrl is RichTextBox)
                {
                    ctrl.Text += v;
                    messengeView.ScrollToCaret();
                }
                else
                {
                    ctrl.Text = v;
                }   
            }
        }

        void Receive(Object obj)
        {
            while(obj!=null)
            {
                recv = obj as Socket;
                byte[] recvdata = new byte[8192];
                recv.Receive(recvdata);
                NewReceive(Encoding.UTF8.GetString(recvdata));
            }
            
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void Form7_Load(object sender, EventArgs e)
        {
            fic = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in fic)
                comboBox1.Items.Add(fi.Name);
            comboBox1.SelectedIndex = 0;
            vcd = new VideoCaptureDevice();            
        }


        private void button18_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiDoAn))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiNMM))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiKTMT))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else if (comboBox3.SelectedIndex == 3)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiKTCT))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else
            {
                MessageBox.Show("Mời bạn chọn đề", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        FilterInfoCollection fic;
        VideoCaptureDevice vcd;    
        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
        private void button19_Click(object sender, EventArgs e)
        {
            vcd = new VideoCaptureDevice(fic[comboBox1.SelectedIndex].MonikerString);
            vcd.NewFrame += VCDNewFrame;
            vcd.Start();
            button19.Enabled = false;
            button3.Enabled = true;
            
        }
        private void VCDNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vcd.IsRunning == true)
                vcd.Stop();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                sqlCon = new SqlConnection(conStringCauHoiDoAn);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox1.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }
            else if(comboBox3.SelectedIndex ==1)
            {
                sqlCon = new SqlConnection(conStringCauHoiNMM);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox1.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }    
            else if(comboBox3.SelectedIndex ==2)
            {
                sqlCon = new SqlConnection(conStringCauHoiKTMT);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox1.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }    
            else if(comboBox3.SelectedIndex ==3)
            {
                sqlCon = new SqlConnection(conStringCauHoiKTCT);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox1.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }    
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                sqlCon = new SqlConnection(conStringCauHoiDoAn);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox4.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                sqlCon = new SqlConnection(conStringCauHoiNMM);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox4.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                sqlCon = new SqlConnection(conStringCauHoiKTMT);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox4.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }
            else if (comboBox3.SelectedIndex == 3)
            {
                sqlCon = new SqlConnection(conStringCauHoiKTCT);
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI WHERE MA_CH LIKE '" + textBox4.Text + "%'", sqlCon);
                dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                sqlCon.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiDoAn))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView3.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else if(comboBox2.SelectedIndex ==1)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiNMM))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView3.DataSource = dtbl;
                    sqlCon.Close();

                }
            }  
            else if(comboBox2.SelectedIndex ==2)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiKTMT))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView3.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else if (comboBox2.SelectedIndex ==3)
            {
                using (sqlCon = new SqlConnection(conStringCauHoiKTCT))
                {
                    sqlCon.Open();
                    sqlDa = new SqlDataAdapter("SELECT * FROM CAUHOI", sqlCon);
                    dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView3.DataSource = dtbl;
                    sqlCon.Close();

                }
            }
            else
            {
                MessageBox.Show("Mời bạn chọn đề", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
                
                
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int h = 0; h < listclientinfo.Count; h++)
            {
                if (int.Parse(IPBOX.Text.Substring(IPBOX.Text.IndexOf(" ") + 1)) - 1 == listclientinfo[h].ipaddr)
                {
                    if (socnum != -1)
                    {
                        if (comboBox2.SelectedIndex == 0)
                        {
                            for (int i = 1; i < 36; i++)
                            {
                                if (i == 0) continue;
                                sqlCon = new SqlConnection(conStringCauHoiDoAn);
                                sqlCon.Open();
                                if (i < 10)
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'H00" + i.ToString() + "'", sqlCon);
                                else
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'H0" + i.ToString() + "'", sqlCon);
                                //if (i == 1)
                                read = sqlCom.ExecuteReader();
                                while (read.Read())
                                {
                                    if (i == 1)
                                    {
                                        data = read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                    }
                                    else
                                        data += read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                }
                                data += ";";
                                sqlCon.Close();
                            }
                            //BF = new BinaryFormatter();
                            //BF.Serialize(NS, ListDT);


                            byte[] transByte = Encoding.UTF8.GetBytes(data);
                            recv.Send(transByte);
                        }
                        else if (comboBox2.SelectedIndex == 1)
                        {
                            for (int i = 1; i < 36; i++)
                            {
                                sqlCon = new SqlConnection(conStringCauHoiNMM);
                                sqlCon.Open();
                                if (i < 10)
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'N00" + i.ToString() + "'", sqlCon);
                                else
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'N0" + i.ToString() + "'", sqlCon);
                                //if(i == 1)
                                read = sqlCom.ExecuteReader();
                                while (read.Read())
                                {
                                    if (i == 1)
                                    {
                                        data = read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                    }
                                    else
                                        data += read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                }
                                data += ";";
                                sqlCon.Close();
                            }
                            //NS = new NetworkStream(Server);
                            //XS.Serialize(NS, ListDT);

                            byte[] transByte = Encoding.UTF8.GetBytes(data);
                            recv.Send(transByte);

                        }
                        else if (comboBox2.SelectedIndex == 2)
                        {
                            for (int i = 1; i < 36; i++)
                            {
                                sqlCon = new SqlConnection(conStringCauHoiKTMT);
                                sqlCon.Open();
                                if (i < 10)
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'M00" + i.ToString() + "'", sqlCon);
                                else
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'M0" + i.ToString() + "'", sqlCon);
                                //if(i == 1)
                                read = sqlCom.ExecuteReader();
                                while (read.Read())
                                {
                                    if (i == 1)
                                    {
                                        data = read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                    }
                                    else
                                        data += read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                }
                                data += ";";
                                sqlCon.Close();
                            }
                            //NS = new NetworkStream(Server);
                            //XS.Serialize(NS, ListDT);                 
                            byte[] transByte = Encoding.UTF8.GetBytes(data);
                            recv.Send(transByte);
                        }
                        else if (comboBox2.SelectedIndex == 3)
                        {
                            for (int i = 1; i < 36; i++)
                            {
                                sqlCon = new SqlConnection(conStringCauHoiKTCT);
                                sqlCon.Open();
                                if (i < 10)
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'K00" + i.ToString() + "'", sqlCon);
                                else
                                    sqlCom = new SqlCommand("SELECT * FROM CAUHOI WHERE MA_CH = 'K0" + i.ToString() + "'", sqlCon);
                                //if(i == 1)
                                read = sqlCom.ExecuteReader();
                                while (read.Read())
                                {
                                    if (i == 1)
                                    {
                                        data = read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                    }
                                    else
                                        data += read.GetString(1).Trim() + "@" + read.GetString(2).Trim() + "@" + read.GetString(3).Trim() + "@" +
                                          read.GetString(4).Trim() + "@" + read.GetString(5).Trim() + "@" + read.GetString(6).Trim();
                                }
                                data += ";";
                                sqlCon.Close();
                            }
                            //NS = new NetworkStream(Server);
                            //XS.Serialize(NS, ListDT);
                            byte[] transByte = Encoding.UTF8.GetBytes(data);
                            recv.Send(transByte);
                        }
                        else
                        {
                            MessageBox.Show("Mời bạn chọn đề", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }               
            }
            MessageBox.Show("Gửi đề thành công", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (vcd.IsRunning == true)
                vcd.Stop();
            pictureBox1.Image = null;
            button3.Enabled = false;
            button19.Enabled = true;
        }
        void NewReceive(string data)
        {
            try
            {
                if (data.Contains('$') == false)
                {
                    //Bỏ tin nhắn
                    string datainfo = data.Remove(data.IndexOf("::"));
                    //lấy tên của client
                    string datafrom = datainfo.Remove(datainfo.IndexOf("/"));
                    //Lấy thông tin địa chỉ IP, Port của Client và Port của Server
                    string datato = datainfo.Substring(datainfo.IndexOf("/") + 1);
                    //Lấy thông tin địa chỉ IP, Port của Client
                    string dataCliInfo = datainfo.Remove(datainfo.IndexOf("#"));
                    //Lấy số port của server từ client
                    string datatoport = datato.Substring(datato.IndexOf("#") + 1);
                    string serverport = serverListener.Server.LocalEndPoint.ToString().Substring(serverListener.Server.LocalEndPoint.ToString().IndexOf(":") + 1);
                    if (datatoport == serverport)
                    {
                        for (int h = 0; h < listclientinfo.Count; h++)
                        {
                            if (dataCliInfo.Substring(dataCliInfo.IndexOf(":") + 1) == listclientinfo[h].pot) /*messengeView.Text += "Thisinh " + (h + 1) + "(" + datafrom + ")" + ": ";*/
                                UpdateThreadSafeCall(messengeView, "Thisinh " + (h + 1) + "(" + datafrom + ")" + ": ");
                        }
                        UpdateThreadSafeCall(messengeView, data.Substring(data.IndexOf("::") + 2));
                        for (int h = 0; h < listclientinfo.Count; h++)
                        {
                            fowardingcheck = 2;
                            string temp = chat.Text;
                            UpdateThreadSafeCall(chat, data);
                            Send(listsocket[h]);
                            UpdateThreadSafeCall(chat, temp);
                            temp = null;
                        }
                    }
                }
                else
                {
                    string[] arrayInfoResRecv = data.Split('$');
                    UpdateThreadSafeCall(rtxtBoxRes, "Họ tên: " + arrayInfoResRecv[0] +
                        "\nMSSV: " + arrayInfoResRecv[1] + "\nLớp: " + arrayInfoResRecv[2] + "\nKhoa: " + arrayInfoResRecv[3]
                        + "\nĐáp án trả lời: ");
                    string[] arrayAnswerRecv = arrayInfoResRecv[4].Split(';');
                    for(int i = 0;i < arrayAnswerRecv.Length;i++)
                    {
                        rtxtBoxRes.Text += arrayAnswerRecv[i] + "\t";
                    }
                    UpdateThreadSafeCall(rtxtBoxRes, "\nSố câu đã làm: " + arrayInfoResRecv[5] + "\nSố câu làm đúng: " + arrayInfoResRecv[6] +
                        "\nĐiểm: " + arrayInfoResRecv[7] + "\nXếp loại: " + arrayInfoResRecv[8] +
                        "\nThời gian làm bài: " + TimeLB(arrayInfoResRecv[9]) + "\n\n");
                    //Lưu vào database
                    SqlConnection sqlConstring = new SqlConnection("Data Source=LAPTOP-NHNMFFBB;Initial Catalog=LuuKQuaClient;Integrated Security=True");
                    SqlCommand commandSave;
                    sqlConstring.Open();
                    commandSave = new SqlCommand("INSERT INTO KETQUA VALUES('" + arrayInfoResRecv[1] + "','" + arrayInfoResRecv[0]
                        + "','" + arrayInfoResRecv[2] + "','" + arrayInfoResRecv[3] + "','" + arrayInfoResRecv[5] + "','" + arrayInfoResRecv[6]
                        + "','" + arrayInfoResRecv[7] + "','" + arrayInfoResRecv[8] + "','" + TimeLB(arrayInfoResRecv[9]) + "')",sqlConstring);
                    commandSave.ExecuteNonQuery();
                    MessageBox.Show("Lưu thông tin kết quả của sinh viên " + arrayInfoResRecv[0] + " thành công", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConstring.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"THÔNG BÁO!!!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private string TimeLB(string v)
        {
            string _returnString = "";
            int countTime = int.Parse(v);
            if (countTime == 3600)
                _returnString = "60 phút";
            else
            {
                if (countTime >= 60 && countTime < 3600)
                {
                    int minute = countTime / 60;
                    int second = countTime % 60;
                    _returnString = minute.ToString() + " phút " + second.ToString() + " giây";
                }
                else
                    _returnString = countTime.ToString() + " giây";
            }    
            return _returnString;
        }

        void Send(Socket sendsock)
        {
            string s = "";
            //fowardingcheck = 0: server gửi tin nhắn server cho Client
            if (fowardingcheck == 0) s = "Admin" + "::" + chat.Text + "\n";
            else s = chat.Text;
            byte[] data = Encoding.UTF8.GetBytes(s);
            if (sendsock != null)
            {
                if (sendsock.Connected) sendsock.Send(data);
                //Server hiện tin nhắn của server
                if (fowardingcheck != 2) 
                    UpdateThreadSafeCall(messengeView, "Me: " + chat.Text + "\n");
                fowardingcheck = 0;
                UpdateThreadSafeCall(chat, null);
            }
            else/* messengeView.Text += ("Hệ thống: Không có thí sinh nào được kết nối \n")*/
                UpdateThreadSafeCall(messengeView, "Hệ thống: Không có thí sinh nào được kết nối \n");
        }

        private void senbt_Click(object sender, EventArgs e)
        {
            if (IPBOX.Text != "" && IPBOX.Text != "Chọn...")
            {
                for (int h = 0; h < listclientinfo.Count; h++)
                {
                    if (int.Parse(IPBOX.Text.Substring(IPBOX.Text.IndexOf(" ") + 1)) - 1 == listclientinfo[h].ipaddr)
                        if (socnum != -1) Send(listsocket[h]);
                }
            }
            else
                UpdateThreadSafeCall(messengeView, "Chọn thí sinh để nhắn tin\n");
        }
    }
}
