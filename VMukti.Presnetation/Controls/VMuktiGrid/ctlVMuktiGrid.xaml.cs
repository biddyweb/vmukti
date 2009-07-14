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
using VMukti.Business;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using VMukti.Presentation.Controls;

namespace VMuktiGrid
{
    /// <summary>
    /// Interaction logic for ctlVMuktiGrid.xaml
    /// </summary>
    public partial class ctlVMuktiGrid : UserControl, IDisposable
    {
        private bool disposed ,blnFirstClick;
     
        public ctlVMuktiGrid()
        {
            try
            {
                InitializeComponent();

                //pageControl.Items.Clear();
                //((Button)pageControl.Template.FindName("PART_NewTabButton", pageControl)).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                //((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Header).Title = "Default Page";
                //((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Header).Title = "Default Tab";

                //if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                //{
                //    ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //    for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //    {
                //        ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                //        int[] arrPermissionValue = new int[objCPC.Count];

                //        for (int percount = 0; percount < objCPC.Count; percount++)
                //        {
                //            arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //        }
                //        ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //    }
                //}
                //else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                //{
                //    if (VMukti.Business.clsDataBaseChannel.OpenDataBaseClient())
                //    {
                //        ClsModuleCollection objCMC = new ClsModuleCollection();
                //        //ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //        //System.Data.SqlClient.SqlParameter sp = new System.Data.SqlClient.SqlParameter();
                //        objCMC.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);

                //        for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //        {
                //            ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                //            int[] arrPermissionValue = new int[objCPC.Count];

                //            for (int percount = 0; percount < objCPC.Count; percount++)
                //            {
                //                arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //            }
                //            ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //        }
                //    }
                //}

                //pageControl.UnSetUserID();

                //((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).OwnerPageIndex = VMukti.App.pageCounter++;
                //((VMuktiGrid.ctlTab.TabItem)tabControl.Items[0]).OwnerTabIndex = VMukti.App.tabCounter++;

                //if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                //{
                //    ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //    for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //    {
                //        ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                //        int[] arrPermissionValue = new int[objCPC.Count];

                //        for (int percount = 0; percount < objCPC.Count; percount++)
                //        {
                //            arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //        }
                //        objGrid.AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                        
                //    }
                //}
                //else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                //{
                //    if (VMukti.Business.clsDataBaseChannel.OpenDataBaseClient())
                //    {
                //        ClsModuleCollection objCMC = new ClsModuleCollection();
                //        //ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //        //System.Data.SqlClient.SqlParameter sp = new System.Data.SqlClient.SqlParameter();
                //        objCMC.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);

                //        for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //        {
                //            ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);

                //            int[] arrPermissionValue = new int[objCPC.Count];

                //            for (int percount = 0; percount < objCPC.Count; percount++)
                //            {
                //                arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //            }
                //            objGrid.AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //        }
                //    }
                //}

                ///////New code is as above old but running with grid without http is as bellow..
                //ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //{
                //    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);

                //    int[] arrPermissionValue = new int[objCPC.Count];

                //    for (int percount = 0; percount < objCPC.Count; percount++)
                //    {
                //        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //    }
                //    objGrid.AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //}
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlVMuktiGrid_VMuktiEvent_SignOut);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SucessfulLogin").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlVMuktiGrid_VMuktiEvent_SignIn);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVmuktiGrid()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }        
        }

