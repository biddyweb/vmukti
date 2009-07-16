using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
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
