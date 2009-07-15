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
* the Free Software Foundation, either version 2 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/
*/
using System;
using User.Common;
using System.Data;
using User.DataAccess;
using System.Text;
using VMuktiAPI;

namespace User.Business
{
    public class ClsRole : ClsBaseObject
        {
       
        #region Fields

        private int  _ID = User.Common.ClsConstants.NullInt;
	    private string _RoleName = User.Common.ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value;}
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {

                ID = GetInt(row, "ID");
                RoleName = GetString(row, "RoleName");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsRole.cs");
                return false;
            }
        }

        #endregion 
    }
}
