using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ScriptRender.Business
{
    [ServiceContract]
    public interface IHTTPFileTransferService
    {
        [OperationContract(IsOneWay = true)]
        void svcHTTPFileTransferServiceJoin();

        [OperationContract(IsOneWay = true)]
        void svcHTTPFileTransferServiceUploadFile(RemoteFileInfo request);

        [OperationContract(IsOneWay = true)]
        void svcHTTPFileTransferServiceUploadFileToInstallationDirectory(RemoteFileInfo request);

        [OperationContract(IsOneWay = false)]
        RemoteFileInfo svcHTTPFileTransferServiceDownloadFile(DownloadRequest request);

    }

    public interface IHTTPFileTransferServiceChannel : IHTTPFileTransferService, IClientChannel
    {
    }

    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;

        [MessageBodyMember]
        public string FolderWhereFileIsStored;

    }
        
    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageHeader(MustUnderstand = true)]
        public string FolderNameToStore;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        

        public void Dispose()
        {
            // close stream when the contract instance is disposed. this ensures that stream is closed when file download is complete, since download procedure is handled by the client and the stream must be closed on server.
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }

    

}
