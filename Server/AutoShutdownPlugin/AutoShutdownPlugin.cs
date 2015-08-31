using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShutdownPlugin
{
    public class AutoShutdownPlugin:Plugin.PluginBase
    {
        public AutoShutdownPlugin()
        {
            Name = "定时关机";
            Executor = new TestExector();
        }

        public override System.Windows.Controls.UserControl GetControl()
        {
            return new UserControl1();
        }

       
    }

    public class TestExector : IExecutor
    {
        public void Excute(AsyncParameters parameters, string cmd)
        {

        }

        public string[] CommandText
        {
            get
            {
                return new string[] { "AutoShutdown" };
            }
        }

       
    }
}
