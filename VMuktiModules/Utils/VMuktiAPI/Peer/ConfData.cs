using System;
using System.Xml;

namespace VMuktiAPI
{
    public static class ConfData
    {
        #region Methods

        public static void Init()
        {
            if (AppDomain.CurrentDomain.FriendlyName != "BootStrapDomain" && AppDomain.CurrentDomain.FriendlyName != "SuperNodeDomain")
            {
                ///////// for VMuktiInfo variables
                if (KeyExists("_strMainConnectionString"))
                {
                    DeleteKey("_strMainConnectionString");
                }

                if (KeyExists("_strSyncConnectionString"))
                {
                    DeleteKey("_strSyncConnectionString");
                }

                if (KeyExists("_strBootStrapIPs"))
                {
                    int _NodeIPsCount = int.Parse(GetKeyValue("_strBootStrapIPs"));
                    for (int i = 0; i < _NodeIPsCount; i++)
                    {
                        if (KeyExists("_strBootStrapIPs" + i.ToString()))
                        {
                            DeleteKey("_strBootStrapIPs" + i.ToString());
                        }
                    }
                    DeleteKey("_strBootStrapIPs");
                }

                if (KeyExists("_IsFileExists"))
                {
                    DeleteKey("_IsFileExists");
                }

                if (KeyExists("_IsSoftPhone"))
                {
                    DeleteKey("_IsSoftPhone");
                }
                AddKey("_strMainConnectionString", ClsConstants.NullString.ToString());
                AddKey("_strSyncConnectionString", ClsConstants.NullString.ToString());
                AddKey("_IsFileExists", ClsConstants.NullString.ToString());
                AddKey("_IsSoftPhone", ClsConstants.NullString.ToString());
                AddKey("_strBootStrapIPs", "0");

                ///////// for  clsPeer variables

                if (KeyExists("_ID"))
                {
                    DeleteKey("_ID");
                }
                if (KeyExists("_DisplayName"))
                {
                    DeleteKey("_DisplayName");
                }
                if (KeyExists("_RoleID"))
                {
                    DeleteKey("_RoleID");
                }
                if (KeyExists("_FName"))
                {
                    DeleteKey("_FName");
                }
                if (KeyExists("_LName"))
                {
                    DeleteKey("_LName");
                }
                if (KeyExists("_EMail"))
                {
                    DeleteKey("_EMail");
                }
                if (KeyExists("_PassWord"))
                {
                    DeleteKey("_PassWord");
                }
                if (KeyExists("_IsActive"))
                {
                    DeleteKey("_IsActive");
                }
                if (KeyExists("_MeshID"))
                {
                    DeleteKey("_MeshID");
                }
                if (KeyExists("_Status"))
                {
                    DeleteKey("_Status");
                }
                if (KeyExists("_CurrPeerType"))
                {
                    DeleteKey("_CurrPeerType");
                }
                if (KeyExists("_CurrAuthType"))
                {
                    DeleteKey("_CurrAuthType");
                }
                if (KeyExists("_Port4000Status"))
                {
                    DeleteKey("_Port4000Status");
                }
                if (KeyExists("_Port5060Status"))
                {
                    DeleteKey("_Port5060Status");
                }
                if (KeyExists("_Port1433Status"))
                {
                    DeleteKey("_Port1433Status");
                }
                if (KeyExists("_AuthServerIP"))
                {
                    DeleteKey("_AuthServerIP");
                }
                if (KeyExists("_SuperNodeIP"))
                {
                    DeleteKey("_SuperNodeIP");
                }
                if (KeyExists("_NodeIPs"))
                {
                    int _NodeIPsCount = int.Parse(GetKeyValue("_NodeIPs"));
                    for (int i = 0; i < _NodeIPsCount; i++)
                    {
                        if (KeyExists("_NodeIPs" + i.ToString()))
                        {
                            DeleteKey("_NodeIPs" + i.ToString());
                        }
                    }
                    DeleteKey("_NodeIPs");
                }

                if (KeyExists("_strSchemaNumber"))
                {
                    DeleteKey("_strSchemaNumber");
                }

                if (KeyExists("_strFTPServerIP"))
                {
                    DeleteKey("_strFTPServerIP");
                }
                if (KeyExists("_strFTPPort"))
                {
                    DeleteKey("_strFTPPort");
                }
                if (KeyExists("_strFTPUserName"))
                {
                    DeleteKey("_strFTPUserName");
                }
                if (KeyExists("_strFTPPassword"))
                {
                    DeleteKey("_strFTPPassword");
                }
                if (KeyExists("_strFTPDirPath"))
                {
                    DeleteKey("_strFTPDirPath");
                }

                if (KeyExists("_CampaignID"))
                {
                    DeleteKey("_CampaignID");
                }
                if (KeyExists("_GroupID"))
                {
                    DeleteKey("_GroupID");
                }
                if (KeyExists("_ActivityID"))
                {
                    DeleteKey("_ActivityID");
                }
                if (KeyExists("_StartTime"))
                {
                    DeleteKey("_StartTime");
                }

                if (KeyExists("_EndTime"))
                {
                    DeleteKey("_EndTime");
                }
                if (KeyExists("_CampaignName"))
                {
                    DeleteKey("_CampaignName");
                }
                if (KeyExists("_GroupName"))
                {
                    DeleteKey("_GroupName");
                }
                if (KeyExists("_RoleName"))
                {
                    DeleteKey("_RoleName");
                }
                if (KeyExists("_strZipFileDownlaodLink"))
                {
                    DeleteKey("_strZipFileDownlaodLink");
                }
                if (KeyExists("_strVMuktiVersion"))
                {
                    DeleteKey("_strVMuktiVersion");
                }
                if (KeyExists("_strExternalPBX"))
                {
                    DeleteKey("_strExternalPBX");
                }
                if (KeyExists("_CurrentMachineIP"))
                {
                    DeleteKey("_CurrentMachineIP");
                }
                if (KeyExists("_ScriptID"))
                {
                    DeleteKey("_ScriptID");
                }

                if (KeyExists("_strPort80"))
                {
                    DeleteKey("_strPort80");
                }


                //===============================-----------------------=================================

                AddKey("_ID", ClsConstants.NullInt.ToString());
                AddKey("_DisplayName", ClsConstants.NullString.ToString());
                AddKey("_RoleID", ClsConstants.NullInt.ToString());
                AddKey("_FName", ClsConstants.NullString.ToString());
                AddKey("_LName", ClsConstants.NullString.ToString());
                AddKey("_EMail", ClsConstants.NullString.ToString());
                AddKey("_PassWord", ClsConstants.NullString.ToString());
                AddKey("_IsActive", "false");
                AddKey("_MeshID", ClsConstants.NullString.ToString());
                AddKey("_Status", ClsConstants.NullString.ToString());
                AddKey("_CurrPeerType", "NotDecided");
                AddKey("_CurrAuthType", "NotDecided");
                AddKey("_Port4000Status", "NotDecided");
                AddKey("_Port5060Status", "NotDecided");
                AddKey("_Port1433Status", "NotDecided");
                AddKey("_AuthServerIP", ClsConstants.NullString.ToString());
                AddKey("_SuperNodeIP", ClsConstants.NullString.ToString());
                AddKey("_NodeIPs", "0");
                AddKey("_strSchemaNumber", ClsConstants.NullString.ToString());

                AddKey("_strFTPServerIP", ClsConstants.NullString.ToString());
                AddKey("_strFTPPort", ClsConstants.NullString.ToString());
                AddKey("_strFTPUserName", ClsConstants.NullString.ToString());
                AddKey("_strFTPPassword", ClsConstants.NullString.ToString());
                AddKey("_strFTPDirPath", ClsConstants.NullString.ToString());

                AddKey("_CampaignID", ClsConstants.NullString.ToString());
                AddKey("_GroupID", ClsConstants.NullString.ToString());
                AddKey("_ActivityID", ClsConstants.NullString.ToString());
                AddKey("_StartTime", ClsConstants.NullString.ToString());
                AddKey("_EndTime", ClsConstants.NullString.ToString());

                AddKey("_CampaignName", ClsConstants.NullString.ToString());
                AddKey("_GroupName", ClsConstants.NullString.ToString());
                AddKey("_RoleName", ClsConstants.NullString.ToString());
                AddKey("_strZipFileDownlaodLink", ClsConstants.NullString.ToString());
                AddKey("_strVMuktiVersion", ClsConstants.NullString.ToString());
                AddKey("_strExternalPBX", ClsConstants.NullString.ToString());
                AddKey("_CurrentMachineIP", ClsConstants.NullString.ToString());
                AddKey("_ScriptID", ClsConstants.NullLong.ToString());


                AddKey("_strPort80", ClsConstants.NullString.ToString());

            }
        }

