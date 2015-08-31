using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace Command
{
    public class KeyBoardCommand:BaseCommand
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            byte bVk,
            byte bScan,   //一般情况下设成为 0
            int dwFlags,  //这里是整数类型  0 为按下，2为释放
            int dwExtraInfo  //这里是整数类型 一般情况下设成为 0
        );
        private Timer timer;
        private int times = 0;
        private int excuteTimes = 0;
        private byte vk;
        private delegate void DelegateKeyEvent(byte vk);
        private DelegateKeyEvent keyhandel;

        public KeyBoardCommand(string cmd):base(cmd)
        { }
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
                if (cmd.Length >= 3)
                {
                    if (int.TryParse(cmd[2], out times)&&times!=0)
                    {
                        timer = new Timer();
                        timer.Elapsed += timer_Elapsed;
                        timer.Interval = 1000;
                    }
                }
                try
                {
                    vk = byte.Parse(cmd[1]);
                    switch(cmd[0].ToLower())
                    {
                        case "keydown":
                            keyhandel = new DelegateKeyEvent(keyDown);
                            break;
                        case "keyup":
                            keyhandel = new DelegateKeyEvent(keyUp);
                            break;
                        case "keypress":
                            keyhandel = new DelegateKeyEvent(keypress);                           
                            break;
                        default:
                            throw new CommandParametersException("The command is not right");
                    }
                    if(timer!=null)
                    {
                        timer.Start();
                    }
                    else
                    {
                        if(keyhandel!=null)
                            keyhandel(vk);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
          if(keyhandel!=null)
          {
              if (times < 0 || excuteTimes < times)
              {
                  keyhandel(vk);
                  excuteTimes++;
              }
          }
        }

        private void keyDown(byte vk)
        {
            keybd_event(vk, 0, 0, 0);
        }
        private void keyUp(byte vk)
        {
            keybd_event(vk, 0, 2, 0);
        }
        private void keypress(byte vk)
        {
            keybd_event(vk, 0, 0, 0);
            keybd_event(vk, 0, 2, 0);
        }
    }
}
