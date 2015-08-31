using Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RemoteControl
{
    public class FileTransmissionExecutor:IExecutor
    {
        public string[] CommandText
        {
            get;
            private set;
        }

        public FileTransmissionExecutor()
        {
            CommandText = new string[] { "getfile" };
        }


        public void Excute(AsyncParameters parameters, string cmd)
        {
            string[] cmdParms = cmd.Split('|');
            if (cmdParms.Length < 2)
            {
                return;
            }
            else
            {
                if (File.Exists(cmdParms[1]))
                {
                    byte[] fileBuffer = new byte[1024];
                    FileStream fs = File.OpenRead(cmdParms[1]);
                    while (fs.Position < fs.Length)
                    {
                        int len = fs.Read(fileBuffer, 0, fileBuffer.Length);
                        parameters.Client.Send(fileBuffer, len, 0);
                    }
                    fs.Close();
                    parameters.Client.Close();
                }
            }
        }
    }
}
