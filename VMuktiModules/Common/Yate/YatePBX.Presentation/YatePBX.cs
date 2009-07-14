using System;
using System.Diagnostics;
using VMukti.Profile;
using VMuktiAPI;
using YatePBX.Business;
using VMukti.ZipUnzip.Zip;

namespace YatePBX.Presentation
{
    public class YatePBX
    {
        Profile myProf;
        Process p;
        ClsCredential PBXCredential = null;
        
        public YatePBX()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.strExternalPBX == "false")
                {
                    if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Assamblies"))
                    {
                        //new WebClient().DownloadFile("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/VMukti/Assamblies.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "Assamblies.zip");
                        FastZip fz = new FastZip();
                        fz.ExtractZip(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Assamblies.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "Assamblies", null);
                    }
                    PBXCredential = new ClsCredential();
                    myProf = new VMukti.Profile.Ini(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\ysipchan.conf");
                }
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "YatePBX()--:--YatePBX.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }
        }

        public void FncStartPBX(string IP)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.strExternalPBX == "false")
                {
                    string[] strCredential = PBXCredential.FncProviderInformation();
                    FncEditYateFile(strCredential);
                    myProf.SetValue("general", "addr", IP);
                    p = new Process();
                    p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies";
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\Yate.exe";
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("FncStartPBX " + ex.Message);

            }
        }

        public void FncStopPBX()
        {
            if (p != null && VMuktiAPI.VMuktiInfo.strExternalPBX == "false")
            {
                p.Kill();
                p.CloseMainWindow();
                p.Close();
                p.Dispose();
                p = null;
            }
        }

        public string FncCreateSIPUser()
        {
            try
            {
                System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regfile.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
                string strTemp = objStreamReader.ReadToEnd();
                int intStratIndex = strTemp.LastIndexOf('[');
                int intEndIndex = strTemp.LastIndexOf(']');
                string strMaxNumber = strTemp.Substring(intStratIndex + 1, intEndIndex - intStratIndex - 1);

                int NewNumber = int.Parse(strMaxNumber);
                NewNumber = NewNumber + 1;
                objStreamReader.Close();

                System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name, true);
                objStreamWriter.Write("[" + NewNumber.ToString() + "]\r\nPassword=" + NewNumber.ToString() + "\r\n");
                objStreamWriter.Close();
                objFileStream.Close();
                return NewNumber.ToString();
            }
            catch
            {
                return null;
            }

        }

        public void FncRemoveSIPUser(string number)
        {
            System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regfile.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
            System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
            string strTemp = objStreamReader.ReadToEnd();
            strTemp = strTemp.ToLower().Replace("[" + number + "]", "");
            strTemp = strTemp.ToLower().Replace("password=" + number, "");
            objStreamReader.Close();
            System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name);
            objStreamWriter.Write(strTemp);
            objStreamWriter.Close();
            objFileStream.Close();
        }

        public string FncCreateConference()
        {
            try
            {
                System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regexroute.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
                string strTemp = objStreamReader.ReadToEnd();
                int intStratIndex = strTemp.LastIndexOf('^');
                int intEndIndex = strTemp.LastIndexOf('$');
                string strMaxNumber = strTemp.Substring(intStratIndex + 1, intEndIndex - intStratIndex - 1);

                int NewNumber = int.Parse(strMaxNumber);
                NewNumber = NewNumber + 1;
                objStreamReader.Close();

                System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name, true);
                objStreamWriter.Write("^" + NewNumber.ToString() + "$ = conf/echo" + NewNumber.ToString() + "\r\n");
                objStreamWriter.Close();
                objFileStream.Close();
                return NewNumber.ToString();
            }
            catch
            {
                return null;
            }
        }

        public void FncRemoveConfNumber(string ConfNumber)
        {
            System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regexroute.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
            System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
            string strTemp = objStreamReader.ReadToEnd();
            strTemp = strTemp.ToLower().Replace("^" + ConfNumber + "$ = conf/echo" + ConfNumber.ToString(), "");
            //strTemp = strTemp.ToLower().Replace("password=" + ConfNumber, "");
            objStreamReader.Close();
            System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name);
            objStreamWriter.Write(strTemp);
            objStreamWriter.Close();
            objFileStream.Close();
        }

        public void FncUpdateYATEFile(string strUserName, string strIP)
        {
            System.IO.FileStream objFileStream = new System.IO.FileStream(@"C:\Documents and Settings\Administrator\Desktop\accfile.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
            System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
            string strTemp = objStreamReader.ReadToEnd();


            int startIndex = strTemp.IndexOf("[" + strUserName + "]");
            string strSubString = strTemp.Substring(startIndex);
            int passWordInxex = strSubString.IndexOf("registrar=");
            int indexToBeInserted = startIndex + passWordInxex + 10;
            string strLst = strTemp.Substring(indexToBeInserted, strIP.Length);
            strTemp = strTemp.Remove(indexToBeInserted, strLst.Length);
            if (strLst == strIP)
            {
                strTemp = strTemp.Remove(indexToBeInserted, strLst.Length);
            }
            else
            {

                strTemp = strTemp.Insert(indexToBeInserted, strIP);
                objStreamReader.Close();
                System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name, false);
                objStreamWriter.Write("");
                objStreamWriter.Write(strTemp);
                objStreamWriter.Close();
                objFileStream.Close();
            }
        }

        private void FncEditYateFile(string[] strCredential)
        {
            try
            {
                if (strCredential != null && VMuktiAPI.VMuktiInfo.strExternalPBX == "false")
                {
                    string SIPUserName = strCredential[0].ToString();
                    string SIPPassword = strCredential[1].ToString();
                    string SIPDomain = strCredential[2].ToString();

                    //string assemblypath = Context.Parameters["assemblypath"].Replace("VMukti Setup.dll", "Adiance");
                    string assemblypath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                    string appConfigPath = assemblypath + @"\Assamblies\conf.d\accfile.conf";
                    System.IO.FileStream objFileStream = new System.IO.FileStream(appConfigPath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                    System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
                    string strTemp = objStreamReader.ReadToEnd();
                    int indexToBeInserted = 0;

                    int startIndex = strTemp.IndexOf("[sip]");
                    string strSubString = strTemp.Substring(startIndex);
                    int userNameIndex = strSubString.IndexOf("username=");
                    int intpwdIndex = strSubString.IndexOf("password=");
                    indexToBeInserted = startIndex + userNameIndex + 9;
                    //string temp = strSubString.Substring(indexToBeInserted, intpwdIndex - userNameIndex);
                    strTemp = strTemp.Remove(indexToBeInserted, intpwdIndex - userNameIndex - 11);
                    strTemp = strTemp.Insert(indexToBeInserted, SIPUserName);


                    startIndex = strTemp.IndexOf("[sip]");
                    strSubString = strTemp.Substring(startIndex);
                    int intregIndex = strSubString.IndexOf("registrar=");
                    int passwordIndex = strSubString.IndexOf("password=");
                    indexToBeInserted = startIndex + passwordIndex + 9;
                    strTemp = strTemp.Remove(indexToBeInserted, intregIndex - passwordIndex - 11);
                    strTemp = strTemp.Insert(indexToBeInserted, SIPPassword);


                    startIndex = strTemp.IndexOf("[sip]");
                    strSubString = strTemp.Substring(startIndex);
                    int intLocalAddress = strSubString.IndexOf("localaddress=");
                    int registarIndex = strSubString.IndexOf("registrar=");
                    indexToBeInserted = startIndex + registarIndex + 10;
                    strTemp = strTemp.Remove(indexToBeInserted, intLocalAddress - registarIndex - 12);
                    string strLst = strTemp.Substring(indexToBeInserted, SIPDomain.Length);
                    strTemp = strTemp.Insert(indexToBeInserted, SIPDomain);

                    objStreamReader.Close();
                    System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name, false);
                    objStreamWriter.Write("");
                    objStreamWriter.Write(strTemp);
                    objStreamWriter.Close();
                    objFileStream.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Could not Edit PBX Files:" + ex.Message);
            }
        }

    }

}
