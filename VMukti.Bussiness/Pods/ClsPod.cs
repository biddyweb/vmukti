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
using VMukti.DataAccess;
using VMukti.Common;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;

namespace VMukti.Business
{
    public class ClsPod : ClsBaseObject
    {  

        #region Fields

	private string test = "";
	      private int moduleId = VMukti.Common.ClsConstants.NullInt;
        private string zipFile = VMukti.Common.ClsConstants.NullString;
        private string assemblyFile = VMukti.Common.ClsConstants.NullString;
        private string className = VMukti.Common.ClsConstants.NullString;

        #endregion 

        #region Properties
     
        public int Moduleid
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public string ZipFile
        {
            get { return zipFile; }
            set { zipFile = value; }
        }

        public string AssemblyFile
        {
            get { return assemblyFile; }
            set { assemblyFile = value; }
        }

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        #endregion 
      
        #region Methods

        public override bool MapData(DataRow row)
        {
            moduleId = GetInt(row, "ID");
            zipFile = GetString(row, "ZipFile");
            assemblyFile = GetString(row, "AssemblyFile");
            className = GetString(row, "ClassName");       
            return base.MapData(row);
        }        

        public static ClsPod GetPodModuleInfo(int intPodId)
        {
            ClsPod obj = new ClsPod();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intPodId;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet ds = null;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodModuleInfo", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetPodModuleInfo(int intPodId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodModuleInfo", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetPodModuleInfo(int intPodId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodModuleInfo", CSqlInfo).dsInfo;
                }

                if (!obj.MapData(ds)) obj = null;
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {                
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intPodId;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;
                               
                lstSP.Add(objInfo);
                
                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet ds=null;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodModuleInfo", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e,"GetPodModuleInfo(int intPodId)","ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodModuleInfo", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetPodModuleInfo(int intPodId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodModuleInfo", CSqlInfo).dsInfo;
                }
             
