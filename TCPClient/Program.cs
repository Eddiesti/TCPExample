using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace TCPClient
{
    public class Program
    {
        private const string server = "localhost";
        private const int port = 777;

        public static TcpState GetState(TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint)
                                 && x.RemoteEndPoint.Equals(tcpClient.Client.RemoteEndPoint)
              );

            return foo != null ? foo.State : TcpState.Unknown;
        }


        static void Main(string[] args)
        {
            TcpClient client;
            using (client = new TcpClient())
            {
                client.Connect(server, port);
                var ns = client.GetStream();
                while (true)
                {

                    if (GetState(client) == TcpState.Established)
                    {
                        do
                        {
                            byte[] data = new byte[1024];
                            int bytes = ns.Read(data, 0, data.Length);
                            Console.WriteLine(Encoding.UTF8.GetString(data, 0, bytes));
                        }
                        while (ns.DataAvailable); // пока данные есть в потоке
                        byte[] msg = new byte[1024];
                        msg = Encoding.UTF8.GetBytes(Console.ReadLine());
                        ns.Write(msg, 0, msg.Length);
                    }
                }
            }
        }
    }
}
