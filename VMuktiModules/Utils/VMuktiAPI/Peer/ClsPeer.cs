using System;
using System.Collections.Generic;

namespace VMuktiAPI
{
    public class ClsPeer    
    {

        #region Fields

        //private XmlDocument _xmlDoc = new XmlDocument();

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

        private long _CampaignID = ClsConstants.NullLong;
        private long _GroupID = ClsConstants.NullLong;
        private long _ActivityID = ClsConstants.NullLong;

        private string _CampaignName = ClsConstants.NullString;
        private string _GroupName = ClsConstants.NullString;
        private string _RoleName = ClsConstants.NullString;

        private DateTime _StartTime = ClsConstants.NullDateTime;
        private DateTime _EndTime = ClsConstants.NullDateTime;

        private string _CurrentMachineIP = ClsConstants.NullString;

        private long _ScriptID = ClsConstants.NullLong;
        private Dictionary<int, int> _ConferenceUser = new Dictionary<int, int>();

        #endregion 

        #region Properties

        public string CurrentMachineIP
        {
            get
            {
                _CurrentMachineIP = ConfData.GetKeyValue("_CurrentMachineIP");
                return _CurrentMachineIP;
            }
            set
            {
                _CurrentMachineIP = value;
                ConfData.UpdateKey("_CurrentMachineIP", _CurrentMachineIP);
            }
        }

        public long CampaignID
        {
            get
            {
                _CampaignID = long.Parse(ConfData.GetKeyValue("_CampaignID"));
                return _CampaignID;
            }
            set
            {
                _CampaignID = value;
                ConfData.UpdateKey("_CampaignID", _CampaignID.ToString());
            }
        }

        public long GroupID
        {
            get
            {
                _GroupID = long.Parse(ConfData.GetKeyValue("_GroupID"));
                return _GroupID;
            }
            set
            {
                _GroupID = value;
                ConfData.UpdateKey("_ActivityID", _GroupID.ToString());
            }
        }

        public long ActivityID
        {
            get
            {
                _ActivityID = long.Parse(ConfData.GetKeyValue("_ActivityID"));
                return _ActivityID;
            }
            set
            {
                _ActivityID = value;
                ConfData.UpdateKey("_ActivityID", _ActivityID.ToString());
            }
        }

        public string GroupName
        {
            get
            {
                _GroupName = ConfData.GetKeyValue("_GroupName");
                return _GroupName;
            }
            set
            {
                _GroupName = value;
                ConfData.UpdateKey("_GroupName", _GroupName);
            }
        }

        public string RoleName
        {
            get
            {
                _RoleName = ConfData.GetKeyValue("_RoleName");
                return _RoleName;
            }
            set
            {
                _RoleName = value;
                ConfData.UpdateKey("_RoleName", _GroupName);
            }
        }

        public string CampaignName
        {
            get
            {
                _CampaignName = ConfData.GetKeyValue("_CampaignName");
                return _CampaignName;
            }
            set
            {
                _CampaignName = value;
                ConfData.UpdateKey("_CampaignName", _CampaignName);
            }
        }
        public DateTime StartTime
        {
            get
            {
                _StartTime = Convert.ToDateTime(ConfData.GetKeyValue("_StartTime"));
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                ConfData.UpdateKey("_StartTime", _StartTime.ToString());
            }
        }

        public DateTime EndTime
        {
         get
            {
                _EndTime = Convert.ToDateTime(ConfData.GetKeyValue("_EndTime"));
                return _EndTime;
            }
            set
            {
                _EndTime = value;
                ConfData.UpdateKey("_EndTime", _EndTime.ToString());
            }
        }

        public int ID
        {
            get
            {
                _ID = int.Parse(ConfData.GetKeyValue("_ID"));
                return _ID;
            }
            set
            {
                _ID = value;
                ConfData.UpdateKey("_ID", value.ToString());

                _MeshID = "VMukti" + _ID.ToString();
                ConfData.UpdateKey("_MeshID", _MeshID);
            }
        }

        public string DisplayName
        {
            get 
            {
                _DisplayName = ConfData.GetKeyValue("_DisplayName");
                return _DisplayName; 
            }
            set 
            { 
                _DisplayName = value;
                ConfData.UpdateKey("_DisplayName", value);
            }
        }

