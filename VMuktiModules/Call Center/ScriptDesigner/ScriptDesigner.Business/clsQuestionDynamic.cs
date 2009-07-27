/* VMukti 2.0 -- An Open Source Video Communications Suite
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

namespace ScriptDesigner.Business
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
            Header = header;
            NoOfOptions = noOfOptions;
            Type = type;
            Options = options;
        }
    }
}
