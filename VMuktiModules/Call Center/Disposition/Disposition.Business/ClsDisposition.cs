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
using Disposition.Common;
using Disposition.DataAccess;
using System.Text;
//using VMuktiAPI;
namespace Disposition.Business
{
    public class ClsDisposition : ClsBaseObject

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

        private Int64  _ID = ClsConstants.NullInt;
        private string _DespositionName = ClsConstants.NullString;
	    private bool _IsDeleted = false;
	    private bool _IsActive =false;
        private string _Description = ClsConstants.NullString;
	    private DateTime _CreatedDate = ClsConstants.NullDateTime;
	    private Int64 _CreatedBy = ClsConstants.NullInt;
	    private DateTime _ModifiedDate = ClsConstants.NullDateTime;
	    private Int64 _ModifiedBy = ClsConstants.NullInt;
        

        #endregion 

        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }
        public string DespositionName
        {
            get { return _DespositionName; }
            set { _DespositionName = value; }
        }
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public Int64 CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }
        public Int64 ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                DespositionName = GetString(row, "DespositionName");
                IsDeleted = GetBool(row, "IsDeleted");
                IsActive = GetBool(row, "IsActive");
                Description = GetString(row, "Description");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex,"MapData()", "ClsDisposition.cs");
                return false;

            }
                
            }
            
       

        public static ClsDisposition GetByDispositionID(Int64 ID)
        {
            try
            {

                ClsDisposition obj = new ClsDisposition();
                //Get data by Disposition_GetByID function from Disposition.DataAccess
                DataSet ds = new Disposition.DataAccess.ClsDispositionDataService().Disposition_GetByID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByDispositionID()", "ClsDisposition.cs");
                return null;
                
            }
            
        }

        public static void Delete(Int64 ID)
        {
            //call Delete Function
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDisposition.cs");
                
                
            }
        }

        public static void Delete(Int64 ID, IDbTransaction txn)
        {
            //call Disposition_Delete function from Disposition.DataAccess
            try
            {
                new Disposition.DataAccess.ClsDispositionDataService(txn).Disposition_Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDisposition.cs");


            }
        }

        public void Delete()
        {
            try
            {
                Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDisposition.cs");


            }
        }

        public void Delete(IDbTransaction txn)
        {
            try
            {
                Delete(ID, txn);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsDisposition.cs");

            }
        }

        public Int64 Save()
        {
            try
            {
                //Call Save Function
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsDisposition.cs");
                return 0;
            }
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                //Call Disposition_Save function from 
                return (new Disposition.DataAccess.ClsDispositionDataService(txn).Disposition_Save(ref _ID, _DespositionName, _IsActive, _Description, _CreatedBy));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsDisposition.cs");
                return 0;
            }
        }

        #endregion 
    }
}
