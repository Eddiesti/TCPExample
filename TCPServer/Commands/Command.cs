using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer.Commands
{
    public abstract class Command
    {
        public string Name { get; private set; }

        public Command(string name)
        {
            this.Name = name;
        }

        public abstract Result Execute();
    }
}
