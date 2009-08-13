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
using System.Data;

using rptCountByState.DataAccess;
using rptCountByState.Common;
using System.Text;
using VMuktiAPI;

namespace rptCountByState.Business
{
    public class ClsRptCountByState : ClsBaseObject
    {
        //public static StringBuilder sb1;
        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}
        #region Fields

        #endregion 

        #region Properties

       

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData(DataRow row)", "ClsRptCountByState.cs");
                return false;

            }
        }

        public static DataSet GetAllCampaign()
        {
            try
            {
                //Get campaign data using rptCountByState_GetAllCampaign from rptCountByState.DataAccess
                return new rptCountByState.DataAccess.ClsRptCountByStateDataService().rptCountByState_GetAllCampaign();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllCampaign()", "ClsRptCountByState.cs");
                return null;
            }
        }

        public static DataSet GetAllListOfCampaign(int CampaignID)
        {
            try
            {
                //Get campaing using rptCountByState_GetAllListByCapmID from rptCountByState.DataAccess
                return new rptCountByState.DataAccess.ClsRptCountByStateDataService().rptCountByState_GetAllListByCapmID(CampaignID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllListOfCampaign()", "ClsRptCountByState.cs");
                return null;
            }
        }

        public static DataSet GetCountByState(int ListID, DateTime dtStartDate, DateTime dtEndDate)
        {
            try
            {
                //Get data using rptCountByState_GetCountByState function from rptCountByState.DataAccess
                return new rptCountByState.DataAccess.ClsRptCountByStateDataService().rptCountByState_GetCountByState(ListID, dtStartDate, dtEndDate);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetCountByState()", "ClsRptCountByState.cs");
                return null;
            }
        }

        public static void Delete(int QuesID)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsRptCountByState.cs");
            }
            //Delete(QuesID, null);
        }

        public static void Delete(int QuesID, IDbTransaction txn)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction txn)", "ClsRptCountByState.cs");
            }
            //new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Delete(QuesID);
        }

        public void Save()
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsRptCountByState.cs");
            }
            //Save(null);
        }

        public void Save(IDbTransaction txn)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save(IDbTransaction txn)", "ClsRptCountByState.cs");
            }

            //new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Save(_ID, _OptionName, _Description, _QuestionID, _ActionQuestionID);
        }

        #endregion 
    }
}
