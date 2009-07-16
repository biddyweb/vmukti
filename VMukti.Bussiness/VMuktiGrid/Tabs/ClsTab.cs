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
using System.Data;
using VMukti.Common;
using VMukti.DataAccess.VMuktiGrid;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using System;
using System.Text;
//using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.VMuktiGrid
{
    public class ClsTab : ClsBaseObject
    {
        #region Fields      
       
        private int intTabId = VMukti.Common.ClsConstants.NullInt;
        private int intPageId = VMukti.Common.ClsConstants.NullInt;
        private int intTabPosition = VMukti.Common.ClsConstants.NullInt;
        private string strTabTitle = VMukti.Common.ClsConstants.NullString;
        private string strTabDesc = VMukti.Common.ClsConstants.NullString;
        private double dblC1Width = VMukti.Common.ClsConstants.NullDouble;
        private double dblC2Width = VMukti.Common.ClsConstants.NullDouble;
        private double dblC3Width = VMukti.Common.ClsConstants.NullDouble;
        private double dblC4Height = VMukti.Common.ClsConstants.NullDouble;
        private double dblC5Height = VMukti.Common.ClsConstants.NullDouble;
        #endregion

        #region Properties

        public int TabId
        {
            get { return intTabId; }
            set { intTabId = value; }
        }

        public int PageId
        {
            get { return intPageId; }
            set { intPageId = value; }
        }

        public int TabPosition
        {
            get { return intTabPosition; }
            set { intTabPosition = value; }
        }

        public string TabTitle
        {
            get { return strTabTitle; }
            set { strTabTitle = value; }
        }

        public string TabDesc
        {
            get { return strTabDesc; }
            set { strTabDesc = value; }
        }

        public double C1Width
        {
            get { return dblC1Width; }
            set { dblC1Width = value; }
        }

        public double C2Width
        {
            get { return dblC2Width; }
            set { dblC2Width = value; }
        }

        public double C3Width
        {
            get { return dblC3Width; }
            set { dblC3Width = value; }
        }

        public double C4Height
        {
            get { return dblC4Height; }
            set { dblC4Height = value; }
        }

        public double C5Height
        {
            get { return dblC5Height; }
            set { dblC5Height = value; }
        }
        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                intTabId = GetInt(row, "ID");
                intPageId = GetInt(row, "PageID");
                intTabPosition = GetInt(row, "TabPosition");
                strTabTitle = GetString(row, "TabTitle");
                strTabDesc = GetString(row, "Description");
                dblC1Width = GetDouble(row, "C1Width");
                dblC2Width = GetDouble(row, "C2Width");
                dblC3Width = GetDouble(row, "C3Width");
                dblC4Height = GetDouble(row, "C4Height");
                dblC5Height = GetDouble(row, "C5Height");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsTab.cs");              
                return false;
            }
        }

        public int Add_Tab(int intTabId, int intPageId, int intTabPosition, string strTabTitle, string strTabDesc, int intUserId, double C1WidthTab, double C2WidthTab, double C3WidthTab, double C4HeigthTab, double C5HeightTab)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intTabId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pPageID";
                    objInfo2.PValue = intPageId;
                    objInfo2.PDBType = "BigInt";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pTabPosition";
                    objInfo3.PValue = intTabPosition;
                    objInfo3.PDBType = "Int";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pTabTitle";
                    objInfo4.PValue = strTabTitle;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pDescription";
                    objInfo5.PValue = strTabDesc;
                    objInfo5.PDBType = "NVarchar";
                    objInfo5.PSize = 200;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pGivenID";
                    objInfo6.PValue = intUserId;
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PSize = 200;

                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pC1Width";
                    objInfo7.PValue = C1WidthTab;
                    objInfo7.PDBType = "Float";
                    objInfo7.PSize = 200;

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pC2Width";
                    objInfo8.PValue = C2WidthTab;
                    objInfo8.PDBType = "Float";
                    objInfo8.PSize = 200;

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pC3Width";
                    objInfo9.PValue = C3WidthTab;
                    objInfo9.PDBType = "Float";
                    objInfo9.PSize = 200;

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pC4Height";
                    objInfo10.PValue = C4HeigthTab;
                    objInfo10.PDBType = "Float";
                    objInfo10.PSize = 200;

                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pC5Height";
                    objInfo11.PValue = C5HeightTab;
                    objInfo11.PDBType = "Float";
                    objInfo11.PSize = 200;

                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Output";
                    objInfo12.PName = "@pReturnMaxID";
                    objInfo12.PValue = -1;
                    objInfo12.PDBType = "BigInt";
                    objInfo12.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    lstSP.Add(objInfo5);
                    lstSP.Add(objInfo6);
                    lstSP.Add(objInfo7);
                    lstSP.Add(objInfo8);
                    lstSP.Add(objInfo9);
                    lstSP.Add(objInfo10);
                    lstSP.Add(objInfo11);
                    lstSP.Add(objInfo12);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                    
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                   List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intTabId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pPageID";
                    objInfo2.PValue = intPageId;
                    objInfo2.PDBType = "BigInt";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pTabPosition";
                    objInfo3.PValue = intTabPosition;
                    objInfo3.PDBType = "Int";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pTabTitle";
                    objInfo4.PValue = strTabTitle;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pDescription";
                    objInfo5.PValue = strTabDesc;
                    objInfo5.PDBType = "NVarchar";
                    objInfo5.PSize = 200;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pGivenID";
                    objInfo6.PValue = intUserId;
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PSize = 200;

                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pC1Width";
                    objInfo7.PValue = C1WidthTab;
                    objInfo7.PDBType = "Float";
                    objInfo7.PSize = 200;

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pC2Width";
                    objInfo8.PValue = C2WidthTab;
                    objInfo8.PDBType = "Float";
                    objInfo8.PSize = 200;

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pC3Width";
                    objInfo9.PValue = C3WidthTab;
                    objInfo9.PDBType = "Float";
                    objInfo9.PSize = 200;

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pC4Height";
                    objInfo10.PValue = C4HeigthTab;
                    objInfo10.PDBType = "Float";
                    objInfo10.PSize = 200;

                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pC5Height";
                    objInfo11.PValue = C5HeightTab;
                    objInfo11.PDBType = "Float";
                    objInfo11.PSize = 200;

                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Output";
                    objInfo12.PName = "@pReturnMaxID";
                    objInfo12.PValue = -1;
                    objInfo12.PDBType = "BigInt";
                    objInfo12.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    lstSP.Add(objInfo5);
                    lstSP.Add(objInfo6);
                    lstSP.Add(objInfo7);
                    lstSP.Add(objInfo8);
                    lstSP.Add(objInfo9);
                    lstSP.Add(objInfo10); 
                    lstSP.Add(objInfo11); 
                    lstSP.Add(objInfo12);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }
                else
                {
                    return (new ClsTabDataService().AddTab(intTabId, intPageId, intTabPosition, strTabTitle, strTabDesc, intUserId, C1WidthTab, C2WidthTab, C3WidthTab, C4HeigthTab, C5HeightTab));
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Add_Tab()", "ClsTab.cs");             
                return -1;
            }
        }

        public int Get_TabMaxId()
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Output";
                    objInfo.PName = "@pReturnMaxID";
                    objInfo.PValue = -1;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                    
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Output";
                    objInfo.PName = "@pReturnMaxID";
                    objInfo.PValue = -1;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                }
                else
                {
                    return (new ClsTabDataService().GetMaxTabId());
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Get_TabMaxId()", "ClsTab.cs");                 
                return -1;
            }
        }

        public void Remove_Tab(int intTabId)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Output";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intTabId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                    
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Output";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intTabId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
                else
                {
                    new ClsTabDataService().RemoveTab(intTabId);
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Remove_Tab()", "ClsTab.cs");   
            }
        }

        public DataSet Get_TabPage(int intPageId)
        {
            try
            {
                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intPageId;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                    
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intPageId;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
                else
                {
                    return (new ClsTabDataService().GetTabPage(intPageId));
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Get_TabPage()", "ClsTab.cs");               
                return null;
            }
        }
  
        public static ClsTab Get_Tab(int intTabId)
        {
            try
            {
                ClsTab obj = new ClsTab();

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    DataSet ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                    if (!obj.MapData(ds)) obj = null;
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataSet ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                    if (!obj.MapData(ds)) obj = null;
                }
                else
                {
                    DataSet ds = new ClsTabDataService().GetTab(intTabId);
                    if (!obj.MapData(ds)) obj = null;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Get_Tab()", "ClsTab.cs");                  
                return null;
            }
        }

        #endregion
    }
}
