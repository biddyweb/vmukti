using System;
using CRM.Common;
using System.Data;
using CRM.DataAccess;

namespace CRM.Business
{
    public class ClsLead : ClsBaseObject
    {
        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _PropertyValue = ClsConstants.NullString;
        
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string PropertyValue
        {
            get { return _PropertyValue; }
            set { _PropertyValue = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            PropertyValue = GetString(row, "PropertyValue");
            
            return base.MapData(row);
        }

        //public void Save(Int64 LeadId, Int64 LeadFieldId, string Value)
        //{
        //    Save(null, LeadId, LeadFieldId, Value);
        //}

        //public void Save(IDbTransaction txn, Int64 LeadId, Int64 LeadFieldId, string Value)
        //{
        //    new CRM.DataAccess.ClsDynamicScriptDataService(txn).Lead_Save(LeadId, LeadFieldId, Value);
        //}

        public static int UpdateLeadInfo(int LeadID, int LeadFieldID, string LeadPropertyValue)
        {
            return (new ClsDynamicScriptDataService().UpdateLeadInfo(LeadID, LeadFieldID, LeadPropertyValue));
        }

        public static int UpdateLeadInfo(int LeadID, string StateName, string ZipCode)
        {
            return (new ClsDynamicScriptDataService().UpdateLeadInfo(LeadID, StateName, ZipCode));
        }

        #endregion 
    }
}
