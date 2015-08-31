using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    public class AtCommand:BaseCommand
    {
        public AtCommand(string cmd)
            : base(cmd)
        {

        }

        protected override void ExcuteCommand()
        {
            if (string.IsNullOrEmpty(this.CommandText))
            {
                throw new ArgumentNullException("CommandText");
            }

            string[] cmd = CommandText.Split('|');

            if (cmd.Length < 4)
            {
                throw new CommandParametersException("CommandText Less than four parameters");
            }
            else if ((cmd[0].ToLower()) != "at")
            {
                throw new CommandParametersException("The command is not 'At'");
            }
            try
            {
            }
            catch
            {
                throw;
            }
        }
    }
}
