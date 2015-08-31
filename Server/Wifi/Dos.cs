using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Win
{
    public abstract class Dos
    {
        protected List<string> Cmds;

        public Dos()
        {
            Cmds = new List<string>();
        }

        public string Runcmd()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.RedirectStandardError = true;
            myProcess.StartInfo.RedirectStandardInput = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            try
            {
                myProcess.Start();
            }
            catch (Exception)
            {

                throw;
            }
            try
            {
                foreach (var item in Cmds)
                {
                    myProcess.StandardInput.WriteLine(item);
                }

            }
            catch (Exception)
            {

                throw;
            }
            myProcess.StandardInput.WriteLine("exit");
            return myProcess.StandardOutput.ReadToEnd();
        }
    }
}
