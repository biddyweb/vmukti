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
using ScriptDesigner.Common;
using ScriptDesigner.DataAccess;
using VMuktiAPI;
namespace ScriptDesigner.Business
{
    public class ClsDisposition : ClsBaseObject
    {
        #region Fields

        private Int64  _ID = ScriptDesigner.Common.ClsConstants.NullInt;
        private string _DespositionName = ScriptDesigner.Common.ClsConstants.NullString;
	    private bool _IsDeleted = false;
	    private bool _IsActive =false;

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
        
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                DespositionName = GetString(row, "DespositionName");
                //IsDeleted = GetBool(row, "IsDeleted");
                //IsActive = GetBool(row, "IsActive");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsDisposition.cs");
                return false;
            }
            
        }

        #endregion 
    }
}
