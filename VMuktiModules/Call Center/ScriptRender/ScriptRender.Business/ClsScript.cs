using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using ScriptRender.DataAccess;
namespace ScriptRender.Business
{
    public class ClsScript
    {
        public static string ClsGetScriptURL(int scriptID)
        {
            ClsLeadDataService leadDataService = new ClsLeadDataService();
            return leadDataService.getWebScriptURL(scriptID);
        }
    }
}
