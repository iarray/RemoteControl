using Metro.Controls;
using Plugin;
using RemoteControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using Wifi;

namespace RemoteControlServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private WiFiCreator wc;
        private IPMulticastServer ipmserver = new IPMulticastServer();
        private ControlServer server;
        List<Plugin.PluginBase> l = new List<Plugin.PluginBase>();
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private System.Windows.Forms.ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
        private System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
        public MainWindow()
        {
            InitializeComponent();
            SetStartPosition();
            CreatAppDataFolder();
            StartServer();
            Loaded += MainWindow_Loaded;

            string dirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            l = Plugin.PluginLoader.Load<PluginBase>(dirPath+"\\Plugins");
            lv.ItemsSource = l;
            foreach (var item in l)
            {
                item.IExecutorUsedChangedEvent += (ie, x) =>
                {
                    if (x && !server.Executors.Contains(ie))
                    {
                        server.Executors.Add(ie);
                    }
                    else
                    {
                        server.Executors.Remove(ie);
                    }
                };
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //隐藏任务栏图标
            this.ShowInTaskbar = false;
            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "NetShuttle";
            StreamResourceInfo mini_ico = Application.GetResourceStream(new Uri("mini.ico", UriKind.Relative));
            notifyIcon.Icon = new System.Drawing.Icon(mini_ico.Stream);
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            item.Text = "退出";
            contextMenu.Items.Add(item);
            this.notifyIcon.ContextMenuStrip = contextMenu;
            item.Click += item_Click;
        }

        private void item_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //如果鼠标左键单击
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Visibility = Visibility.Visible;
                this.ShowInTaskbar = true;
                this.Activate();
            }
        }

        public void StartServer()
        {
            if(server!=null)
            {
                server.Dispose();
            }
            int port = 0;
            if(!int.TryParse(tbPort.Text,out port)||port>65536||port<1024)
            {
                port=9999;
                tbPort.Text = "9999";
            }
            server = new ControlServer(IPAddress.Any, port);
            if(cbKey.IsChecked==true&&tbKey.Text.Length<=12&&tbKey.Text.Length>=2)
            {
                ipmserver.AuthenticationString = tbKey.Text;
                server.Executors.Add(new AuthenticateExecutor(tbKey.Text));
            }
            else
            {
                tbKey.Text = "";
                ipmserver.AuthenticationString = null;
                server.Executors.Add(new AuthenticateExecutor());
            }
            if(cbAllowMouse.IsChecked==true)
            {
                server.Executors.Add(new CommandExecutor());
            }
            if(cbAllowFileList.IsChecked==true)
            {
                server.Executors.Add(new FileListTransmissionExecutor());
            }
            if(cbAllowFileTrans.IsChecked==true)
            {
                server.Executors.Add(new FileTransmissionExecutor());
            }
            if(ipmserver.ServerStatus==ServerStatus.Stop)
            {
                ipmserver.Start();
            }
            server.Executors.Add(new DesktopTransportExecutor());
            server.Listen();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(wc!=null)
            {
                wc.Dispose();
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void SetStartPosition()
        {
            this.Left = SystemParameters.WorkArea.Width - this.Width;
            this.Top = SystemParameters.WorkArea.Height - this.Height;
        }

        private void btnOpenWifi_Click(object sender, RoutedEventArgs e)
        {
            if(tbName.Text.Length<2||tbName.Text.Length>20)
            {
                tbSSIDJG.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                tbSSIDJG.Visibility = Visibility.Hidden;
            }
            if (pbPwd.Password.Length < 8 || pbPwd.Password.Length > 20)
            {
                tbPwdJG.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                tbPwdJG.Visibility = Visibility.Hidden;
            }
            if (btnOpenWifi.Content.ToString() == "开启热点")
            {
                wc = new WiFiCreator(tbName.Text, pbPwd.Password);
                btnOpenWifi.Content = "正在开启...";
                btnOpenWifi.IsEnabled = false;
                Thread thread=new Thread(new ThreadStart(()=>{
                    wc.IP = "192.168.137.1";
                    wc.Creat();
                    if (wc.StartAp())
                    {
                        this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                            {
                                btnOpenWifi.Content = "关闭热点";
                                icoWifi.Source = new BitmapImage(new Uri("Images/wifi-white.png", UriKind.Relative));
                                btnOpenWifi.IsEnabled = true;
                            }));
                        
                    }
                }));
                thread.IsBackground = true;
                thread.Start();
            }
            else if (btnOpenWifi.Content.ToString() == "关闭热点")
            {
                if(wc!=null&&wc.WiFiStatus==WiFiStatus.Running)
                {
                    if (wc.StopAp())
                    {
                        btnOpenWifi.Content = "开启热点";
                        icoWifi.Source = new BitmapImage(new Uri("Images/wifi-black.png", UriKind.Relative));
                    }
                }
            }
            else
            {

            }
        }

        private void btnUse_Click(object sender, RoutedEventArgs e)
        {
            StartServer();
        }

        private void cbKey_Click(object sender, RoutedEventArgs e)
        {
            if(cbKey.IsChecked==true)
            {
                tbKey.IsEnabled = true;
            }
            else
            {
                tbKey.IsEnabled = false;
            }
        }

        private void CreatAppDataFolder()
        {
            string path =Path.Combine(System.Environment.GetEnvironmentVariable("appdata"),"PocketDesktop");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void PluginSetting_Click(object sender, RoutedEventArgs e)
        {
            int index = lv.SelectedIndex;
            if(index>=0)
            {
                MetroWindow w = new MetroWindow();
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                w.Background = new SolidColorBrush(Color.FromRgb(138,184,200));
                //w.RectangleColor = new SolidColorBrush(Color.FromRgb(76,119,128));
                w.RectangleColor = new SolidColorBrush(Colors.Transparent);
                Grid g = new Grid();
                //g.Background = new SolidColorBrush(Color.FromRgb(120,120,120));
                g.Background = new SolidColorBrush(Color.FromRgb(239, 238, 233));
                w.Title = l[index].Name + "-设置";
                w.Content = g;
                UserControl uc = l[index].GetControl();
                w.Height = uc.Height+20;
                w.Width = uc.Width+20;
                g.Children.Add(uc);
                w.ShowDialog();
            }
        }
    }
}