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
using Campaign.Common;
using System.Data;
using Campaign.DataAccess;
using VMuktiAPI;
using System.Text;
namespace Campaign.Business
{
    public class ClsGroup : ClsBaseObject
    {
        //public static StringBuilder sb1;
        #region Fields

        private Int64 _ID = Campaign.Common.ClsConstants.NullLong;
        private Int64 _CampID = Campaign.Common.ClsConstants.NullLong;
        private string _GroupName = Campaign.Common.ClsConstants.NullString;
        private bool _IsActive = false;

        #endregion

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public Int64 CampID
        {
            get { return _CampID; }
            set { _CampID = value; }
        }

        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

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
                ID = GetLong(row, "ID");
                GroupName = GetString(row, "GroupName");
                IsActive = GetBool(row, "IsActive");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsGroup.cs");
            }

            return false;
        }

        public void Save()
        {
            try
            {
                Save(null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsGroup.cs");
            }
        }
        public void Save(IDbTransaction txn)
        {
            try
            {
                new Campaign.DataAccess.ClsCampaignDataService(txn).CampaignGroup_Save(_CampID, _ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsGroup.cs");
            }

        }
    }

        #endregion 
  
}
