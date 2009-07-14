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
using VMukti.DataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;

namespace VMukti.Business
{
    public class ClsTabLogic : ClsBaseObject
    {
        #region Fields

        private int intTabId = VMukti.Common.ClsConstants.NullInt;
        private string strTabTitle = VMukti.Common.ClsConstants.NullString;

        #endregion

        #region Properties

        public int TabId
        {
            get { return intTabId; }
            set { intTabId = value; }
        }

        public string TabTitle
        {
            get { return strTabTitle; }
            set { strTabTitle = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            intTabId = GetInt(row, "ID");
            strTabTitle = GetString(row, "TabTitle");
            return base.MapData(row);
        }

        public  int Add_Tab(int intTabId, int intPageId, int intTabPosition, string strTabTitle, string strTabDesc, int intUserId)
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
                objInfo7.Direction = "Output";
                objInfo7.PName = "@pReturnMaxID";
                objInfo7.PValue = -1;
                objInfo7.PDBType = "BigInt";
                objInfo7.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);
                lstSP.Add(objInfo6);
                lstSP.Add(objInfo7);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Tab(int intTabId...)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Tab(int intTabId...)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }


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
                objInfo7.Direction = "Output";
                objInfo7.PName = "@pReturnMaxID";
                objInfo7.PValue = -1;
                objInfo7.PDBType = "BigInt";
                objInfo7.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);
                lstSP.Add(objInfo6);
                lstSP.Add(objInfo7);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Tab(int intTabId...)", "ClsTabLogic.cs");                                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Tab(int intTabId...)", "ClsTabLogic.cs");                                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAETabs", CSqlInfo).ToString());
                }
            }
            else
            {

                return (new ClsTabDataService().AddTab(intTabId, intPageId, intTabPosition, strTabTitle, strTabDesc, intUserId));
            }
        }
        }

        public  int Get_TabMaxId()
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

                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabMaxId()", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabMaxId()", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                }
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

                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                 }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabMaxId()", "ClsTabLogic.cs");  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabMaxId()", "ClsTabLogic.cs");  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spGMaxTabId", CSqlInfo).ToString());
                }
                

            }
            else
            {
                return (new ClsTabDataService().GetMaxTabId());
            }
        }
        }

        public  void Remove_Tab(int intTabId)
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
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Tab(int intTabId", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Tab(int intTabId", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
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
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Tab(int intTabId", "ClsTabLogic.cs");                     
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Tab(int intTabId", "ClsTabLogic.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDTabs", CSqlInfo);
                }
            }
            else
            {
                new ClsTabDataService().RemoveTab(intTabId);
            }
        }
        }

        public  DataSet Get_TabPage(int intPageId)
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


                try
                {
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabPage(int intPageId)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabPage(int intPageId)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
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


                try
                {
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabPage(int intPageId)", "ClsTabLogic.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_TabPage(int intPageId)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGTabPage", CSqlInfo).dsInfo;
                }
                
            }
            else
            {
                return (new ClsTabDataService().GetTabPage(intPageId));
            }
        }
        }

        public  DataSet Get_Tab(int intTabId)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_Tab(int intTabId)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_Tab(int intTabId)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_Tab(int intTabId)", "ClsTabLogic.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_Tab(int intTabId)", "ClsTabLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Tabs where id=" + intTabId + "and isdeleted='False'").dsInfo;
                }
                
            }
            else
            {
                return new ClsTabDataService().GetTab(intTabId);
                }
            }
        }

        #endregion
    }
}
