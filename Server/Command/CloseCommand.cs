using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Command
{
    public class CloseCommand:BaseCommand
    {
        public CloseCommand(string cmd) : base(cmd) { }

        protected override void ExcuteCommand()
        {
            if (string.IsNullOrEmpty(this.CommandText))
            {
                throw new ArgumentNullException("CommandText");
            }

            string[] cmd = CommandText.Split('|');

            if (cmd.Length < 2)
            {
                throw new CommandParametersException("CommandText Less than two parameters");
            }
            else if ((cmd[0].ToLower()) != "close")
            {
                throw new CommandParametersException("The command is not 'Close'");
            }
            try
            {
                Process[] processes = Process.GetProcesses();
                foreach(Process p in processes)
                {
                    if(p.ProcessName.ToLower()==cmd[1].ToLower())
                    {
                        p.Kill();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
