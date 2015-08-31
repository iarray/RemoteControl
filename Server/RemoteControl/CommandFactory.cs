using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl
{
    public class CommandFactory
    {
        public static ICommand Creat(string cmd)
        {
            string[] cmds = cmd.Split('|');
            if (cmds.Length > 0)
            {
                switch (cmds[0].ToLower())
                {
                    case "open":
                        return new OpenCommand(cmd);
                    case "close":
                        return new CloseCommand(cmd);
                    case "keydown":
                    case "keyup":
                    case "keypress":
                        return new KeyBoardCommand(cmd);
                    case "mousemove":
                    case "mouselbdown":
                    case "mouserbdown":
                    case "mouselbup":
                    case "mouserbup":
                    case "mouselbpress":
                    case "mouserbpress":
                    case "mouseadmove":
                    case "mousescrollup":
                    case "mousescrolldown":
                        return new MouseCommand(cmd);
                    default:
                        throw new ArgumentNullException("CommandText");
                }
            }
            else
            {
                throw new CommandParametersException("CommandText Less than two parameters");
            }
        }
    }
}
