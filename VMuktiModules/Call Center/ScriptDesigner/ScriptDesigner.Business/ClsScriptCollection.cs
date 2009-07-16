using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsScriptCollection : ClsBaseCollection<ClsScript>
    {
        public static ClsScriptCollection GetAll()
        {
            ClsScriptCollection obj = new ClsScriptCollection();
            obj.MapObjects(new ClsQuestionAnsDataService().Script_GetAll());
            return obj;
        }

        public static string GetScriptType(string ScriptName)
        {
            return (new ClsQuestionAnsDataService().GetScriptType(ScriptName));
        }

    }
}
