using System;
using CRM.DataAccess;

namespace CRM.Business
{
    public class ClsQuestionCollectionR : ClsBaseCollection<ClsQuestionR>
    {
        public static ClsQuestionCollectionR GetAll(int ScriptID)
        {
            ClsQuestionCollectionR obj = new ClsQuestionCollectionR();
            obj.MapObjects(new ClsDynamicScriptDataService().Questions_GetAll(ScriptID));
            return obj;
        }

    }
}
