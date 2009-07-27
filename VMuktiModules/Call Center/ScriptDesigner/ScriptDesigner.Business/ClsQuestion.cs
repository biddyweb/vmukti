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
    public class ClsQuestion : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _Header = ClsConstants.NullString;
        private string _Name = ClsConstants.NullString;
        private string _Description = ClsConstants.NullString;
        private string _Category = ClsConstants.NullString;
        private int _ScriptID = ClsConstants.NullInt;

        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public int ScriptID
        {
            get { return _ScriptID; }
            set { _ScriptID = value; }
        }

        
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            Header = GetString(row, "QuestionName");
            Description = GetString(row, "Description");
            Name = GetString(row, "QuestionText");
            Category = GetString(row, "Category");
            ScriptID = GetInt(row, "ScriptID");
            
            return base.MapData(row);
        }

        public static ClsQuestion GetByQueID(int QueID)
        {
            ClsQuestion obj = new ClsQuestion();
            DataSet ds = new ScriptDesigner.DataAccess.ClsQuestionAnsDataService().Question_GetByID(QueID);

            if (!obj.MapData(ds.Tables[0])) obj = null;
            return obj;
        }

        public static void Delete(int ID)
        {
            Delete(ID, null);
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Question_Delete(ID);
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
            return(Save(null));
        }

        public int Save(IDbTransaction txn)
        {
            return (new ScriptDesigner.DataAccess.ClsQuestionAnsDataService(txn).Question_Save(_ID, _Header, _Name, _Description, _Category, _ScriptID));
        }

        #endregion 
    }
}
