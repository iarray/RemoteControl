using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomCommandPlugin
{
    public enum Action
    {
        打开程序,关闭程序,关机
    }
    public class Item
    {
        public string Cmd { get; set; }
        public string Parameter { get; set; }
        public Action Action { get; set; }

        public static Item Parse(string str)
        {
            Item i = new Item();
            string[] s = str.Split(',');
            if(s.Length==3)
            {
                i.Cmd = s[0];
                int at = 0;
                int.TryParse(s[1], out at);
                i.Action = (Action)at;
                i.Parameter = s[2];
                return i;
            }
            return null;
        }

        public string ToCmd()
        {
            string header = "";
            switch(Action)
            {
                case Action.打开程序:
                    header = "Open|";
                    break;
                case Action.关闭程序:
                    header = "Open|";
                    break;
                case Action.关机:
                    header = "Open|";
                    Parameter = "Shutdown -s";
                    break;
            }
            return header+Parameter;
        }

        public override string ToString()
        {
            string s=Cmd+","+((int)Action).ToString()+","+Parameter;
            return s;
        }
    }
}
