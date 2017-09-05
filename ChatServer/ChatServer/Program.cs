using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections;
using System.Net;

namespace ChatServer
{
    public class Program
    {
        public static Hashtable clientsList = new Hashtable();
        static void Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(localAddr,8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine("Serveris running and listening on port: " + serverSocket.LocalEndpoint.ToString());
            counter = 0;
            while (true)
            {
                counter++;
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[10025];
                string dataFromClient = null;

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                clientsList.Add(dataFromClient, clientSocket);
                Broadcast(dataFromClient + " joined chat room",dataFromClient,false);
                HandleClient client = new HandleClient();

                client.StartClient(clientSocket, dataFromClient, clientsList);

                clientSocket.Close();
                serverSocket.Stop();
                Console.WriteLine("Exit");
                Console.ReadLine();
            }
        }

        public static void Broadcast(string message, string userName, bool flag)
        {
            foreach(DictionaryEntry item in clientsList)
            {
                TcpClient broadcastSocket = (TcpClient)item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                byte[] broadcastBytes = null;

                if (flag)
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(userName + ": " + message);
                }
                else
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(message);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
    }
}
