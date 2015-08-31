using Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RemoteControl
{
    public class FileListTransmissionExecutor:IExecutor
    {
        private StringBuilder sb = new StringBuilder(100);
        public string[] CommandText
        {
            get;
            private set;
        }

        public FileListTransmissionExecutor()
        {
            CommandText = new string[] { "getfilelist" };
        }

        public void Excute(AsyncParameters parameters, string cmd)
        {
            string[] cmdParms = cmd.Split('|');
            if(cmdParms.Length<2)
            {
                return;
            }
            else
            {
                sb.Remove(0, sb.Length);
                if (cmdParms[1].ToLower() == "disk")
                {
                    DriveInfo[] d = DriveInfo.GetDrives();
                    foreach(DriveInfo difo in d)
                    {
                        if (difo.DriveType == DriveType.Fixed)
                        {
                            sb.Append(difo.Name + ",Disk;");
                        }
                    }
                }
                else
                {
                    if(Directory.Exists(cmdParms[1]))
                    {
                        //文件夹路径
                        string[] dirs = Directory.GetDirectories(cmdParms[1]);
                        foreach(string s in dirs)
                        {
                            sb.Append(s + ",Dir;");
                        }
                        string[] files=Directory.GetFiles(cmdParms[1]);
                        foreach(string fs in files)
                        {
                            FileInfo info = new FileInfo(fs);
                            sb.Append(fs + ",File,"+(info.Length/1024).ToString()+";");
                            
                        }
                    }
                    else
                    {
                        //什么都不是
                    }
                }
                byte[] data=Encoding.UTF8.GetBytes(sb.ToString());
                parameters.Client.Send(data);
            }
        }
    }
}
