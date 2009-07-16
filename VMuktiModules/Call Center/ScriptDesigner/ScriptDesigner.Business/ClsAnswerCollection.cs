using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsAnswerCollection : ClsBaseCollection<ClsAnswer>
    {
        public static ClsAnswerCollection GetAll(int QueID)
        {
            ClsAnswerCollection obj = new ClsAnswerCollection();            
            obj.MapObjects(new ClsQuestionAnsDataService().Options_GetAll(QueID));
            return obj;
        }

    }
}
