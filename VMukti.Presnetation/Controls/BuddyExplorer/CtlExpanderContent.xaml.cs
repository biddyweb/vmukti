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
using System.Windows;
using System.Windows.Controls;
using VMuktiAPI;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using VMukti.Business.VMuktiGrid;
using System.Text;
using System.Windows.Controls.Primitives;

namespace VMukti.Presentation.Controls
{
    public enum ImageType
    {
        Page,
        Module,
        MaleBuddy,
        FemaleBuddy
    }

    public partial class CtlExpanderContent : System.Windows.Controls.UserControl, IDisposable
    {        
        public delegate void DelItemSelectionChanged(string strTagText, string strContent);
        public event DelItemSelectionChanged EntItemSelectionChanged;

        public delegate void DelCModBuddies(Dictionary<CtlExpanderItem, string> buddiesname, int modid);
        public event DelCModBuddies EntCModBuddies;

        CtlExpanderItem objExpanderItem = null;
        
        public Dictionary<CtlExpanderItem, string> selectedExItems = new Dictionary<CtlExpanderItem, string>();

        #region BuddySingleClick + Widgets

        public delegate void DelWidgetToolBar(string text);
        public event DelWidgetToolBar EntWidgetToolBar;

        #endregion

        public CtlExpanderContent()
        {
            try
            {
                this.InitializeComponent();
                //trvExpanderContent.PreviewMouseRightButtonDown += new MouseButtonEventHandler(trvExpanderContent_PreviewMouseRightButtonDown);

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CtlExpanderContent()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }


        void menuItem1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedExItems != null)
                {
                    MessageBoxResult msgResult = MessageBox.Show("Are you sure to remove selected buddies from your buddylist", "Remove Buddy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (msgResult == MessageBoxResult.Yes)
                    {
                        List<string> lstBuddiesName = new List<string>();
                        foreach (KeyValuePair<CtlExpanderItem, string> kvp in selectedExItems)
                        {
                            lstBuddiesName.Add(kvp.Value);
                            for (int i = 0; i < trvExpanderContent.Items.Count; i++)
                            {
                                if (kvp.Value == ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[i]).Header).Caption.ToString())
                                {
                                    trvExpanderContent.Items.RemoveAt(i);
                                }
                            }
                           // trvExpanderContent.Items.Remove(kvp.Key);
                        }
                        try
                        {
                            App.chHttpBootStrapService.svcHttpRemoveBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddiesName);
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "menuItem1_Click()--1", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
                            VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                            App.chHttpBootStrapService.svcHttpRemoveBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddiesName);
                        }
                        catch (System.ServiceModel.CommunicationException ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "menuItem1_Click()--2", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
                            VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                            App.chHttpBootStrapService.svcHttpRemoveBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddiesName);
                        }

                        try
                        {
                            App.chHttpSuperNodeService.RemoveBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddiesName);
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "menuItem1_Click()--3", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
                            VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                            App.chHttpSuperNodeService.RemoveBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddiesName);
                        }
                        catch (System.ServiceModel.CommunicationException ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "menuItem1_Click()--4", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
                            VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                            App.chHttpSuperNodeService.RemoveBuddies(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddiesName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "menuItem1_Click()--5", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        public void AddItem(string strCaption)
        {
            try
            {
                objExpanderItem = new CtlExpanderItem();
                objExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                objExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;

                objExpanderItem.Image = "";
                objExpanderItem.Caption = strCaption;

                TreeViewItem objTrviExpanderItem = new TreeViewItem();
                objTrviExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                objTrviExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;

                objTrviExpanderItem.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(objTrviExpanderItem_PreviewMouseLeftButtonDown);
                objTrviExpanderItem.Header = objExpanderItem;

                trvExpanderContent.Items.Add(objTrviExpanderItem);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddItem()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }        

        public void AddItem(string strCaption, bool ShowImage, ImageType objImageType, string strTag)
        {
            try
            {
                objExpanderItem = new CtlExpanderItem();


                objExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                objExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;

                if (objImageType == ImageType.MaleBuddy || objImageType == ImageType.FemaleBuddy)
                {
                    objExpanderItem.AllowDrop = true;
                }

                objExpanderItem.Tag = strTag;

                
                if (ShowImage)
                {
                    if (objImageType == ImageType.Page)
                    {
                        objExpanderItem.Image = @"\Skins\Images\Page.png";
                    }
                    else if (objImageType == ImageType.Module)
                    {
                        objExpanderItem.Image = @"\Skins\Images1\Skins.png";
                    }
                    else if (objImageType == ImageType.MaleBuddy)
                    {
                        objExpanderItem.Image = @"\Skins\Images\Buddy.Png";
                    }
                    else if (objImageType == ImageType.FemaleBuddy)
                    {
                        objExpanderItem.Image = @"\Skins\Images\FBuddy.Png";
                    }
                }
                else
                {
                    objExpanderItem.Image = "";
                }

                objExpanderItem.Caption = strCaption;

                if (strTag == "offline")
                {
                    
                    objExpanderItem.Image = @"\Skins\Images\FBuddy.Png";
                    objExpanderItem.ToolTip = strCaption + " is offline";

                    objExpanderItem.PreviewMouseDown -= new System.Windows.Input.MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
                }
                else if (strTag == "online")
                {
                    
                    objExpanderItem.Image = @"\Skins\Images\Buddy.Png";
                    objExpanderItem.ToolTip = strCaption + " is online";
                    objExpanderItem.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
                }

                objExpanderItem.EntLoadSelectedWid += new CtlExpanderItem.DelLoadSelectedWid(objExpanderItem_EntLoadSelectedWid);

                TreeViewItem objTrviExpanderItem = new TreeViewItem();
                objTrviExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                objTrviExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;
                objTrviExpanderItem.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(objTrviExpanderItem_PreviewMouseLeftButtonDown);
                objTrviExpanderItem.Header = objExpanderItem;


                trvExpanderContent.Items.Add(objTrviExpanderItem);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddItem()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        public void RemoveItem(string strCaption)
        {
            try
            {
                for (int i = 0; i < trvExpanderContent.Items.Count; i++)
                {
                    if (((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[i]).Header).Caption == strCaption)
                    {
                        trvExpanderContent.Items.RemoveAt(i);
                        break;
                    }
                }
            
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemoveItem()--1", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        public void RemoveItem(string strCaption, string strTag)
        {
            try
            {
                for (int i = 0; i < trvExpanderContent.Items.Count; i++)
                {
                    if (((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[i]).Header).Caption == strCaption && ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[i]).Header).Tag.ToString() == strTag)
                    {
                        trvExpanderContent.Items.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemoveItem()--2", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        public void EditItem(string strCaption, string strTag)
        {
            try
            {
                for (int i = 0; i < trvExpanderContent.Items.Count; i++)
                {
                    if (((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[i]).Header).Tag.ToString() == strTag)
                    {
                        ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[i]).Header).Caption = strCaption;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemoveItem()--2", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

     
        void mnuViewLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            VMuktiAPI.VMuktiHelper.CallEvent("ViewLog", ((MenuItem)sender).Tag, null);
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "mnuViewLog_Click()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }

        }       

        public void IsItemAdded(string Caption, string Status)
        {
            try
            {
                int j = 0;
                for (j = 0; j < trvExpanderContent.Items.Count; j++)
                {
                    if (((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Caption == Caption)
                    {
                        if (Status != ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Tag.ToString())
                        {
                            if (Status == "offline")
                            {
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Image = @"\Skins\Images\FBuddy.Png";
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).ToolTip = Caption + " is offline";
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Tag = Status;
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).AllowDrop = false;
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).PreviewMouseDown -= new MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
                            }
                            else
                            {
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Image = @"\skins\Images\Buddy.Png";
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).ToolTip = Caption + " is online";
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Tag = Status;
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).AllowDrop = true;
                                ((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).PreviewMouseDown += new MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
                            }
                        }
                        break;
                    }
                }
                if (j == trvExpanderContent.Items.Count)
                {
                    AddItem(Caption, true, ImageType.MaleBuddy, Status);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "IsItemAdded", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        public void RemoveBuddy(List<string> lstBuddy)
        {
            try
            {
                List<int> BuddyToRemove = new List<int>();
                for (int j = 0; j < trvExpanderContent.Items.Count; j++)
                {
                    bool isBuddyPresent = false;
                    foreach (string str in lstBuddy)
                    {
                        if (((CtlExpanderItem)((TreeViewItem)trvExpanderContent.Items[j]).Header).Caption == str)
                        {
                            isBuddyPresent = true;
                            break;
                        }
                    }
                    if (!isBuddyPresent)
                    {
                        BuddyToRemove.Add(j);
                    }
                }
                BuddyToRemove.Reverse();
                foreach (int i in BuddyToRemove)
                {
                    trvExpanderContent.Items.RemoveAt(i);
                }
            }
           catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemoveBuddy()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }
        

        void objExpanderItem_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (objExpanderItem.AllowDrop == true)
                {
                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((CtlExpanderItem)sender), ((CtlExpanderItem)sender), DragDropEffects.All);
                }
                else
                {
                    if (EntItemSelectionChanged != null)
                    {
                        EntItemSelectionChanged(((CtlExpanderItem)sender).Tag.ToString(), ((CtlExpanderItem)sender).Caption);
                    }
                }
                //ClsException.WriteToLogFile("buddy to be dropped " + ((CtlExpanderItem)sender).Caption);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objExpanderItem_PreviewMouseDown()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        #region Multiple Buddy Selection

        void objTrviExpanderItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CtlExpanderItem ucExpItemTemp = e.Source as CtlExpanderItem;
                Separator spTemp = ((CtlExpanderItem)(e.Source)).spWidgets as Separator;
                UniformGrid ufgTemp = ((CtlExpanderItem)(e.Source)).ufgWidgets as UniformGrid;

                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (selectedExItems.ContainsValue(ucExpItemTemp.Caption))
                    {
                        ucExpItemTemp.Background = Brushes.Transparent;
                        selectedExItems.Remove(ucExpItemTemp);
                    }
                    else
                    {
                        ucExpItemTemp.Background = Brushes.DarkBlue;
                        selectedExItems.Add(ucExpItemTemp, ucExpItemTemp.Caption);


                        if (EntWidgetToolBar != null)
                        {
                            EntWidgetToolBar("Show");
                        }
                    }
                    foreach (CtlExpanderItem objExpItem in selectedExItems.Keys)
                    {
                        objExpItem.spWidgets.Visibility = Visibility.Collapsed;
                        objExpItem.ufgWidgets.Visibility = Visibility.Collapsed;
                        ((Grid)objExpItem.ufgWidgets.Parent).MinHeight = 0.0;
                    }
                }

                else
                {
                    if (EntWidgetToolBar != null)
                    {
                        EntWidgetToolBar("Hide");
                    }
                    if (selectedExItems.Count == 0)
                    {
                        
                            ucExpItemTemp.Background = Brushes.DarkBlue;
                            spTemp.Visibility = Visibility.Visible;
                            ufgTemp.Visibility = Visibility.Visible;
                            ((Grid)ufgTemp.Parent).MinHeight = 80.0;
                            selectedExItems.Add(ucExpItemTemp, ucExpItemTemp.Caption);
                            if (ucExpItemTemp.Tag.ToString() == "offline")
                            {
                                for (int i = 0; i < ufgTemp.Children.Count - 2; i++)
                                {
                                    ufgTemp.Children[i].IsEnabled = false;
                                    ufgTemp.Children[i].Opacity = 0.5;
                                }
                          }
                        else
                        {
                                for (int i = 0; i < ufgTemp.Children.Count - 2; i++)
                                {
                                    ufgTemp.Children[i].IsEnabled = true;
                                    ufgTemp.Children[i].Opacity = 1;
                                }

                            }

                        
                    }
                    else
                    {
                        if (selectedExItems.Count == 1)
                        {
                            if (selectedExItems.ContainsKey(ucExpItemTemp))
                            {
                                ucExpItemTemp.Background = Brushes.Transparent;
                                spTemp.Visibility = Visibility.Collapsed;
                                ufgTemp.Visibility = Visibility.Collapsed;
                                ((Grid)ufgTemp.Parent).MinHeight = 0.0;
                                selectedExItems.Clear();
                            }
                            else
                            {
                                foreach (CtlExpanderItem objExpItem in selectedExItems.Keys)
                                {
                                    objExpItem.Background = Brushes.Transparent;
                                    objExpItem.spWidgets.Visibility = Visibility.Collapsed;
                                    objExpItem.ufgWidgets.Visibility = Visibility.Collapsed;
                                    ((Grid)objExpItem.ufgWidgets.Parent).MinHeight = 0.0;
                                }
                                selectedExItems.Clear();
                                
                                    ucExpItemTemp.Background = Brushes.DarkBlue;
                                    spTemp.Visibility = Visibility.Visible;
                                    ufgTemp.Visibility = Visibility.Visible;
                                    ((Grid)ufgTemp.Parent).MinHeight = 80.0;
                                    selectedExItems.Add(ucExpItemTemp, ucExpItemTemp.Caption);
                                    
                                if (ucExpItemTemp.Tag.ToString() == "offline")
                                    {
                                        for (int i = 0; i < ufgTemp.Children.Count - 2; i++)
                                        {
                                            ufgTemp.Children[i].IsEnabled = false;
                                            ufgTemp.Children[i].Opacity = 0.5;
                                        }
                                }
                                else
                                {
                                        for (int i = 0; i < ufgTemp.Children.Count - 2; i++)
                                        {
                                            ufgTemp.Children[i].IsEnabled = true;
                                            ufgTemp.Children[i].Opacity = 1;
                                        }

                                    }
                                
                            }
                        }

                        else if (selectedExItems.Count > 1)
                        {
                            foreach (CtlExpanderItem objExpItem in selectedExItems.Keys)
                            {
                                objExpItem.Background = Brushes.Transparent;
                                objExpItem.spWidgets.Visibility = Visibility.Collapsed;
                                objExpItem.ufgWidgets.Visibility = Visibility.Collapsed;
                                ((Grid)objExpItem.ufgWidgets.Parent).MinHeight = 0.0;
                            }
                            selectedExItems.Clear();
                            
                                ucExpItemTemp.Background = Brushes.DarkBlue;
                                spTemp.Visibility = Visibility.Visible;
                                ufgTemp.Visibility = Visibility.Visible;
                                ((Grid)ufgTemp.Parent).MinHeight = 80.0;
                                selectedExItems.Add(ucExpItemTemp, ucExpItemTemp.Caption);
                               
                             if (ucExpItemTemp.Tag.ToString() == "offline")
                                {
                                    for (int i = 0; i < ufgTemp.Children.Count - 2; i++)
                                    {
                                        ufgTemp.Children[i].IsEnabled = false;
                                        ufgTemp.Children[i].Opacity = 0.5;
                                    }
                            }
                            else
                            {
                                    for (int i = 0; i < ufgTemp.Children.Count - 2; i++)
                                    {
                                        ufgTemp.Children[i].IsEnabled = true;
                                        ufgTemp.Children[i].Opacity = 1;
                                    }

                                }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objTrviExpanderItem_PreviewMouseLeftButtonDown()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        void mnuColl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (EntCModBuddies != null)
            {                        
                EntCModBuddies(selectedExItems, int.Parse(((MenuItem)sender).Tag.ToString()));
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "mnuColl_Click()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        //void mnuViewProfile_Click(object sender, RoutedEventArgs e)
        //{
        //    string buddies = ((MenuItem)sender).Tag.ToString();

        //    //buddies = "123,asd,234,zxcvg,3456,sdfgh,4245,wdfgs,";
        //    buddies = buddies.Substring(0,buddies.LastIndexOf(','));
        //    string buddyProfile = null;
        //    if (buddies.Contains(","))
        //    {
        //         buddyProfile = buddies.Substring(buddies.LastIndexOf(',')+1);
        //    }
        //    else
        //    {
        //        buddyProfile = buddies;
        //    }

        //    int userID = VMukti.Business.clsProfile.GetUserID(buddyProfile);

        //    VMuktiAPI.VMuktiHelper.CallEvent("ViewProfile", null, new VMuktiEventArgs(userID));
        //}

        //void trvExpanderContent_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        if (this.Tag.ToString() == "BuddyType")
        //        {
        //            if (selectedExItems.Count > 0)
        //            {
        //                ContextMenu cntMenuCollMod = new ContextMenu();

        //                ClsModuleCollection cmc = ClsModuleCollection.GetOnlyCollMod();
        //                for (int i = 0; i < cmc.Count; i++)
        //                {
        //                    MenuItem mnuColl = new MenuItem();
        //                    mnuColl.Header = cmc[i].ModuleTitle;
        //                    mnuColl.Tag = cmc[i].ModuleId;
        //                    mnuColl.Click += new RoutedEventHandler(mnuColl_Click);
        //                    cntMenuCollMod.Items.Add(mnuColl);
        //                }

        //                Separator sp = new Separator();
        //                cntMenuCollMod.Items.Add(sp);

        //                string strBuddiesName = "";
        //                string allBuddies = "";
        //                //((TreeViewItem)((TreeView)sender).SelectedValue).Header
        //                foreach (KeyValuePair<CtlExpanderItem, string> strBName in selectedExItems)
        //                {
        //                    if (((CtlExpanderItem)strBName.Key).Tag.ToString().Trim() == "online")
        //                    {
        //                        strBuddiesName += strBName.Value + ",";
        //                    }
        //                    allBuddies += strBName.Value + ",";                               
        //                }
        //                if (strBuddiesName == "")
        //                {
        //                    cntMenuCollMod.Items.Clear();
        //                    for (int i = 0; i < cmc.Count; i++)
        //                    {
        //                        MenuItem mnuColl = new MenuItem();
        //                        mnuColl.Header = cmc[i].ModuleTitle;
        //                        mnuColl.Tag = cmc[i].ModuleId;
        //                        mnuColl.IsEnabled = false;
        //                        mnuColl.Click += new RoutedEventHandler(mnuColl_Click);
        //                        cntMenuCollMod.Items.Add(mnuColl);
        //                    }
        //                }
        //                System.Windows.Controls.MenuItem menuItem1;
        //                menuItem1 = new System.Windows.Controls.MenuItem();
        //                menuItem1.Header = "Remove Buddy";
        //                //menuItem1.Tag = ctlItem.Caption;
        //                menuItem1.Click += new RoutedEventHandler(menuItem1_Click);
        //                cntMenuCollMod.Items.Add(menuItem1);

        //                if (VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID == 1)
        //                {
        //                    MenuItem mnuViewLog = new MenuItem();
        //                    mnuViewLog.Header = "View Log";
        //                    mnuViewLog.Click += new RoutedEventHandler(mnuViewLog_Click);
        //                    mnuViewLog.Tag = strBuddiesName;
        //                    cntMenuCollMod.Items.Add(mnuViewLog);
        //                }
        //                MenuItem mnuViewProfile = new MenuItem();
        //                mnuViewProfile.Header = "View Profile";
        //                mnuViewProfile.Click += new RoutedEventHandler(mnuViewProfile_Click);
        //                mnuViewProfile.Tag = allBuddies;
        //                cntMenuCollMod.Items.Add(mnuViewProfile);

        //                trvExpanderContent.ContextMenu = cntMenuCollMod;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiHelper.ExceptionHandler(ex, "trvExpanderContent_PreviewMouseRightButtonDown()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
        //    }
        //}
        #endregion 

        #region BuddySingleClick + Widgets

        void objExpanderItem_EntLoadSelectedWid(CtlExpanderItem buddyname, int modid)
        {
            Dictionary<CtlExpanderItem, string> dicttemp = new Dictionary<CtlExpanderItem, string>();
            dicttemp.Add(buddyname, buddyname.Caption);
            if (modid == 999)
            {               
                selectedExItems.Add(buddyname, buddyname.Caption);
                this.menuItem1_Click(null, null);
            }           
            else
            {
            if (EntCModBuddies != null)
            {
                EntCModBuddies(dicttemp, modid);
                }
            }
        }

        #endregion

        ~CtlExpanderContent()
        {
            try
            {
                Dispose();
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlExpanderContent()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (EntItemSelectionChanged != null)
                {
                    EntItemSelectionChanged = null;
                }
                if (objExpanderItem != null)
                {
                    objExpanderItem = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\BuddyExplorer\\CtlExpanderContent.xaml.cs");
            }
        }
        #endregion
    }
}