        public static string GetKeyValue(string strKey)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                {
                    return childNode.Attributes["value"].Value.ToString();
                }
            }

            return "";
        }

        public static void AddKey(string strKey, string strValue)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");
            try
            {
                if (KeyExists(strKey))
                    throw new ArgumentException("Key name: <" + strKey + "> already exists in the configuration.");
                XmlNode newChild = appSettingsNode.FirstChild.Clone();
                newChild.Attributes["key"].Value = strKey;
                newChild.Attributes["value"].Value = strValue;
                appSettingsNode.AppendChild(newChild);

                //_xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");
                _xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateKey(string strKey, string newValue)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            if (!KeyExists(strKey))
                throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");

            XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

            // Attempt to locate the requested setting.
            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    childNode.Attributes["value"].Value = newValue;
            }
            //_xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");
            _xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        public static void DeleteKey(string strKey)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            if (!KeyExists(strKey))
                throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");
            XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

            // Attempt to locate the requested setting.
            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    appSettingsNode.RemoveChild(childNode);
            }
            //_xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "VMukti.Presentation.exe.config");
            _xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        public static bool KeyExists(string strKey)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");

            // Attempt to locate the requested setting.
            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    return true;
            }
            return false;
        }

        //public static void loadFromConfig()
        //{
        //    XmlNode appSettingsNode = _xmlDoc.SelectSingleNode("configuration/appSettings");
        //    foreach (XmlNode node in appSettingsNode.ChildNodes)
        //    {
        //        switch (node.Attributes["key"].Value.ToString())
        //        {

        //            case "_ID":
        //                ConfData._ID = int.Parse(node.Attributes["value"].Value.ToString());
        //                break;

        //            case "_DisplayName":
        //                ConfData._DisplayName = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_RoleID":
        //                ConfData._RoleID = int.Parse(node.Attributes["value"].Value.ToString());
        //                break;

        //            case "_FName":
        //                ConfData._FName = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_LName":
        //                ConfData._LName = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_EMail":
        //                ConfData._EMail = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_PassWord":
        //                ConfData._PassWord = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_IsActive":
        //                if (node.Attributes["value"].Value.ToString() == "true")
        //                {
        //                    ConfData._IsActive = true;
        //                }
        //                else if (node.Attributes["value"].Value.ToString() == "false")
        //                {
        //                    ConfData._IsActive = false;
        //                }

        //                break;

        //            case "_MeshID":
        //                ConfData._MeshID = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_Status":
        //                ConfData._Status = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_CurrPeerType":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        ConfData._CurrPeerType = PeerType.NotDecided;
        //                        break;

        //                    case "BootStrap":
        //                        ConfData._CurrPeerType = PeerType.BootStrap;
        //                        break;

        //                    case "SuperNode":
        //                        ConfData._CurrPeerType = PeerType.SuperNode;
        //                        break;

        //                    case "NodeWithNetP2P":
        //                        ConfData._CurrPeerType = PeerType.NodeWithNetP2P;
        //                        break;

        //                    case "NodeWithHttp":
        //                        ConfData._CurrPeerType = PeerType.NodeWithHttp;
        //                        break;
        //                }
        //                break;

        //            case "_CurrAuthType":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        ConfData._CurrAuthType = AuthType.NotDecided;
        //                        break;

        //                    case "SQLAuthentication":
        //                        ConfData._CurrAuthType = AuthType.SQLAuthentication;
        //                        break;

        //                    case "SIPAuthentication":
        //                        ConfData._CurrAuthType = AuthType.SIPAuthentication;
        //                        break;
        //                }
        //                break;

        //            case "_Port4000Status":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        ConfData._Port4000Status = PortStatus.NotDecided;
        //                        break;

        //                    case "InBoundOpen":
        //                        ConfData._Port4000Status = PortStatus.InBoundOpen;
        //                        break;

        //                    case "OutBoundOpen":
        //                        ConfData._Port4000Status = PortStatus.OutBoundOpen;
        //                        break;

        //                    case "InOutBoundOpen":
        //                        ConfData._Port4000Status = PortStatus.InOutBoundOpen;
        //                        break;

        //                    case "InOutBoundClosed":
        //                        ConfData._Port4000Status = PortStatus.InOutBoundClosed;
        //                        break;
        //                }
        //                break;

        //            case "_Port5060Status":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        ConfData._Port5060Status = PortStatus.NotDecided;
        //                        break;

        //                    case "InBoundOpen":
        //                        ConfData._Port5060Status = PortStatus.InBoundOpen;
        //                        break;

        //                    case "OutBoundOpen":
        //                        ConfData._Port5060Status = PortStatus.OutBoundOpen;
        //                        break;

        //                    case "InOutBoundOpen":
        //                        ConfData._Port5060Status = PortStatus.InOutBoundOpen;
        //                        break;

        //                    case "InOutBoundClosed":
        //                        ConfData._Port5060Status = PortStatus.InOutBoundClosed;
        //                        break;
        //                }
        //                break;

        //            case "_Port1433Status":
        //                switch (node.Attributes["value"].Value.ToString())
        //                {
        //                    case "NotDecided":
        //                        ConfData._Port1433Status = PortStatus.NotDecided;
        //                        break;

        //                    case "InBoundOpen":
        //                        ConfData._Port1433Status = PortStatus.InBoundOpen;
        //                        break;

        //                    case "OutBoundOpen":
        //                        ConfData._Port1433Status = PortStatus.OutBoundOpen;
        //                        break;

        //                    case "InOutBoundOpen":
        //                        ConfData._Port1433Status = PortStatus.InOutBoundOpen;
        //                        break;

        //                    case "InOutBoundClosed":
        //                        ConfData._Port1433Status = PortStatus.InOutBoundClosed;
        //                        break;
        //                }
        //                break;

        //            case "_AuthServerIP":
        //                ConfData._AuthServerIP = node.Attributes["value"].Value.ToString();
        //                break;

        //            case "_SuperNodeIP":
        //                ConfData._SuperNodeIP = node.Attributes["value"].Value.ToString();
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
    }
}
