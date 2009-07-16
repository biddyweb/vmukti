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
using ImportLeads.Common;
using System.Data;
using ImportLeads.DataAccess;
using VMuktiAPI;
using System.Text;
namespace ImportLeads.Business
{
    public class ClsCountry : ClsBaseObject
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

        private int _ID = ImportLeads.Common.ClsConstants.NullInt;
        private string _Name = ImportLeads.Common.ClsConstants.NullString;
        private int _CountryCode = ImportLeads.Common.ClsConstants.NullInt;

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public int CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                Name = GetString(row, "Name");
                CountryCode = GetInt(row, "CountryCode");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsCountry.cs");
                return false;
            }
        }

        //public static ClsCountry GetByID(int ID)
        //{
        //    ClsCountry obj = new ClsCountry();
        //    DataSet ds = new SIPUserInfo.DataAccess.ClsSIPUserInfoDataService().SIPUserInfo_GetByID(ID);

        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new SIPUserInfo.DataAccess.ClsSIPUserInfoDataService(txn).SIPUserInfo_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        //public void Save()
        //{
        //    Save(null);
        //}

        //public void Save(IDbTransaction txn)
        //{
        //    new SIPUserInfo.DataAccess.ClsSIPUserInfoDataService(txn).SIPUserInfo_Save(ref _ID, _SIPID, _SIPPass, _ActiveServerID, _CreatedBy);
        //}

        #endregion
    }
}
