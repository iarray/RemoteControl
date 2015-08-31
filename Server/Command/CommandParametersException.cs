using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    public class CommandParametersException:Exception
    {
       public CommandParametersException(string msg) : base(msg) { }
    }
}
