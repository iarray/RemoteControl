using Command;
using RemoteControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CustomCommandPlugin
{
    public class CustomCommandExecutor:IExecutor
    {
        private string[] _commandText;
        public string[] CommandText
        {
            get { return _commandText; }
        }

        private ObservableCollection<Item> Items;

        public void SetCommandText()
        {
            _commandText = new string[Items.Count];
            for (int i = 0; i < Items.Count; i++)
            {
                _commandText[i] = Items[i].Cmd;
            }
        }
       public CustomCommandExecutor( ObservableCollection<Item> items)
        {
            Items = items;
            SetCommandText();
        }

        public void Excute(AsyncParameters parameters, string cmd)
        {
            foreach (Item i in Items)
            {
                if(i.Cmd==cmd)
                try
                {
                    ICommand icmd = CommandFactory.Creat(i.ToCmd());
                    icmd.Excute();
                }
                catch { }
            }
        }
    }
}