        public int RoleID
        {
            get 
            {
                _RoleID = int.Parse(ConfData.GetKeyValue("_RoleID"));
                return _RoleID; 
            }
            set 
            { 
                _RoleID = value;
                ConfData.UpdateKey("_RoleID", value.ToString());
            }
        }

        public string FName
        {
            get 
            {
                _FName = ConfData.GetKeyValue("_FName");
                return _FName; 
            }
            set 
            { 
                _FName = value;
                ConfData.UpdateKey("_FName", value);
            }
        }

        public string LName
        {
            get 
            {
                _LName = ConfData.GetKeyValue("_LName");
                return _LName; 
            }
            set 
            { 
                _LName = value;
                ConfData.UpdateKey("_LName", value);
            }
        }

        public string EMail
        {
            get 
            {
                _EMail = ConfData.GetKeyValue("_EMail");
                return _EMail; 
            }
            set 
            { 
                _EMail = value;
                ConfData.UpdateKey("_EMail", value);
            }
        }

        public string PassWord
        {
            get 
            {
                _PassWord = ConfData.GetKeyValue("_PassWord");
                return _PassWord; 
            }
            set 
            { 
                _PassWord = value;
                ConfData.UpdateKey("_PassWord", value);
            }
        }

        public bool IsActive
        {
            get
            {
                if (ConfData.GetKeyValue("_IsActive") == "true")
                {
                    _IsActive = true;
                }
                else if (ConfData.GetKeyValue("_IsActive") == "false")
                {
                    _IsActive = false;
                }
                return _IsActive;
            }
            set 
            { 
                _IsActive = value;
                ConfData.UpdateKey("_IsActive", value.ToString());
            }
        }

        public string MeshID
        {
            get 
            {
                _MeshID = ConfData.GetKeyValue("_MeshID");
                return _MeshID; 
            }
            //set 
            //{ 
            //    _MeshID = value; 
            //}
        }

        public string Status
        {
            get 
            {
                _Status = ConfData.GetKeyValue("_Status");
                return _Status; 
            }
            set 
            { 
                _Status = value;
                ConfData.UpdateKey("_Status", value);
            }
        }

        public PeerType CurrPeerType
        {
            get 
            {
                switch (ConfData.GetKeyValue("_CurrPeerType"))
                {
                    case "NotDecided":
                        _CurrPeerType = PeerType.NotDecided;
                        break;

                    case "BootStrap":
                        _CurrPeerType = PeerType.BootStrap;
                        break;

                    case "SuperNode":
                        _CurrPeerType = PeerType.SuperNode;
                        break;

                    case "NodeWithNetP2P":
                        _CurrPeerType = PeerType.NodeWithNetP2P;
                        break;

                    case "NodeWithHttp":
                        _CurrPeerType = PeerType.NodeWithHttp;
                        break;
                }
                return _CurrPeerType; 
            }
            set 
            { 
                _CurrPeerType = value;
                ConfData.UpdateKey("_CurrPeerType", value.ToString());
            }
        }

        public AuthType CurrAuthType
        {
            get 
            {
                switch (ConfData.GetKeyValue("_CurrAuthType"))
                {
                    case "NotDecided":
                        _CurrAuthType = AuthType.NotDecided;
                        break;

                    case "SQLAuthentication":
                        _CurrAuthType = AuthType.SQLAuthentication;
                        break;

                    case "SIPAuthentication":
                        _CurrAuthType = AuthType.SIPAuthentication;
                        break;
                }
                return _CurrAuthType; 
            }
            set 
            { 
                _CurrAuthType = value;
                ConfData.UpdateKey("_CurrAuthType", value.ToString());
            }
        }

        public PortStatus Port4000Status
        {
            get 
            {
                switch (ConfData.GetKeyValue("_Port4000Status"))
                {
                    case "NotDecided":
                        _Port4000Status = PortStatus.NotDecided;
                        break;

                    case "InBoundOpen":
                        _Port4000Status = PortStatus.InBoundOpen;
                        break;

                    case "OutBoundOpen":
                        _Port4000Status = PortStatus.OutBoundOpen;
                        break;

                    case "InOutBoundOpen":
                        _Port4000Status = PortStatus.InOutBoundOpen;
                        break;

                    case "InOutBoundClosed":
                        _Port4000Status = PortStatus.InOutBoundClosed;
                        break;
                }
                return _Port4000Status; 
            }
            set 
            { 
                _Port4000Status = value;
                ConfData.UpdateKey("_Port4000Status", value.ToString());
            }
        }

