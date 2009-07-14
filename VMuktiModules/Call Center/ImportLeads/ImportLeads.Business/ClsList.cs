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
using System.Xml;
using VMuktiAPI;
using System.Text;
namespace ImportLeads.Business
{
    public class ClsList : ClsBaseObject
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
        private string _ListName = ImportLeads.Common.ClsConstants.NullString;
        private bool _IsDNC = false;
        private bool _IsActive = false;
        private int _CampID = ImportLeads.Common.ClsConstants.NullInt;

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int CampID
        {
            get { return _CampID; }
            set { _CampID = value; }
        }

        public string ListName
        {
            get { return _ListName; }
            set { _ListName = value; }
        }

        public bool IsDNC
        {
            get { return _IsDNC; }
            set { _IsDNC = value; }
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
                ID = GetInt(row, "ID");
                ListName = GetString(row, "ListName");
                //CampID = GetInt(row, "");
                IsDNC = GetBool(row, "IsDNCList");
                IsActive = GetBool(row, "IsActive");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsList.cs");
                return false;
            }
        }

        public static DataSet TimeZone_GetAll()
        {
            try
            {
                return (TimeZone_GetAll(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "TimeZone_GetAll()", "ClsList.cs");
                return null;
            }

        }

        public static DataSet TimeZone_GetAll(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).Timezone_GetAll());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "TimeZone_GetAll(IDbTransaction txn)", "ClsList.cs");
                return null;
            }
        }

        public static DataSet PhoneNumbers_GetAll(Int64 ListID)
        {
            try
            {
                return (PhoneNumbers_GetAll(null, ListID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PhoneNumbers_GetAll()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet PhoneNumbers_GetAll(IDbTransaction txn, Int64 ListID)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).PhoneNumbers_GetAll(ListID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PhoneNumbers_GetAll(IDbTransaction txn)", "ClsList.cs");
                return null;
            }
        }

        public static DataSet GetLeadFields(Int64 LeadFormatID)
        {
            try
            {
                return (GetLeadFields(null, LeadFormatID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLeadFields()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet GetLeadFields(IDbTransaction txn, Int64 LeadFormatID)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetLeadFields(LeadFormatID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLeadFields()", "ClsList.cs");
                return null;
            }
        }  


        public static DataSet AreaCode_GetAll()
        {
            try
            {
                return (AreaCode_GetAll(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet AreaCode_GetAll(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).AreaCode_GetAll());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet AreaCode_GetAll(string strTimeZone, string flag)
        {
            try
            {
                return (AreaCode_GetAll(null, strTimeZone, flag));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet AreaCode_GetAll(IDbTransaction txn, string strTimeZone, string flag)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).AreaCode_GetAll(strTimeZone, flag));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll(IDbTransaction txn)", "ClsList.cs");
                return null;
            }
        }
        //////////////////////////////////
        public static DataSet AreaCode_GetID(string strTimeZone)
        {
            try
            {
                return (AreaCode_GetID(null, strTimeZone));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetID()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet AreaCode_GetID(IDbTransaction txn, string strTimeZone)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).AreaCode_GetID(strTimeZone));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetID(IDbTransaction txn)", "ClsList.cs");
                return null;
            }
        }
        ///////////////////////////////////////

        /// <summary>
        public static DataSet StateCountry_GetAll()
        {
            try
            {
                return (StateCountry_GetAll(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "StateCountry_GetAll()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet StateCountry_GetAll(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).StateCountry_GetAll());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "StateCountry_GetAll(IDbTransaction txn)", "ClsList.cs");
                return null;
            }
        }

        /// 
        /// 
        /// </summary>
        /// <returns></returns>

        public static int GetMaxIDLeads()
        {
            try
            {
                return (GetMaxIDLeads(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxIDLeads()", "ClsList.cs");
                return 0;
            }
        }

        public static int GetMaxIDLeads(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetMaxIDLeads());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxIDLeads(IDbTransaction txn)", "ClsList.cs");
                return 0;
            }
        }


        public static int GetMaxIDTrendWestID()
        {
            try
            {
                return (GetMaxIDTrendWestID(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxIDTrendWestID()", "ClsList.cs");
                return 0;
            }
        }

        public static int GetMaxIDTrendWestID(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetMaxIDTrendWestID());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxIDTrendWestID()", "ClsList.cs");
                return 0;
            }
        }

        //GetLocationValues
        //public static DataSet GetLocationID(Int64 gotIt,Int64 CounID,Int64 SstID,Int64 ARCode,Int64 Zip)
        public static DataSet GetLocationID(Int64 Zip)
        {
            try
            {
                return (GetLocationID(null,Zip));
                //return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetLocationID()
                //return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).AreaCode_GetID(strTimeZone));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLocationID()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet GetLocationID(IDbTransaction txn, Int64 Zip)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetLocationID(Zip));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLocationID()", "ClsList.cs");
                return null;
            }
        }
        public static DataSet GetLocationDetail()
        {
            try
            {
                return (GetLocationDetail(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCode()", "ClsList.cs");
                return null;
            }
        }
        public static DataSet GetLocationDetail(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetLocationDetail());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCode()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet GetZipCode()
        {
            try
            {
                return (GetZipCode(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCode()", "ClsList.cs");
                return null;
            }
        }

        public static DataSet GetZipCode(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetZipCode());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCode()", "ClsList.cs");
                return null;
            }
        }

        public static int GetZipCodeID(Int64 finalzipcode)
        {
            try
            {
                return (GetZipCodeID(null, finalzipcode));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCodeID()", "ClsList.cs");
                return 0;
            }
        }

        public static int GetZipCodeID(IDbTransaction txn, Int64 finalzipcode)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetZipCodeID(finalzipcode)); 
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCodeID()", "ClsList.cs");
                return 0;
            }
        }

        public static int GetMaxLocationID()
        {
            try
            {

                return (GetMaxLocationID(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxLocationID()", "ClsList.cs");
                return 0;
            }
        }
        public static int GetMaxLocationID(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetMaxLocationID());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxLocationID(IDbTransaction txn)", "ClsList.cs");
                return 0;
            }
        }

        public static int GetMaxLeadDetailID()
        {
            try
            {

                return (GetMaxLeadDetailID(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxLeadDetailID()", "ClsList.cs");
                return 0;
            }
        }
        public static int GetMaxLeadDetailID(IDbTransaction txn)
        {
            try
            {
                return (new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).GetMaxLeadDetailID());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxLeadDetailID(IDbTransaction txn)", "ClsList.cs");
                return 0;
            }
        }

        //public static ClsUser GetByGroupID(int ID)
        //{
        //    ClsUser obj = new ClsUser();
        //    DataSet ds = new User.DataAccess.ClsUserDataService().User_GetByID(ID);

        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new User.DataAccess.ClsUserDataService(txn).User_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        public static void Lead_Save(string x)
        {
            try
            {
                Lead_Save(x, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Lead_Save()", "ClsList.cs");
            }
        }

        public static void Lead_Save(string x, IDbTransaction txn)
        {
            try
            {
                new ImportLeads.DataAccess.ClsImportLeadsDataService(txn).Lead_Save(x);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Lead_Save(IDbTransaction txn)", "ClsList.cs");
            }
        }

        #endregion
    }
}