        void ctlVMuktiGrid_VMuktiEvent_SignOut(object sender, VMuktiEventArgs e)
        {
            try
            {
               pageControl.Items.Clear();

                ((Button)pageControl.Template.FindName("PART_NewTabButton", pageControl)).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                VMuktiGrid.CustomGrid.ctlGrid objgrid = ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content);

                ((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Header).Title = "Default Page";
                ((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Header).Title = "Default Tab";
                //if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                //{
                //    if (VMukti.Business.clsDataBaseChannel.OpenDataBaseClient())
                //    {
                //        ClsModuleCollection objCMC = new ClsModuleCollection();
                //        //ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //        //System.Data.SqlClient.SqlParameter sp = new System.Data.SqlClient.SqlParameter();
                //        objCMC.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);

                //        for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //        {
                //            ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                //            int[] arrPermissionValue = new int[objCPC.Count];

                //            for (int percount = 0; percount < objCPC.Count; percount++)
                //            {
                //                arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //            }
                //            ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //        }
                //    }
                //}
                //else
                //{

                //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                //    {
                //        ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //        for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //        {
                //            ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                //            int[] arrPermissionValue = new int[objCPC.Count];

                //            for (int percount = 0; percount < objCPC.Count; percount++)
                //            {
                //                arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //            }
                //            ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //        }
                //    }
                //    else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                //    {
                //        if (VMukti.Business.clsDataBaseChannel.OpenDataBaseClient())
                //        {
                //            ClsModuleCollection objCMC = new ClsModuleCollection();
                //            //ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                //            //System.Data.SqlClient.SqlParameter sp = new System.Data.SqlClient.SqlParameter();
                //            objCMC.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);

                //            for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                //            {
                //                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                //                int[] arrPermissionValue = new int[objCPC.Count];

                //                for (int percount = 0; percount < objCPC.Count; percount++)
                //                {
                //                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                //                }
                //                ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                //            }
                //        }
                //    }
                //}


                ClsModuleCollection objCMC = ClsModuleCollection.GetNonAuthenticatedMod();
                if (objCMC != null)
                {
                    for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                    {
                        ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                        int[] arrPermissionValue = new int[objCPC.Count];

                        for (int percount = 0; percount < objCPC.Count; percount++)
                        {
                            arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                        }
                        ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                    }
                }


                pageControl.UnSetUserID();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVmuktigrid_VMuktiEvent_SignOut()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            } 
        }

        void ctlVMuktiGrid_VMuktiEvent_SignIn(object sender, VMuktiEventArgs e)
        {
            try
            {
                for (int i = 0; i < pageControl.Items.Count; i++)
                {
                     VMuktiGrid.ctlTab.TabControl objtabcontrol = ((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.Items[i]).Content);
                    ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[i]).AddBuddy(e._args[0].ToString());
                    ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[i]).SetMaxCounter(0, e._args[0].ToString());

                    for (int j = 0; j < objtabcontrol.Items.Count; j++)
                    {
                        ((VMuktiGrid.ctlTab.TabItem)objtabcontrol.Items[j]).SetMaxCounter(0, e._args[0].ToString());
                    }
                }
                pageControl.SetUserID();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVmuktiGrid_VMuktiEvent_SignIn()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }
        }

        public bool LoadPage(int pageID)
        {
            try
            {
                pageControl.LoadPage(pageID);
                return true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPage()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
                return false;
            }
        }

        public void LoadPage(int pageID,int confID)
        {
            try
            {
                pageControl.LoadPage(pageID,confID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPage()--2", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }
        }

        public void LoadMeetingPage(clsPageInfo objPageInfo)
        {
            try
            {
                int pCnt = 0;
                for (pCnt = 0; pCnt < pageControl.Items.Count; pCnt++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).OwnerID == objPageInfo.intOwnerID && ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).OwnerPageIndex == objPageInfo.intOwnerPageIndex)
                    {
                        pageControl.LoadMeetingPage(objPageInfo, pCnt);
                        break;
                    }
                }
                if (pCnt == pageControl.Items.Count)
                {
                    pageControl.LoadNewMeetingPage(objPageInfo);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMeetingPage()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }
        }

        public void CloseConfPage(int confid)
        {
            for (int pCnt = 0; pCnt < pageControl.Items.Count; pCnt++)
            {
                if (((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).ConfID == confid)
                {
                    ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).Close();
                   // pageControl.Items.RemoveAt(pCnt);
                }
            }
        }

