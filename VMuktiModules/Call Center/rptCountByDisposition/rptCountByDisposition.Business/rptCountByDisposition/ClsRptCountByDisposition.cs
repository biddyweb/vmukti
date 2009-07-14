
/* VMukti 1.0 -- An Open Source Unified Communications Engine
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

using rptCountByDisposition.DataAccess;
using rptCountByDisposition.Common;
using System.Text;
using VMuktiAPI;

namespace rptCountByDisposition.Business
{
    public class ClsRptCountByDisposition : ClsBaseObject
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
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsRptCountByDisposition.cs");
                return false;
            }
        }

        public static DataSet GetAllCampaign()
        {
            try
            {
                return new rptCountByDisposition.DataAccess.ClsRptCountByDispositionDataService().rptCountByDisposition_GetAllCampaign();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllCampaign()", "ClsRptCountByDisposition.cs");
                return null;
            }
        }

        public static DataSet GetAllListOfCampaign(int CampaignID)
        {
            try
            {
                return new rptCountByDisposition.DataAccess.ClsRptCountByDispositionDataService().rptCountByDisposition_GetAllListByCapmID(CampaignID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllListOfCampaign()", "ClsRptCountByDisposition.cs");
                return null;
            }
        }

        public static DataSet GetCountByDisposition(int ListID, DateTime dtStartDate, DateTime dtEndDate)
        {
            try
            {
                //Get data using rptCountByDisposition_GetCountByDisposition function from rptCountByDispositionDataService
                return new rptCountByDisposition.DataAccess.ClsRptCountByDispositionDataService().rptCountByDisposition_GetCountByDisposition(ListID, dtStartDate, dtEndDate);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetCountByDisposition()", "ClsRptCountByDisposition.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsRptCountByDisposition.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsRptCountByDisposition.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsRptCountByDisposition.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Save(IDbTransaction txn)", "ClsRptCountByDisposition.cs");
            }

            //new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Save(_ID, _OptionName, _Description, _QuestionID, _ActionQuestionID);
        }

        #endregion 
    }
}
