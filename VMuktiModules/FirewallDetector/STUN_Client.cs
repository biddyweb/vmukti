using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace VMukti.StunFireWallDetector
{
    public class STUN_Client
    {
        // Methods
        public static bool connectNet()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            if (reader.ReadToEnd().Length < 1)
            {
                return false;
            }
            return true;
        }

        private static STUN_Message DoTransaction(STUN_Message request, Socket socket, IPEndPoint remoteEndPoint)
        {
            byte[] buffer = request.ToByteData();
            DateTime now = DateTime.Now;
            while (now.AddSeconds(2.0) > DateTime.Now)
            {
                try
                {
                    socket.SendTo(buffer, remoteEndPoint);
                  //  MessageBox.Show(string.Concat(new object[] { "My local IpAddress is :", IPAddress.Parse(((IPEndPoint)socket.LocalEndPoint).Address.ToString()), "  I am connected on port number ", ((IPEndPoint)socket.LocalEndPoint).Port.ToString() }));
                    if (socket.Poll(100, SelectMode.SelectRead))
                    {
                        byte[] buffer2 = new byte[0x200];
                        socket.Receive(buffer2);
                        STUN_Message message = new STUN_Message();
                        message.Parse(buffer2);
                        if (request.TransactionID.Equals(message.TransactionID))
                        {
                            return message;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        private void GetSharedSecret()
        {
        }

        public static STUN_Result Query(string host, int port, Socket socket)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
            if (port < 1)
            {
                throw new ArgumentException("Port value must be >= 1 !");
            }
            if (socket.ProtocolType != ProtocolType.Udp)
            {
                throw new ArgumentException("Socket must be UDP socket !");
            }
            IPEndPoint remoteEndPoint = null;
            try
            {
                remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            }
            catch (Exception)
            {
                try
                {
                    if (connectNet())
                    {
                        return new STUN_Result(STUN_NetType.HttpPort80, null);
                    }
                    return new STUN_Result(STUN_NetType.NoInternetSupport, null);
                }
                catch (Exception)
                {
                    return new STUN_Result(STUN_NetType.NoInternetSupport, null);
                }
            }
            socket.ReceiveTimeout = 0xbb8;
            socket.SendTimeout = 0xbb8;
            STUN_Message request = new STUN_Message();
            request.Type = STUN_MessageType.BindingRequest;
            STUN_Message message2 = DoTransaction(request, socket, remoteEndPoint);
            if (message2 == null)
            {
                return new STUN_Result(STUN_NetType.UdpBlocked, null);
            }
            STUN_Message message3 = new STUN_Message();
            message3.Type = STUN_MessageType.BindingRequest;
            message3.ChangeRequest = new STUN_t_ChangeRequest(true, true);
            if (socket.LocalEndPoint.Equals(message2.MappedAddress))
            {
                if (DoTransaction(message3, socket, remoteEndPoint) != null)
                {
                    return new STUN_Result(STUN_NetType.OpenInternet, message2.MappedAddress);
                }
                return new STUN_Result(STUN_NetType.SymmetricUdpFirewall, message2.MappedAddress);
            }
            if (DoTransaction(message3, socket, remoteEndPoint) != null)
            {
                return new STUN_Result(STUN_NetType.FullCone, message2.MappedAddress);
            }
            STUN_Message message5 = new STUN_Message();
            message5.Type = STUN_MessageType.BindingRequest;
            STUN_Message message6 = DoTransaction(message5, socket, message2.ChangedAddress);
            if (message6 == null)
            {
                return new STUN_Result(STUN_NetType.RestrictedCone, message2.MappedAddress);
                //throw new Exception("STUN Test I(II) dind't get resonse !");
            }
            if (!message6.MappedAddress.Equals(message2.MappedAddress))
            {
                return new STUN_Result(STUN_NetType.Symmetric, message2.MappedAddress);
            }
            STUN_Message message7 = new STUN_Message();
            message7.Type = STUN_MessageType.BindingRequest;
            message7.ChangeRequest = new STUN_t_ChangeRequest(false, true);
            if (DoTransaction(message7, socket, message2.ChangedAddress) != null)
            {
                return new STUN_Result(STUN_NetType.RestrictedCone, message2.MappedAddress);
            }
            return new STUN_Result(STUN_NetType.PortRestrictedCone, message2.MappedAddress);
        }
    }
}
