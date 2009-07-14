using System;
using ScriptDesigner.DataAccess;
using VMuktiAPI;

namespace ScriptDesigner.Business
{
    public class ClsCampaignCollection : ClsBaseCollection<ClsCampaign>
    {
        public static ClsCampaignCollection GetAll()
        {
            try
            {
                ClsCampaignCollection obj = new ClsCampaignCollection();
                obj.MapObjects(new ClsQuestionAnsDataService().Campaign_GetAll());
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAll", "ClsCampaignCollection.cs");
                return null;
            } 
        }

    }
}
