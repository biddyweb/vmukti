using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Dialer_AutoProgressive.Business.Services.MessageContract;

namespace Dialer_AutoProgressive.Business.Services.RecordedFileServices
{
    [ServiceContract(CallbackContract = typeof(INetP2PBootStrapRecordedFileService))]
    public interface INetP2PBootStrapRecordedFileService
    {
        [OperationContract(IsOneWay = true)]
        void svcRecordedFileJoin(clsMessageContract mcRFJoin);

        [OperationContract(IsOneWay = true)]
        void svcRecordedFileUnJoin(clsMessageContract mcRFUnJoin);

        [OperationContract(IsOneWay = true)]
        void svcSendRecordedFiles(clsMessageContract mcSendRecordedFiles);
    }
    public interface INetP2PBootStrapRecordedFileChannel : IClientChannel, INetP2PBootStrapRecordedFileService
    {
    }
    
}
