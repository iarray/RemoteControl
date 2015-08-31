using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl
{
    public class AuthenticateExecutor:IExecutor
    {
        private string[] _commandText;
        public string[] CommandText
        {
            get { return _commandText; }
        }

        public string AuthenticationString { get; set; }

        public AuthenticateExecutor()
        {
            _commandText = new string[] { "connect"};
        }

        public AuthenticateExecutor(string astr):this()
        {
            AuthenticationString = astr;
        }

        public void Excute(AsyncParameters parameters, string cmd)
        {
            try
            {
                if (string.IsNullOrEmpty(AuthenticationString))
                {
                    parameters.Client.Send(Encoding.UTF8.GetBytes("ok"));
                }
                else
                {
                    string[] parms = cmd.Split('|');
                    if (parms != null && parms.Length > 1)
                    {
                        if (parms[1] == AuthenticationString)
                        {
                            parameters.Client.Send(Encoding.UTF8.GetBytes("ok"));
                        }
                        else
                        {
                            parameters.Client.Send(Encoding.UTF8.GetBytes("failed"));
                        }
                    }
                    else
                    {
                        parameters.Client.Send(Encoding.UTF8.GetBytes("key"));
                    }
                }
            }
            catch
            {

            }
        }
    }
}
