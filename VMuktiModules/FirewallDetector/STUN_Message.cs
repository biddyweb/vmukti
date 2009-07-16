using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace VMukti.StunFireWallDetector
{
    public class STUN_Message
    {
        // Fields
        private string m_Password = null;
        private IPEndPoint m_pChangedAddress = null;
        private STUN_t_ChangeRequest m_pChangeRequest = null;
        private STUN_t_ErrorCode m_pErrorCode = null;
        private IPEndPoint m_pMappedAddress = null;
        private IPEndPoint m_pReflectedFrom = null;
        private IPEndPoint m_pResponseAddress = null;
        private IPEndPoint m_pSourceAddress = null;
        private Guid m_pTransactionID = Guid.Empty;
        private string m_ServerName = null;
        private STUN_MessageType m_Type = STUN_MessageType.BindingRequest;
        private string m_UserName = null;

        // Methods
        public STUN_Message()
        {
            this.m_pTransactionID = Guid.NewGuid();
        }

        public void Parse(byte[] data)
        {
            if (data.Length < 20)
            {
                throw new ArgumentException("Invalid STUN message value !");
            }
            int sourceIndex = 0;
            int num2 = (data[sourceIndex++] << 8) | data[sourceIndex++];
            if (num2 == 0x111)
            {
                this.m_Type = STUN_MessageType.BindingErrorResponse;
            }
            else if (num2 == 1)
            {
                this.m_Type = STUN_MessageType.BindingRequest;
            }
            else if (num2 == 0x101)
            {
                this.m_Type = STUN_MessageType.BindingResponse;
            }
            else if (num2 == 0x112)
            {
                this.m_Type = STUN_MessageType.SharedSecretErrorResponse;
            }
            else if (num2 == 2)
            {
                this.m_Type = STUN_MessageType.SharedSecretRequest;
            }
            else
            {
                if (num2 != 0x102)
                {
                    throw new ArgumentException("Invalid STUN message type value !");
                }
                this.m_Type = STUN_MessageType.SharedSecretResponse;
            }
            int num3 = (data[sourceIndex++] << 8) | data[sourceIndex++];
            byte[] destinationArray = new byte[0x10];
            Array.Copy(data, sourceIndex, destinationArray, 0, 0x10);
            this.m_pTransactionID = new Guid(destinationArray);
            sourceIndex += 0x10;
            while ((sourceIndex - 20) < num3)
            {
                this.ParseAttribute(data, ref sourceIndex);
            }
        }

        private void ParseAttribute(byte[] data, ref int offset)
        {
            AttributeType type = ((AttributeType)(data[offset++] << 8)) | ((AttributeType)data[offset++]);
            int count = (data[offset++] << 8) | data[offset++];
            if (type == AttributeType.MappedAddress)
            {
                this.m_pMappedAddress = this.ParseEndPoint(data, ref offset);
            }
            else if (type == AttributeType.ResponseAddress)
            {
                this.m_pResponseAddress = this.ParseEndPoint(data, ref offset);
            }
            else if (type == AttributeType.ChangeRequest)
            {
                offset += 3;
                this.m_pChangeRequest = new STUN_t_ChangeRequest((data[offset] & 4) != 0, (data[offset] & 2) != 0);
                offset++;
            }
            else if (type == AttributeType.SourceAddress)
            {
                this.m_pSourceAddress = this.ParseEndPoint(data, ref offset);
            }
            else if (type == AttributeType.ChangedAddress)
            {
                this.m_pChangedAddress = this.ParseEndPoint(data, ref offset);
            }
            else if (type == AttributeType.Username)
            {
                this.m_UserName = Encoding.Default.GetString(data, offset, count);
                offset += count;
            }
            else if (type == AttributeType.Password)
            {
                this.m_Password = Encoding.Default.GetString(data, offset, count);
                offset += count;
            }
            else if (type == AttributeType.MessageIntegrity)
            {
                offset += count;
            }
            else if (type == AttributeType.ErrorCode)
            {
                int code = ((data[offset + 2] & 7) * 100) + (data[offset + 3] & 0xff);
                this.m_pErrorCode = new STUN_t_ErrorCode(code, Encoding.Default.GetString(data, offset + 4, count - 4));
                offset += count;
            }
            else if (type == AttributeType.UnknownAttribute)
            {
                offset += count;
            }
            else if (type == AttributeType.ReflectedFrom)
            {
                this.m_pReflectedFrom = this.ParseEndPoint(data, ref offset);
            }
            else if (type == AttributeType.ServerName)
            {
                this.m_ServerName = Encoding.Default.GetString(data, offset, count);
                offset += count;
            }
            else
            {
                offset += count;
            }
        }

        private IPEndPoint ParseEndPoint(byte[] data, ref int offset)
        {
            offset++;
            offset++;
            return new IPEndPoint(new IPAddress(new byte[] { data[offset++], data[offset++], data[offset++], data[offset++] }), (data[offset++] << 8) | data[offset++]);
        }

        private void StoreEndPoint(AttributeType type, IPEndPoint endPoint, byte[] message, ref int offset)
        {
            message[offset++] = (byte)(((int)type) >> 8);
            message[offset++] = (byte)(type & ((AttributeType)0xff));
            message[offset++] = 0;
            message[offset++] = 8;
            message[offset++] = 0;
            message[offset++] = 1;
            message[offset++] = (byte)(endPoint.Port >> 8);
            message[offset++] = (byte)(endPoint.Port & 0xff);
            byte[] addressBytes = endPoint.Address.GetAddressBytes();
            message[offset++] = addressBytes[0];
            message[offset++] = addressBytes[0];
            message[offset++] = addressBytes[0];
            message[offset++] = addressBytes[0];
        }

        public byte[] ToByteData()
        {
            byte[] destinationArray = new byte[0x200];
            int destinationIndex = 0;
            destinationArray[destinationIndex++] = (byte)(((int)this.Type) >> 8);
            destinationArray[destinationIndex++] = (byte)(this.Type & ((STUN_MessageType)0xff));
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0;
            Array.Copy(this.m_pTransactionID.ToByteArray(), 0, destinationArray, destinationIndex, 0x10);
            destinationIndex += 0x10;
            if (this.MappedAddress != null)
            {
                this.StoreEndPoint(AttributeType.MappedAddress, this.MappedAddress, destinationArray, ref destinationIndex);
            }
            else if (this.ResponseAddress != null)
            {
                this.StoreEndPoint(AttributeType.ResponseAddress, this.ResponseAddress, destinationArray, ref destinationIndex);
            }
            else if (this.ChangeRequest != null)
            {
                destinationArray[destinationIndex++] = 0;
                destinationArray[destinationIndex++] = 3;
                destinationArray[destinationIndex++] = 0;
                destinationArray[destinationIndex++] = 4;
                destinationArray[destinationIndex++] = 0;
                destinationArray[destinationIndex++] = 0;
                destinationArray[destinationIndex++] = 0;
                destinationArray[destinationIndex++] = (byte)((Convert.ToInt32(this.ChangeRequest.ChangeIP) << 2) | (Convert.ToInt32(this.ChangeRequest.ChangePort) << 1));
            }
            else if (this.SourceAddress != null)
            {
                this.StoreEndPoint(AttributeType.SourceAddress, this.SourceAddress, destinationArray, ref destinationIndex);
            }
            else if (this.ChangedAddress != null)
            {
                this.StoreEndPoint(AttributeType.ChangedAddress, this.ChangedAddress, destinationArray, ref destinationIndex);
            }
            else
            {
                byte[] bytes;
                if (this.UserName != null)
                {
                    bytes = Encoding.ASCII.GetBytes(this.UserName);
                    destinationArray[destinationIndex++] = 0;
                    destinationArray[destinationIndex++] = 6;
                    destinationArray[destinationIndex++] = (byte)(bytes.Length >> 8);
                    destinationArray[destinationIndex++] = (byte)(bytes.Length & 0xff);
                    Array.Copy(bytes, 0, destinationArray, destinationIndex, bytes.Length);
                    destinationIndex += bytes.Length;
                }
                else if (this.Password != null)
                {
                    bytes = Encoding.ASCII.GetBytes(this.UserName);
                    destinationArray[destinationIndex++] = 0;
                    destinationArray[destinationIndex++] = 7;
                    destinationArray[destinationIndex++] = (byte)(bytes.Length >> 8);
                    destinationArray[destinationIndex++] = (byte)(bytes.Length & 0xff);
                    Array.Copy(bytes, 0, destinationArray, destinationIndex, bytes.Length);
                    destinationIndex += bytes.Length;
                }
                else if (this.ErrorCode != null)
                {
                    byte[] sourceArray = Encoding.ASCII.GetBytes(this.ErrorCode.ReasonText);
                    destinationArray[destinationIndex++] = 0;
                    destinationArray[destinationIndex++] = 9;
                    destinationArray[destinationIndex++] = 0;
                    destinationArray[destinationIndex++] = (byte)(4 + sourceArray.Length);
                    destinationArray[destinationIndex++] = 0;
                    destinationArray[destinationIndex++] = 0;
                    destinationArray[destinationIndex++] = (byte)Math.Floor((double)(this.ErrorCode.Code / 100));
                    destinationArray[destinationIndex++] = (byte)(this.ErrorCode.Code & 0xff);
                    Array.Copy(sourceArray, destinationArray, sourceArray.Length);
                    destinationIndex += sourceArray.Length;
                }
                else if (this.ReflectedFrom != null)
                {
                    this.StoreEndPoint(AttributeType.ReflectedFrom, this.ReflectedFrom, destinationArray, ref destinationIndex);
                }
            }
            destinationArray[2] = (byte)((destinationIndex - 20) >> 8);
            destinationArray[3] = (byte)((destinationIndex - 20) & 0xff);
            byte[] buffer4 = new byte[destinationIndex];
            Array.Copy(destinationArray, buffer4, buffer4.Length);
            return buffer4;
        }

        // Properties
        public IPEndPoint ChangedAddress
        {
            get
            {
                return this.m_pChangedAddress;
            }
            set
            {
                this.m_pChangedAddress = value;
            }
        }

        public STUN_t_ChangeRequest ChangeRequest
        {
            get
            {
                return this.m_pChangeRequest;
            }
            set
            {
                this.m_pChangeRequest = value;
            }
        }

        public STUN_t_ErrorCode ErrorCode
        {
            get
            {
                return this.m_pErrorCode;
            }
            set
            {
                this.m_pErrorCode = value;
            }
        }

        public IPEndPoint MappedAddress
        {
            get
            {
                return this.m_pMappedAddress;
            }
            set
            {
                this.m_pMappedAddress = value;
            }
        }

        public string Password
        {
            get
            {
                return this.m_Password;
            }
            set
            {
                this.m_Password = value;
            }
        }

        public IPEndPoint ReflectedFrom
        {
            get
            {
                return this.m_pReflectedFrom;
            }
            set
            {
                this.m_pReflectedFrom = value;
            }
        }

        public IPEndPoint ResponseAddress
        {
            get
            {
                return this.m_pResponseAddress;
            }
            set
            {
                this.m_pResponseAddress = value;
            }
        }

        public string ServerName
        {
            get
            {
                return this.m_ServerName;
            }
            set
            {
                this.m_ServerName = value;
            }
        }

        public IPEndPoint SourceAddress
        {
            get
            {
                return this.m_pSourceAddress;
            }
            set
            {
                this.m_pSourceAddress = value;
            }
        }

        public Guid TransactionID
        {
            get
            {
                return this.m_pTransactionID;
            }
        }

        public STUN_MessageType Type
        {
            get
            {
                return this.m_Type;
            }
            set
            {
                this.m_Type = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.m_UserName;
            }
            set
            {
                this.m_UserName = value;
            }
        }

        // Nested Types
        private enum AttributeType
        {
            ChangedAddress = 5,
            ChangeRequest = 3,
            ErrorCode = 9,
            MappedAddress = 1,
            MessageIntegrity = 8,
            Password = 7,
            ReflectedFrom = 11,
            ResponseAddress = 2,
            ServerName = 0x8022,
            SourceAddress = 4,
            UnknownAttribute = 10,
            Username = 6,
            XorMappedAddress = 0x8020,
            XorOnly = 0x21
        }

        private enum IPFamily
        {
            IPv4 = 1,
            IPv6 = 2
        }
    }
}