                if (!obj.MapData(ds)) obj = null;
            }
            else
            {
                DataSet ds = new ClsPodDataService().Pod_GetPodModuleInfo(intPodId);
                if (!obj.MapData(ds)) obj = null;
            }
            }
            return obj;
        }

        public static ClsPod GetModInfo(int intModuleId)
        {
            ClsPod obj = new ClsPod();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intModuleId;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet ds = null;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGModule", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetModInfo(int intModuleId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGModule", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetModInfo(int intModuleId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGModule", CSqlInfo).dsInfo;
                }
                if (!obj.MapData(ds)) obj = null;
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intModuleId;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet ds = null;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGModule", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetModInfo(int intModuleId)", "ClsPod.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGModule", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetModInfo(int intModuleId)", "ClsPod.cs");  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGModule", CSqlInfo).dsInfo;
                }
                if (!obj.MapData(ds)) obj = null;
            }
            else
            {
                DataSet ds = new ClsPodDataService().GetModuleInfo(intModuleId);
                if (!obj.MapData(ds)) obj = null;
            }
            }
            return obj;
        }

        public static int AddPod(int PodId, string TabId, string strPodTitle, double PodHeight, double PodWidth, double PodLeft, double PodTop, string IFile, int intModuleId, int intUserid)
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

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pTabID";
                objInfo2.PValue = TabId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pPodTitle";
                objInfo3.PValue = strPodTitle;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pPodHeight";
                objInfo4.PValue = PodHeight;
                objInfo4.PDBType = "Float";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pPodWidth";
                objInfo5.PValue = PodWidth;
                objInfo5.PDBType = "Float";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Input";
                objInfo6.PName = "@pPodLeft";
                objInfo6.PValue = PodLeft;
                objInfo6.PDBType = "Float";
                objInfo6.PSize = 200;

                clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                objInfo7.Direction = "Input";
                objInfo7.PName = "@pPodTop";
                objInfo7.PValue = PodTop;
                objInfo7.PDBType = "Float";
                objInfo7.PSize = 200;

                clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                objInfo8.Direction = "Input";
                objInfo8.PName = "@pIconFile";
                objInfo8.PValue = IFile;
                objInfo8.PDBType = "NVarChar";
                objInfo8.PSize = 200;

                clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                objInfo9.Direction = "Input";
                objInfo9.PName = "@pModuleID";
                objInfo9.PValue = intModuleId;
                objInfo9.PDBType = "BigInt";
                objInfo9.PSize = 200;

                clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                objInfo10.Direction = "Input";
                objInfo10.PName = "@pUserID";
                objInfo10.PValue = intUserid;
                objInfo10.PDBType = "BigInt";
                objInfo10.PSize = 200;

                clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                objInfo11.Direction = "Output";
                objInfo11.PName = "@pReturnMaxID";
                objInfo11.PValue = -1;
                objInfo11.PDBType = "BigInt";
                objInfo11.PSize = 200;


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


                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddPod(int PodId...)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddPod(int PodId...)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
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
                objInfo.PValue = PodId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pTabID";
                objInfo2.PValue = TabId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pPodTitle";
                objInfo3.PValue = strPodTitle;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pPodHeight";
                objInfo4.PValue = PodHeight;
                objInfo4.PDBType = "Float";
                objInfo4.PSize = 200;                

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pPodWidth";
                objInfo5.PValue = PodWidth;
                objInfo5.PDBType = "Float";
                objInfo5.PSize = 200;
                
                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Input";
                objInfo6.PName = "@pPodLeft";
                objInfo6.PValue = PodLeft;
                objInfo6.PDBType = "Float";
                objInfo6.PSize = 200;

                clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                objInfo7.Direction = "Input";
                objInfo7.PName = "@pPodTop";
                objInfo7.PValue = PodTop;
                objInfo7.PDBType = "Float";
                objInfo7.PSize = 200;

                clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                objInfo8.Direction = "Input";
                objInfo8.PName = "@pIconFile";
                objInfo8.PValue = IFile;
                objInfo8.PDBType = "NVarChar";
                objInfo8.PSize = 200;

                clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                objInfo9.Direction = "Input";
                objInfo9.PName = "@pModuleID";
                objInfo9.PValue = intModuleId;
                objInfo9.PDBType = "BigInt";
                objInfo9.PSize = 200;

                clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                objInfo10.Direction = "Input";
                objInfo10.PName = "@pUserID";
                objInfo10.PValue = intUserid;
                objInfo10.PDBType = "BigInt";
                objInfo10.PSize = 200;

                clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                objInfo11.Direction = "Output";
                objInfo11.PName = "@pReturnMaxID";
                objInfo11.PValue = -1;
                objInfo11.PDBType = "BigInt";
                objInfo11.PSize = 200;


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


                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddPod(int PodId...)", "ClsPod.cs");                                        
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddPod(int PodId...)", "ClsPod.cs");  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPODLayout", CSqlInfo).ToString());
                }
            }
            else
            {

                return (new ClsPodDataService().Add_Pod(PodId, TabId, strPodTitle, PodHeight, PodWidth, PodLeft, PodTop, IFile, intModuleId, intUserid));
            }
        }
        }

        public static DataSet Get_PodTab(int intTabId)
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

                try
                {
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PodTab(int intTabId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PodTab(int intTabId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
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
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PodTab(int intTabId)", "ClsPod.cs");                     
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PodTab(int intTabId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPodTab", CSqlInfo).dsInfo;
                }
                
            }
            else
            {
                return (new ClsPodDataService().GetPodTab(intTabId));
            }
        }
        }

        public static void Remove_PodTab(int intTabId)
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
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PodTab(int intTabId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PodTab(int intTabId)", "ClsPod.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
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

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PodTab(int intTabId)", "ClsPod.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Remove_PodTab(int intTabId)", "ClsPod.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spDPodTab", CSqlInfo);
                }      
            }
            else
            {
                new ClsPodDataService().RemovePodTab(intTabId);
                }
            }
        }

        #endregion

    }
}
