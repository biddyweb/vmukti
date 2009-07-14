using System;
using System.Collections.Generic;

namespace VMuktiAPI
{
    public static partial class VMuktiInfo
    {
        private static string _strMainConnectionString = "";
        private static string _strSchemaNumber = "";
        private static string _strSyncConnectionString = "";
        private static List<string> _strBootStrapIPs = new List<string>();

        //****Before Performance Rule.
        //private static bool _IsFileExists=false;
        //private static bool _IsSoftPhone = false;
        //****After applying Rule.
        private static bool _IsFileExists;
        private static bool _IsSoftPhone ;
        private static bool _Port80;
        private static ClsPeer _objClsPeer = new ClsPeer();

        //private static string intCurrentpublicip = "59.95.248.170";
        //= "Data Source=ADIANCE03\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir";

        //****Before Performance Rule.
        //private static AppDomain _BootStrapDomain = null;
        //private static AppDomain _SuperNodeDomain = null;
        //****After applying Rule.
        private static AppDomain _BootStrapDomain;
        private static AppDomain _SuperNodeDomain;

        private static string _strFTPServerIP = "";
        private static string _strFTPPort = "";
        private static string _strFTPUserName = "";
        private static string _strFTPPassword = "";
        private static string _strFTPDirPath = "";
        private static string _strVMuktiVersion = "";
        private static string _strZipFileDownlaodLink = "";
        private static string _strExternalPBX = "";
        private static string _strPort80 = "";

        public static string FTPServerIP
        {
            get { return _strFTPServerIP; }
            set { _strFTPServerIP = value; }
        }

        public static string VMuktiVersion
        {
            get
            {
                _strVMuktiVersion = ConfData.GetKeyValue("_strVMuktiVersion");
                return _strVMuktiVersion;
            }
            set
            {
                _strVMuktiVersion = value;
                ConfData.UpdateKey("_strVMuktiVersion", value);
            }
        }

        public static string ZipFileDownloadLink
        {
            get
            {
                _strZipFileDownlaodLink = ConfData.GetKeyValue("_strZipFileDownlaodLink");
                return _strZipFileDownlaodLink;
            }
            set
            {
                _strZipFileDownlaodLink = value;
                ConfData.UpdateKey("_strZipFileDownlaodLink", value);
            }
        }

        public static string strExternalPBX
        {
            get
            {
                _strExternalPBX = ConfData.GetKeyValue("_strExternalPBX");
                return _strExternalPBX;
            }
            set
            {
                _strExternalPBX = value;
                ConfData.UpdateKey("_strExternalPBX", value);
            }
        }

        public static string Port80
        {
            get
            {
                _strPort80 = ConfData.GetKeyValue("_strPort80");
                return _strPort80;
            }
            set
            {
                _strPort80 = value;
                ConfData.UpdateKey("_strPort80", value);
            }
        }

        public static string FTPPort
        {
            get { return _strFTPPort; }
            set { _strFTPPort = value; }
        }

        public static string FTPUserName
        {
            get { return _strFTPUserName; }
            set { _strFTPUserName = value; }
        }

        public static string FTPPassword
        {
            get { return _strFTPPassword; }
            set { _strFTPPassword = value; }
        }

        public static string FTPDirPath
        {
            get { return _strFTPDirPath; }
            set { _strFTPDirPath = value; }
        }
       

        public static string MainConnectionString
        {
            get 
            {
                //_strMainConnectionString = ConfData.GetKeyValue("_strMainConnectionString");
                System.Text.UTF32Encoding objUTF32 = new System.Text.UTF32Encoding();
                byte[] objbytes = Convert.FromBase64String(ConfData.GetKeyValue("_strMainConnectionString"));
                _strMainConnectionString= objUTF32.GetString(objbytes);
                return _strMainConnectionString; 
            }
            set 
            {
                _strMainConnectionString = value;
                ConfData.UpdateKey("_strMainConnectionString", value);
            }
        }

        public static string SyncConnectionString
        {
            get
            {
                _strSyncConnectionString = ConfData.GetKeyValue("_strSyncConnectionString");
                return _strSyncConnectionString;
            }
            set
            {
                _strSyncConnectionString = value;
                ConfData.UpdateKey("_strSyncConnectionString", value);
            }
        }

        public static List<string> BootStrapIPs
        {
            get 
            {
                int _BootStrapNodeIPsCount = int.Parse(ConfData.GetKeyValue("_strBootStrapIPs"));
                _strBootStrapIPs.Clear();
                for (int i = 0; i < _BootStrapNodeIPsCount; i++)
                {
                    if (ConfData.KeyExists("_strBootStrapIPs" + i.ToString()))
                    {
                        _strBootStrapIPs.Add(ConfData.GetKeyValue("_strBootStrapIPs" + i.ToString()));
                    }
                }
                return _strBootStrapIPs;
            }
            set 
            {
                _strBootStrapIPs = value;
                int _NodeIPsCount = int.Parse(ConfData.GetKeyValue("_strBootStrapIPs"));
                for (int i = 0; i < _NodeIPsCount; i++)
                {
                    if (ConfData.KeyExists("_strBootStrapIPs" + i.ToString()))
                    {
                        ConfData.DeleteKey("_strBootStrapIPs" + i.ToString());
                    }
                }

                ConfData.UpdateKey("_strBootStrapIPs", value.Count.ToString());
                for (int i = 0; i < value.Count; i++)
                {
                    ConfData.AddKey("_strBootStrapIPs" + i.ToString(), value[i].ToString());
                }
            }
        }

        public static bool IsFileExists
        {
            get 
            {
                if (ConfData.GetKeyValue("_IsFileExists") == "true")
                {
                    _IsFileExists = true;
                }
                else if (ConfData.GetKeyValue("_IsFileExists") == "false")
                {
                    _IsFileExists = false;
                }
                return _IsFileExists;
            }
            set 
            {
                _IsFileExists = value;
                ConfData.UpdateKey("_IsFileExists", value.ToString());
            }
        }

        public static bool IsSoftPhone
        {
            get 
            {
                if (ConfData.GetKeyValue("_IsSoftPhone") == "true")
                {
                    _IsSoftPhone = true;
                }
                else if (ConfData.GetKeyValue("_IsSoftPhone") == "false")
                {
                    _IsSoftPhone = false;
                }
                return _IsSoftPhone;
            }
            set 
            {
                _IsSoftPhone = value;
                ConfData.UpdateKey("_IsSoftPhone", value.ToString());
            }
        }

        public static string strSchemaNumber
        {
            get
            {
                _strSchemaNumber = ConfData.GetKeyValue("_strSchemaNumber");
                return _strSchemaNumber;
            }
            set
            {
                _strSchemaNumber = value;
                ConfData.UpdateKey("_strSchemaNumber", value);
            }
        }

        public static ClsPeer CurrentPeer
        {
            get 
            {
                return _objClsPeer;
            }
            set 
            {
                _objClsPeer = value;
            }
        }

        public static AppDomain BootStrapDomain
        {
            get
            {
                return _BootStrapDomain;
            }
            set
            {
                _BootStrapDomain = value;
            }
        }

        public static AppDomain SuperNodeDomain
        {
            get
            {
                return _SuperNodeDomain;
            }
            set
            {
                _SuperNodeDomain = value;
            }
        }

    }
}
