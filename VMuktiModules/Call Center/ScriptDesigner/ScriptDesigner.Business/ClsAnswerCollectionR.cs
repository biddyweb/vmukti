using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsAnswerCollectionR : ClsBaseCollection<ClsAnswerR>
    {
        public static ClsAnswerCollectionR GetAll()
        {
            ClsAnswerCollectionR obj = new ClsAnswerCollectionR();            
            obj.MapObjects(new ClsDynamicScriptDataService().Answer_GetAll());
            return obj;
        }

    }
}
