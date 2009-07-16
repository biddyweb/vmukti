using System;
using System.Data;
using YatePBX.DataAccess;

namespace YatePBX.Business
{
    public class ClsCredential
    {
        ClsUserDataService svcDataService = new ClsUserDataService();
        public string[] FncProviderInformation()
        {
            try
            {
                DataSet ds = svcDataService.SIP_GetAll();
                string[] strTemp = new string[3];

                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() == "ProviderUserName")
                        {
                            strTemp[0] = ds.Tables[0].Rows[i][1].ToString();
                        }
                        else if (ds.Tables[0].Rows[i][0].ToString() == "ProviderPassword")
                        {
                            strTemp[1] = ds.Tables[0].Rows[i][1].ToString();
                        }
                        else if (ds.Tables[0].Rows[i][0].ToString() == "ProvideDomain")
                        {
                            strTemp[2] = ds.Tables[0].Rows[i][1].ToString();
                        }
                    }
                    return strTemp;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
