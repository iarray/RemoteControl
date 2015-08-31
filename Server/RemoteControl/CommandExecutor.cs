using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl
{
    public class CommandExecutor:IExecutor
    {
        private string[] _commandText;
        public CommandExecutor()
        {
            _commandText = new string[] { "open", "close", "keydown", "keyup", "keypress", 
                                          "mousemove", "mouselbdown", "mouserbdown", "mouselbup",
                                          "mouserbup","mouselbpress","mouserbpress","mouseadmove",
                                          "mousescrollup","mousescrolldown"};
        }
        public void Excute(AsyncParameters parameters, string cmd)
        {
            try
            {
                ICommand icmd = CommandFactory.Creat(cmd);
                icmd.Excute();
            }
            catch { }
        }

        public string[] CommandText
        {
            get
            {
                return _commandText;
            }
        }
    }
}
