using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Command
{
    public class OpenCommand:BaseCommand
    {
        public OpenCommand(string cmd) : base(cmd) { }
        protected override void ExcuteCommand()
        {
            if(string.IsNullOrEmpty( this.CommandText ))
            {
                throw new ArgumentNullException("CommandText");
            }

            string[] cmd = CommandText.Split('|');

            if(cmd.Length<2)
            {
                throw new CommandParametersException("CommandText Less than two parameters");
            }
            else if( (cmd[0].ToLower())!="open" )
            {
                throw new CommandParametersException("The command is not 'Open'");
            }
            try
            {
                if (cmd.Length < 3)
                {
                    Process.Start(cmd[1]);
                }
                else
                {
                    Process.Start(cmd[1], cmd[2]);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
