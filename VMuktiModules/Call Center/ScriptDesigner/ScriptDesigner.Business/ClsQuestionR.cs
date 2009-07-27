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
using ScriptDesigner.Common;
using System.Data;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsQuestionR : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _QuestionName = ClsConstants.NullString;
        private string _Category = ClsConstants.NullString;
        private string _QuestionText = ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string QuestionName
        {
            get { return _QuestionName; }
            set { _QuestionName = value;}
        }

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public string QuestionText
        {
            get { return _QuestionText; }
            set { _QuestionText = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            QuestionName = GetString(row, "QuestionName");
            Category = GetString(row, "Category");
            QuestionText = GetString(row, "QuestionText");
            
            return base.MapData(row);
        }

        //public static ClsQuestion GetByRoleID(int ID)
        //{
        //    ClsQuestion obj = new ClsQuestion();
        //    DataSet ds = new Role.DataAccess.ClsRoleDataService().Role_GetByID(ID);
        //    if (!obj.MapData(ds.Tables[0])) obj = null;
        //    return obj;
        //}

        //public static void Delete(int ID)
        //{
        //    Delete(ID, null);
        //}

        //public static void Delete(int ID, IDbTransaction txn)
        //{
        //    new Role.DataAccess.ClsRoleDataService(txn).Role_Delete(ID);
        //}

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        //public int Save()
        //{
        //    return(Save(null));
        //}

        //public int Save(IDbTransaction txn)
        //{
        //   return(new Role.DataAccess.ClsRoleDataService(txn).Role_Save(ref _ID,_QuestionName,_Category,_IsAdmin,_CreatedBy));
        //}

        #endregion 
    }
}
