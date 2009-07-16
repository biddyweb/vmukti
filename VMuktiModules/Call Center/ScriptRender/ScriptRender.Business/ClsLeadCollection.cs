using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using ScriptRender.DataAccess;

namespace ScriptRender.Business
{
    public class ClsLeadCollection : ClsBaseCollection<ClsLead>
    {
        public static ClsLeadCollection GetAll(int LeadID, string LeadFieldName, string LeadFormat)
        {
            ClsLeadCollection obj = new ClsLeadCollection();
            obj.MapObjects(new ClsLeadDataService().Lead_GetAll(LeadID, LeadFieldName, LeadFormat));
            return obj;
        }



    }
}
