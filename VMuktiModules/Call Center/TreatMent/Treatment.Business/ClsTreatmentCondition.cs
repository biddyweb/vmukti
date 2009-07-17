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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Treatment.Common;

namespace Treatment.Business
{
    public class ClsTreatmentCondition : ClsBaseObject
    {
        #region Fields

        private int _ID = ClsConstants.NullInt;
        public int _TreatmentID = ClsConstants.NullInt;
        private int _LeadFormatID = ClsConstants.NullInt;
        private string _FieldName = ClsConstants.NullString;
        private string _Operator = ClsConstants .NullString ;
        private string _DataTypes = ClsConstants.NullString;
        private string _FieldValues = ClsConstants.NullString;
        private string _LeadFormatName = ClsConstants.NullString;
        private string _Duration = ClsConstants.NullString;
        private string _Disposition = ClsConstants.NullString;


        #endregion

        #region Properties

        public string Duration
        {
            get { return _Duration; }
            set { _Duration = value; }
        }
        public string Disposition
        {
            get { return _Disposition; }
            set { _Disposition = value; }
        }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int TreatmentID
        {
            get { return _TreatmentID; }
            set { _TreatmentID = value; }
        }

        public int LeadFormatID
        {
            get { return _LeadFormatID; }
            set { _LeadFormatID = value; }
        }
        public string LeadFormatName
        {
            get { return _LeadFormatName; }
            set { _LeadFormatName = value; }
        }

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
        public string Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }
        public string DataType
        {
            get { return _DataTypes; }
            set { _DataTypes = value; }
        }
        public string FieldValues
        {
            get { return _FieldValues; }
            set { _FieldValues = value; }
        }

        #endregion
        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            FieldName = GetString(row, "FieldName");
            Operator = GetString(row, "Operator");
            FieldValues = GetString(row, "FieldValues");
            LeadFormatName = GetString(row, "LeadFormatName");
            return base.MapData(row);
        }

        public static ClsTreatmentCondition GetByTreatmentID(int ID)
        {
            ClsTreatmentCondition obj = new ClsTreatmentCondition();
            DataSet ds = new Treatment.DataAccess.ClsTreatmentConditionDataService().TreatmentCondition_GetByTreatmentID(ID);
            if (!obj.MapData(ds)) obj = null;
            return obj;
        }

        public static void Delete(int ID)
        {
            Delete(ID, null);
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            new Treatment.DataAccess.ClsTreatmentConditionDataService(txn).TreatmentCondition_Delete(ID);
        }

        public void Delete()
        {
            Delete(ID);
        }

        public void Delete(IDbTransaction txn)
        {
            Delete(ID, txn);
        }
        public void Save()
        {
            Save(null);
        }
        public void Save(IDbTransaction txn)
        {
            new Treatment.DataAccess.ClsTreatmentConditionDataService(txn).TreatmentCondition_Save(ref _ID, _TreatmentID,_LeadFormatName, _FieldName , _Operator , _DataTypes , _FieldValues);
        }
        public void SaveDisposition()
        {
            SaveDisposition(null);
        }
        public void SaveDisposition(IDbTransaction txn)
        {
            new Treatment.DataAccess.ClsTreatmentConditionDataService(txn).TreatmentCondition_SaveDisposition(ref _ID, _TreatmentID, _Duration,_Disposition);
        }
        #endregion
    }
}
