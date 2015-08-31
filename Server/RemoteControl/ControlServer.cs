using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteControl
{
    public enum ServerStatus
    {
        Runing,Stop
    }
    public class ControlServer:IDisposable
    {

        private string _ip;
        private int _port;
        private System.Net.Sockets.Socket serverSocket;
        private List<EndPoint> userList=new List<EndPoint>();
        private IPAddress addr;
        private Thread listenThread;
        private bool controller = true;
        public ServerStatus ServerStatus { get; private set; }
        public List<IExecutor> Executors { get; private set; }

        public ControlServer(IPAddress ip,int port)
        {
            addr = ip;
            _port = port;
            ServerStatus = ServerStatus.Stop;
            Executors = new List<IExecutor>();
            serverSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        }
        public ControlServer(string ip,int port)
        {
            _ip = ip;
            _port = port;
            addr = IPAddress.Parse(_ip);
            ServerStatus = ServerStatus.Stop;
            Executors = new List<IExecutor>();
            serverSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        }
        private void listenWorkThread()
        {
            while (controller)
            {
                try
                {
                    Socket client = serverSocket.Accept();
                    ThreadPool.QueueUserWorkItem((obj) =>
                        {
                            clientWorkThread(client);
                        });
                }
                catch { }
            }
        }

        public void Listen()
        {
            serverSocket.Bind(new IPEndPoint(addr, _port));
            serverSocket.Listen(10);
            listenThread = new Thread(new ThreadStart(listenWorkThread));
            listenThread.IsBackground = true;
            listenThread.Start();
            ServerStatus = ServerStatus.Runing;
        }

        private void clientWorkThread(Socket client)
        {
            AsyncParameters asynParam = new AsyncParameters(serverSocket, client);
            client.BeginReceive(asynParam.Data, 0, asynParam.Data.Length, 0, new AsyncCallback(endReceive), asynParam);
        }
        int recLen = 0;

        private void endReceive(IAsyncResult rs)
        {
            try
            {
                AsyncParameters asyncParam = (AsyncParameters)rs.AsyncState;
                int len = asyncParam.Client.EndReceive(rs);
                string recStr = Encoding.UTF8.GetString(asyncParam.Data, 0, len);
                recLen += len;
                if (asyncParam.Client.Available > 0)
                {
                    asyncParam.Client.BeginReceive(asyncParam.Data, 0, asyncParam.Data.Length, 0, new AsyncCallback(endReceive), asyncParam);
                }
                else
                {
                    //Console.WriteLine(recStr);

                    recLen = 0;
                }
                ResolveAndExcuteCMD(asyncParam, recStr);
              
                if (asyncParam.Client != null && asyncParam.Client.Connected == true)
                {
                    asyncParam.Client.Close();
                }
            }
            catch { }
            
        }

        private void ResolveAndExcuteCMD(AsyncParameters asyncParam ,string cmd)
        {
            int index = cmd.IndexOf('|');
            string mainCmd="";
            if(index>0)
            {
                 mainCmd= cmd.Substring(0, index);
            }
            else
            {
                mainCmd = cmd;
            }
            foreach (IExecutor ie in Executors)
            {
                if (ie.CommandText.Contains(mainCmd.ToLower()))
                {

                    ie.Excute(asyncParam, cmd);
                }
            }
        }


        public void Dispose()
        {
            if(listenThread!=null)
            {
                controller = false;
            }
            if(serverSocket!=null)
            {
                serverSocket.Close();
                serverSocket = null;
            }
            ServerStatus = ServerStatus.Stop;
        }
    }
}
