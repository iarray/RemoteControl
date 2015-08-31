using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteControl
{
    public class IPMulticastServer:IDisposable
    {
        private const string commandText = "getserver";
        private const string MulticastAddress = "224.0.0.1";
        private const int appPort = 9999;
        private int _port;
        private bool canFindClient = false;
        public ServerStatus ServerStatus { get;private set; }
        private Thread workThread;
        public string AuthenticationString { get; set; }

        public int ClientPort
        {
            get
            {
                return _port;
            }
        }

        public int ServerPort { get; set; }


        private bool PortIsRight (int value)
        {
            if (value < 65536 && value > 1024)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IPMulticastServer()
        {
            workThread = new Thread(new ThreadStart(work));
            ServerPort = 8888;
            _port = 7777;
            workThread.IsBackground = true;
            ServerStatus = ServerStatus.Stop;
        }

        public void Start()
        {
           workThread.Start();
           ServerStatus = ServerStatus.Runing;
        }

        private void work()
        {
            while (!canFindClient)
            {
                string cmd = Recieve();
                Console.WriteLine(cmd);
                if (cmd!=null && cmd.ToLower().Trim() == commandText)
                {
                    if(String.IsNullOrEmpty(AuthenticationString))
                    {
                        SendMsg("ok");
                    }
                    else
                    {
                        SendMsg("key");
                        string recv = Recieve();
                        if (recv.Trim() == AuthenticationString)
                        {
                            SendMsg("ok");
                        }
                    }
                    Thread.Sleep(1000);
                }
                else
                {

                }
            }
        }
        private void SendMsg(string msg)
        {
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(MulticastAddress), _port);
                EndPoint ep = (EndPoint)iep;

                byte[] b = Encoding.ASCII.GetBytes(msg);
                s.SendTo(b, ep);
                s.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string Recieve()
        {
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, ServerPort);
                EndPoint ep = (EndPoint)iep;
                s.Bind(iep);
                s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(MulticastAddress)));
                byte[] b = new byte[1024];
                int len = s.ReceiveFrom(b, ref ep);

                string test;
                test = System.Text.Encoding.ASCII.GetString(b,0,len);
                s.Close();
                return test;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Dispose()
        {
            canFindClient = false;
            workThread.Abort();
            ServerStatus = ServerStatus.Stop;
        }
    }
}
