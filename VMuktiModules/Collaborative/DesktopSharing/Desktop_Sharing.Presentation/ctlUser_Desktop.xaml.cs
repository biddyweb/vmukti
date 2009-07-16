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
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Drawing;
using VMuktiAPI;
using VMuktiService;

namespace Desktop_Sharing.Presentation
{
    /// <summary>
    /// Interaction logic for ctlUser_Desktop.xaml
    /// </summary>
    public partial class ctlUser_Desktop : UserControl
    {
        public delegate void delSelected(string uName);
        public event delSelected EntSelected;

        public delegate void DelRemoveUser(string UName);
        public event DelRemoveUser EntRemoveUser;

        public delegate void DelFullScreen(string uName);
        public event DelFullScreen EntFullScreen;

        public ctlUser_Desktop(string uName)
        {
            try
            {
                
                InitializeComponent();

                //cbMenu.Items.Add("Full Screen");
                cbMenu.Items.Add("Close");


                lblUName.Content = uName;
                picUserVideo.MouseLeftButtonDown += new MouseButtonEventHandler(picUserVideo_MouseDown);
                cbMenu.DropDownClosed += new EventHandler(cbMenu_DropDownClosed);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcGetUserList", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void cbMenu_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cbMenu.SelectedItem != null && cbMenu.SelectedItem.ToString() == "Full Screen")
                {
                    try
                    {
                        if (EntFullScreen != null)
                        {
                            EntFullScreen(lblUName.Content.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUser_Desktop.xaml.cs");
                    }
                }
                else if (cbMenu.SelectedItem != null && cbMenu.SelectedItem.ToString() == "Close")
                {
                    try
                    {
                        MessageBoxResult result = MessageBox.Show("Do You Really Want To Remove " + lblUName.Content.ToString() + "'s Desktop", "Remove Desktop", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            if (EntRemoveUser != null)
                            {
                                EntRemoveUser(lblUName.Content.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUser_Desktop.xaml.cs");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUser_Desktop.xaml.cs"); 
            }
        }

        void picUserVideo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EntSelected != null)
                {
                    EntSelected(lblUName.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "picUserVideo_MouseDown", "ctlUser_Desktop.xaml.cs");
            }
        }
    }
}