        public PortStatus Port5060Status
        {
            get 
            {
                switch (ConfData.GetKeyValue("_Port5060Status"))
                {
                    case "NotDecided":
                        _Port5060Status = PortStatus.NotDecided;
                        break;

                    case "InBoundOpen":
                        _Port5060Status = PortStatus.InBoundOpen;
                        break;

                    case "OutBoundOpen":
                        _Port5060Status = PortStatus.OutBoundOpen;
                        break;

                    case "InOutBoundOpen":
                        _Port5060Status = PortStatus.InOutBoundOpen;
                        break;

                    case "InOutBoundClosed":
                        _Port5060Status = PortStatus.InOutBoundClosed;
                        break;
                }
                return _Port5060Status; 
            }
            set 
            { 
                _Port5060Status = value;
                ConfData.UpdateKey("_Port5060Status", value.ToString());
            }
        }

        public PortStatus Port1433Status
        {
            get 
            {
                switch (ConfData.GetKeyValue("_Port1433Status"))
                {
                    case "NotDecided":
                        _Port1433Status = PortStatus.NotDecided;
                        break;

                    case "InBoundOpen":
                        _Port1433Status = PortStatus.InBoundOpen;
                        break;

                    case "OutBoundOpen":
                        _Port1433Status = PortStatus.OutBoundOpen;
                        break;

                    case "InOutBoundOpen":
                        _Port1433Status = PortStatus.InOutBoundOpen;
                        break;

                    case "InOutBoundClosed":
                        _Port1433Status = PortStatus.InOutBoundClosed;
                        break;
                }
                return _Port1433Status; 
            }
            set 
            { 
                _Port1433Status = value;
                ConfData.UpdateKey("_Port1433Status", value.ToString());
            }
        }

        public string AuthServerIP
        {
            get 
            {
                _AuthServerIP = ConfData.GetKeyValue("_AuthServerIP");
                return _AuthServerIP; 
            }
            set 
            { 
                _AuthServerIP = value;
                ConfData.UpdateKey("_AuthServerIP", value);
            }
        }

        public string SuperNodeIP
        {
            get 
            {
                _SuperNodeIP = ConfData.GetKeyValue("_SuperNodeIP");
                return _SuperNodeIP; 
            }
            set 
            {
                _SuperNodeIP = value;
                ConfData.UpdateKey("_SuperNodeIP", value);
            }
        }

        public List<string> NodeIPs
        {
            get 
            {
                int _NodeIPsCount=int.Parse(ConfData.GetKeyValue("_NodeIPs"));
                _NodeIPs.Clear();
                for (int i = 0; i < _NodeIPsCount; i++)
                {
                    if(ConfData.KeyExists("_NodeIPs"+i.ToString()))
                    {
                        _NodeIPs.Add(ConfData.GetKeyValue("_NodeIPs" + i.ToString()));
                    }
                }
                return _NodeIPs; 
            }
            set
            {
                _NodeIPs = value;
                int _NodeIPsCount = int.Parse(ConfData.GetKeyValue("_NodeIPs"));
                for (int i = 0; i < _NodeIPsCount; i++)
                {
                    if (ConfData.KeyExists("_NodeIPs" + i.ToString()))
                    {
                        ConfData.DeleteKey("_NodeIPs" + i.ToString());
                    }
                }

                ConfData.UpdateKey("_NodeIPs", value.Count.ToString());
                for (int i = 0; i < value.Count; i++)
                {
                    ConfData.AddKey("_NodeIPs" + i.ToString(), value[i].ToString());
                }
            }
        }

        public long ScriptID
        {
            get
            {
                _ScriptID = long.Parse(ConfData.GetKeyValue("_ScriptID"));
                return _ScriptID;
            }
            set
            {
                _ScriptID = value;
                ConfData.UpdateKey("_ScriptID", value.ToString());
            }
        }


