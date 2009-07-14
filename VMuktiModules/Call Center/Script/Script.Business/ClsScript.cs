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
//using System.Xml.Linq;
using System.Text;
using Script.Common;
using System.Data;
using Script.DataAccess;


namespace Script.Business
{
    public class ClsScript:ClsBaseObject
    {
       #region Fields

        private int _ID=ClsConstants.NullInt;
        private string _ScriptURL = ClsConstants.NullString;
        private string _ScriptName = ClsConstants.NullString;
        private int _ScriptTypeID = ClsConstants.NullInt;
        private string _ScriptType = ClsConstants.NullString;
        private bool _IsActive = false;
        private bool _IsDeleted = false;
        private DateTime _CreatedDate = ClsConstants.NullDateTime;
        private int _CreatedBy = ClsConstants.NullInt;
        private DateTime _ModifiedDate = ClsConstants.NullDateTime;
        private int _ModifiedBy = ClsConstants.NullInt;

        #endregion 


        #region properties

        public int ID
        { get{return _ID;}
            set{_ID=value;}
        }

        public string ScriptURL
        {
            get { return _ScriptURL; }
            set { _ScriptURL = value; }
        }
        public string ScriptName
        {
            get { return _ScriptName; }
            set { _ScriptName = value; }
        }

        public int ScriptTypeID
        {
            get { return _ScriptTypeID; }
            set { _ScriptTypeID = value; }
        }

        public string Scripttype
        {
            get { return _ScriptType; }
            set { _ScriptType = value; }
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


         public override bool MapData(DataRow row)
        {
            ID = GetInt(row,"ID");
            ScriptURL = GetString(row, "ScriptURL");
            ScriptName = GetString(row, "ScriptName");
            ScriptTypeID = GetInt(row, "ScriptTypeID");
            Scripttype = GetString(row, "Scripttype");
            IsDeleted = GetBool(row, "IsDeleted");
            IsActive = GetBool(row, "IsActive");
            CreatedDate = GetDateTime(row, "CreatedDate");
            CreatedBy = GetInt(row, "CreatedBy");
            ModifiedDate = GetDateTime(row, "ModifiedDate");
            ModifiedBy = GetInt(row, "ModifiedBy");
            
            
            return base.MapData(row);
        }
          public static ClsScript GetByCtlScriptID(int ID)
        {
            ClsScript obj = new ClsScript();
            DataSet ds = new Script.DataAccess.ClsScriptDataService().Script_GetByID(ID);
            if (!obj.MapData(ds)) obj = null;
            return obj;
        }
          public static void Delete(int ID)
          {
              Delete(ID, null);
          }

          public static void Delete(int ID, IDbTransaction txn)
          {
              new Script.DataAccess.ClsScriptDataService(txn).Script_Delete(ID);
          }

          public void Delete()
          {
              Delete(ID);
          }

          public void Delete(IDbTransaction txn)
          {
              Delete(ID, txn);
          }
        
          public int Save()
          {
              return (Save(null));
          }

          public int Save(IDbTransaction txn)
          {
              return (new Script.DataAccess.ClsScriptDataService(txn).Script_Save(ref _ID, _ScriptURL, _ScriptName, _ScriptTypeID,_IsActive,_CreatedBy));
          }




        
    }
}
#endregion