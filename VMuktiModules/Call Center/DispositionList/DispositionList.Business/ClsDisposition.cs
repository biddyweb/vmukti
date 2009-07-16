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
using DispositionList.Common;
using VMuktiAPI;
namespace DispositionList.Business
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

        private Int64 _ID = Constants.NullInt64;
        private string _DispositionName = Constants.NullString;
        public Int64 DispositionListId = Constants.NullInt64;
        private bool _IsActive;

        #endregion

        #region Properties  

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string DispositionName
        {
            get { return _DispositionName; }
            set { _DispositionName = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
       
        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt64(row, "ID");
                DispositionName = GetString(row, "DespositionName");
                IsActive = GetBool(row, "IsActive");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsDisposition.cs");
                return false;
            } 
        }

        public static ClsDisposition GetByDispositionListID(Int64 ID)
        {
            try
            {
                ClsDisposition obj = new ClsDisposition();
                DataSet ds = new DispositionList.DataAccess.ClsDispositionDataService().Disposition_GetByDispositionListID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByDispositionListID()", "ClsDisposition.cs");
                return null;
            } 
            
        }

        public static void Delete(Int64 ID)
        {
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
            try
            {
                new DispositionList.DataAccess.ClsDispositionDataService(txn).Disposition_Delete(ID);
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

        public void Save()
        {
            try
            {
                Save(null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsDisposition.cs");

            } 
        }

        public void Save(IDbTransaction txn)
        {
            try
            {
                new DispositionList.DataAccess.ClsDispositionDataService(txn).Disposition_Save(ref _ID, _DispositionName, DispositionListId);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsDisposition.cs");

            } 
        }

        #endregion 
    }
}
