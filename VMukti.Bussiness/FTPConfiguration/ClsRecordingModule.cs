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
using System.Text;
using System;

namespace VMukti.Business
{
    public class ClsRecordingModule    
    {        
    
        ClsRecordingDataService clsUserDataService = null;
        public ClsRecordingModule()
        {
            try
            {
                clsUserDataService = new ClsRecordingDataService();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClsRecordingModule()","ClsRecordingModule.cs");
            }
        }
        public void AddNewFTPDetail(ref int ID, string ServerIP, string ServerPort, string UserName, string DirectoryName, string strPassword,string strTag)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "InputOutput";
                objInfo.PName = "@pID";
                objInfo.PValue = ID;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pServerIP";
                objInfo2.PValue = ServerIP;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pServerPort";
                objInfo3.PValue = ServerPort;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pUserName";
                objInfo4.PValue = UserName;
                objInfo4.PDBType = "NVarChar";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pDirectoryName";
                objInfo5.PValue = DirectoryName;
                objInfo5.PDBType = "NVarChar";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Input";
                objInfo6.PName = "@pPassword";
                objInfo6.PValue = strPassword;
                objInfo6.PDBType = "NVarChar";
                objInfo6.PSize = 200;

                clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                objInfo7.Direction = "Input";
                objInfo7.PName = "@pTag";
                objInfo7.PValue = strTag;
                objInfo7.PDBType = "NVarChar";
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
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAERecordingFTPServer", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddNewFTPDetail(ref int ID, string ServerIP, string ServerPort, string UserName, string DirectoryName, string strPassword,string strTag)", "ClsRecordingModule.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAERecordingFTPServer", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddNewFTPDetail(ref int ID, string ServerIP, string ServerPort, string UserName, string DirectoryName, string strPassword,string strTag)", "ClsRecordingModule.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAERecordingFTPServer", CSqlInfo);
                }

            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "InputOutput";
                objInfo.PName = "@pID";
                objInfo.PValue = ID;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pServerIP";
                objInfo2.PValue = ServerIP;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pServerPort";
                objInfo3.PValue = ServerPort;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pUserName";
                objInfo4.PValue = UserName;
                objInfo4.PDBType = "NVarChar";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pDirectoryName";
                objInfo5.PValue = DirectoryName;
                objInfo5.PDBType = "NVarChar";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Input";
                objInfo6.PName = "@pPassword";
                objInfo6.PValue = strPassword;
                objInfo6.PDBType = "NVarChar";
                objInfo6.PSize = 200;

                clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                objInfo7.Direction = "Input";
                objInfo7.PName = "@pTag";
                objInfo7.PValue = strTag;
                objInfo7.PDBType = "NVarChar";
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
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAERecordingFTPServer", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddNewFTPDetail(ref int ID, string ServerIP, string ServerPort, string UserName, string DirectoryName, string strPassword,string strTag)", "ClsRecordingModule.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAERecordingFTPServer", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddNewFTPDetail(ref int ID, string ServerIP, string ServerPort, string UserName, string DirectoryName, string strPassword,string strTag)", "ClsRecordingModule.cs");                                      
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAERecordingFTPServer", CSqlInfo);
                }


            }
            else
            {
                clsUserDataService.User_Save(ref ID, ServerIP, ServerPort, UserName, DirectoryName, strPassword, strTag);
            }
        }
        }

        public void SetRecordingFTPDetails(string strTag)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pTag";
                objInfo.PValue = strTag;
                objInfo.PDBType = "NVarChar";
                objInfo.PSize = 200;

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet myDS = null;
                try
                {
                    myDS = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEGetRecordingFTPDetail", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SetRecordingFTPDetails(string strTag)", "ClsRecordingModule.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    myDS = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEGetRecordingFTPDetail", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SetRecordingFTPDetails(string strTag)", "ClsRecordingModule.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    myDS = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEGetRecordingFTPDetail", CSqlInfo).dsInfo;
                }

                if (myDS.Tables[0].Rows.Count > 0)
                {
                    VMuktiInfo.FTPServerIP = myDS.Tables[0].Rows[0][1].ToString();
                    VMuktiInfo.FTPPort = myDS.Tables[0].Rows[0][2].ToString();
                    VMuktiInfo.FTPUserName = myDS.Tables[0].Rows[0][3].ToString();
                    VMuktiInfo.FTPDirPath = myDS.Tables[0].Rows[0][4].ToString();
                    VMuktiInfo.FTPPassword = myDS.Tables[0].Rows[0][5].ToString();
                }
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {                

                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pTag";
                objInfo.PValue = strTag;
                objInfo.PDBType = "NVarChar";
                objInfo.PSize = 200;

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet myDS=null;
                try
                {
                    myDS = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEGetRecordingFTPDetail", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SetRecordingFTPDetails(string strTag)", "ClsRecordingModule.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    myDS = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEGetRecordingFTPDetail", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SetRecordingFTPDetails(string strTag)", "ClsRecordingModule.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    myDS = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEGetRecordingFTPDetail", CSqlInfo).dsInfo;
                }

                if (myDS.Tables[0].Rows.Count > 0)
                {
                    VMuktiInfo.FTPServerIP = myDS.Tables[0].Rows[0][1].ToString();
                    VMuktiInfo.FTPPort = myDS.Tables[0].Rows[0][2].ToString();
                    VMuktiInfo.FTPUserName = myDS.Tables[0].Rows[0][3].ToString();
                    VMuktiInfo.FTPDirPath = myDS.Tables[0].Rows[0][4].ToString();
                    VMuktiInfo.FTPPassword = myDS.Tables[0].Rows[0][5].ToString();
                }
            }
            else
            {
                clsUserDataService.SetFTPDetails(strTag);
                }
            }
        }
    }
    
}
