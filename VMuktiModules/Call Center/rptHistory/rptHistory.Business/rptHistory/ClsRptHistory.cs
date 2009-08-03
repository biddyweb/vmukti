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
using VMuktiAPI;
using rptHistory.DataAccess;
using rptHistory.Common;
using System.Text;

namespace rptHistory.Business
{
    public class ClsRptHistory : ClsBaseObject
    {
        //public static StringBuilder sb1;
        #region Fields

        #endregion 

        #region Properties

        #endregion 

        #region Methods

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

        public override bool MapData(DataRow row)
        {
            try
            {

            return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsRptHistory.cs");
                return false;

            }
        }
        public static DataSet GetHistoryDataOfDates(DateTime dtStartDate, DateTime dtEndDate)
        {
            try
            {
                //Get data using rptHistory_GetHistoryDataOfDates function from rptHistory.DataAccess
            return new rptHistory.DataAccess.ClsRptHistoryDataService().rptHistory_GetHistoryDataOfDates(dtStartDate, dtEndDate);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetHistoryDataOfDates()", "ClsRptHistory.cs");
                return null;

            }
        }

        public static void Delete(int QuesID)
        {
            try
            {
            //Delete(QuesID, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsRptHistory.cs");
            }
        }

        public static void Delete(int QuesID, IDbTransaction txn)
        {
            try

            {
            //new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Delete(QuesID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction txn)", "ClsRptHistory.cs");
            }
        }

        public void Save()
        {
            try
            {
            //Save(null);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsRptHistory.cs");
            }
        }
        public void Save(IDbTransaction txn)
        {
            try
            {
            //new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Options_Save(_ID, _OptionName, _Description, _QuestionID, _ActionQuestionID);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save(IDbTransaction txn)", "ClsRptHistory.cs");
            }
        }
        #endregion 
    }
}
