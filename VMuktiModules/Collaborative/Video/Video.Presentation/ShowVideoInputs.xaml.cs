using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Video.Presentation
{
    /// <summary>
    /// Interaction logic for ShowVideoInputs.xaml
    /// </summary>
    public partial class ShowVideoInputs : Window
    {
        public delegate void delSelectedDevice(int selectedID);
        public event delSelectedDevice EntSelectedDevice;
        int InputDevice;

        public ShowVideoInputs(List<string> lstVideoInputs)
        {
            try
            {
                if (lstVideoInputs != null)
                {
                    InitializeComponent();
                    btnOK.Click += new RoutedEventHandler(btnOK_Click);

                    for (int i = 0; i < lstVideoInputs.Count; i++)
                    {
                        cmbVideoDevice.Items.Add(lstVideoInputs[i]);
                    }

                    cmbVideoDevice.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowVideoInputs", "ShowVideoInputs.xaml.cs");

            }
        }

        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbVideoDevice.SelectedItem != null)
                {
                    for (int i = 0; i < cmbVideoDevice.Items.Count; i++)
                    {
                        if (cmbVideoDevice.Items[i].ToString() == cmbVideoDevice.SelectedItem.ToString())
                        {
                            InputDevice = i;
                            break;
                        }
                    }

                    if (EntSelectedDevice != null)
                    {
                        EntSelectedDevice(InputDevice);
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnOK_Click", "ShowVideoInputs.xaml.cs");
            }
        }
    }
}
