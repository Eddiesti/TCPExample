using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer.Commands
{
    public class BaseCommand : Command
    {
        private string _answer;
        private bool _end;

        public BaseCommand(string name, string answer, bool end) : base(name)
        {
            this._answer = answer;
            this._end = end;
        }

        public override Result Execute()
        {
            return new Result()
            {
                Message = _answer,
                End = _end
            };
        }
    }
}
