using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    public abstract class BaseCommand:ICommand
    {
        public string CommandText { get; set; }
        public BaseCommand(string cmd)
        {
            CommandText = cmd;
        }
        protected abstract void ExcuteCommand();

        public void Excute()
        {
            ExcuteCommand();
        }
    }
}
