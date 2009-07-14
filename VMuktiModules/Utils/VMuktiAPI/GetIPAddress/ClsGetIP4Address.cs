using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace VMuktiAPI.GetIPAddress
{
    public static class ClsGetIP4Address
    {
        public static string GetIP4Address()
        {
            try
            {
                return VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetIP4Address()", "ClsGetIP4Address.cs");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                return string.Empty;

            }
        }

    }
}
