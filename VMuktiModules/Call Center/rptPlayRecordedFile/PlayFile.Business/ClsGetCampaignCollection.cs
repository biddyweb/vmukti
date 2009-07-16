using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFile.DataAccess;

namespace PlayFile.Business
{
   public class ClsGetCampaignCollection : ClsBaseCollection<ClsCampaignDetails>
    {
    public static ClsGetCampaignCollection GetCampaignCollection()
        {
            try
            {
                ClsGetCampaignCollection obj = new ClsGetCampaignCollection();
                obj.MapObjects(new ClsCampaignCollection().CampaignCollection());
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }

}}
}
