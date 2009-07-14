using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ScreenRecording.Presentation
{
    [ServiceContract]
    public interface IHTTPScreenRecordingService
    {

        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName);

        [OperationContract(IsOneWay = false)]
        int StreamSuperNode();

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void ReStream(int Port, string uName);

        [OperationContract(IsOneWay = true)]
        void StopRecording(string uName);

    }

    public interface IHTTPScreenRecordingChannel : IHTTPScreenRecordingService, IClientChannel
    {
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class clsHTTPScreenRecordingService : IHTTPScreenRecordingService
    {
        public delegate void delSvcJoin(string uName);
        public delegate int delStreamSuperNode();
        public delegate void delSvcUnJoin(string uName);
        public delegate void delReStream(int Port, string uName);
        public delegate void delStopRecording(string uName);

        public event delSvcJoin EntSvcjoin;
        public event delStreamSuperNode EntSvcStreamSuperNode;
        public event delSvcUnJoin EntSvcUnJoin;
        public event delReStream EntReStream;
        public event delStopRecording EntStopRecording;


        #region IHTTPScreenRecordingService Members

        public void svcJoin(string uName)
        {
            if (EntSvcjoin != null)
            {
                EntSvcjoin(uName);
            }
        }

        public int StreamSuperNode()
        {
            if (EntSvcStreamSuperNode != null)
            {
                return EntSvcStreamSuperNode();
            }
            else
            {
                return 0;
            }
        }

        public void ReStream(int Port, string uName)
        {
            if (EntReStream != null)
            {
                EntReStream(Port, uName);
            }
        }

        public void StopRecording(string uName)
        {
            if (EntStopRecording != null)
            {
                EntStopRecording(uName);
            }
        }


        public void svcUnJoin(string uName)
        {
            if (EntSvcUnJoin != null)
            {
                EntSvcUnJoin(uName);
            }
        }

        #endregion
    }
}
