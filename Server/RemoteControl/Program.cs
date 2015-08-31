using Command;
using RemoteControl.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Wifi;
using Win;

namespace RemoteControl
{
    class Program
    {
        static void Main(string[] args)
        {
            //OpenCommand ocmd = new OpenCommand(@"Open|Shutdown|-a");
            //ocmd.ExcuteCommand();
            //BaseCommand cmd = new CloseCommand("close|qq");
            //cmd.ExcuteCommand();
            //ManagementClass vNetworkAdapter = new ManagementClass("Win32_NetworkAdapter");
            //ManagementObjectCollection vNetworkAdapters = vNetworkAdapter.GetInstances();
            //foreach (ManagementObject vNetworkAdapterInfo in vNetworkAdapters)
            //{
            //    Object obj = vNetworkAdapterInfo.Properties["NetConnectionID"].Value;
            //    if (obj!=null
            //        &&(obj.ToString()).Contains("无线"))
            //    {
            //        foreach (PropertyData v in vNetworkAdapterInfo.Properties)
            //        {
            //            Console.WriteLine(v.Value);
            //        }
            //    }
            //    Console.WriteLine("Name：{0}", vNetworkAdapterInfo.Properties["Name"].Value);
            //    Console.WriteLine("NetConnectionID：{0}", vNetworkAdapterInfo.Properties["NetConnectionID"].Value);
            //    Console.WriteLine("Caption：{0}", vNetworkAdapterInfo.Properties["Caption"].Value);
            //    Console.WriteLine("Description：{0}", vNetworkAdapterInfo.Properties["Description"].Value);
            //}


            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(IPAddress ip in ips)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPHostEntry ih =Dns.GetHostEntry(ip);
                    Console.WriteLine(ip.ToString());
                   
                }
            }
            //WiFiCreator wc = new WiFiCreator();
            //wc.Creat();
            //wc.IP = "192.168.123.1";
            //wc.StartAp();
            //IPProvider.SetIPAddress("192.168.123.1", "255.255.0.0", null);
           //Console.WriteLine( IPHelper.GetWirelessNetworkIP());
            //ThreadPool.QueueUserWorkItem((obj) =>
            //    {
            //        ControlServer server1 = new ControlServer("192.168.137.1", 9999);
            //        server1.Executors.Add(new DesktopTransportExecutor());
            //        server1.Listen();
            //    }, null);
            IPMulticastServer ipmserver = new IPMulticastServer();
            ipmserver.Start();
            ControlServer server = new ControlServer(IPAddress.Any, 9999);
            server.Executors.Add(new CommandExecutor());
            server.Executors.Add(new FileListTransmissionExecutor());
            server.Executors.Add(new FileTransmissionExecutor());
            DesktopTransportExecutor dte = new DesktopTransportExecutor();
            dte.ShowModel = ShowModels.FullScreen;
            server.Executors.Add(dte);
            server.Listen();
            Console.ReadKey();
            //wc.StopAp();
            //wc.Dispose();
        }
    }
}
