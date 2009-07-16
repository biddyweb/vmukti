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
using VMuktiAPI;

namespace VMuktiGrid.CustomMenu
{
    /// <summary>
    /// Interaction logic for addBuddy.xaml
    /// </summary>
    public partial class ctlPgTabHeader : UserControl, IDisposable
    {       
        #region Public Properties
        private string strTitle;
        private bool disposed = false;

        public string Title
        {
            set
            {
                strTitle = value;
                try
                {
                    for (int i = 0; i < ((Grid)this.Content).Children.Count - 1; i++)
                    {
                        if (((Grid)this.Content).Children[i].GetType() == typeof(Border))
                        {
                            if (((Border)((Grid)this.Content).Children[i]).Child.GetType() == typeof(TextBlock))
                            {
                                ((TextBlock)((Border)((Grid)this.Content).Children[i]).Child).Text = value;
                            }
                            else if (((Border)((Grid)this.Content).Children[i]).Child.GetType() == typeof(TextBox))
                            {
                                ((TextBox)((Border)((Grid)this.Content).Children[i]).Child).Text = value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Title", "Controls\\VMuktiGrid\\Menu\\ctlPgTabHeader.xaml.cs");
                }
                //txtTitle.Text = value;
            }
            get
            {
                return strTitle;
            }
        }

        #endregion

        public ctlPgTabHeader()
        {
            try
            {
                InitializeComponent();
                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlPgTabHeader()", "Controls\\VMuktiGrid\\Menu\\ctlPgTabHeader.xaml.cs");
            }
            this.Loaded += new RoutedEventHandler(ctlPgTabHeader_Loaded);
            //txtTitle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(txtTitle_MouseDown);
        }

        void ctlPgTabHeader_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(ctlTab.TabItem))
            {
                brdIsSaved.Visibility = Visibility.Collapsed;
            }
        }
       
        #region Private Members

        public void txtTitle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //int count = 0;
                int i = 0;
                //((Border)(((TextBlock)sender).Parent)).Visibility = Visibility.Collapsed;

                for (i = 0; i < ((Grid)this.Content).Children.Count; i++)
                {
                    if (((Grid)this.Content).Children[i].GetType() == typeof(Border))
                    {
                        if (i == 1)
                        {
                            ((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Visible;
                            ((TextBox)((Border)((Grid)this.Content).Children[i]).Child).Text = strTitle;
                            ((TextBox)((Border)((Grid)this.Content).Children[i]).Child).SelectAll();
                            ((TextBox)((Border)((Grid)this.Content).Children[i]).Child).Focus();
                            e.Handled = true;
                        }
                        else if (i == 0)
                        {
                            ((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Collapsed;
                        }
                        else if (this.Parent.GetType() == typeof(ctlPage.TabItem))
                        {
                            //((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtTitle_MouseDown()", "Controls\\VMuktiGrid\\Menu\\ctlPgTabHeader.xaml.cs");
            }
        }

        #endregion

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //int count = 0;
                int i = 0;
                //((Border)(((TextBlock)sender).Parent)).Visibility = Visibility.Collapsed;

                for (i = 0; i < ((Grid)this.Content).Children.Count; i++)
                {
                    if (((Grid)this.Content).Children[i].GetType() == typeof(Border))
                    {
                        if (i == 1)
                        {
                            ((TextBlock)((Border)((Grid)this.Content).Children[0]).Child).Text = ((TextBox)sender).Text;
                            strTitle = ((TextBox)sender).Text;
                            ((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Collapsed;
                        }
                        else if (i == 0)
                        {
                            ((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Visible;
                        }
                        else if (this.Parent.GetType() == typeof(ctlPage.TabItem))
                        {
                            //((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Visible;
                            //((TextBlock)((Border)((Grid)this.Content).Children[i]).Child).Text = ((TextBox)sender).Text;
                            //strTitle = ((TextBox)sender).Text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TextBox_LostFocus()", "Controls\\VMuktiGrid\\Menu\\ctlPgTabHeader.xaml.cs");
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.Parent.GetType() == typeof(ctlPage.TabItem) && ((ctlPage.TabItem)this.Parent).IsSaved)
                {
                    ((ctlPage.TabItem)this.Parent).IsSaved = false;
                }

                if (e.Key == Key.Enter)
                {
                    //int count = 0;
                    int i = 0;
                    //((Border)(((TextBlock)sender).Parent)).Visibility = Visibility.Collapsed;

                    for (i = 0; i < ((Grid)this.Content).Children.Count; i++)
                    {
                        if (((Grid)this.Content).Children[i].GetType() == typeof(Border))
                        {
                            if (i == 1)
                            {
                                ((TextBlock)((Border)((Grid)this.Content).Children[0]).Child).Text = ((TextBox)sender).Text;
                                strTitle = ((TextBox)sender).Text;
                                ((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Collapsed;
                            }
                            else if (i == 0)
                            {
                                ((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Visible;
                            }
                            else if (this.Parent.GetType() == typeof(ctlPage.TabItem))
                            {
                                //((Border)((Grid)this.Content).Children[i]).Visibility = Visibility.Visible;
                                //((TextBlock)((Border)((Grid)this.Content).Children[i]).Child).Text = ((TextBox)sender).Text;
                                //strTitle = ((TextBox)sender).Text;
                            }
                        }
                    }
                }
                else
                {
                    ((TextBlock)((Border)((Grid)this.Content).Children[0]).Child).Text = ((TextBox)sender).Text;
                    strTitle = ((TextBox)sender).Text;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TextBox_PreviewKeyDown()", "Controls\\VMuktiGrid\\Menu\\ctlPgTabHeader.xaml.cs");
            }
        }

        #region IDisposable Members

       public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
			{
				if (disposing)
				{
					try
					{
                       
					}
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Menu\\ctlPgTabHeader.xaml.cs");
                    }
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlPgTabHeader()
        {
            Dispose(false);
        }
        #endregion
    }
}
