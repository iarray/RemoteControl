using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net.NetworkInformation;
using System.Management;
namespace RemoteControl.Net
{
    public class IPHelper
    {
        public static string GetRealIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    var temp = webClient.DownloadString("http://iframe.ip138.com/ic.asp");
                    var ip = Regex.Match(temp, @"\[(?<ip>\d+\.\d+\.\d+\.\d+)]").Groups["ip"].Value;
                    return !string.IsNullOrEmpty(ip) ? ip : null;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static string GetWirelessNetworkMac()
        {
            string strMAC = "";
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                strMAC = adapter.GetPhysicalAddress().ToString();
                if (!adapter.Description.Contains("PCI") && adapter.Description.Contains("Wireless"))
                    return strMAC;
            }
            return null;
        }

        public static string GetWirelessNetworkIP()
        {
            try
            {
                string wlmac = GetWirelessNetworkMac();
                string mac = "";
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        for (int i = 0; i < ar.GetLength(0); i++)
                        {
                            st = ar.GetValue(i).ToString();
                            if (mac.Replace(":","") == wlmac)
                            {
                                return st;
                            }
                        }
                    }
                }
                moc = null;
                mc = null;
                return null;
            }catch
            {
                return null;
            }
        }
    }
}
