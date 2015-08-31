using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Command
{
    public class AsyncParameters
    {
        public Socket Server { get; set; }
        public Socket Client { get; set; }
        public byte[] Data { get; set; }

        public AsyncParameters()
        {
            Data = new byte[1024];
        }

        public AsyncParameters(Socket server,Socket client):this()
        {
            Server = server;
            Client = client;
        }
    }
}
