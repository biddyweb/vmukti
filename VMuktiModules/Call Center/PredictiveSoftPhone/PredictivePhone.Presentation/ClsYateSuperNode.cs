/* VMukti 1.0 -- An Open Source Unified Communications Engine
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using VMuktiService;
using VMuktiAPI;

namespace PredictivePhone.Presentation
{
    public class ClsYateSuperNode
    {
        object objHttpService = null;
        public string strLastNumber = "1110";
        public string strLastConfNumber = "860000";

        public ClsYateSuperNode()
        {
            objHttpService = new clsService();
            ((clsService)objHttpService).EntsvcAddSIPUser += new clsService.DelsvcAddSIPUser(ClsYateSuperNode_EntsvcAddSIPUser);
            ((clsService)objHttpService).EntsvcGetConferenceNumber += new clsService.DelsvcGetConferenceNumber(ClsYateSuperNode_EntsvcGetConferenceNumber);
            ((clsService)objHttpService).EntsvcRemoveSIPUser += new clsService.DelsvcsvcRemoveSIPUser(ClsYateSuperNode_EntsvcRemoveSIPUser);

            BasicHttpServer bhsHttpBootStrap = new BasicHttpServer(ref objHttpService, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/SNServicePredictive");
            bhsHttpBootStrap.AddEndPoint<IService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/SNServicePredictive");
            bhsHttpBootStrap.OpenServer();
            
        }

        string ClsYateSuperNode_EntsvcAddSIPUser()
        {
            try
            {
                //System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regfile.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                //System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
                //string strTemp = objStreamReader.ReadToEnd();
                //int intStratIndex = strTemp.LastIndexOf('[');
                //int intEndIndex = strTemp.LastIndexOf(']');
                //string strMaxNumber = strTemp.Substring(intStratIndex + 1, intEndIndex - intStratIndex - 1);

                //int NewNumber = int.Parse(strMaxNumber);
                //NewNumber = NewNumber + 1;
                //objStreamReader.Close();

                //System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name, true);
                //objStreamWriter.Write("[" + NewNumber.ToString() + "]\r\nPassword=" + NewNumber.ToString() + "\r\n");
                //objStreamWriter.Close();
                //objFileStream.Close();
                //return NewNumber.ToString();

                int Number = int.Parse(strLastNumber);
                Number = Number + 1;
                strLastNumber = Number.ToString();
                //return VMuktiAPI.VMuktiInfo.strSchemaNumber + strLastNumber;
                return strLastNumber;
            }
            catch
            {
                return null;
            }
        }

        string ClsYateSuperNode_EntsvcGetConferenceNumber()
        {
            try
            {
                //System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regexroute.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                //System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
                //string strTemp = objStreamReader.ReadToEnd();
                //int intStratIndex = strTemp.LastIndexOf('^');
                //int intEndIndex = strTemp.LastIndexOf('$');
                //string strMaxNumber = strTemp.Substring(intStratIndex + 1, intEndIndex - intStratIndex - 1);

                //int NewNumber = int.Parse(strMaxNumber);
                //NewNumber = NewNumber + 1;
                //objStreamReader.Close();

                //System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name, true);
                //objStreamWriter.Write("^" + NewNumber.ToString() + "$ = conf/echo" + NewNumber.ToString() + "\r\n");
                //objStreamWriter.Close();
                //objFileStream.Close();
                //return NewNumber.ToString();

                int ConfNumber = int.Parse(strLastConfNumber);
                ConfNumber = ConfNumber + 1;
                strLastConfNumber = ConfNumber.ToString();
                //return VMuktiAPI.VMuktiInfo.strSchemaNumber + strLastConfNumber;
                return strLastConfNumber;
            }
            catch
            {
                return null;
            }
        }

        void ClsYateSuperNode_EntsvcRemoveSIPUser(string strSIPNumber)
        {
            try
            {
                System.IO.FileStream objFileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Assamblies\conf.d\regfile.conf", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                System.IO.StreamReader objStreamReader = new System.IO.StreamReader(objFileStream);
                string strTemp = objStreamReader.ReadToEnd();
                strTemp = strTemp.ToLower().Replace("[" + strSIPNumber + "]", "");
                strTemp = strTemp.ToLower().Replace("password=" + strSIPNumber, "");
                objStreamReader.Close();
                System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(objFileStream.Name);
                objStreamWriter.Write(strTemp);
                objStreamWriter.Close();
                objFileStream.Close();
            }
            catch
            {
            }
        }

    }
}
