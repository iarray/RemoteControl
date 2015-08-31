using Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CustomCommandPlugin
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private ObservableCollection<Item> items;
        CustomCommandExecutor E;
        public UserControl1(ObservableCollection<Item> items,CustomCommandExecutor e)
        {
            InitializeComponent();
            this.Loaded += UserControl1_Loaded;
            this.items = items;
            E = e;
        }

        void UserControl1_Loaded(object sender, EventArgs e)
        {
            load();
            lv.ItemsSource = items;
        }

        private void load()
        {
            try
            {
                string path = Path.Combine(System.Environment.GetEnvironmentVariable("appdata"), "PocketDesktop");
                string cfgPath = Path.Combine(path, "CustomCommand.cfg");
                if (File.Exists(cfgPath))
                {
                    items.Clear();
                    using (StreamReader sr = new StreamReader(cfgPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            Item i = Item.Parse(s);
                            if (i != null)
                                items.Add(i);
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void save()
        {
            try
            {

                string path = Path.Combine(System.Environment.GetEnvironmentVariable("appdata"), "PocketDesktop");
                string cfgPath = Path.Combine(path, "CustomCommand.cfg");
                using (StreamWriter sw = new StreamWriter(cfgPath))
                {
                    foreach (Item i in items)
                    {
                        sw.WriteLine(i.ToString());
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(tbCmd.Text)||string.IsNullOrEmpty(tbParm.Text))
            {
                return;
            }
            Item i=new Item();
            i.Cmd = tbCmd.Text;
            i.Action = (Action)cbAction.SelectedIndex;
            i.Parameter = tbParm.Text;
            items.Add(i);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            save();
            E.SetCommandText();
            tbTz.Visibility=Visibility.Visible;
        }
        
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (lv.SelectedIndex >= 0)
            {
                items.RemoveAt(lv.SelectedIndex);
            }
        }
    }
}
