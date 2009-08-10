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
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace rptActiveAgent.Presentation
{
    public class ActiveAgent : INotifyPropertyChanged
    {
        private string _uName;
        private string _Campaign_Id;
        private string _Group_Name;
        private string _Phone_No;
        private string _Status;
        private string _Color;

        public string uName
        {
            get
            {
                return _uName;
            }
            set
            {
                _uName = value;
                OnPropertyChanged("uName");
            }
        }

        public string Campaign_Id
        {
            get
            {
                return _Campaign_Id;
            }
            set
            {
                _Campaign_Id = value;
                OnPropertyChanged("Campaign_Id");
            }
        }

        public string Group_Name
        {
            get
            {
                return _Group_Name;
            }
            set
            {
                _Group_Name = value;
                OnPropertyChanged("Group_Name");
            }
        }

        public string Phone_No
        {
            get
            {
                return _Phone_No;
            }
            set
            {
                _Phone_No = value;
                OnPropertyChanged("Phone_No");
            }
        }

        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        public string Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                OnPropertyChanged("Color");
            }
        }

        public static ActiveAgent Create(string uName, string Camapign_Id, string Group_Name, string Phone_No, string Status,string color)
        {
            ActiveAgent ai = new ActiveAgent();
            ai.uName = uName;
            ai.Campaign_Id = Camapign_Id;            
            ai.Group_Name = Group_Name;
            ai.Phone_No=Phone_No;
            ai.Status = Status;
            ai.Color = color;
            return ai;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string PopName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PopName));
            }
        }

        #endregion



    }
}
