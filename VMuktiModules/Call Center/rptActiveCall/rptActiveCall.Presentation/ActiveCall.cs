using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace rptActiveCall.Presentation
{
    public class ActiveCall:INotifyPropertyChanged
    {
        private string _uName;
        private string _Campaign_Id;
        private string _Status;
        private string _Group_Name;
        private string _Phone_No;
        private string _callDuration;

        public string uName
        {
            get { return _uName; }
            set
            {
                _uName = value;
                OnPropertyChanged("uName");
            }
            
        }
        public string Campaign_Id
        {
            get { return _Campaign_Id; }
            set
            {
                _Campaign_Id = value;
                OnPropertyChanged("Campaign_Id");
            }
        }
        public string Status
        {
            get { return _Status; }
            set { _Status = value;
                  OnPropertyChanged("Status");  }
        }
        public string Group_Name
        {
            get { return _Group_Name; }
            set { _Group_Name = value;
            OnPropertyChanged("Group_Name");
            }
        }
        public string Phone_No
        {
            get { return _Phone_No; }
            set
            {
                _Phone_No = value;
                OnPropertyChanged("Phone_No");
            }
        }
        public string callDuration
        {
            get { return _callDuration; }
            set { _callDuration = value;
            OnPropertyChanged("callDuration");
            }
        }

        public static ActiveCall Create(string uName, string Camapign_Id, string Status,string Group_Name,string Phone_No,string callDuration)
        {
            ActiveCall ai = new ActiveCall();
            ai.uName = uName;
            ai.Campaign_Id = Camapign_Id;
            ai.Status = Status;
            ai.Group_Name = Group_Name;
            ai.Phone_No = Phone_No;
            ai.callDuration = callDuration;
            return ai;
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string PopName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(PopName));
            }
        }

        #endregion
    }
}