        public void SetReturnBuddyStatus(clsBuddyRetPageInfo objBuddyRetPageInfo)
        {
            try
            {
                int pCnt = 0;
                for (pCnt = 0; pCnt < pageControl.Items.Count; pCnt++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).OwnerID == objBuddyRetPageInfo.intOwnerID && ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).OwnerPageIndex == objBuddyRetPageInfo.intOwnerPageIndex)
                    {
                        if (objBuddyRetPageInfo.strDropType == "OnPage")
                        {
                            ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).ShowBuddy(objBuddyRetPageInfo.strFrom);
                        }

                        pageControl.SetReturnBuddyStatus(objBuddyRetPageInfo, pCnt);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetReturnBuddyStatus()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }
        }

        #region Multiple Buddy Selection

        public void LoadNewMultipleBuddyPage(Dictionary<CtlExpanderItem, string> buddiesname, int modid)
        {
            try
            {
                /// check for number of pages
                /// for each page check the number of users
                /// if same number users, check for names of buddies
                /// if not open new page
                if (pageControl.Items.Count == 0)
                {
            pageControl.LoadMultipleBuddyPage(buddiesname, modid);
                }
                else
                {
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)(pageControl.SelectedItem)).Content).SelectedItem).Content).LoadMultipleBuddyGrid(buddiesname, modid);

                }

        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadNewMultipleBuddyPage()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }
        }


        public void LoadNewMultipleBuddyPage(clsModuleInfo objModInfo)
        {
            try
            {
               


                if (pageControl.Items.Count == 0)
                {
            pageControl.LoadMultipleBuddyPage(objModInfo);
        }
                else
                {
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)(pageControl.SelectedItem)).Content).SelectedItem).Content).LoadMultipleBuddyGrid(objModInfo);

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyPage()--2", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
            }
        }
        #endregion 

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
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
                       
					}
					catch (Exception ex)
					{
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool Disposing)", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlVMuktiGrid()
        {
            try
            {
            Dispose(false);
        }
        #endregion
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~ctlVmuktiGrid()", "Controls\\VMuktiGrid\\ctlVmuktiGrid.cs");
}
        }


        private void btnPane_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FncControllPane(!blnFirstClick);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"btnPane_Click()","Controls\\VMuktiGrid\\ctlVMuktiGrid.xaml.cs");
            }
        }

        public void FncControllPane(bool Close)
        {

            try
            {
                if (blnFirstClick != Close)
                {

                    if (Close)
                {
                    //close
                    if (pageControl != null)
                    {
                        pageControl.FncOpenPageStrip(false);
                        blnFirstClick = false;
                        if (pageControl.SelectedItem != null)
                        {
                            VMuktiGrid.ctlTab.TabControl objt = ((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.SelectedItem).Content);
                            if (objt != null)
                            {
                                objt.FncOpenTabStrip(false);
                                btnPane.ToolTip = "Show Page and Tab";
                            }
                        }
                    }
                   
                }
                else
                {
                   //open
                    if (pageControl != null)
                    {
                          
                        pageControl.FncOpenPageStrip(true);
                        if (pageControl.SelectedItem != null)
                        {
                            VMuktiGrid.ctlTab.TabControl objt = (((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)pageControl.SelectedItem).Content));
                            if (objt != null)
                            {
                                objt.FncOpenTabStrip(true);
                                btnPane.ToolTip = "Hide Page and Tab";
                            }
                        }
                    }

                }

                ((ScaleTransform)((TransformGroup)btnPane.GetValue(Button.RenderTransformProperty)).Children[0]).ScaleX *= -1;
                    blnFirstClick = !blnFirstClick;
                }
            
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"FncClosePane()","Controls\\VMuktiGrid\\ctlVMuktiGrid.xaml.cs");
           
            }
        }

        public void FncOpenPane()
        {

            try
            {
                blnFirstClick = true;
                btnPane_Click(null, null);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"FncClosePane()","Controls\\VMuktiGrid\\ctlVMuktiGrid.xaml.cs");
           
            }
        }
    }
}
