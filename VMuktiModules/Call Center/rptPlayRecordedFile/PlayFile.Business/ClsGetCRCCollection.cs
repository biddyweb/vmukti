using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFile.DataAccess;

namespace PlayFile.Business
{
    public class ClsGetCRCCollection : ClsBaseCollection<ClsCRCDetail>
    {
        public static ClsGetCRCCollection GetCRCCollection()
        {
            try
            {
                //ClsGetCampaignCollection obj = new ClsGetCampaignCollection();
                //obj.MapObjects("select DespositionName from Disposition where ID in(select DespositionListID from CampaignDespoList where CampaignID in(select ID from Campaign where Name=TestCampaign) )");
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ClsGetCRCCollection GetCRCCollection(string Campaign)
        {
            try
            {
                ClsGetCRCCollection obj = new ClsGetCRCCollection();
                obj.MapObjects(new ClsCRCCollection().CRCCollection(Campaign));
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
