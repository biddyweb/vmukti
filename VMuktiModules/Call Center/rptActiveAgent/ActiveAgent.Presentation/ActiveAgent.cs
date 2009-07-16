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