        public Dictionary<int, int> ConferenceUser
        {
            get
            {
                return _ConferenceUser;
            }
            set
            {
                _ConferenceUser = value;
            }
        }


        #endregion 

        #region Methods

        public ClsPeer()
        {
            ConfData.Init();
        }

        #region xmlMethods

        //public ClsPeer()
        //{
        //    _xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");

        //    if (AppDomain.CurrentDomain.FriendlyName != "BootStrapDomain" && AppDomain.CurrentDomain.FriendlyName != "SuperNodeDomain")
        //    {
        //        if (KeyExists("_ID"))
        //        {
        //            DeleteKey("_ID");
        //        }
        //        if (KeyExists("_DisplayName"))
        //        {
        //            DeleteKey("_DisplayName");
        //        }
        //        if (KeyExists("_RoleID"))
        //        {
        //            DeleteKey("_RoleID");
        //        }
        //        if (KeyExists("_FName"))
        //        {
        //            DeleteKey("_FName");
        //        }
        //        if (KeyExists("_LName"))
        //        {
        //            DeleteKey("_LName");
        //        }
        //        if (KeyExists("_EMail"))
        //        {
        //            DeleteKey("_EMail");
        //        }
        //        if (KeyExists("_PassWord"))
        //        {
        //            DeleteKey("_PassWord");
        //        }
        //        if (KeyExists("_IsActive"))
        //        {
        //            DeleteKey("_IsActive");
        //        }
        //        if (KeyExists("_MeshID"))
        //        {
        //            DeleteKey("_MeshID");
        //        }
        //        if (KeyExists("_Status"))
        //        {
        //            DeleteKey("_Status");
        //        }
        //        if (KeyExists("_CurrPeerType"))
        //        {
        //            DeleteKey("_CurrPeerType");
        //        }
        //        if (KeyExists("_CurrAuthType"))
        //        {
        //            DeleteKey("_CurrAuthType");
        //        }
        //        if (KeyExists("_Port4000Status"))
        //        {
        //            DeleteKey("_Port4000Status");
        //        }
        //        if (KeyExists("_Port5060Status"))
        //        {
        //            DeleteKey("_Port5060Status");
        //        }
        //        if (KeyExists("_Port1433Status"))
        //        {
        //            DeleteKey("_Port1433Status");
        //        }
        //        if (KeyExists("_AuthServerIP"))
        //        {
        //            DeleteKey("_AuthServerIP");
        //        }
        //        if (KeyExists("_SuperNodeIP"))
        //        {
        //            DeleteKey("_SuperNodeIP");
        //        }
        //        if (KeyExists("_NodeIPs"))
        //        {
        //            int _NodeIPsCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["_NodeIPs"].ToString());
        //            for (int i = 0; i < _NodeIPsCount; i++)
        //            {
        //                if (KeyExists("_NodeIPs" + i.ToString()))
        //                {
        //                    DeleteKey("_NodeIPs" + i.ToString());
        //                }
        //            }
        //            DeleteKey("_NodeIPs");
        //        }

        //        //===============================-----------------------=================================

        //        AddKey("_ID", ClsConstants.NullInt.ToString());
        //        AddKey("_DisplayName", ClsConstants.NullString.ToString());
        //        AddKey("_RoleID", ClsConstants.NullInt.ToString());
        //        AddKey("_FName", ClsConstants.NullString.ToString());
        //        AddKey("_LName", ClsConstants.NullString.ToString());
        //        AddKey("_EMail", ClsConstants.NullString.ToString());
        //        AddKey("_PassWord", ClsConstants.NullString.ToString());
        //        AddKey("_IsActive", "false");
        //        AddKey("_MeshID", ClsConstants.NullString.ToString());
        //        AddKey("_Status", ClsConstants.NullString.ToString());
        //        AddKey("_CurrPeerType", "NotDecided");
        //        AddKey("_CurrAuthType", "NotDecided");
        //        AddKey("_Port4000Status", "NotDecided");
        //        AddKey("_Port5060Status", "NotDecided");
        //        AddKey("_Port1433Status", "NotDecided");
        //        AddKey("_AuthServerIP", ClsConstants.NullString.ToString());
        //        AddKey("_SuperNodeIP", ClsConstants.NullString.ToString());
        //        AddKey("_NodeIPs", "0");


