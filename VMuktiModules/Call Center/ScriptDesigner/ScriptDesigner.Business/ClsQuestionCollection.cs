using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsQuestionCollection : ClsBaseCollection<ClsQuestion>
    {
        public static ClsQuestionCollection GetAll(int ScriptID)
        {
            ClsQuestionCollection obj = new ClsQuestionCollection();
            obj.MapObjects(new ClsQuestionAnsDataService().Question_GetAll(ScriptID));
            return obj;
        }

    }
}
