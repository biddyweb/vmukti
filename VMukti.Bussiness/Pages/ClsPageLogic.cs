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
    public class ClsPageLogic : ClsBaseObject
    {
        #region Fields

        private int intPageId = VMukti.Common.ClsConstants.NullInt;
        private string strPageTitle = VMukti.Common.ClsConstants.NullString;

        #endregion

        #region Properties

        public int PageId
        {
            get { return intPageId; }
            set { intPageId = value; }
        }

        public string PageTitle
        {
            get { return strPageTitle; }
            set { strPageTitle = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            intPageId = GetInt(row, "ID");
            strPageTitle = GetString(row, "PageTitle");
            return base.MapData(row);
        }

        public  int Add_Page(int intPageId, string strPageTitle, string strPageDesc, bool blnIsPublic, int intUserId)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intPageId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pPageTitle";
                objInfo2.PValue = strPageTitle;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pDescription";
                objInfo3.PValue = strPageDesc;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pIsPublic";
                objInfo4.PValue = blnIsPublic;
                objInfo4.PDBType = "Bit";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pUserID";
                objInfo5.PValue = intUserId;
                objInfo5.PDBType = "BigInt";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Output";
                objInfo6.PName = "@pReturnMaxID";
                objInfo6.PValue = -1;
                objInfo6.PDBType = "BigInt";
                objInfo6.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);
                lstSP.Add(objInfo6);


                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPage", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Page(int intPag...)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPage", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Page(int intPag...)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPage", CSqlInfo).ToString());
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
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pPageTitle";
                objInfo2.PValue = strPageTitle;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pDescription";
                objInfo3.PValue = strPageDesc;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pIsPublic";
                objInfo4.PValue = blnIsPublic;
                objInfo4.PDBType = "Bit";
                objInfo4.PSize = 200;
                
                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pUserID";
                objInfo5.PValue = intUserId;
                objInfo5.PDBType = "BigInt";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Output";
                objInfo6.PName = "@pReturnMaxID";
                objInfo6.PValue = -1;
                objInfo6.PDBType = "BigInt";
                objInfo6.PSize = 200;

               
                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);
                lstSP.Add(objInfo6);
                

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
               
                try
                {
                return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPage", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Page(int intPag...)", "ClsPageLogic.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPage", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Page(int intPag...)", "ClsPageLogic.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPage", CSqlInfo).ToString());
                }
              
            }
            else
            {
                return (new ClsPageDataService().AddPage(intPageId, strPageTitle, strPageDesc, blnIsPublic, intUserId));
            }
        }
        }

        public  void Page_Allocated(int intPageAllocationId, int intPageId, int intUserid)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intPageAllocationId;
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
                objInfo3.PName = "@pUserID";
                objInfo3.PValue = intUserid;
                objInfo3.PDBType = "BigInt";
                objInfo3.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;



                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Page_Allocated(int intPageAllocat...)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Page_Allocated(int intPageAllocat...)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEPageAllocation", CSqlInfo);
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
                objInfo.PValue = intPageAllocationId;
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
                objInfo3.PName = "@pUserID";
                objInfo3.PValue = intUserid;
                objInfo3.PDBType = "BigInt";
                objInfo3.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;


               
                try
                {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Page_Allocated(int intPageAllocat...)", "ClsPageLogic.cs");                     
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Page_Allocated(int intPageAllocat...)", "ClsPageLogic.cs");                     
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEPageAllocation", CSqlInfo);
                }
             
                
            }
            else
            {
                new ClsPageDataService().PageAllocated(intPageAllocationId, intPageId, intUserid);
            }
        }
        }

        public  int Get_PageMaxId()
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

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;



                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPageAllocation", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageMaxId", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPageAllocation", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageMaxId", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPageAllocation", CSqlInfo).ToString());
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

                lstSP.Add(objInfo);                

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                
                
                try
                {
                return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPageAllocation", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageMaxId", "ClsPageLogic.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPageAllocation", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageMaxId", "ClsPageLogic.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPageAllocation", CSqlInfo).ToString());
                }
                          
            }
            else
            {
                return (new ClsPageDataService().GetMaxPageId());
            }
        }
        }

        public  ClsPageLogic Get_PageInfo(int intPageId)
        {
            ClsPageLogic obj = new ClsPageLogic();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                DataSet ds;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'").dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageInfo(int intPageId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'").dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageInfo(int intPageId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'").dsInfo;
                }
                if (!obj.MapData(ds)) obj = null;
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                DataSet ds;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'").dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageInfo(int intPageId)", "ClsPageLogic.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'").dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PageInfo(int intPageId)", "ClsPageLogic.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select id,pagetitle from page where ID=" + intPageId + "and isdeleted='false'").dsInfo;
                }
              if (!obj.MapData(ds)) obj = null;
            }
            else
            {                
                DataSet ds = new ClsPageDataService().GetPageInfo(intPageId);
                if (!obj.MapData(ds)) obj = null;
            }
            }
            return obj;
        }

        public  void Remove_Page(int intPageId)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intPageId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;


                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPage", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Page(int intPageId)", "ClsPageLogic.cs");
                    ClsException.LogError(e);
                    ClsException.WriteToErrorLogFile(e);
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPage", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Page(int intPageId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPage", CSqlInfo);
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
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

               
                try
                {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPage", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Page(int intPageId)", "ClsPageLogic.cs");
                    ClsException.LogError(e);
                    ClsException.WriteToErrorLogFile(e);
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPage", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_Page(int intPageId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPage", CSqlInfo);
                }
           
            }
            else
            {
                new ClsPageDataService().RemovePage(intPageId);
            }
        }
        }
        public  void Remove_PageAllocated(int intPageId, int intUserId)
        {
            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intPageId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@uID";
                objInfo2.PValue = intUserId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PageAllocated(int intPageId, int intUserId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PageAllocated(int intPageId, int intUserId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPageAllocation", CSqlInfo);
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
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;
                                
                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@uID";
                objInfo2.PValue = intUserId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PageAllocated(int intPageId, int intUserId)", "ClsPageLogic.cs");                  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPageAllocation", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PageAllocated(int intPageId, int intUserId)", "ClsPageLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPageAllocation", CSqlInfo);
                }
            }
            else
            {
                new ClsPageDataService().RemovePageAllocation(intPageId, intUserId);
                }
            }
        }


        #endregion
    }
}
