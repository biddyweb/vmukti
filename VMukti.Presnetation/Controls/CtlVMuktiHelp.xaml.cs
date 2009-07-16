/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists      
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
 
*/

using System;
using System.Collections.Generic;
//using System.Linq;
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
using VMukti.ZipUnzip.Zip;
//VMukti.Presentation.Controls.UploadModule
namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CtlVMuktiHelp : UserControl
    {
        private System.Timers.Timer t1 = new System.Timers.Timer(1000);
        int counter = 0;
        public string strBasePath;

        public CtlVMuktiHelp()
        {
            try
            {
                InitializeComponent();
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "HelpDocs"))
                {
                    FastZip fz = new FastZip();
                    fz.ExtractZip(AppDomain.CurrentDomain.BaseDirectory.ToString() + "HelpDocs.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "HelpDocs", null);
                }
                strBasePath = AppDomain.CurrentDomain.BaseDirectory;
                BitmapImage bmi = new BitmapImage(new Uri(strBasePath + "HelpDocs\\Help_0.JPG"));
                image1.Source = bmi;

            }
            catch
            {
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textBox1.Text = "";

                if (counter == 7)
                {
                    btn_next.IsEnabled = false;
                }
                else
                {
                    counter++;
                    btn_prev.IsEnabled = true;
                }

                FileStream f11 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "HelpDocs\\Help_" + counter + ".txt", FileMode.Open);
                string filename = AppDomain.CurrentDomain.BaseDirectory + "HelpDocs\\Help_" + counter + ".JPG";
                StreamReader sr = new StreamReader(f11);
                String line;

                // Read in the data line by line and add it to our ArrayList
                while ((line = sr.ReadLine()) != null)
                {
                    textBox1.Text += line + "\n";
                }
                sr.Close();

                byte[] mydata = new byte[244];
                MemoryStream ms = new MemoryStream(mydata);
                BitmapImage bi = new BitmapImage(new Uri(filename));

                bi.StreamSource = ms;

                image1.Source = bi;
            }
            catch { }

        }

        private void btn_prev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textBox1.Text = "";

                if (counter == 0)
                {
                    btn_prev.IsEnabled = false;
                }
                else
                {
                    counter--;
                    btn_next.IsEnabled = true;
                }

                // there is no help_0.txt
                if (counter == 0)
                {
                    image1.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "HelpDocs\\Help_0.jpg"));
                    return;
                }
                FileStream f12 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "HelpDocs\\Help_" + counter + ".txt", FileMode.Open);
                string filename = AppDomain.CurrentDomain.BaseDirectory + "HelpDocs\\Help_" + counter + ".JPG";
                StreamReader sr = new StreamReader(f12);
                String line;

                // Read in the data line by line and add it to our ArrayList
                while ((line = sr.ReadLine()) != null)
                {
                    textBox1.Text += line + "\n";
                }

                sr.Close();
                byte[] mydata = new byte[244];
                MemoryStream ms = new MemoryStream(mydata);
                BitmapImage bi = new BitmapImage(new Uri(filename));

                bi.StreamSource = ms;

                image1.Source = bi;
            }
            catch { }

        }

    }
}
