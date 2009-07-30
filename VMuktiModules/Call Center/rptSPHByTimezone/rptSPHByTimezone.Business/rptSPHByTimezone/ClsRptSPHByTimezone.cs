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

using rptSPHByTimezone.DataAccess;
using rptSPHByTimezone.Common;
using System.Text;
using VMuktiAPI;

namespace rptSPHByTimezone.Business
{
    public class ClsRptSPHByTimezone : ClsBaseObject
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
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsRptSPHByTimezone.cs");
                return false;
            }
        }

        public static DataSet GetAllCampaign()
        {
            try
            {
            return new rptSPHByTimezone.DataAccess.ClsRptSPHByTimezone().rptSPHByTimezone_GetAllCampaign();
        }

        catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllCampaign()", "ClsRptSPHByTimezone.cs");
                return null;

            }
        }
        public static DataSet GetAllListOfCampaign(int CampaignID)
        {
            try
            {
            return new rptSPHByTimezone.DataAccess.ClsRptSPHByTimezone().rptSPHByTimezone_GetAllListByCapmID(CampaignID);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllListOfCampaign()", "ClsRptSPHByTimezone.cs");
                return null;
            }
        }
        public static DataSet GetSPHByTimezone(int ListID,DateTime StartDate,DateTime EndDate)
        {
            try
            {
            return new rptSPHByTimezone.DataAccess.ClsRptSPHByTimezone().rptSPHByTimezone_GetCountByDisposition(ListID,StartDate,EndDate);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetSPHByTimezone()", "ClsRptSPHByTimezone.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsRptSPHByTimezone.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction txn)", "ClsRptSPHByTimezone.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsRptSPHByTimezone.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Save(IDbTransaction txn)", "ClsRptSPHByTimezone.cs");
            }
        }
        #endregion 
    }
}
