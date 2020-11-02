using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Program
    {

        static void Main(string[] args)
        {
            Server server = new Server(777);
            server.Listen();

        }
    }

}