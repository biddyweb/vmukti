using System;
using CRM.DataAccess;

namespace CRM.Business
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
