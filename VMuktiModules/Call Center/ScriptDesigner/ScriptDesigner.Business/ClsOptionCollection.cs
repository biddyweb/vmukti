using System;
using ScriptDesigner.DataAccess;

namespace ScriptDesigner.Business
{
    public class ClsOptionCollection : ClsBaseCollection<ClsOption>
    {
        public static ClsOptionCollection GetAll(int QuesID)
        {
            ClsOptionCollection obj = new ClsOptionCollection();            
            obj.MapObjects(new ClsDynamicScriptDataService().Options_GetAll(QuesID));
            return obj;
        }

    }
}
