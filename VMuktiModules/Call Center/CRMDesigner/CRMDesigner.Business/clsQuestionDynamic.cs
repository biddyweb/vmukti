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
using VMuktiAPI;

namespace CRMDesigner.Business
{
    public enum TypeOfOptions
    {
        RadioButton = 0,
        CheckBox = 1,
        ComboBox = 2,
        ListBox = 3,
        TextBox = 4
    }

    public class clsQuestionDynamic
    {
        public static StringBuilder sb1;
        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }
        private int _NoOfOptions = 0;
        private string _Header = "";
        private List<string> _Options = null;
        private TypeOfOptions _Type = TypeOfOptions.RadioButton;

        public int NoOfOptions
        {
            get { return _NoOfOptions; }
            set { _NoOfOptions = value; }
        }

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        public List<string> Options
        {
            get { return _Options; }
            set { _Options = value; }
        }

        public TypeOfOptions Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public clsQuestionDynamic(string header, int noOfOptions, TypeOfOptions type, List<string> options)
        {
            try
            {
                Header = header;
                NoOfOptions = noOfOptions;
                Type = type;
                Options = options;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Business--:--ClsQuestionDynamic.cs--:--clsQuestionDynamic()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
    }
}
