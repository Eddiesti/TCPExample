using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPServer.Commands
{
    public class UserCommand : Command
    {
        private List<User> _users;
        public UserCommand(List<User> users) : base("Connected users")
        {
            _users = users;
        }

        public override Result Execute()
        {
            return new Result()
            {
                Message = string.Join(",", _users.Select(x => x.Name)),
                End = false
            };
        }
    }
}
