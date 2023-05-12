using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

using System.Text;
using System.Threading.Tasks;

namespace Project_Final_Semester__NT106_
{
    public class Ketnoi
    {
        IPAddress ipServer;
        int port;
        IPEndPoint SEP;
        Stream RecvStream;
        bool recvState = true;
        //ipServer = ipser;
        //port = portser;
        public void Connect()
        {

            try
            {
                SEP = new IPEndPoint(ipServer, port);
                TcpClient thisClient = new TcpClient();
                thisClient.Connect(SEP);
                string s = "Thí sinh có đăng nhập thành công";
                byte[] testing = Encoding.UTF8.GetBytes(s);
                thisClient.Client.Send(testing);
                RecvStream = thisClient.GetStream();
                Thread receiveThread = new Thread(receive);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void receive()
        {
            try
            {
                while (recvState)
                {
                    byte[] receivebyte = new byte[8192];
                    RecvStream.Read(receivebyte, 0, receivebyte.Length);
                    string recvToString = Encoding.UTF8.GetString(receivebyte);
                    receiveWhat(recvToString);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void receiveWhat(string rec)
        {
            //xử lý 
        }
    }
}
