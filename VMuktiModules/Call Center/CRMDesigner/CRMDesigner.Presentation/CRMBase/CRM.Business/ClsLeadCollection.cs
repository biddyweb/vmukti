using System;
using CRM.DataAccess;

namespace CRM.Business
{
    public class ClsLeadCollection : ClsBaseCollection<ClsLead>
    {
        public static ClsLeadCollection GetAll(int LeadID,int LeadFieldID)
        {
            ClsLeadCollection obj = new ClsLeadCollection();
            obj.MapObjects(new ClsDynamicScriptDataService().Lead_GetAll(LeadID, LeadFieldID));
            return obj;
        }


    }
}
