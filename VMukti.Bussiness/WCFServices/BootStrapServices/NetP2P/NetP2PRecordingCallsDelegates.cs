using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;
using VMukti.Business.CommonDataContracts;

namespace VMukti.Business.WCFServices.BootStrapServices.NetP2P
{
    public class NetP2PRecordingCallsDelegates : INetP2PRecordingCallsService
    {
        public delegate void DelsvcNetP2PRecordingServiceJoin();
        public delegate void DelsvcNetP2PSendRecordedFile(string strFileName, byte[] bytearr);
        public delegate void DelsvcNetP2PReceiveRecordedFile(string strFileName, byte[] bytearr, int intSig);
        public delegate void DelsvcNetP2PRecordingUnJoin();
       

        public event DelsvcNetP2PRecordingServiceJoin EntsvcNetP2PRecordingServiceJoin;
        public event DelsvcNetP2PSendRecordedFile EntsvcNetP2PSendRecordedFile;
        public event DelsvcNetP2PReceiveRecordedFile EntsvcNetP2PReceiveRecordedFile;
        public event DelsvcNetP2PRecordingUnJoin EntsvcNetP2PUnJoin;


        public void svcNetP2PRecordingServiceJoin()
        {
            if (EntsvcNetP2PRecordingServiceJoin != null)
            {
                EntsvcNetP2PRecordingServiceJoin();
            }
        }

        public void svcNetP2PSendRecordedFile(string strFileName, byte[] bytearr)
        {
            if (EntsvcNetP2PSendRecordedFile != null)
            {
                EntsvcNetP2PSendRecordedFile(strFileName, bytearr);
            }
        }

        public void svcNetP2PReceiveRecordedFile(string strFileName, byte[] bytearr, int intSig)
        {
            if (EntsvcNetP2PReceiveRecordedFile != null)
            {
                EntsvcNetP2PReceiveRecordedFile(strFileName, bytearr, intSig);
            }
        }

        public void svcNetP2PRecordingUnJoin()
        {
            if (EntsvcNetP2PUnJoin != null)
            {
                EntsvcNetP2PUnJoin();
            }
        }
    }
}
