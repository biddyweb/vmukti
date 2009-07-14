using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenRecording.Business;
using VMuktiService;
using VMuktiAPI;
using System.Diagnostics;
using System.Windows;

namespace ScreenRecording.Presentation
{
    public class ScreenRecordingServer
    {
        object screenReocrderServer = null;
        static int SPort = 7000;
        static int HttpPort = 8000;
        Process prc = null;
        List<string> CurrentRecording = new List<string>();

        public ScreenRecordingServer()
        {
            screenReocrderServer = new clsHTTPScreenRecordingService();

            ((clsHTTPScreenRecordingService)screenReocrderServer).EntSvcjoin += new clsHTTPScreenRecordingService.delSvcJoin(ScreenRecordingServer_EntSvcjoin);
            ((clsHTTPScreenRecordingService)screenReocrderServer).EntSvcUnJoin += new clsHTTPScreenRecordingService.delSvcUnJoin(ScreenRecordingServer_EntSvcUnJoin);
            ((clsHTTPScreenRecordingService)screenReocrderServer).EntSvcStreamSuperNode += new clsHTTPScreenRecordingService.delStreamSuperNode(ScreenRecordingServer_EntSvcStreamSuperNode);
            ((clsHTTPScreenRecordingService)screenReocrderServer).EntReStream += new clsHTTPScreenRecordingService.delReStream(ScreenRecordingServer_EntReStream);
            ((clsHTTPScreenRecordingService)screenReocrderServer).EntStopRecording += new clsHTTPScreenRecordingService.delStopRecording(ScreenRecordingServer_EntStopRecording);


            BasicHttpServer bhsScreenRecordingServer = new BasicHttpServer(ref screenReocrderServer, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/ScreenRecording");
            bhsScreenRecordingServer.AddEndPoint<IHTTPScreenRecordingService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/ScreenRecording");
            bhsScreenRecordingServer.OpenServer();
        }

        void ScreenRecordingServer_EntStopRecording(string uName)
        {
            for (int i = 0; i < CurrentRecording.Count; i++)
            {
                if(CurrentRecording[i].Split('/')[0]==uName)
                {
                    Process myProcess = Process.GetProcessById(int.Parse(CurrentRecording[i].Split('/')[1]));
                    myProcess.Kill();
                }
            }
        }

        void ScreenRecordingServer_EntReStream(int Port, string uName)
        {
            try
            {
                
                string strCurrentPeerType = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString();
                prc = new Process();
                //prc.ProcessName = uName;
                prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\VideoExe\\Video.exe";
                prc.StartInfo.Arguments = uName + "\\" + strCurrentPeerType + "\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + "\\" + Port;
                prc.Start();
                string strNameID = uName + "//" + prc.Id.ToString();
                CurrentRecording.Add(strNameID);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "ScreenRecordingServer_EntReStream()--:--ScreenRecordingServer.cs--:--" + ex.Message + " :--:--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
            }
        }

        int ScreenRecordingServer_EntSvcStreamSuperNode()
        {
            try
            {
                // returning port to node
                SPort += 1;
                HttpPort += 1;

                return SPort;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ScreenRecordingServer_EntSvcStreamSuperNode()--:--ScreenRecordingServer.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return 0;
            }
        }

        void ScreenRecordingServer_EntSvcUnJoin(string uName)
        {
            try
            {
                
                if (prc.ProcessName == uName)
                {

                }
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ScreenRecordingServer_EntSvcUnJoin()--:--ScreenRecordingServer.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                //return 0;
            }
        }

        void ScreenRecordingServer_EntSvcjoin(string uName)
        {
            try
            {
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ScreenRecordingServer_EntSvcjoin()--:--ScreenRecordingServer.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }
        }
    }
}
