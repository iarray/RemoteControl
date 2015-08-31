using Command;
using RemoteControl.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace RemoteControl
{
    public enum ShowModels
    {
        FullScreen,
        FollowMouse
    }
    public class DesktopTransportExecutor:IExecutor
    {
        /// <summary>
        /// 设置图像传输参数,包含图像的宽高和图像质量
        /// </summary>
        public VideoParameters VideoParameters;

        private readonly VideoParameters defaultVideoParameters;

        private string[] _commandText;
        public string[] CommandText
        {
            get { return _commandText; }
        }

        public ShowModels ShowModel { get; set; }

        public DesktopTransportExecutor()
        {
            //get|width|height|showModel
            _commandText = new string[] { "get" };
            ShowModel = ShowModels.FollowMouse;
            defaultVideoParameters = new VideoParameters() { Width = 800, Height = 480, Quality = 30L };
        }

        public void Excute(AsyncParameters parameters, string cmd)
        {
            try
            {
                string[] parms = cmd.Split('|');
                if (parms != null && parms.Length > 1)
                {
                    VideoParameters = new VideoParameters();
                    foreach (string p in parms)
                    {
                        string prm = p.ToLower();
                        if (prm == "followmouse")
                        {
                            ShowModel = ShowModels.FollowMouse;
                        }
                        else if (prm == "fullscreen")
                        {
                            ShowModel = ShowModels.FullScreen;
                        }
                        else if (prm[prm.Length - 1] == 'w')
                        {
                            VideoParameters.Width = int.Parse(prm.Substring(0, prm.Length - 1));
                        }
                        else if (prm[prm.Length - 1] == 'h')
                        {
                            VideoParameters.Height = int.Parse(prm.Substring(0, prm.Length - 1));
                        }
                        else if (prm[prm.Length - 1] == 'q')
                        {
                            VideoParameters.Quality = long.Parse(prm.Substring(0, prm.Length - 1));
                        }
                    }
                }
            }
            catch { }

            VideoParameters v;
            if(VideoParameters==null||VideoParameters.Width==0||VideoParameters.Height==0)
            {
                v = defaultVideoParameters;
            }
            else
            {
                v = VideoParameters;
            }
            try
            {
                
                Screenshot ss = new Screenshot();
                Image img = null;
                if (ShowModel == ShowModels.FollowMouse)
                {
                    img = ss.GetRectangleScreenshot(v);
                }
                else
                {
                    img = ss.GetFullScreenImage(v);
                }
                 
                parameters.Data = ImageProcessing.CompressedImageQuality(img, v.Quality);
                img.Dispose();
                //Console.WriteLine(parameters.Data.Length / 1024);
                parameters.Client.BeginSend(parameters.Data, 0, parameters.Data.Length, 0, new AsyncCallback(EndSend), parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+e.Source+e.StackTrace);
            }
        }
        
        public static void EndSend(IAsyncResult ar)
        {
            try
            {

                AsyncParameters data = (AsyncParameters)ar.AsyncState;
                int senLen = data.Client.EndSend(ar);
                data.Client.Close();
                data.Data = new byte[0];
            }
            catch (Exception)
            {

            }
        }

        

    }
}
