using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace ChatServer
{
    public class HandleClient
    {
        TcpClient clientSocket;
        string clientID;
        Hashtable clientsList;

        public void StartClient(TcpClient clientSocket, string clientID, Hashtable clientsList)
        {
            this.clientSocket = clientSocket;
            this.clientID = clientID;
            this.clientsList = clientsList;
            Thread thread = new Thread(DoChat);
            thread.Start();
        }

        public void DoChat()
        {          
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string count = null;
            string serverResponse = null;
            int requestCount = 0;

            while (true)
            {
                try
                {
                    requestCount++;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("From client - " + clientID + " : " + dataFromClient);
                    count = Convert.ToString(requestCount);

                    Program.Broadcast(dataFromClient, clientID, true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
