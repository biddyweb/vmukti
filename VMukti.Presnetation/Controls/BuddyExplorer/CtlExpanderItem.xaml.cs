/*
*VMukti -- An open source video conferencing platform.
*
* Copyright (C) 2007 - 2008, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.
*
* This program is free software, distributed under the terms of
* the GNU General Public License Version 2. See the LICENSE file
* at the top of the source tree.
*/
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VMuktiAPI;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using VMukti.Business;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.ComponentModel;

namespace VMukti.Presentation.Controls
{
    public partial class CtlExpanderItem : System.Windows.Controls.UserControl, IDisposable
    {

        SolidColorBrush objMouseover = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAEE7EE"));
        SolidColorBrush objMouseoverborder = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00C8C7"));
        string strImgPath = "";

        public static StringBuilder sb1=new StringBuilder();

        #region BuddySingleClick + Widgets

        UniformGrid objuniformGrid;

        public delegate void DelLoadSelectedWid(CtlExpanderItem buddyname, int modid);
        public event DelLoadSelectedWid EntLoadSelectedWid;

        #endregion

        #region BuddySingleClick + Widgets

        public Separator spWidgets
        {
            get
            {
                return (Separator)(((Grid)(((Border)(this.Content)).Child)).Children[2]);
            }

        }

        public UniformGrid ufgWidgets
        {
            get
            {
                return objuniformGrid;
            }

        }

        #endregion

        public string Caption
        {
            get
            {
               return ((Label)(((Grid)(((Border)(this.Content)).Child)).Children[1])).Content.ToString();
            }
            set
            {
                ((Label)(((Grid)(((Border)(this.Content)).Child)).Children[1])).Content = value;
            }
        }

