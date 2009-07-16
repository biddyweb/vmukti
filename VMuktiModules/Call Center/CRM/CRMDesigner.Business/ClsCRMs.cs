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
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;

namespace CRMDesigner.Business
{
    public class ClsCRMs : ClsBaseObject
    {
        //public static StringBuilder sb1;

        #region Fields

        //creates a fields required for CRM table
        private int _ID = CRMDesigner.Common.ClsConstants.NullInt;
        private string _CRMURL = CRMDesigner.Common.ClsConstants.NullString;
        private string _CRMName = CRMDesigner.Common.ClsConstants.NullString;
        //private int _ScriptTypeID = ClsConstants.NullInt;
        private bool _IsActive = false;
        private bool _IsDeleted = false;
        private DateTime _CreatedDate = CRMDesigner.Common.ClsConstants.NullDateTime;
        private int _CreatedBy = CRMDesigner.Common.ClsConstants.NullInt;
        private DateTime _ModifiedDate = CRMDesigner.Common.ClsConstants.NullDateTime;
        private int _ModifiedBy = CRMDesigner.Common.ClsConstants.NullInt;

        #endregion

        #region properties

        //Creates properties of the fields.
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string CRMURL
        {
            get { return _CRMURL; }
            set { _CRMURL = value; }
        }

        public string CRMName
        {
            get { return _CRMName; }
            set { _CRMName = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }
        public int ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }

        #endregion

        #region Methods
        //Function for try and catch block.
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

        //Function for mapping data.
        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                CRMURL = GetString(row, "CRMURL");
                CRMName = GetString(row, "CRMName");
                IsDeleted = GetBool(row, "IsDeleted");
                IsActive = GetBool(row, "IsActive");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");


                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsCRMs.cs");
                return false;
            }
        }

        public static ClsCRM GetByCtlScriptID(int ID)
        {
            try
            {

                ClsCRM obj = new ClsCRM();
                DataSet ds = new CRMDesigner.DataAccess.ClsCRMDataService().CRM_GetByID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByCtlScriptID()", "ClsCRMs.cs");
                return null;
            }
        }

        //Function for deleting the CRM.
        public static void Delete(int ID)
        {
            try
            {
                //Calls the Delete function of the same class.
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCRMs.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {
                //calls the Delete function of the CRM.Dataaccess
                new CRMDesigner.DataAccess.ClsCRMDataService(txn).CRM_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCRMs.cs");
            }
        }
        public void Delete()
        {
            try
            {
                //Overloaded function of Delete.
                Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCRMs.cs");
            }
        }

        public void Delete(IDbTransaction txn)
        {
            try
            {
                //Overloaded function of delete.
                Delete(ID, txn);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCRMs.cs");
            }
        }

        public int Save()
        {
            try
            {
                //This function calls the save function of the same class.
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsCRMs.cs");
                return 0;
            }
        }

        public int Save(IDbTransaction txn)
        {
            try
            {
                //This function calls the CRM_Save function of ClsCRMDataService of CRM.DataAccess.
                return (new CRMDesigner.DataAccess.ClsCRMDataService(txn).CRM_Save(ref _ID, _CRMURL, _CRMName, _IsActive, _CreatedBy));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsCRMs.cs");
                return 0;
            }
        }
        #endregion
    }
}
