using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using System.Timers;

namespace VMuktiMonitor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        static Timer t;
        static Timer tLogFiles;
        static long fsLength = 0;

        public delegate void DelUpdateRTB(string strRTB);
        
        DelUpdateRTB objDelUpdateRtb = null;

        public delegate void DelUpdateRTB4Logging(string strRTB);
        DelUpdateRTB4Logging objDelUpdateRtbLogging = null;

        public Window1()
        {
            try
            {
                InitializeComponent();

                t = new System.Timers.Timer(2000);
                t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
                t.Start();


                objDelUpdateRtb = new DelUpdateRTB(UpdateRTB);
                objDelUpdateRtbLogging = new DelUpdateRTB4Logging(UpdateRTBLogging);

                rtbConsole.IsReadOnly = true;
                rtbConsole.Height = this.Height;
                rtbConsole.MouseDoubleClick += new MouseButtonEventHandler(rtbConsole_MouseDoubleClick);
                rtbConsole.MouseDown += new MouseButtonEventHandler(rtbConsole_MouseDown);
                rtbConsole.MouseLeftButtonDown += new MouseButtonEventHandler(rtbConsole_MouseLeftButtonDown);
                rtbConsole.MouseRightButtonDown += new MouseButtonEventHandler(rtbConsole_MouseRightButtonDown);                
                //if (Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "LogFiles").Length > 0)
                //{
                tLogFiles = new Timer(3000);
                tLogFiles.Elapsed += new ElapsedEventHandler(tLogFiles_Elapsed);
                tLogFiles.Start();
                //}
            }
            catch (Exception exp)
            {
                //MessageBox.Show("exp " + exp.Message);
            }
        }

        void rtbConsole_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rtbConsole.CaretPosition = rtbConsole.Document.ContentEnd;
        }

        void rtbConsole_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rtbConsole.CaretPosition = rtbConsole.Document.ContentEnd;
        }

        void rtbConsole_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtbConsole.CaretPosition = rtbConsole.Document.ContentEnd;
        }

        void rtbConsole_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            rtbConsole.CaretPosition = rtbConsole.Document.ContentEnd;
        }

        void tLogFiles_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                string[] dirpath = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "LogFiles");

                foreach (string s in dirpath)
                {
                    string[] filepath = Directory.GetFiles(s);
                    foreach (string f in filepath)
                    {
                        string strRTB = File.ReadAllText(f);                        
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUpdateRtbLogging, strRTB);
                    }
                    Directory.Delete(s, true);                 
                }
            }
            catch (Exception exp)
            {
            }
        }
    
        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //read the file and write to console
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt"))
                {
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    
                    if (fs.Length > 0 && fs.Length > fsLength)
                    {
                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\temp.txt",FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                        long newlength = fs.Length - fsLength;
                        byte[] buffer = new byte[int.Parse(newlength.ToString())];
                        fs1.Position = fsLength;
                        int bytesRead = fs1.Read(buffer, 0, int.Parse(newlength.ToString()));
                        string strRTB=ASCIIEncoding.ASCII.GetString(buffer);
                        fsLength = fs.Length;
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelUpdateRtb, strRTB);                                                                                             
                    }                   
                }
            }
            catch (Exception exp)
            {
                // Console.WriteLine("t_Elapsed " + exp.Message);
                //MessageBox.Show("exp : " + exp.Message);
                if (t != null)
                {
                    t.Stop();
                }
            }
        }

        public void UpdateRTB(string strRTB)
        {
            try
            {
                Run r = new Run(strRTB, rtbConsole.CaretPosition);
                r.Foreground = Brushes.Black;
                rtbConsole.AppendText("\r\n");
                rtbConsole.ScrollToEnd();
                rtbConsole.CaretPosition = rtbConsole.Document.ContentEnd;
            }
            catch (Exception exp)
            {

            }
        }

        void UpdateRTBLogging(string strRTB)
        {
            try
            {
                Run r = new Run(strRTB, rtbConsole.CaretPosition);
                r.Foreground = Brushes.Blue;
                rtbConsole.AppendText("\r\n");
                rtbConsole.ScrollToEnd();
                rtbConsole.CaretPosition = rtbConsole.Document.ContentEnd;
                
            }
            catch (Exception exp)
            {

            }
        }
    }
}
