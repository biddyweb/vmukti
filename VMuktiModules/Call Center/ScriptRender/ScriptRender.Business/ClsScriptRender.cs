<<<<<<< HEAD:VMuktiModules/Call Center/ScriptRender/ScriptRender.Business/ClsScriptRender.cs
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
=======
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ScriptRender/ScriptRender.Business/ClsScriptRender.cs
using System;
using System.Data;
using ScriptRender.Common;

namespace ScriptRender.Business
{
    public class ClsScriptRender : ClsBaseObject
    {
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

        public override bool MapData(DataRow row)
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

        public static ClsScriptRender GetByScriptID(Int64 LeadID)
        {
            ClsScriptRender obj = new ClsScriptRender();
            DataSet ds = new ScriptRender.DataAccess.ClsScriptRenderDataService().Script_GetByID(LeadID);
            if (!obj.MapData(ds)) obj = null;
            return obj;
        }

        public static string GetZipName(Int64 CampaignID)
        {
            string str = new ScriptRender.DataAccess.ClsScriptRenderDataService().File_GetByID(CampaignID);
            return str;
        }

        //public static DataSet GetByScriptID1(Int64 LeadID)
        //{
        //    ClsScriptRender obj = new ClsScriptRender();
        //    DataSet ds = new ScriptRender.DataAccess.ClsScriptRenderDataService().Script_GetByID(LeadID);
        //    //if (!obj.MapData(ds)) obj = null;
        //    return ds;
        //}

        public static string Script_GetScriptType(long ScriptID)
        {
            try
            {
                return (new ScriptRender.DataAccess.ClsScriptRenderDataService().Script_GetScriptType(ScriptID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Script_GetScriptType", "ClsScriptRender.cs");
                return null;
            }
        }
        #endregion 
    }
}
