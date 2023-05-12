using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Project_Final_Semester__NT106_
{
    
    public partial class FormDienThongTin : Form
    {
        string corrAnswer = "", XepLoai;
        double SoCauDung = 0, Score, SoCauDaLam = 0;
        int[] countCheckedQues = new int[40];
        string[] AnsTest = new string[40];
        string[] ChamDiem = new string[40];
        //Ktra xem câu đó đã chấm chưa
        string[] checkedPoint = new string[40];
        //Ktra xem câu đó client đã làm chưa
        string[] checkedQuestion = new string[40];
        //Lưu câu trả lời các câu hỏi của client
        string[,] Luu = new string[40, 4];
        //Gửi câu trả lời kèm điểm, số câu đúng, số câu đã làm
        string[] sendAns = new string[40];
        int SoCauHoi = 0;
        string dataSend = "";
        // Ketnoi kn;
        List<ThongTin> DanhSach = new List<ThongTin>();
        public FormDienThongTin(IPAddress ipser,int portser)
        {
            ipServer = ipser;
            port = portser;
            InitializeComponent();
            connect();
        }

        public FormDienThongTin()
        {

        }       
        IPAddress ipServer;
        int port;
        IPEndPoint ipep;
        Stream RecvStream;
        TcpClient cli;
        string recvToString;
        bool recvState = true;
        public delegate void SafeCallThreadSafe(Control ctrl, string text);
        public void connect()
        {
            try
            {
                ipep = new IPEndPoint(ipServer, port);
                cli = new TcpClient();
                cli.Connect(ipep);
                string s = "Thí sinh có đăng nhập thành công";
                byte[] testing = Encoding.UTF8.GetBytes(s);
                cli.Client.Send(testing);
                RecvStream = cli.GetStream();
                Thread receiveThread = new Thread(receive);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                //ThemNguoiDung();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void receive()
        {
            try
            {
                while (RecvStream.CanRead)
                {
                    byte[] receivebyte = new byte[9999];
                    RecvStream.Read(receivebyte, 0, receivebyte.Length);
                    recvToString = Encoding.UTF8.GetString(receivebyte);
                    if (recvToString.Contains("@") == false)
                        receiveWhat(recvToString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void receiveWhat(string text)
        {
            UpdateThreadSafeCall(messengeView1, text);
        }

        private void UpdateThreadSafeCall(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(new SafeCallThreadSafe(UpdateThreadSafeCall), new object[] { ctrl, text });
            else
            {
                if (ctrl is RichTextBox)
                {
                    ctrl.Text += text;
                }
                else
                {
                    ctrl.Text = text;
                }
            }
        }

        private void HienThi()
        { 
            StringBuilder sb = new StringBuilder();
            foreach (ThongTin sv in DanhSach)
            {
                sb.Append(sv);
                sb.AppendLine();
            }
            richTextBox1.Text = sb.ToString();
            richTextBox1.Text = "Thông tin của bạn: " + "\n" + richTextBox1.Text.ToUpper();
        }
        void Send()
        {
            if (cli != null)
            {
                string s = textBox1.Text + "/" + cli.Client.LocalEndPoint.ToString() + "#" + port.ToString() + "::" + chat1.Text + "\n";
                byte[] data = Encoding.UTF8.GetBytes(s);
                RecvStream.Write(data, 0, data.Length);
                chat1.Text = "";
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ThongTin tt = new ThongTin();
                tt.Ten = textBox1.Text;
                tt.Lop = textBox3.Text;
                tt.Khoa = textBox2.Text;
                tt.MSSV = int.Parse(textBox4.Text);
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
                {
                    if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "" || textBox4.Text != "")
                    {
                        if (tt.MSSV.ToString().Length == 8 && textBox4.Text[2] == '5' && textBox4.Text[3] == '2')
                        {

                            if (textBox3.Text.Length == 8)
                            {
                                DanhSach.Add(tt);
                                HienThi();
                                button4.Visible = true;
                                senbt1.Visible = true;
                                chat1.Visible = true;
                            }
                            else
                            {
                                throw new Exception("Mời nhập lại lớp của bạn");
                            }
                        }
                        else
                        {
                            throw new Exception("Mời nhập lại MSSV");
                        }
                    }
                    else
                    {
                        throw new Exception("Mời nhập đầy đủ thông tin");
                    }
                }
                else
                {
                    throw new Exception("Mời nhập đầy đủ thông tin");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        public delegate void ChuyenMSSV(TextBox text);
        public delegate void ChuyenTen(TextBox text);
        public delegate void ChuyenLop(TextBox text);
        public delegate void ChuyenKhoa(TextBox text);
        public delegate void ChuyenMon(TextBox text);

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                LoadCauHoi();
                if (textBox10.Text == "")
                    throw new Exception("CHƯA CÓ ĐỀ THI, VUI LÒNG CHỜ");
                textBox6.Text = textBox1.Text;
                textBox9.Text = textBox4.Text;
                textBox8.Text = textBox3.Text;
                textBox7.Text = textBox2.Text;
                panel1.Visible = true;
                timer1.Start();
                button8.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                textBox3.Enabled = true;
            }
            else
            {
                MessageBox.Show("Bạn hãy nhập tên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
                
                
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                textBox4.Enabled = true;
            }
            else
            {
                MessageBox.Show("Bạn hãy nhập Khoa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                textBox2.Enabled = true;
            }
            else
            {
                MessageBox.Show("Bạn hãy nhập Lớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("Bạn hãy nhập MSSV", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void senbt1_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void cntbt_Click(object sender, EventArgs e)
        {

        }
        public delegate void ChuyenThongBao(TextBox text);
        public delegate void ChuyenDiem(TextBox text);
        public delegate void ChuyenCauSai(TextBox text);
        public delegate void ChuyenCauDaLam(TextBox text);
        public delegate void ChuyenTDiem(TextBox text);
        public delegate void ChuyenXLoai(string text);
        private void button6_Click(object sender, EventArgs e)
        {
            FormDapAn DapAn = new FormDapAn();
            //Số câu đúng
            ChuyenDiem cd = new ChuyenDiem(DapAn.NhanDiem);
            ChuyenCauDaLam ccdl = new ChuyenCauDaLam(DapAn.NhanCauDaLam);
            //Điểm
            ChuyenTDiem ctd = new ChuyenTDiem(DapAn.NhanTDiem);
            ChuyenXLoai cxl = new ChuyenXLoai(DapAn.NhanXLoai);
            cd(this.textBox18);
            ccdl(this.textBox16);
            ctd(this.textBox15);
            cxl(XepLoai);
            DapAn.Show();
            this.Visible = false;
        }
        public void KetThuc()
        {
            this.timer1.Enabled = false;
            this.panel1.Visible = false;
            this.panel3.Visible = true;
            this.panel3.Enabled = true;
            this.button6.Enabled = true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedQuestion[SoCauHoi] == "true" && countCheckedQues[SoCauHoi] == 0)
                {
                    SoCauDaLam++;
                    countCheckedQues[SoCauHoi]++;
                }
                CountCorrectQuestion();
                dataSend += textBox1.Text + "$" + textBox4.Text + "$" + textBox3.Text + "$" + textBox2.Text + "$";
                for (int i = 0; i < 35; i++)
                {
                    if (i != 34)
                        dataSend += (i + 1).ToString() + sendAns[i] + ";";
                    else
                        dataSend += (i + 1).ToString() + sendAns[i];
                }
                Score = Math.Round((SoCauDung * 10 / 35), 2);
                if (Score >= 5)
                    XepLoai = "ĐẠT";
                else
                    XepLoai = "KHÔNG ĐẠT";
                dataSend += "$" + SoCauDaLam.ToString() + "$" + SoCauDung.ToString() + "$" + Score.ToString() + "$" + XepLoai + "$";
                if (MessageBox.Show("Bạn có muốn kết thúc bài thi không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    timer1.Stop();
                    dataSend += progressBar1.Value.ToString();
                    KetThuc();
                    button4.Enabled = false;
                    senbt1.Visible = false;
                    chat1.Visible = false;
                    label78.Text = "KẾT THÚC!";
                    textBox18.Text = SoCauDung.ToString();
                    textBox16.Text = SoCauDaLam.ToString();
                    textBox15.Text = Score.ToString();
                    byte[] sendByte = Encoding.UTF8.GetBytes(dataSend);
                    RecvStream.Write(sendByte, 0, sendByte.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value++;
            label79.Text = (progressBar1.Value / 60) + " / 60 phút (" + progressBar1.Value / 36 + "%)";
            if (progressBar1.Value == progressBar1.Maximum)
            {
                timer1.Stop();
                panel3.Visible = true;
                panel1.Visible = false;
            }
        }
        private void LoadCauHoi()
        {
            lblCauHoi.Text = "Câu Hỏi Số " + (SoCauHoi+1).ToString();
            if (recvToString.Contains('@') == true)
            {
                string[] arrayQuestion = recvToString.Split(';');
                string[] arrayMultiple = arrayQuestion[SoCauHoi].Split('@');
                textBox10.Text = arrayMultiple[0];
                textBox14.Text = arrayMultiple[1];
                textBox11.Text = arrayMultiple[2];
                textBox12.Text = arrayMultiple[3];
                textBox13.Text = arrayMultiple[4];
                corrAnswer = arrayMultiple[5];
            }
            //Kiểm tra đáp án nào vừa được chọn
                for (int i = 0; i < 4; i++)
                {
                    if (Luu[SoCauHoi, i] == AnsTest[SoCauHoi])
                    {
                        switch (i)
                        {
                            case 0:
                                radioButton61.Checked = true;
                                break;
                            case 1:
                                radioButton62.Checked = true;
                                break;
                            case 2:
                                radioButton63.Checked = true;
                                break;
                            case 3:
                                radioButton64.Checked = true;
                                break;
                        }
                        break;
                    }
                }
        }
        //Lưu câu trả lời client đã chọn
        private void radiobutton_Click(object sender, EventArgs e)
        {
            if (radioButton61.Checked == true)
            {
                AnsTest[SoCauHoi] = "true";
                Luu[SoCauHoi, 0] = AnsTest[SoCauHoi];
                checkedQuestion[SoCauHoi] = "true";
                Luu[SoCauHoi, 1] = "false";
                Luu[SoCauHoi, 2] = "false";
                Luu[SoCauHoi, 3] = "false";
            }
            else if (radioButton62.Checked == true)
            {
                AnsTest[SoCauHoi] = "true";
                Luu[SoCauHoi, 1] = AnsTest[SoCauHoi];
                checkedQuestion[SoCauHoi] = "true";
                Luu[SoCauHoi, 0] = "false";
                Luu[SoCauHoi, 2] = "false";
                Luu[SoCauHoi, 3] = "false";
            }
            else if (radioButton63.Checked == true)
            {
                AnsTest[SoCauHoi] = "true";
                Luu[SoCauHoi, 2] = AnsTest[SoCauHoi];
                checkedQuestion[SoCauHoi] = "true";
                Luu[SoCauHoi, 1] = "false";
                Luu[SoCauHoi, 0] = "false";
                Luu[SoCauHoi, 3] = "false";
            }
            else if (radioButton64.Checked == true)
            {
                AnsTest[SoCauHoi] = "true";
                Luu[SoCauHoi, 3] = AnsTest[SoCauHoi];
                checkedQuestion[SoCauHoi] = "true";
                Luu[SoCauHoi, 1] = "false";
                Luu[SoCauHoi, 2] = "false";
                Luu[SoCauHoi, 0] = "false";
            }
            SaveQuestion();
        }

        //Lưu đáp án
        private void SaveQuestion()
        {
            if (radioButton61.Checked == true)
                sendAns[SoCauHoi] = radioButton61.Text;
            else if (radioButton62.Checked == true)
                sendAns[SoCauHoi] = radioButton62.Text;
            else if (radioButton63.Checked == true)
                sendAns[SoCauHoi] = radioButton63.Text;
            else if (radioButton64.Checked == true)
                sendAns[SoCauHoi] = radioButton64.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (checkedQuestion[SoCauHoi] == "true" && countCheckedQues[SoCauHoi] == 0)
            {
                SoCauDaLam++;
                countCheckedQues[SoCauHoi]++;
            }
            CountCorrectQuestion();
            radioAutoCheckedFix();
            SoCauHoi--;
            if (SoCauHoi == 0)
            {
                button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
                button1.Enabled = true;
            }
            LoadCauHoi();
        }

        private void CountCorrectQuestion()
        {
            if (checkedQuestion[SoCauHoi] == "true")
            {
                //Khi câu chưa được chấm điểm
                if (checkedPoint[SoCauHoi] == "false")
                {
                    if (sendAns[SoCauHoi] == corrAnswer && ChamDiem[SoCauHoi] == "false")
                    {
                        SoCauDung++;
                        checkedPoint[SoCauHoi] = "true";
                        ChamDiem[SoCauHoi] = "true";
                    }
                }
                //Câu phải chấm rồi mới được trừ
                else
                {
                    if (sendAns[SoCauHoi] == corrAnswer && ChamDiem[SoCauHoi] == "false")
                    {
                        SoCauDung++;
                        ChamDiem[SoCauHoi] = "true";
                    }
                    if (SoCauDung >= 0 && ChamDiem[SoCauHoi] == "true" && sendAns[SoCauHoi] != corrAnswer)
                    {
                        SoCauDung--;
                        ChamDiem[SoCauHoi] = "false";
                    }
                }
            }
        }

        private void FormDienThongTin_Load(object sender, EventArgs e)
        {
            for (int n = 0; n < 35; n++)
            {
                ChamDiem[n] = "false";
                checkedPoint[n] = "false";
                checkedQuestion[n] = "false";
                //countCheckedQues[n] = 0;
                for (int j = 0; j < 4; j++)
                {
                    Luu[n, j] = "false";
                }
            }
            //LoadCauHoi();
            fic = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in fic)
                comboBox1.Items.Add(fi.Name);
            comboBox1.SelectedIndex = 0;
            vcd = new VideoCaptureDevice();
        }
        FilterInfoCollection fic;
        VideoCaptureDevice vcd;
        private void VCDNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedQuestion[SoCauHoi] == "true" && countCheckedQues[SoCauHoi] == 0)
            {
                SoCauDaLam++;
                countCheckedQues[SoCauHoi]++;
            }
            CountCorrectQuestion();
            radioAutoCheckedFix();
            SoCauHoi++;
            if (SoCauHoi == 34)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button5.Enabled = true;
            }
            LoadCauHoi();
        }

        private void radioAutoCheckedFix()
        {
            if (radioButton61.Checked == true)
            {
                radioButton61.Checked = false;
            }
            else if(radioButton62.Checked == true)
            {
                radioButton62.Checked = false;
            }
            else if (radioButton63.Checked == true)
            {
                radioButton63.Checked = false;
            }
            else if (radioButton64.Checked == true)
            {
                radioButton64.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vcd = new VideoCaptureDevice(fic[comboBox1.SelectedIndex].MonikerString);
            vcd.NewFrame += VCDNewFrame;
            vcd.Start();
            button7.Visible = true;
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (vcd.IsRunning == true)
                vcd.Stop();
            pictureBox1.Image = null;
            button7.Enabled = false;
        }

        private void btnRecv_Click(object sender, EventArgs e)
        {
            //receiveQuestion(recvToString);
        }
    }
}
