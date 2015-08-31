using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    public class CommandExcuteException:Exception
    {
        public CommandExcuteException(string msg) : base(msg) { }
    }
}
