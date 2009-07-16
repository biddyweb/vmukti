using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialer_AutoProgressive.Business.Services.MessageContract;
using VMuktiAPI;

namespace Dialer_AutoProgressive.Business.Services.RecordedFileServices
{
    public class NetP2PBootStrapRecordedFileDelegate : INetP2PBootStrapRecordedFileService 
    {
        public delegate void delsvcRecordedFileJoin(clsMessageContract mcRFJoin);
        public delegate void delsvcRecordedFileUnJoin(clsMessageContract mcRFUnJoin);
        public delegate void delsvcSendRecordedFiles(clsMessageContract mcSendRecordedFiles);

        public event delsvcRecordedFileJoin EntsvcRecordedFileJoin;
        public event delsvcRecordedFileUnJoin EntsvcRecordedFileUnJoin;
        public event delsvcSendRecordedFiles EntsvcSendRecordedFiles;


        #region INetP2PBootStrapRecordedFileService Members

        public void svcRecordedFileJoin(clsMessageContract mcRFJoin)
        {
            try
            {
                if (EntsvcRecordedFileJoin != null)
                {
                    EntsvcRecordedFileJoin(mcRFJoin);
                }
            }
            catch(Exception ex)
            {
                ClsException.WriteToLogFile("svcRecordedFileJoin() :- NetP2PBootStrapRecordedFileDelegate.cs" + ex.Message);
            }
        }

        public void svcRecordedFileUnJoin(clsMessageContract mcRFUnJoin)
        {
            try
            {
                if (EntsvcRecordedFileUnJoin != null)
                {
                    EntsvcRecordedFileUnJoin(mcRFUnJoin);
                }
            }
            catch (Exception ex)
            {
                ClsException.WriteToLogFile("svcRecordedFileUnJoin() :- NetP2PBootStrapRecordedFileDelegate.cs" + ex.Message);
            }
        }

        public void svcSendRecordedFiles(clsMessageContract mcSendRecordedFiles)
        {
            try
            {
                if (EntsvcSendRecordedFiles != null)
                {
                    EntsvcSendRecordedFiles(mcSendRecordedFiles);
                }
            }
            catch (Exception ex)
            {
                ClsException.WriteToLogFile("svcSendRecordedFiles() :- NetP2PBootStrapRecordedFileDelegate.cs" + ex.Message);
            }
        }

        #endregion
    }
}