        //    }
        //    //else
        //    //{
        //    //    loadFromConfig();
        //    //}
        //}

        //private string GetKeyValue(string strKey)
        //{
        //    XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

        //    foreach (XmlNode childNode in appSettingsNode)
        //    {
        //        if (childNode.Attributes["key"].Value == strKey)
        //        {
        //            return childNode.Attributes["value"].Value.ToString();
        //        }
        //    }

        //    return "";
        //}

        //private void AddKey(string strKey, string strValue)
        //{
        //    XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");
        //    try
        //    {
        //        if (KeyExists(strKey))
        //            throw new ArgumentException("Key name: <" + strKey + "> already exists in the configuration.");
        //        XmlNode newChild = appSettingsNode.FirstChild.Clone();
        //        newChild.Attributes["key"].Value = strKey;
        //        newChild.Attributes["value"].Value = strValue;
        //        appSettingsNode.AppendChild(newChild);

        //        _xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");
        //        _xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void UpdateKey(string strKey, string newValue)
        //{
        //    if (!KeyExists(strKey))
        //        throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");
            
        //    XmlNode appSettingsNode =_xmlDoc.SelectSingleNode("configuration/appSettings");

        //    // Attempt to locate the requested setting.
        //    foreach (XmlNode childNode in appSettingsNode)
        //    {
        //        if (childNode.Attributes["key"].Value == strKey)
        //            childNode.Attributes["value"].Value = newValue;
        //    }
        //    _xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");
        //    _xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        //}

        //private void DeleteKey(string strKey)
        //{
        //    if (!KeyExists(strKey))
        //        throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");
        //    XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

        //    // Attempt to locate the requested setting.
        //    foreach (XmlNode childNode in appSettingsNode)
        //    {
        //        if (childNode.Attributes["key"].Value == strKey)
        //            appSettingsNode.RemoveChild(childNode);
        //    }
        //    _xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");
        //    _xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        //}

        //private bool KeyExists(string strKey)
        //{
        //    XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

        //    // Attempt to locate the requested setting.
        //    foreach (XmlNode childNode in appSettingsNode)
        //    {
        //        if (childNode.Attributes["key"].Value == strKey)
        //            return true;
        //    }
        //    return false;
        //}

        //private void loadFromConfig()
        //{
        //    XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");
        //    foreach (XmlNode node in appSettingsNode.ChildNodes)
        //    {
        //        switch (node.Attributes["key"].Value.ToString())
        //        {

        //            case "_ID":
        //                this._ID = int.Parse(node.Attributes["value"].Value.ToString());
        //                break;

        //            case "_DisplayName":
        //                this._DisplayName = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_RoleID":
        //                this._RoleID = int.Parse(node.Attributes["value"].Value.ToString());
        //                break;

        //            case "_FName":
        //                this._FName = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_LName":
        //                this._LName = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_EMail":
        //                this._EMail = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_PassWord":
        //                this._PassWord = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_IsActive":
        //                if(node.Attributes["value"].Value.ToString()=="true")
        //                {
        //                    this._IsActive = true;
        //                }
        //                else if (node.Attributes["value"].Value.ToString() == "false")
        //                {
        //                    this._IsActive = false;
        //                }
                        
        //                break;

        //            case "_MeshID":
        //                this._MeshID = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_Status":
        //                this._Status = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_CurrPeerType":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        this._CurrPeerType = PeerType.NotDecided;
        //                        break;

        //                    case "BootStrap":
        //                        this._CurrPeerType = PeerType.BootStrap;
        //                        break;

        //                    case "SuperNode":
        //                        this._CurrPeerType = PeerType.SuperNode;
        //                        break;

        //                    case "NodeWithNetP2P":
        //                        this._CurrPeerType = PeerType.NodeWithNetP2P;
        //                        break;

        //                    case "NodeWithHttp":
        //                        this._CurrPeerType = PeerType.NodeWithHttp;
        //                        break;
        //                }
        //                break;

        //            case "_CurrAuthType":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        this._CurrAuthType = AuthType.NotDecided;
        //                        break;

