/* VMukti 2.0 -- An Open Source Unified Communications Engine
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
using Treatment.Common;
using Treatment.DataAccess;

namespace Treatment.Business
{
    public class ClsTreatment : ClsBaseObject
    {
        #region Fields

        private int _ID = ClsConstants.NullInt;
        private string _TreatmentName = ClsConstants.NullString;
        private string _Description = ClsConstants.NullString;
        private string _Type = ClsConstants.NullString;
        private bool _IsInclude = false;
        private int _UserID = ClsConstants.NullInt;
        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string TreatmentName
        {
            get { return _TreatmentName; }
            set { _TreatmentName = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public bool IsInclude
        {
            get { return _IsInclude; }
            set { _IsInclude = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }


        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            TreatmentName = GetString(row, "TreatmentName");            
            Description = GetString(row, "Description");
            Type = GetString(row, "Type");
            IsInclude = GetBool(row, "IsInclude");
            UserID = GetInt(row, "ModifiedBy");
            return base.MapData(row);
        }

        public static void Delete_All(int TreatID)
        {
            Delete_All(TreatID, null);
        }
        public static void Delete_Disposition(int TreatID)
        {
            Delete_Disposition(TreatID, null);
        }
        public static void Delete_Disposition(int TreatID, IDbTransaction txn)
        {
            new Treatment.DataAccess.ClsTreatmentDataService(txn).Delete_Disposition(TreatID);
        }

        public static void Delete_All(int TreatID, IDbTransaction txn)
        {
            new Treatment.DataAccess.ClsTreatmentDataService(txn).Delete_All(TreatID);
        }
        public static DataSet GetCampaign(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).GetCampaign());

        }
        public static DataSet GetCampaignDisp(IDbTransaction txn,string CampaignName)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).GetCampaignDisp(CampaignName));
        }


        public static ClsTreatment GetByTreatmentID(int ID)
        {
            ClsTreatment obj = new ClsTreatment();
            DataSet ds = new Treatment.DataAccess.ClsTreatmentDataService().Treatment_GetByID(ID);
            if (!obj.MapData(ds)) obj = null;
            return obj;
        }

        public static DataSet GetFields()
        {
            return(GetFields(null));
        }

        public static DataSet GetFields(IDbTransaction txn)
        {
            return(new Treatment.DataAccess.ClsTreatmentDataService(txn).GetFields());
        }


        public static void Delete(int ID)
        {
            Delete(ID, null);
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            new Treatment.DataAccess.ClsTreatmentDataService(txn).Treatment_Delete(ID);
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
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).Treatment_Save(ref _ID, _TreatmentName, _Description, _Type , _IsInclude, _UserID));
        }

        public static DataSet GetFieldValues(string FieldName)
        {
            return (GetFieldValues(null, FieldName));
        }
        public static DataSet GetFieldValues(IDbTransaction txn, string FieldName)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).GetFieldValues(FieldName));
        }


        public static DataSet GetLeadFormat()
        {
            return (GetLeadFormat(null));
        }
        public static DataSet GetLeadFormat(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).GetLeadFormat());
        }

        public static DataSet GetAllFormat(int FormatID)
        {
            //throw new NotImplementedException();
            try
            {
                
              DataSet ds=(new  ClsTreatmentDataService().FormatField_GetAll(FormatID));
                return ds;
            }
            catch 
            {
                return null;
            }
            
        }

        public static DataSet Timezone_GetAll()
        {
            return (Timezone_GetAll(null));
        }
        public static DataSet Timezone_GetAll(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).Timezone_GetAll());
        }
        public static DataSet Country_GetAll()
        {
            return (Country_GetAll(null));
        }
        public static DataSet Country_GetAll(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).Country_GetAll());
        }

        public static DataSet AreaCode_GetAll()
        {
            return (AreaCode_GetAll(null));
        }
        public static DataSet AreaCode_GetAll(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).AreaCode_GetAll());
        }

        public static DataSet State_GetAll()
        {
            return (State_GetAll(null));
        }
        public static DataSet State_GetAll(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).State_GetAll());
        }

        public static DataSet GetOtherDetail()
        {
            return (GetOtherDetail(null));
        }
        public static DataSet GetOtherDetail(IDbTransaction txn)
        {
            return (new Treatment.DataAccess.ClsTreatmentDataService(txn).GetOtherDetail());
        }

        #endregion



        
    }
}
