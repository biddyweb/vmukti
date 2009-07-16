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
using VMukti.DataAccess.VMuktiGrid;
using VMukti.Common;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using System;
using VMuktiAPI;
using System.Text;
//using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.VMuktiGrid
{
    public class ClsPod : ClsBaseObject
    {  

        #region Fields     
      
        private int _podId = VMukti.Common.ClsConstants.NullInt;
        private int _tabId = VMukti.Common.ClsConstants.NullInt;
        private string _podTitle = VMukti.Common.ClsConstants.NullString;
        private int _columnId = VMukti.Common.ClsConstants.NullInt;
        private string _iconFile = VMukti.Common.ClsConstants.NullString;
        private int _moduleId = VMukti.Common.ClsConstants.NullInt;

        #endregion 

        #region Properties

        public int PodId 
        {
            get { return _podId; }
            set { _podId = value; }
        }

        public int TabId 
        {
            get { return _tabId; }
            set { _tabId = value; }
        }

        public string PodTitle 
        {
            get { return _podTitle; }
            set { _podTitle = value; }
        }

        public int ColumnId 
        {
            get { return _columnId; }
            set { _columnId = value; }
        }

        public string IconFile 
        {
            get { return _iconFile; }
            set { _iconFile = value; }
        }

        public int ModuleId 
        {
            get { return _moduleId; }
            set { _moduleId = value; }
        }


        //ID, ModuleName, ModuleVersion, Description, AssemblyFile, ClassName, ZipFile, IsCollaborative, NeedsAuthentication

        #endregion 
      
        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                _podId = GetInt(row, "ID");
                _tabId = GetInt(row, "TabID");
                _podTitle = GetString(row, "PodTitle");
                _columnId = GetInt(row, "ColumnID");
                _iconFile = GetString(row, "IconFile");
                _moduleId = GetInt(row, "ModuleID");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsPod.cs"); 
                return false;
            }
        }        

       
        public static int AddPod(int PodId, int TabId, string strPodTitle, int intColumnID, string IFile, int intModuleId, int intUserid)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = PodId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pTabID";
                    objInfo1.PValue = TabId;
                    objInfo1.PDBType = "BigInt";
                    objInfo1.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pPodTitle";
                    objInfo2.PValue = strPodTitle;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pColumnID";
                    objInfo3.PValue = intColumnID;
                    objInfo3.PDBType = "BigInt";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pIconFile";
                    objInfo4.PValue = IFile;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pModuleID";
                    objInfo5.PValue = intModuleId;
                    objInfo5.PDBType = "BigInt";
                    objInfo5.PSize = 200;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pUserID";
                    objInfo6.PValue = intUserid;
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PSize = 200;

                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Output";
                    objInfo7.PName = "@pReturnMaxID";
                    objInfo7.PValue = -1;
                    objInfo7.PDBType = "BigInt";
                    objInfo7.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo1);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    lstSP.Add(objInfo5);
                    lstSP.Add(objInfo6);
                    lstSP.Add(objInfo7);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                    
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                   
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = PodId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pTabID";
                    objInfo1.PValue = TabId;
                    objInfo1.PDBType = "BigInt";
                    objInfo1.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pPodTitle";
                    objInfo2.PValue = strPodTitle;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pColumnID";
                    objInfo3.PValue = intColumnID;
                    objInfo3.PDBType = "BigInt";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pIconFile";
                    objInfo4.PValue = IFile;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pModuleID";
                    objInfo5.PValue = intModuleId;
                    objInfo5.PDBType = "BigInt";
                    objInfo5.PSize = 200;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pUserID";
                    objInfo6.PValue = intUserid;
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PSize = 200;

                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Output";
                    objInfo7.PName = "@pReturnMaxID";
                    objInfo7.PValue = -1;
                    objInfo7.PDBType = "BigInt";
                    objInfo7.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo1);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    lstSP.Add(objInfo5);
                    lstSP.Add(objInfo6);
                    lstSP.Add(objInfo7);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                }
                else
                {
                    return (new ClsPodDataService().Add_Pod(PodId, TabId, strPodTitle, intColumnID, IFile, intModuleId, intUserid));
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddPod()", "ClsPod.cs");             
                return -1;
            }
        }

       

        public static DataSet Get_PodTab(int intTabId)
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
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
                    

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
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
            }
            else
            {
                return (new ClsPodDataService().GetPodTab(intTabId));
            }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Get_PodTab()", "ClsPod.cs");
                return null;
            }
        }

        public static void Remove_PodTab(int intTabId)
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

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
                    
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

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
            }
            else
            {
                new ClsPodDataService().RemovePodTab(intTabId);
            }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Remove_PodTab()", "ClsPod.cs");  
            }
        }

        public void Remove_Pod(int intPodID)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intPodID;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPODLayout", CSqlInfo);

                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = intPodID;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPODLayout", CSqlInfo);

                }
                else
                {
                    new ClsPodDataService().RemovePod(intPodID);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Remove_PodTab()", "ClsPod.cs");  
            }
        }

        #endregion

    }
}