        //                    case "SQLAuthentication":
        //                        this._CurrAuthType = AuthType.SQLAuthentication;
        //                        break;

        //                    case "SIPAuthentication":
        //                        this._CurrAuthType = AuthType.SIPAuthentication;
        //                        break;
        //                }
        //                break;

        //            case "_Port4000Status":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        this._Port4000Status = PortStatus.NotDecided;
        //                        break;

        //                    case "InBoundOpen":
        //                        this._Port4000Status = PortStatus.InBoundOpen;
        //                        break;

        //                    case "OutBoundOpen":
        //                        this._Port4000Status = PortStatus.OutBoundOpen;
        //                        break;

        //                    case "InOutBoundOpen":
        //                        this._Port4000Status = PortStatus.InOutBoundOpen;
        //                        break;

        //                    case "InOutBoundClosed":
        //                        this._Port4000Status = PortStatus.InOutBoundClosed;
        //                        break;
        //                }
        //                break;

        //            case "_Port5060Status":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        this._Port5060Status = PortStatus.NotDecided;
        //                        break;

        //                    case "InBoundOpen":
        //                        this._Port5060Status = PortStatus.InBoundOpen;
        //                        break;

        //                    case "OutBoundOpen":
        //                        this._Port5060Status = PortStatus.OutBoundOpen;
        //                        break;

        //                    case "InOutBoundOpen":
        //                        this._Port5060Status = PortStatus.InOutBoundOpen;
        //                        break;

        //                    case "InOutBoundClosed":
        //                        this._Port5060Status = PortStatus.InOutBoundClosed;
        //                        break;
        //                }
        //                break;

        //            case "_Port1433Status":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        this._Port1433Status = PortStatus.NotDecided;
        //                        break;

        //                    case "InBoundOpen":
        //                        this._Port1433Status = PortStatus.InBoundOpen;
        //                        break;

        //                    case "OutBoundOpen":
        //                        this._Port1433Status = PortStatus.OutBoundOpen;
        //                        break;

        //                    case "InOutBoundOpen":
        //                        this._Port1433Status = PortStatus.InOutBoundOpen;
        //                        break;

        //                    case "InOutBoundClosed":
        //                        this._Port1433Status = PortStatus.InOutBoundClosed;
        //                        break;
        //                }
        //                break;

        //            case "_AuthServerIP":
        //                this._AuthServerIP = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_SuperNodeIP":
        //                this._SuperNodeIP = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_NodeIPs":
        //                int _NodeIPsCount = int.Parse(node.Attributes["value"].Value.ToString());
        //                for (int i = 0; i < _NodeIPsCount; i++)
        //                {
        //                    if (KeyExists("_NodeIPs" + i.ToString()))
        //                    {
        //                        _NodeIPs.Add(GetKeyValue("_NodeIPs" + i.ToString()));
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //}

        #endregion

        public ClsPeerDataContract GetPeerDataContract()
        {
            ClsPeerDataContract objPeerDataContract = new ClsPeerDataContract();

            objPeerDataContract.ID = this.ID;
            objPeerDataContract.DisplayName = this.DisplayName;
            objPeerDataContract.RoleID = this.RoleID;
            objPeerDataContract.FName = this.FName;
            objPeerDataContract.LName = this.LName;
            objPeerDataContract.EMail = this.EMail;
            objPeerDataContract.PassWord = this.PassWord;
            objPeerDataContract.IsActive = this.IsActive;

            objPeerDataContract.MeshID = this.MeshID;
            objPeerDataContract.Status = this.Status;

            objPeerDataContract.CurrPeerType = this.CurrPeerType;
            objPeerDataContract.CurrAuthType = this.CurrAuthType;
            objPeerDataContract.Port4000Status = this.Port4000Status;
            objPeerDataContract.Port5060Status = this.Port5060Status;
            objPeerDataContract.Port1433Status = this.Port1433Status;


            objPeerDataContract.AuthServerIP = this.AuthServerIP;
            objPeerDataContract.SuperNodeIP = this.SuperNodeIP;
            objPeerDataContract.NodeIPs = this.NodeIPs;

            return objPeerDataContract;
        }

        #endregion
    }
}