        public string Image
        {
            get
            {
                return strImgPath;
            }
            set
            {
                strImgPath = value;
                if (value != "")
                {
                    ((Grid)(((Border)(this.Content)).Child)).ColumnDefinitions[0].Width = new GridLength(20);
                    ((Image)(((Grid)(((Border)(this.Content)).Child)).Children[0])).Source = new BitmapImage(new Uri(strImgPath, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    ((Grid)(((Border)(this.Content)).Child)).ColumnDefinitions[0].Width = new GridLength(0);
                    ((Image)(((Grid)(((Border)(this.Content)).Child)).Children[0])).Source = null;
                
                }
            }
        }

        public CtlExpanderItem()
        {
            try
            {
                this.InitializeComponent();
                this.MouseEnter += new System.Windows.Input.MouseEventHandler(CtlExpanderItem_MouseEnter);
                this.MouseLeave += new System.Windows.Input.MouseEventHandler(CtlExpanderItem_MouseLeave);

                int rem;
                //ClsModuleCollection cmc = ClsModuleCollection.GetOnlyCollMod();
                int i = App.lstCollOnly.Count + 2;
               
                int quotient = Math.DivRem(i, 3, out rem);
                objuniformGrid =((UniformGrid)(((Grid)(((Border)(this.Content)).Child)).Children[3]))as UniformGrid;
                objuniformGrid.Columns = 3;
                if (rem > 0)
                {
                    objuniformGrid.Rows = quotient + 1;
                }
                else
                {
                    objuniformGrid.Rows = quotient;
                }

                for (int widCnt = 0; widCnt < App.lstCollOnly.Count; widCnt++)
                {
                    Button btnWidget = new Button();

                    try
                    {
                        BitmapImage bimg = new BitmapImage();
                        Image img = new Image();
                        if (App.lstCollOnly[widCnt].ImageFile != null)
                        {
                            bimg.BeginInit();
                            bimg.StreamSource = new MemoryStream(App.lstCollOnly[widCnt].ImageFile);
                            bimg.EndInit();

                           
                            img.Source = bimg;
                            img.Height = 20;
                            img.Width = 20;
                        }
                        btnWidget.ToolTip = App.lstCollOnly[widCnt].ModuleTitle;
                        btnWidget.Content = img;

                        btnWidget.Tag = App.lstCollOnly[widCnt].ModuleId;
                        btnWidget.PreviewMouseDown += new MouseButtonEventHandler(btnWidget_PreviewMouseDown);
                        btnWidget.Margin = new Thickness(2, 0, 0, 0);
                        btnWidget.BorderBrush = Brushes.Transparent;
                        btnWidget.BorderThickness = new Thickness(0);
                        objuniformGrid.Children.Add(btnWidget);
                    }
                    catch (Exception exp)
                    {
                        VMuktiHelper.ExceptionHandler(exp, "CtlExpanderItem(ImageLoading)", "Controls\\BuddyExplorer\\CtlExpanderItem.xaml.cs");
                    }

                   }
                #region dhaval
                for (int im = 0; im <= 1; im++)
                {
                    Button btnWidget2 = new Button();
                    try
                    {
                        if (im == 0)
                        {

                          //  int ds = System.AppDomain.CurrentDomain.BaseDirectory.IndexOf("VMukti.Presentation.exe");                            
                          //  string url = System.AppDomain.CurrentDomain.BaseDirectory.ToString().Remove(ds);
                            System.Drawing.Bitmap BM = new System.Drawing.Bitmap(System.AppDomain.CurrentDomain.BaseDirectory + @"Skins/Images1/userprofile.png");
                            MemoryStream mms = new MemoryStream();
                            BM.Save(mms, System.Drawing.Imaging.ImageFormat.Png);
                            mms.Position = 0;
                            BitmapImage bimg = new BitmapImage();
                            bimg.BeginInit();
                            bimg.StreamSource = new MemoryStream(mms.ToArray());
                            bimg.EndInit();
                            Image img = new Image();
                            img.Source = bimg;
                            img.Height = 20;
                            img.Width = 20;
                            btnWidget2.ToolTip = "View Profile";
                            btnWidget2.Content = img;
                            btnWidget2.Tag = "@";
                            btnWidget2.PreviewMouseDown += new MouseButtonEventHandler(btnWidget2_PreviewMouseDown);
                            btnWidget2.Margin = new Thickness(2, 0, 0, 0);
                            btnWidget2.BorderBrush = Brushes.Transparent;
                            btnWidget2.BorderThickness = new Thickness(0);
                            objuniformGrid.Children.Add(btnWidget2);
                        }
                        else
                        {
                           // int ds = System.AppDomain.CurrentDomain.BaseDirectory.IndexOf("bin");
                           // string url = System.AppDomain.CurrentDomain.BaseDirectory.ToString().Remove(ds);
                            System.Drawing.Bitmap BM = new System.Drawing.Bitmap(System.AppDomain.CurrentDomain.BaseDirectory + @"Skins/Images1/Delete.png");
                            MemoryStream mms = new MemoryStream();
                            BM.Save(mms, System.Drawing.Imaging.ImageFormat.Png);
                            mms.Position = 0;
                            BitmapImage bimg = new BitmapImage();
                            bimg.BeginInit();
                            bimg.StreamSource = new MemoryStream(mms.ToArray());
                            bimg.EndInit();
                            Image img = new Image();
                            img.Source = bimg;
                            img.Height = 20;
                            img.Width = 20;
                            btnWidget2.ToolTip = "Remove Buddy";
                            btnWidget2.Content = img;
                            btnWidget2.Tag = 999;
                            btnWidget2.PreviewMouseDown += new MouseButtonEventHandler(btnWidget_PreviewMouseDown);
                            btnWidget2.Margin = new Thickness(2, 0, 0, 0);
                            btnWidget2.BorderBrush = Brushes.Transparent;
                            btnWidget2.BorderThickness = new Thickness(0);
                            objuniformGrid.Children.Add(btnWidget2);
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "CtlExpanderItem(RemoveBuddy)", "Controls\\BuddyExplorer\\CtlExpanderItem.xaml.cs");
                    }
                }
                #endregion dhaval
            

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlExpanderItem()", "Controls\\BuddyExplorer\\CtlExpanderItem.xaml.cs");
            }
        }

        void CtlExpanderItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                ((Grid)(((Border)(this.Content)).Child)).Background = System.Windows.Media.Brushes.Transparent;
                ((Border)(this.Content)).BorderBrush = System.Windows.Media.Brushes.Transparent;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlExpanderItem_MouseLeave()", "Controls\\BuddyExplorer\\CtlExpanderItem.xaml.cs");
            }
        }

        void CtlExpanderItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                ((Grid)(((Border)(this.Content)).Child)).Background = objMouseover;
                ((Border)(this.Content)).BorderBrush = objMouseoverborder;
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlExpandertem_MouseEnter()", "Controls\\BuddyExplorer\\CtlExpanderItem.xaml.cs");
            }
        }

        ~CtlExpanderItem()
        {
            Dispose();
        }

        #region BuddySingleClick + Widgets

        void btnWidget_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (EntLoadSelectedWid != null)
            {
                EntLoadSelectedWid(this, int.Parse(((Button)sender).Tag.ToString()));
            }
        }      
        #region dhaval

        void btnWidget2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (((Button)sender).ToolTip.ToString() == "View Profile")
            {
                
                string buddies = this.Caption.ToString();                
                string buddyProfile = null;
                if (buddies.Contains(","))
                {
                    buddyProfile = buddies.Substring(buddies.LastIndexOf(',') + 1);
                }
                else
                {
                    buddyProfile = buddies;
                }

                int userID = VMukti.Business.clsProfile.GetUserID(buddyProfile);

                VMuktiAPI.VMuktiHelper.CallEvent("ViewProfile", null, new VMuktiEventArgs(userID));
            }

        }
        #endregion dhaval

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (strImgPath != null)
            {
                strImgPath = null;
            }
            
        }

        #endregion
    }
}