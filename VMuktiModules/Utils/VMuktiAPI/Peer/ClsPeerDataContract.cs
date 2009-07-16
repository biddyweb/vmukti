using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VMuktiAPI
{
    [DataContract]
    public enum PeerType
    {
        [EnumMember]
        NotDecided,

        [EnumMember]
        BootStrap,

        [EnumMember]
        SuperNode,

        [EnumMember]
        NodeWithNetP2P,

        [EnumMember]
        NodeWithHttp
    }

    [DataContract]
    public enum PortStatus
    {
        [EnumMember]
        NotDecided,

        [EnumMember]
        InBoundOpen,

        [EnumMember]
        OutBoundOpen,

        [EnumMember]
        InOutBoundOpen,

        [EnumMember]
        InOutBoundClosed
    }

    [DataContract]
    public enum AuthType
    {
        [EnumMember]
        NotDecided,

        [EnumMember]
        SQLAuthentication,

        [EnumMember]
        SIPAuthentication,
    }

    [DataContract]
    public class ClsPeerDataContract    
    {
        

        #region Fields

        private int  _ID = ClsConstants.NullInt;
	    private string _DisplayName = ClsConstants.NullString;
        private int _RoleID = ClsConstants.NullInt;
        private string _FName = ClsConstants.NullString;
        private string _LName = ClsConstants.NullString;
        private string _EMail = ClsConstants.NullString;
        private string _PassWord = ClsConstants.NullString;

        //Before Performance Rule.
        //private bool _IsActive = false;
        //After applying Rule.
        private bool _IsActive;

        private string _MeshID = ClsConstants.NullString;
        private string _Status = ClsConstants.NullString;
        
        private PeerType _CurrPeerType = PeerType.NotDecided;
        private AuthType _CurrAuthType = AuthType.NotDecided;
        private PortStatus _Port4000Status = PortStatus.NotDecided;
        private PortStatus _Port5060Status = PortStatus.NotDecided;
        private PortStatus _Port1433Status = PortStatus.NotDecided;


        private string _AuthServerIP = ClsConstants.NullString;
        private string _SuperNodeIP = ClsConstants.NullString;
        private List<string> _NodeIPs = new List<string>();
        
        #endregion 

        #region Properties

        [DataMember]
        public int ID
        {
            get{return _ID;}
            set
            {
                _ID = value;
                _MeshID = "VMukti" + _ID.ToString();
            }
        }

        [DataMember]
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value;}
        }

        [DataMember]
        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }

        public string FName
        {
            get { return _FName; }
            set { _FName = value; }
        }

        [DataMember]
        public string LName
        {
            get { return _LName; }
            set { _LName = value; }
        }

        [DataMember]
        public string EMail
        {
            get { return _EMail; }
            set { _EMail = value; }
        }

        [DataMember]
        public string PassWord
        {
            get { return _PassWord; }
            set { _PassWord = value; }
        }

        [DataMember]
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        [DataMember]
        public string MeshID
        {
            get { return _MeshID; }
            set { _MeshID = value; }
        }

        [DataMember]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        [DataMember]
        public PeerType CurrPeerType
        {
            get { return _CurrPeerType; }
            set { _CurrPeerType = value; }
        }

        [DataMember]
        public AuthType CurrAuthType
        {
            get { return _CurrAuthType; }
            set { _CurrAuthType = value; }
        }

        [DataMember]
        public PortStatus Port4000Status
        {
            get { return _Port4000Status; }
            set { _Port4000Status = value; }
        }

        [DataMember]
        public PortStatus Port5060Status
        {
            get { return _Port5060Status; }
            set { _Port5060Status = value; }
        }

        [DataMember]
        public PortStatus Port1433Status
        {
            get { return _Port1433Status; }
            set { _Port1433Status = value; }
        }

        [DataMember]
        public string AuthServerIP
        {
            get { return _AuthServerIP; }
            set { _AuthServerIP = value; }
        }

        [DataMember]
        public string SuperNodeIP
        {
            get { return _SuperNodeIP; }
            set { _SuperNodeIP = value; }
        }

        [DataMember]
        public List<string> NodeIPs
        {
            get { return _NodeIPs; }
            set { _NodeIPs = value; }
        }

        #endregion 
    }
}
