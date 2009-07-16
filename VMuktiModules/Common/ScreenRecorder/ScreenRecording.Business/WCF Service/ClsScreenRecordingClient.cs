using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VMuktiService;
using VMuktiAPI;

namespace ScreenRecording.Business
{

    public class ClsScreenRecordingClient
    {
        public static IHTTPScreenRecordingService chHTTPScreenRecordingChannel;
        public static BasicHttpClient bhcHttpScreenRecordingService = new BasicHttpClient();

        public static bool OpenScreenRecordingClient()
        {
            try
            {
                chHTTPScreenRecordingChannel = (IHTTPScreenRecordingService)bhcHttpScreenRecordingService.OpenClient<IHTTPScreenRecordingService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/ScreenRecording");
                chHTTPScreenRecordingChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                return true;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "OpenScreenRecordingClient()--:--ClsScreenRecordingClient.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return false;

            }

        }

    }
}
