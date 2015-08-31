using NETCONLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Win;

namespace Wifi
{
    public enum WiFiStatus
    {
        Running,Stop
    }
    public class WiFiCreator :Dos ,ICreator,IDisposable
    {
        private static string show = @"netsh wlan show hostednetwork";
        private static string start = @"netsh wlan start hostednetwork";
        private static string stop = @"netsh wlan stop hostednetwork";
        private static string allow = "netsh wlan set hostednetwork mode=allow ";
        private static string disallow = @"netsh wlan set hostednetwork mode=disallow";
        private static string setIP1 = "netsh interface ip set address \"";
        public WiFiStatus WiFiStatus
        {
            get;
            private set;
        }
        public WiFiCreator(string name,string password)
        {
            Name = name;
            Password = password;
        }
        public string Name { get; set; }
        private string _ip;
        public String IP
        {
            get
            {
                return _ip;
            }
            set
            {
                IPAddress ip;
                if(_ip!=value&&IPAddress.TryParse(value,out ip))
                {
                    if(setWifiIP("无线网络连接 2",_ip))
                    {
                        _ip = value;
                    }
                }
            }
        }
        public string Password { get; set; }

        public bool Creat()
        {
            Cmds.Clear();
            string setting = allow + " ssid=\"" + Name + "\" key=" + Password;
            if (Name.Length > 0)
            {

                if (Password.Length < 8 || Password.Length > 12)
                {
                    return false;
                }
                Cmds.Add(setting);
                Cmds.Add(show);
                try
                {
                    
                    Runcmd();
                    //if (string.IsNullOrEmpty(IP))
                    //{
                    //    IP = "192.168.196.1";
                    //}
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                throw new FormatException("The password length can not be less than 8 more than 12");
            }
        }

        public bool StartAp()
        {
            string setting = start;
            Cmds.Clear();
            Cmds.Add(setting);
            try
            {
                Runcmd();
                WiFiStatus = WiFiStatus.Running;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool StopAp()
        {
            string setting = stop;
            Cmds.Clear();
            Cmds.Add(setting);
            try
            {
                Runcmd();
                WiFiStatus = WiFiStatus.Stop;
                return true;
            }
            catch
            {

                return false;
            }
        }

        private bool setWifiIP(string wifiAdapter,string ip)
        {
            string IPSetting = setIP1 + wifiAdapter + "\" static "+ip;
            Cmds.Clear();
            Cmds.Add(IPSetting);
            try
            {
                Runcmd();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EnableSharing(string connectionToShare, string sharedForConnection)
        {
            try
            {
                var manager = new NetSharingManager();
                var connections = manager.EnumEveryConnection;
                foreach (INetConnection c in connections)
                {
                    var props = manager.NetConnectionProps[c];
                    var sharingCfg = manager.INetSharingConfigurationForINetConnection[c];
                    if (props.Name == connectionToShare)
                    {
                        //MessageBox.Show(connectionToShare.ToString());
                        sharingCfg.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC);
                    }
                    else if (props.Name == sharedForConnection)
                    {
                        //MessageBox.Show(sharedForConnection.ToString());
                        sharingCfg.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Disallow()
        {
            string setting = disallow;
            Cmds.Clear();
            Cmds.Add(setting);
            try
            {
                Runcmd();
            }
            catch { }
        }
        public void Dispose()
        {
            if (WiFiStatus==WiFiStatus.Running)
            {
                if(StopAp())
                {
                    WiFiStatus = WiFiStatus.Stop;
                    Disallow();
                }
            }
            else
            {
                Disallow();
            }
        }
    }
}
