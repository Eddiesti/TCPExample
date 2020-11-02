using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPServer.Commands;

namespace TCPServer
{
    public class Server
    {
        private TcpListener _listener;
        private static List<User> _users = new List<User>();

        private List<Command> commands = new List<Command>()
        {
            new BaseCommand("How are you?", "I'm fine", false),
            new BaseCommand("What are you doing ?", "I'm trying to get a job with you", false),
            new BaseCommand("Close chat", "Buy", true),
            new UserCommand(_users)
        };

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
        }


        public void Listen()
        {
            try
            {
                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    Task.Run(() => ConnectionCallback(client));
                }
            }
            finally { _listener.Stop(); }
        }

        private void ConnectionCallback(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            Write(ns, "Enter your name");
            var msg = Read(ns);
            Console.WriteLine(msg);
            var user = new User() { Id = Guid.NewGuid(), Name = msg };
            _users.Add(user);
            Write(ns, "Available commands");
            foreach (var c in commands)
            {
                Write(ns, "\r\n" + c.Name);
            }

            try
            {
                while (client.Connected)
                {
                    msg = Read(ns);
                    var command = commands.Where(x => x.Name == msg).FirstOrDefault();
                    if (command != null)
                    {
                        var result = command.Execute();
                        Write(ns, result.Message);
                        if (result.End)
                            break;
                    }
                    else { Write(ns, "Command not found"); }
                }
            }
            finally
            {
                _users.Remove(user);
            }
        }


        private void Write(Stream ns, string message)
        {
            byte[] data = new byte[100];
            data = Encoding.UTF8.GetBytes(message);

            ns.Write(data, 0, data.Length);
        }

        private string Read(Stream ns)
        {

            byte[] msg = new byte[1024];
            int count = ns.Read(msg, 0, msg.Length);
            return Encoding.UTF8.GetString(msg, 0, count);
        }

    }
}
