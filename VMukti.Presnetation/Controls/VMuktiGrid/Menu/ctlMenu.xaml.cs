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
    /// Interaction logic for comboBox.xaml
    /// </summary>
    public partial class ctlMenu : UserControl, IDisposable
    {     
        private bool disposed = false;
        ctlMenuItem _objMISave = null;
        bool _ShowSaveOption = false;

        public bool ShowSaveOption
        {
            get { return _ShowSaveOption; }
            set 
            { 
                _ShowSaveOption = value;
                if (!value)
                {
                    stPanel.Children.Remove(_objMISave);
                }
                else
                {
                    stPanel.Children.Insert(4, _objMISave);
                }
            }
        }

        public ctlMenu()
        {
            try
            {
                InitializeComponent();
                objMIClose.strLeftImage = @"\Skins\Images\Delete.Png";
                _objMISave = objMISave;
                stPanel.Children.Remove(_objMISave);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlMenu()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        public void AddItem(string strBuddyName)
        {
            try
            {
                ctlMenuItem objMenuItem = new ctlMenuItem();
                objMenuItem.Title = strBuddyName;
                stPanel.Children.Add(objMenuItem);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddItem()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menuPopUP.IsOpen = (!menuPopUP.IsOpen);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMenu_Click()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        private void btnClosePopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menuPopUP.IsOpen = (!menuPopUP.IsOpen);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClosePopUp_click()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        public bool AddBuddy(string strBuddyName)
        {
            try
            {
                return objEMIBuddyList.AddBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");                
                return false;
            }
        }

        public bool RemoveBuddy(string strBuddyName)
        {
            try
            {
                return objEMIBuddyList.RemoveBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
                return false;
            }
        }

        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                objEMIBuddyList.SetMaxCounter(intMaxCounter, strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetMaxCounter()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        public bool CheckBuddy(string strBuddyName)
        {
            try
            {
                return objEMIBuddyList.CheckBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");                
                return false;
            }
        }

        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                objEMIBuddyList.ShowBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        public List<string> GetBuddies()
        {
            return objEMIBuddyList.GetBuddies();
        }

        private void objMIRename_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ((ctlPgTabHeader)((ContentPresenter)((Grid)this.Parent).FindName("conHeader")).Content).txtTitle_MouseDown(sender, e);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objMIRename_PreviewMouseLeftButtonDown()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        private void objMISave_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ((VMuktiGrid.ctlPage.TabItem)((Border)((Grid)this.Parent).Parent).TemplatedParent).Save();
                objMISave.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objMISave_PreviewMouseLeftButtonDown()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        private void objMIDelete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Border)((Grid)this.Parent).Parent).TemplatedParent.GetType() == typeof(VMuktiGrid.ctlPage.TabItem))
                {
                    ((VMuktiGrid.ctlPage.TabItem)((Border)((Grid)this.Parent).Parent).TemplatedParent).Delete();
                }
                else if (((Border)((Grid)this.Parent).Parent).TemplatedParent.GetType() == typeof(VMuktiGrid.ctlTab.TabItem))
                {
                    ((VMuktiGrid.ctlTab.TabItem)((Border)((Grid)this.Parent).Parent).TemplatedParent).Delete();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objMIelete_PreviewMouseLeftButtonDown()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        private void objMIClose_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Border)((Grid)this.Parent).Parent).TemplatedParent.GetType() == typeof(VMuktiGrid.ctlPage.TabItem))
                {
                    ((VMuktiGrid.ctlPage.TabItem)((Border)((Grid)this.Parent).Parent).TemplatedParent).Close();
                }
                else if (((Border)((Grid)this.Parent).Parent).TemplatedParent.GetType() == typeof(VMuktiGrid.ctlTab.TabItem))
                {
                    ((VMuktiGrid.ctlTab.TabItem)((Border)((Grid)this.Parent).Parent).TemplatedParent).Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objMIClose_PreviewMouseLeftButtonDown()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");
            }
        }
        public void Dispose(bool disposing)
        {
             if (!this.disposed)
                {
                    if (disposing)
                    {
                        try
                        {
                            stPanel.Children.Clear();
                        }

                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "Controls\\VMuktiGrid\\Menu\\ctlMenu.xaml.cs");                            
                            disposed = true;

                            GC.SuppressFinalize(this);
                        }
                    }
                }
           
        }

        ~ctlMenu()
        {
            Dispose(false);
        }

        #endregion
    }
}
