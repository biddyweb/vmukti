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
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Collections.Generic;
using VMuktiGrid.Buddy;
using VMuktiGrid.CustomMenu;
using System;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using System.Text;
using System.Windows.Documents;
using VMukti.Business;
using VMukti;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace VMuktiGrid.ctlPage
{
    public enum TabType
    {
        Null, Page, Tab
    }

    [TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "objMenu", Type = typeof(ctlMenu))]
    [TemplatePart(Name = "Bd", Type = typeof(Border))]
    [TemplatePart(Name = "stbBlink", Type = typeof(System.Windows.Media.Animation.Storyboard))]
   
   
    public class TabItem : System.Windows.Controls.TabItem, IDisposable
    {
        private bool disposed = false;
        private List<string> buddyList = new List<string>();
        public string ObjectType = "";
        public int ObjectID = int.MinValue;
        private int _NoOfPODs = 0;
        
        #region Blinking

      
        bool isblinking = false;
       
        #endregion


        public int NoOfPODs 
        {
            get 
            {
                return _NoOfPODs;
            }
            set
            {
                _NoOfPODs = value;
            }
        }

        private bool _IsSaved = false;

        public bool IsSaved
        {
            get { return _IsSaved; }
            set
            {
                _IsSaved = value;
                try
                {
                    ((ctlMenu)this.Template.FindName("objMenu", this)).objMISave.IsEnabled = (!_IsSaved);
                    if (value)
                    {
                        ((ctlPgTabHeader)this.Header).brdIsSaved.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((ctlPgTabHeader)this.Header).brdIsSaved.Visibility = Visibility.Visible;
                    }
                }
                catch { }
            }
        }

        public bool CanDelete
        {
            set
            {

                if (((ctlMenu)(this.Template.FindName("objMenu", this))) != null)
                    ((ctlMenu)(this.Template.FindName("objMenu", this))).objMIDelete.IsEnabled = value;
            }
        }

        public bool CanRename
        {
            set
            {
                if (((ctlMenu)(this.Template.FindName("objMenu", this))) != null)
                    ((ctlMenu)this.Template.FindName("objMenu", this)).objMIRename.IsEnabled = value;
            }
        }

        public int OwnerID = int.MinValue;
        public int OwnerPageIndex = int.MinValue;

        public delegate void DelClosePage();
        public DelClosePage objClosePage;

        public int ConfID;

        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ctlPage.TabItem), new FrameworkPropertyMetadata(typeof(ctlPage.TabItem)));
        }

        /// <summary>
        /// Provides a place to display an Icon on the TabItem and on the DropDown Context Menu
        /// </summary>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(TabItem), new UIPropertyMetadata(null));

        /// <summary>
        /// Allow the TabItem to be Deleted by the end user
        /// </summary>
        public bool AllowDelete
        {
            get { return (bool)GetValue(AllowDeleteProperty); }
            set { SetValue(AllowDeleteProperty, value); }
        }
        public static readonly DependencyProperty AllowDeleteProperty = DependencyProperty.Register("AllowDelete", typeof(bool), typeof(TabItem), new UIPropertyMetadata(true));

        /// <summary>
        /// OnApplyTemplate override
        /// </summary>
        public override void OnApplyTemplate()
        {
            try
            {
                base.OnApplyTemplate();
                #region blinking
                this.GotFocus += new RoutedEventHandler(TabItem_GotFocus);               
                #endregion

                ((ctlMenu)this.Template.FindName("objMenu", this)).objMISave.IsEnabled = (!_IsSaved);

                // wire up the CloseButton's Click event if the button exists
                ButtonBase button = this.Template.FindName("PART_CloseButton", this) as ButtonBase;
                if (button != null)
                {
                    button.Click += delegate
                    {
                        // get the parent tabcontrol 
                        TabControl tc = Helper.FindParentControl<TabControl>(this);
                        if (tc == null) return;

                        // remove this tabitem from the parent tabcontrol
                        tc.RemoveItem(this);
                    };
                }

                if (buddyList.Count > 0)
                {
                    for (int i = 0; i < buddyList.Count; i++)
                    {
                        ((ctlMenu)this.Template.FindName("objMenu", this)).AddBuddy(buddyList[i]);
                        ((ctlMenu)this.Template.FindName("objMenu", this)).SetMaxCounter(0, buddyList[i]);
                    }
                    buddyList.Clear();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnapplyTemplate()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
            }
        }

        /// <summary>
        /// OnMouseEnter, Create and Display a Tooltip if the Header is cropped
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            //this.ToolTip = Helper.CloneElement(Header);
            e.Handled = true;
        }

        /// <summary>
        /// OnMouseLeave, remove the tooltip
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            //this.ToolTip = null;
            e.Handled = true;
        }

        public bool AddBuddy(string strBuddyName)
        {
            try
            {
                for (int i = 0; i < ((VMuktiGrid.ctlTab.TabControl)this.Content).Items.Count; i++)
                {
                    ((VMuktiGrid.ctlTab.TabControl)this.Content).AddBuddy(strBuddyName, i);
                }

                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    return ((ctlMenu)this.Template.FindName("objMenu", this)).AddBuddy(strBuddyName);
                }
                else
                {
                    buddyList.Add(strBuddyName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");                
                return false;
            }
        }

        public bool RemoveBuddy(string strBuddyName)
        {
            try 
            {
                for (int i = 0; i < ((VMuktiGrid.ctlTab.TabControl)this.Content).Items.Count; i++)
                {
                    ((VMuktiGrid.ctlTab.TabControl)this.Content).RemoveBuddy(strBuddyName, i);
                }

                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    return ((ctlMenu)this.Template.FindName("objMenu", this)).RemoveBuddy(strBuddyName);
                }
                else
                {
                    buddyList.Remove(strBuddyName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
                return false;
            }
        }

        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    ((ctlMenu)this.Template.FindName("objMenu", this)).SetMaxCounter(intMaxCounter, strBuddyName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetMaxCounter()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
            }
        }

        public bool CheckBuddy(string strBuddyName)
        {
            try
            {
                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    return ((ctlMenu)this.Template.FindName("objMenu", this)).CheckBuddy(strBuddyName);
                }
                else
                {
                    buddyList.Add(strBuddyName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");                
                return false;
            }
        }

        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                for (int i = 0; i < ((VMuktiGrid.ctlTab.TabControl)this.Content).Items.Count; i++)
                {
                    ((VMuktiGrid.ctlTab.TabControl)this.Content).ShowBuddy(strBuddyName);
                }
                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    ((ctlMenu)this.Template.FindName("objMenu", this)).ShowBuddy(strBuddyName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
            }
        }

        public void Save()
        {
            try
            {
                if (!IsSaved && (ObjectID != 1 && ObjectID != 2))
                {
                    if (ObjectID == int.MinValue)
                    {
                        try
                        {
                            ObjectID = (new VMukti.Business.VMuktiGrid.ClsPage()).Add_Page(-1, ((ctlPgTabHeader)this.Header).Title, "", false, VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                            (new VMukti.Business.VMuktiGrid.ClsPage()).Page_Allocated(-1, ObjectID, VMuktiAPI.VMuktiInfo.CurrentPeer.ID);

                            VMuktiAPI.VMuktiHelper.CallEvent("PageAdded", null, new VMuktiEventArgs(new object[] { ((ctlPgTabHeader)this.Header).Title, ObjectID }));
                            MessageBox.Show("Page Saved successfully");
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }
                    }

                    else
                    {
                        try
                        {
                            (new VMukti.Business.VMuktiGrid.ClsPage()).Add_Page(ObjectID, ((ctlPgTabHeader)this.Header).Title, "", false, VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                            (new VMukti.Business.VMuktiGrid.ClsPage()).Page_Allocated(-1, ObjectID, VMuktiAPI.VMuktiInfo.CurrentPeer.ID);

                            VMuktiAPI.VMuktiHelper.CallEvent("PageEdited", null, new VMuktiEventArgs(new object[] { ((ctlPgTabHeader)this.Header).Title, ObjectID }));
                            MessageBox.Show("Page Edited successfully");
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }

                    }
                    foreach (VMuktiGrid.ctlTab.TabItem item in ((VMuktiGrid.ctlTab.TabControl)this.Content).Items)
                    {
                        try
                        {
                            item.Save(ObjectID);
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }
                    }
                    IsSaved = true;
                }
                else
                {
                    MessageBox.Show("User Cannot Save Default pages.");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
            }
        }

        public void Delete()
        {
            try
            {
                MessageBoxResult MResult = MessageBox.Show("Want to Confirm Delete Operation?", "Delete Page", MessageBoxButton.YesNo);
                if (MResult == MessageBoxResult.Yes)
                {
                    if (ObjectID == int.MinValue)
                    {
                        try
                        {
                            ((VMuktiGrid.ctlPage.TabControl)this.Parent).Items.Remove(this);
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            (new VMukti.Business.VMuktiGrid.ClsPage()).Remove_Page(ObjectID);
                            ((VMuktiGrid.ctlPage.TabControl)this.Parent).Items.Remove(this);
                            VMuktiAPI.VMuktiHelper.CallEvent("PageDeleted", null, new VMuktiEventArgs(new object[] { ((ctlPgTabHeader)this.Header).Title, ObjectID }));
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }

                    }
                }
                else
                { }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
            }
        }

        public void Close()
        {
            try
            {
                objClosePage = new DelClosePage(ClosePage);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objClosePage);

                //if (this.ConfID != 0)
                //{
                //    objClosePage = new DelClosePage(ClosePage);
                //    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objClosePage);
                //}
                //else
                //{
                    //MessageBoxResult MResult = MessageBox.Show("Want to Confirm Close Operation?", "Close Page", MessageBoxButton.YesNo);
                    //if (MResult == MessageBoxResult.Yes)
                    //{
                        //objClosePage = new DelClosePage(ClosePage);
                        //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objClosePage);
                    //}
                    //else
                    //{ }
              //  }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Close()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
            }
        }

        public void ClosePage()
        {
            try
            {
                int j = 0;
                int i = 0;

                

                for (i = 0; i < ((VMuktiGrid.ctlTab.TabControl)this.Content).Items.Count; i++)
                {
                    for (j = 0; j < ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).CentralPanelContainer.Items.Count; j++)
                    {
                        if (((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).CentralPanelContainer.Items[j].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                        {
                            ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).CentralPanelContainer.Items[j]).btnCloseFromPage_Click(null, null);
                        }
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).CentralPanelContainer.Items.Clear();

                    


                    

                    for (j = 0; j < ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).LeftPanelContainer.Items.Count; j++)
                    {
                        if (((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).LeftPanelContainer.Items[j].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                        {
                            ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).LeftPanelContainer.Items[j]).btnCloseFromPage_Click(null, null);
                        }
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).LeftPanelContainer.Items.Clear();



                    

                    for (j = 0; j < ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).RightPanelContainer.Items.Count; j++)
                    {
                        if (((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).RightPanelContainer.Items[j].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                        {
                            ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).RightPanelContainer.Items[j]).btnCloseFromPage_Click(null, null);
                        }
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).RightPanelContainer.Items.Clear();



                    for (j = 0; j < ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).TopPanelContainer.Items.Count; j++)
                    {
                        if (((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).TopPanelContainer.Items[j].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                        {
                            ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).TopPanelContainer.Items[j]).btnCloseFromPage_Click(null, null);
                        }
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).TopPanelContainer.Items.Clear();



                    for (j = 0; j < ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).BottomPanelContainer.Items.Count; j++)
                    {
                        if (((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).BottomPanelContainer.Items[j].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                        {
                            ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).BottomPanelContainer.Items[j]).btnCloseFromPage_Click(null, null);
                        }
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Content).Items[i]).Content).BottomPanelContainer.Items.Clear();

                }


                try
                {

                    List<int> lstKeyRemove = new List<int>();
                    int myKey;

                    foreach (KeyValuePair<int, int> kvp in VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser)
                    {
                        if (kvp.Key == this.ConfID)
                        {
                            if (kvp.Value == VMuktiAPI.VMuktiInfo.CurrentPeer.ID)
                            {

                                myKey = kvp.Key;
                                //update this entry from database UPDATE ConferenceUsers set Started='true' where ConfID='3' and UserID='3'
                                ClsConferenceUsers objUpdateStarted = new ClsConferenceUsers();
                                objUpdateStarted.UpdateStarted(kvp.Key, kvp.Value, false);

                                List<string> lstBuddyName = new List<string>();

                                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                                {
                                    lstBuddyName = ((ctlMenu)this.Template.FindName("objMenu", this)).GetBuddies();
                                }
                                if (lstBuddyName.Count > 0)
                                {
                                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                                    {
                                        App.chHttpSuperNodeService.svcUnJoinConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddyName, kvp.Key, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                    }
                                    else
                                    {
                                        App.chNetP2PSuperNodeChannel.svcUnJoinConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddyName, kvp.Key, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                    }
                                }

                                //if possible tell other buddies to close this page of conference
                                //on buddy side open the window displaying "Thank You for attending Meeting"

                            }
                            else
                            {
                                //CALL WCF FUNCTION TO TELL HOST AND OTHER PARTICIPANT THAT U R LEAVING THE CONF

                                List<string> lstBuddyName = new List<string>();

                                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                                {
                                    lstBuddyName = ((ctlMenu)this.Template.FindName("objMenu", this)).GetBuddies();
                                }
                                if (lstBuddyName.Count > 0)
                                {
                                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                                    {
                                        App.chHttpSuperNodeService.svcRemoveConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddyName, kvp.Key, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);                                        
                                    }
                                    else
                                    {
                                        App.chNetP2PSuperNodeChannel.svcRemoveConf(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstBuddyName, kvp.Key, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                    }
                                }
                            }

                            //remove this entry
                            //VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.Remove(kvp.Key);
                            lstKeyRemove.Add(kvp.Key);
                        }
                    }

                    

                    for (int KCnt = 0; KCnt < lstKeyRemove.Count; KCnt++)
                    {
                        VMuktiAPI.VMuktiInfo.CurrentPeer.ConferenceUser.Remove(lstKeyRemove[KCnt]);
                    }


                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePage()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
                }

                ((VMuktiGrid.ctlPage.TabControl)this.Parent).Items.Remove(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePage()--2", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing())", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~TabItem()
        {
            Dispose(false);
        }
        #endregion


        #region blinking

        Storyboard sb = new Storyboard();

        public void StartBlinking()
        {
            try
            {
                if (!this.IsSelected)
                {
                    sb = this.Template.Resources["stbBlink"] as Storyboard;
                    sb.Begin((this.Template.FindName("Bd", this) as Border));
                    isblinking = true;
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartBlinking()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");
     
            }
        }

        public void StopBlinking()
        {
            try
            {
               //this.Dispatcher.BeginInvoke(objDelBlink, DispatcherPriority.Background, false);
               sb = this.Template.Resources["stbBlink"] as Storyboard;
               sb.Stop((this.Template.FindName("Bd", this) as Border));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StopBlinking()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");

            }
        }


        void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isblinking && this.IsSelected)
                {
                    StopBlinking();
                    isblinking = false;
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabItem_LostFocus()", "Controls\\VMuktiGrid\\Page\\TabItem.cs");

            }
        }

        #endregion
    }
}
