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
using CRMContainer.Common;
using System.Text;

namespace CRMContainer.Business
{
    public class ClsCRMContainer : ClsBaseObject
    {
        //public static StringBuilder sb1;
        #region Fields

        private Int64 _ID = ClsConstants.NullInt;
        private Int64 _LeadID = ClsConstants.NullInt;
        private Int64 _VendorLeadID = ClsConstants.NullInt;
        private string _FirstName = ClsConstants.NullString;
        private string _LastName = ClsConstants.NullString;
        private string _Address = ClsConstants.NullString;
        private string _City = ClsConstants.NullString;
        private string _State = ClsConstants.NullString;
        private string _Event = ClsConstants.NullString;
        private string _Control = ClsConstants.NullString;
        private string _Zip = ClsConstants.NullString;
        private Int64 _Work = ClsConstants.NullInt;
        private Int64 _ProgramCode = ClsConstants.NullInt;
        private string _Email = ClsConstants.NullString;
        private string _Site = ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public Int64 LeadID
        {
            get { return _LeadID; }
            set { _LeadID = value; }
        }
        public Int64 VendorLeadID
        {
            get { return _VendorLeadID; }
            set { _VendorLeadID = value; }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }

        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }

        }
        public string Control
        {
            get { return _Control; }
            set { _Control = value; }

        }
        public string Event
        {
            get { return _Event; }
            set { _Event = value; }

        }
        public Int64 Work
        {
            get { return _Work; }
            set { _Work = value; }

        }
        public Int64 ProgramCode
        {
            get { return _ProgramCode; }
            set { _ProgramCode = value; }
        }
        public string Site
        {
            get { return _Site; }
            set { _Site = value; }

        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }

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
            ID = GetInt(row,"ID");
            LeadID = GetInt(row, "LeadID");
            VendorLeadID = GetInt(row, "VendorLeadID");
            FirstName = GetString(row, "FirstName");
            LastName = GetString(row, "LastName");
            Address = GetString(row, "Address");
            City = GetString(row, "City");
            State = GetString(row, "State");
            Zip = GetString(row, "Zip");
            Control = GetString(row, "Control");
            Event = GetString(row, "Event");
            Work = GetInt(row, "Work");
            ProgramCode = GetInt(row, "ProgramCode");
            Site = GetString(row, "Site");
            Email = GetString(row, "Email");
            
            return base.MapData(row);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "MapData()", "ClsCRMContainer.cs");
                return false;
            }
        }
        //public static ClsScriptRender GetByScriptID(Int64 LeadID)
        //{
        //    ClsScriptRender obj = new ClsScriptRender();
        //    DataSet ds = new ScriptRender.DataAccess.ClsScriptRenderDataService().Script_GetByID(LeadID);
        //    if (!obj.MapData(ds)) obj = null;
        //    return obj;
        //}

        //This function calls the function of the CRMContainer.DataAccess for getting
        //the zip name from the database.
        public static string GetZipName(Int64 CampaignID)
        {
            string str = "";
            try
            {
                //Calling the function of the CRMContainer.DataAccess.
                str = new CRMContainer.DataAccess.ClsCRMContainerDataService().File_GetByID(CampaignID);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "GetZipName()", "ClsCRMContainer.cs");
                return null;
            }
            return str;
        }
            
        //public static DataSet GetByScriptID1(Int64 LeadID)
        //{
        //    ClsScriptRender obj = new ClsScriptRender();
        //    DataSet ds = new ScriptRender.DataAccess.ClsScriptRenderDataService().Script_GetByID(LeadID);
        //    //if (!obj.MapData(ds)) obj = null;
        //    return ds;
        //}
        #endregion 
    }
}
