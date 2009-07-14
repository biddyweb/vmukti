using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsLeadFormatCollection : ClsBaseCollection<ClsLeadFormat>
    {
        public static ClsLeadFormatCollection GetAll()
        {
            ClsLeadFormatCollection obj = new ClsLeadFormatCollection();            
            obj.MapObjects(new ClsQuestionAnsDataService().LeadFormat_GetAll());
            return obj;
        }

        public static ClsLeadFormatCollection GetAll(Int64 FormatID)
        {
            ClsLeadFormatCollection obj = new ClsLeadFormatCollection();
            obj.MapObjects(new ClsQuestionAnsDataService().LeadFields_GetAll(FormatID));
            return obj;
        }
    }
}
