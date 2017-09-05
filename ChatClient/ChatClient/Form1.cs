using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            readData = "Connected to chat server";
            Message();
            clientSocket.Connect("172.0.0.1", 8888);
            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(messageBox.Text = "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread thread = new Thread(GetMessage);
            thread.Start();
        }

        private void GetMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffsize = 0;
                byte[] inStream = new byte[10025];
                buffsize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffsize);
                string returnData = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returnData;
                Message();
            }
        }

        private void Message()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(Message));
            }
            else
            {
                
            }
        }

        private void messageButton_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(messageBox.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
    }
}
