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
using VMukti.Presentation.Controls;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using System.Windows.Media.Animation;

namespace VMuktiGrid.ctlTab
{
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "Bd", Type = typeof(Border))]
    [TemplatePart(Name = "stbBlink", Type = typeof(System.Windows.Media.Animation.Storyboard))]
   
    public class TabItem : System.Windows.Controls.TabItem, IDisposable
    {
        #region Blinking
        bool isblinking = false;
        Storyboard sb = new Storyboard();
        #endregion

        private List<string> buddyList = new List<string>();
        
        private bool disposed = false;

        public string ObjectType = "";
        public int ObjectID = int.MinValue; 
        public int NoOfPODs = 0;

        private bool _IsSaved = false;

        public bool IsSaved
        {
            get { return _IsSaved; }
            set 
            { 
                _IsSaved = value;
                try
                {
                    ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Parent).Parent).IsSaved = value;
                }
                catch { }
            }
        }

        public int OwnerTabIndex = int.MinValue;

        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ctlTab.TabItem), new FrameworkPropertyMetadata(typeof(ctlTab.TabItem)));
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
                  if (isblinking)
                {
                    sb = this.Template.Resources["stbBlink"] as Storyboard;                 
                    sb.Begin(this.Template.FindName("Bd", this) as Border);
                    isblinking = true;
                }
                #endregion

                ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)this.Parent).Parent).IsSaved = _IsSaved;

                this.AllowDrop = true;

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

                        ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).AddBuddy(buddyList[i]);
                        //((ctlBuddyList)this.Template.FindName("objBuddyList", this)).AddBuddy(buddyList[i]);
                    }
                    buddyList.Clear();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnapplyTemplate()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
            }
        }

        /// <summary>
        /// OnMouseEnter, Create and Display a Tooltip if the Header is cropped
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                base.OnMouseEnter(e);

                //this.ToolTip = Helper.CloneElement(Header);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnMouseEnter()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
            }
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
                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).AddBuddy(strBuddyName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");                
                return false;
            }
        }

        public bool RemoveBuddy(string strBuddyName)
        {
            try
            {
                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).RemoveBuddy(strBuddyName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveBuddy()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetMaxCounter()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Checkbuddy()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");                
                return false;
            }
        }

        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                if (((ctlMenu)this.Template.FindName("objMenu", this)) != null)
                {
                    ((ctlMenu)this.Template.FindName("objMenu", this)).ShowBuddy(strBuddyName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
            }
        }

        public void Save(int pageID)
        {
            try
            {
                if (!IsSaved)
                {
                    if (ObjectID == int.MinValue)
                    {
                        ObjectID = (new VMukti.Business.VMuktiGrid.ClsTab()).Add_Tab(-1, pageID, ((VMuktiGrid.ctlTab.TabControl)this.Parent).Items.IndexOf(this) + 1, ((ctlPgTabHeader)this.Header).Title, "", VMuktiAPI.VMuktiInfo.CurrentPeer.ID, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).LeftPanelContainer.ActualWidth, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).CentralPanelContainer.ActualWidth, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).RightPanelContainer.ActualWidth, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).TopPanelContainer.ActualHeight, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).BottomPanelContainer.ActualHeight);
                    }
                    else
                    {
                        (new VMukti.Business.VMuktiGrid.ClsTab()).Add_Tab(ObjectID, pageID, ((VMuktiGrid.ctlTab.TabControl)this.Parent).Items.IndexOf(this) + 1, ((ctlPgTabHeader)this.Header).Title, "", VMuktiAPI.VMuktiInfo.CurrentPeer.ID, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).LeftPanelContainer.ActualWidth, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).CentralPanelContainer.ActualWidth, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).RightPanelContainer.ActualWidth, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).TopPanelContainer.ActualHeight, ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).BottomPanelContainer.ActualHeight);
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).Save(ObjectID);
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
            }
        }

        public void Delete()
        {
            try
            {
                if (ObjectID == int.MinValue)
                {
                    try
                    {
                        ((VMuktiGrid.ctlTab.TabControl)this.Parent).Items.Remove(this);
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
                        (new VMukti.Business.VMuktiGrid.ClsTab()).Remove_Tab(ObjectID);
                        ((VMuktiGrid.ctlTab.TabControl)this.Parent).Items.Remove(this);
                        VMukti.Business.VMuktiGrid.ClsPod.Remove_PodTab(ObjectID);
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
            }
        }

        public void Close()
        {
            try
            {
                for (int i = 0; i < ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).LeftPanelContainer.Items.Count; i++)
                {
                    if (((VMuktiGrid.CustomGrid.ctlGrid)this.Content).LeftPanelContainer.Items[i].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                    {
                        ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)this.Content).LeftPanelContainer.Items[i]).btnCloseFromPage_Click(null, null);
                    }
                }
                for (int j = 0; j < ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).RightPanelContainer.Items.Count; j++)
                {
                    if (((VMuktiGrid.CustomGrid.ctlGrid)this.Content).RightPanelContainer.Items[j].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                    {
                        ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)this.Content).RightPanelContainer.Items[j]).btnCloseFromPage_Click(null, null);
                    }
                }
                for (int k = 0; k < ((VMuktiGrid.CustomGrid.ctlGrid)this.Content).CentralPanelContainer.Items.Count; k++)
                {
                    if (((VMuktiGrid.CustomGrid.ctlGrid)this.Content).CentralPanelContainer.Items[k].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                    {
                        ((VMuktiGrid.CustomGrid.ctlPOD)((VMuktiGrid.CustomGrid.ctlGrid)this.Content).CentralPanelContainer.Items[k]).btnCloseFromPage_Click(null, null);
                    }
                }


             

                    //for (int i = 0; i < this.objBuddyList.stPanel.Children.Count; i++)
                    //{
                    //    if (this.objBuddyList.stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    //    {
                    //        if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != ((ctlBuddy)this.objBuddyList.stPanel.Children[i]).Title)
                    //        {
                    //            lstUsersDropped.Add(((ctlBuddy)this.objBuddyList.stPanel.Children[i]).Title);
                    //        }
                    //    }
                    //}
                    //if (this.lstUsersDropped.Count > 0)
                    //{
                    //    clsModuleInfo cmi = new clsModuleInfo();
                    //    cmi.strPageid = ((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).Parent).Parent).ObjectID.ToString();
                    //    cmi.strTabid = ((VMuktiGrid.ctlTab.TabItem)((ctlGrid)((Grid)((ItemsControl)this.Parent).Parent).Parent).Parent).ObjectID.ToString();
                    //    cmi.strPodid = this.ObjectID.ToString();
                    //    cmi.strDropType = "Pod Type";

                    //    try
                    //    {
                    //        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                    //        {
                    //            App.chNetP2PSuperNodeChannel.svcSetRemoveDraggedBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstUsersDropped, "CLOSE MODULE", cmi, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                    //        }
                    //        else
                    //        {
                    //            App.chHttpSuperNodeService.svcSetRemoveDraggedBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstUsersDropped, "CLOSE MODULE", cmi);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        ex.Data.Add("My Key", "btnClose_Click()--:--ctlPOD.xaml.cs--:--" + ex.Message + " :--:--");
                    //        ClsException.LogError(ex);
                    //        ClsException.WriteToErrorLogFile(ex);
                    //    }
                    //}
                






                //((VMuktiGrid.CustomGrid.ctlPOD)(((VMuktiGrid.CustomGrid.ctlGrid)this.Content)).objPOD).btnClose_Click(null, null);
                ((VMuktiGrid.ctlTab.TabControl)this.Parent).Items.Remove(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Close()", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
            }
        }

       

        #region blinking

       
     
        public void StartBlinking()
        {
            try
            {
                if (!this.IsSelected)
                {
                    //  Border bd = this.Template.FindName("Bd", this) as Border;
                    isblinking = true;
                    if (this.Template.FindName("Bd", this) as Border != null)
                    {
                        sb = this.Template.Resources["stbBlink"] as Storyboard;
                        //sb.Begin((this.Template.FindName("Bd", this) as Border));
                        sb.Begin(this.Template.FindName("Bd", this) as Border);
                        isblinking = true;
                    }
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
                if (this.Template.FindName("Bd", this) as Border != null)
                {
                    //this.Dispatcher.BeginInvoke(objDelBlink, DispatcherPriority.Background, false);
                    sb = this.Template.Resources["stbBlink"] as Storyboard;
                    sb.Stop(this.Template.FindName("Bd", this) as Border);
                }
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "Controls\\VMuktiGrid\\Tab\\TabItem.cs");
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
    }
}
