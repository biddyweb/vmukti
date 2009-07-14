using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DashBoard.Presentation.Active
{
    public class ActiveAgent : INotifyPropertyChanged
    {
        private string _uName;
        private string _Campaign_Id;
        private string _Group_Name;
        private string _Phone_No;
        private string _Status;
        private string _Color;
        private string _CallDuration;
        private string _btnBargeContent;
        private string _btnHangUpContent;
        private bool _isEnable;
        private string _Tag;

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
        public string CallDuration
        {
            get
            {
                return _CallDuration;
            }
            set
            {
                _CallDuration = value;
                OnPropertyChanged("CallDuration");
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

        public string BtnBargeContent
        {
            get
            {
                return _btnBargeContent;
            }
            set
            {
                _btnBargeContent = value;
                OnPropertyChanged("BtnBargeContent");
            }
        }

        public string BtnHangUpContent
        {
            get
            {
                return _btnHangUpContent;
            }
            set
            {
                _btnHangUpContent = value;
                OnPropertyChanged("BtnHangUpContent");
            }
        }
        
        public bool Enable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                _isEnable = value;
                OnPropertyChanged("Enable");
            }
        }

        public string Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                _Tag = value;
                OnPropertyChanged("Tag");
            }

        }


        public static ActiveAgent Create(string uName, string Camapign_Id, string Group_Name, string Phone_No, string Status, string color, string CallDuration)
        {
            ActiveAgent ai = new ActiveAgent();
            
            ai.uName = uName;
            ai.Campaign_Id = Camapign_Id;            
            ai.Group_Name = Group_Name;
            ai.Phone_No=Phone_No;
            ai.Status = Status;
            ai.Color = color;
            ai.CallDuration = CallDuration;
            ai.BtnBargeContent = "Barge";
            ai.BtnHangUpContent = "HangUp";
            ai.Tag = uName + "," + Phone_No;

            if (string.Compare(Status.ToLower(), "connected") == 0)
            {
                ai.Enable = true;
            }
            else if (string.Compare(Status.ToLower(), "hold") == 0)
            {
                ai.Enable = true;
            }
            else
            {
                ai.Enable = false;
            }
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
