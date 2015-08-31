using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace Win
{
    public class IPProvider
    {
         /// <summary>
        /// 启用DHCP服务器
        /// </summary>
        public static void EnableDHCP()
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            foreach(ManagementObject mo in moc)
            {
                //如果没有启用IP设置的网络设备则跳过
                if(!(bool)mo["IPEnabled"])continue;

                //重置DNS为空
                mo.InvokeMethod("SetDNSServerSearchOrder",null);
                //开启DHCP
                mo.InvokeMethod("EnableDHCP",null);
            }
        }

         public static bool IsIPAddress(string ip)
        {
             IPAddress addr;
             return IPAddress.TryParse(ip,out addr);
        }

         /// <summary>
         /// 设置IP地址，掩码，网关和DNS
         /// </summary>
         /// <param name="ip"></param>
         /// <param name="submask"></param>
         /// <param name="getway"></param>
         /// <param name="dns"></param>
         public static void SetIPAddress(string[] ip, string[] submask, string[] getway, string[] dns)
         {
             ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
             ManagementObjectCollection moc = wmi.GetInstances();
             ManagementBaseObject inPar = null;
             ManagementBaseObject outPar = null;
             foreach (ManagementObject mo in moc)
             {
                 //如果没有启用IP设置的网络设备则跳过
                 if (!(bool)mo["IPEnabled"]) continue;


                 //设置IP地址和掩码
                 if (ip != null && submask != null)
                 {
                     inPar = mo.GetMethodParameters("EnableStatic");
                     inPar["IPAddress"] = ip;
                     inPar["SubnetMask"] = submask;
                     outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                 }

                 //设置网关地址
                 if (getway != null)
                 {
                     inPar = mo.GetMethodParameters("SetGateways");
                     inPar["DefaultIPGateway"] = getway;
                     outPar = mo.InvokeMethod("SetGateways", inPar, null);
                 }

                 //设置DNS地址
                 if (dns != null)
                 {
                     inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                     inPar["DNSServerSearchOrder"] = dns;
                     outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                 }
             }

         }
        /// <summary>
         /// 设置IP地址，掩码和网关
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
         public static void SetIPAddress(string ip,string submask,string getway)
         {
             SetIPAddress(new string[]{ip},new string[]{submask},new string[]{getway},null);
         }
    }
}
