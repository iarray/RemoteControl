using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomCommandPlugin
{
    public class CustomCommandPlugin:Plugin.PluginBase
    {
        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        public CustomCommandPlugin()
        {
            Name = "自定义命令插件";
            load();
            Executor = new CustomCommandExecutor(items);
        }
        public override System.Windows.Controls.UserControl GetControl()
        {
            return new UserControl1(items, (CustomCommandExecutor)Executor);
        }

        private void load()
        {
            try
            {
                string path = Path.Combine(System.Environment.GetEnvironmentVariable("appdata"), "PocketDesktop");
                string cfgPath = Path.Combine(path, "CustomCommand.cfg");
                if (File.Exists(cfgPath))
                {
                    items.Clear();
                    using (StreamReader sr = new StreamReader(cfgPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            Item i = Item.Parse(s);
                            if (i != null)
                                items.Add(i);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
