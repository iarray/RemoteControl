using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Command
{
    public class MouseCommand:BaseCommand
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标左键按下
        private const int MOUSEEVENTF_MOVE = 0x0001;//模拟鼠标移动
        private const int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;//鼠标绝对位置
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 
        private const int MOUSEEVENTF_WHEEL = 0x800; //模拟滑轮滚动，滚动幅度传递到dwData
        private const int WHEEL_DELTA = 120;//滑轮滚动幅度，1次滚动值,传递到dwData

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        [DllImport("user32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public MouseCommand(string cmd) : base(cmd) 
        {
            ScreenWidth = 1366;
            ScreenHeight = 768;
        }

        protected override void ExcuteCommand()
        {
            if (string.IsNullOrEmpty(this.CommandText))
            {
                throw new ArgumentNullException("CommandText");
            }

            string[] cmd = CommandText.Split('|');

            if (cmd.Length < 2)
            {
                throw new CommandParametersException("CommandText Less than 2 parameters");
            }
            else
            {
                switch(cmd[0].ToLower())
                {
                    case "mousemove":
                        mouseEvent(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mouseadmove":
                        System.Drawing.Point p = System.Windows.Forms.Cursor.Position;
                         int index = 0;
                        index =  cmd[1].IndexOf(',');
                        if(index>0)
                        {
                            try
                            {
                                int x = int.Parse(cmd[1].Substring(0, index));
                                int y = int.Parse(cmd[1].Substring(index + 1));
                                mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, (p.X + x) * 65535 / ScreenWidth, (p.Y + y) * 65535 / ScreenHeight, 0, 0);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        break;
                    case "mouselbdown":
                        mouseEvent(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mouserbdown":
                        mouseEvent(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mouselbup":
                        mouseEvent(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mouserbup":
                        mouseEvent(MOUSEEVENTF_RIGHTUP | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mouselbpress":
                        mouseEvent(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        mouseEvent(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mouserbpress":
                        mouseEvent(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        mouseEvent(MOUSEEVENTF_RIGHTUP | MOUSEEVENTF_ABSOLUTE, cmd[1]);
                        break;
                    case "mousescrollup":
                        mouseEvent(MOUSEEVENTF_WHEEL, cmd[1], WHEEL_DELTA);
                        break;
                    case "mousescrolldown":
                        mouseEvent(MOUSEEVENTF_WHEEL, cmd[1], -WHEEL_DELTA);
                        break;
                }
            }
        }

        private void mouseEvent(int msg , string paramers,int dwdata=0)
        {
            int index = 0;
            index = paramers.IndexOf(',');
            if(index>0)
            {
                try
                {
                    int x = int.Parse(paramers.Substring(0, index));
                    int y = int.Parse(paramers.Substring(index + 1));
                    mouse_event(msg, x * 65535 / ScreenWidth, y * 65535 / ScreenHeight, dwdata, 0);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
