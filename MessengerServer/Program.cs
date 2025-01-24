using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MessengerServer
{
    internal class Program
    {
        private static List<TcpClient> clients = new List<TcpClient>();
        private static TcpListener server; 

        static void Main(string[] args)
        {
            server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Сервер запущен, ожидаем подключения...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Клиент подключен!");
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        private static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Сообщение от клиента: " + message);
                BroadcastMessage(message, client);
            }

            clients.Remove(client);
            client.Close();
        }

        private static void BroadcastMessage(string message, TcpClient excludeClient)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach(var client in clients)
            {
                if(client != excludeClient)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
            }
        }
    }
}
