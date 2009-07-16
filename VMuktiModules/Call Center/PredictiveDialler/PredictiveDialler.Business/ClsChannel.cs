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
using PredictiveDialler.DataAccess;
using PredictiveDialler.Common;
using VMuktiAPI;

namespace PredictiveDialler.Business
{

    public class ClsChannel : ClsBaseObject
    {
        public ClsChannel()
        {}        

        #region Fields

        private string _ChannelID = PredictiveDialler.Common.ClsConstants.NullString;
        private long _LeadID = PredictiveDialler.Common.ClsConstants.NullLong;
        private DateTime _CalledDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private DateTime _StartDate = PredictiveDialler.Common.ClsConstants.NullDateTime;
        private string _StartTime = PredictiveDialler.Common.ClsConstants.NullString;
        private int _CallDuration = PredictiveDialler.Common.ClsConstants.NullInt;
        private long _CurrentCampainID = PredictiveDialler.Common.ClsConstants.NullLong;
        private long _DispositionID = PredictiveDialler.Common.ClsConstants.NullLong;
        private CallStatus _CallResult = CallStatus.NotInCall;
        private long _ConfID = PredictiveDialler.Common.ClsConstants.NullLong;
        private string _CallNote = PredictiveDialler.Common.ClsConstants.NullString;
        private bool _IsDNC = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private bool _IsGlobal = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private long _UserID = PredictiveDialler.Common.ClsConstants.NullLong;
        private string _RecordedFileName = PredictiveDialler.Common.ClsConstants.NullString;
        private long _CurrentPhoneNo = PredictiveDialler.Common.ClsConstants.NullLong;
        private long _ListID = PredictiveDialler.Common.ClsConstants.NullLong;
        private bool _CallSucess = PredictiveDialler.Common.ClsConstants.NullBoolean;
        private string _CurrentDialStatus = PredictiveDialler.Common.ClsConstants.NullString;
        private long _CountryCode = PredictiveDialler.Common.ClsConstants.NullLong;
        private string _CampaignPrefix = PredictiveDialler.Common.ClsConstants.NullString;

        #endregion

        public enum CallStatus
        {
            NotInCall, CallInProgress, DialingToDest, RingingToDest,
            BusyTone, AMD, DTMF, DestUserStratListen, CallHangUp, CallHoldByAgent,
            CallHoldBySys, CallDispose, ReadyState
        };        
        
        #region Property

        public string CurrentDialStatus
        {
            get { return _CurrentDialStatus; }
            set { _CurrentDialStatus = value; }
        }
        public long LeadID
        {
            get { return _LeadID; }
            set { _LeadID = value; }

        }
        public DateTime CalledDate
        {
            get { return _CalledDate; }
            set { _CalledDate = value; }

        }
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        public int CallDuration
        {
            get { return _CallDuration; }
            set { _CallDuration = value; }
        }

        public long CurrentCampainID
        {
            get { return _CurrentCampainID; }
            set { _CurrentCampainID = value; }
        }
        public long DispositionID
        {
            get { return _DispositionID; }
            set { _DispositionID = value; }
        }
        public long ConfID
        {
            get { return _ConfID; }
            set { _ConfID = value; }
        }

        public string CallNote
        {
            get { return _CallNote; }
            set { _CallNote = value; }

        }
       
     
        public long CurrentPhoneNo
        {
            get { return _CurrentPhoneNo; }
            set { _CurrentPhoneNo = value; }
        }
        public bool IsDNC
        {
            get { return _IsDNC; }
            set { _IsDNC = value; }

        }

        public string RecordedFileName
        {
            get { return _RecordedFileName; }
            set { _RecordedFileName = value; }
        }

        public bool IsGlobal
        {
            get { return _IsGlobal; }
            set { _IsGlobal = value; }

        }

        public long UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string ChannelID
        {
            get { return _ChannelID; }
            set { _ChannelID = value; }
        }     
      
        public CallStatus CallResult
        {
            get { return _CallResult; }
            set
            {
                try
                {
                    _CallResult = value;

                    //TimeSpan t;
                    switch (value)
                    {
                        case CallStatus.NotInCall:
                            break;

                        case CallStatus.ReadyState:
                            CalledDate = DateTime.Now;
                            StartDate = DateTime.Now;
                            StartTime = DateTime.Now.ToString();
                            ClsException.WriteToLogFile("ClsChannel():- CallStatus=ReadyState n goin to call 'Dial' event with ph=" + CurrentPhoneNo.ToString());
                            if (CurrentDialStatus == "Predictive")
                            {
                                CurrentPhoneNo = long.Parse(CampaginPrefix + CountryCode + CurrentPhoneNo.ToString());
                                VMuktiHelper.CallEvent("Dial", this, new VMuktiEventArgs(CurrentPhoneNo, ChannelID));
                            }
                            else
                            {
                                VMuktiHelper.CallEvent("Dial", this, new VMuktiEventArgs(CurrentPhoneNo, ChannelID));
                            }
                            break;

                        case CallStatus.DialingToDest:
                            //Wait for the new status
                            //or pass this dialing msg to the server or upderbase calss so it can take desion on it
                            break;

                        case CallStatus.CallInProgress:
                            StartTime = DateTime.Now.ToString();
                            //make get perameter how much time it take to call in progress after sometime
                            //or set the perameter of dialer channes to get back after some time with new status
                            break;

                        case CallStatus.BusyTone:
                            //VMuktiHelper.CallEvent("SetStatus4Channel", this, new VMuktiEventArgs(ChannelID, CallStatus.CallHangUp.ToString()));
                            break;

                        case CallStatus.AMD:
                            //VMuktiHelper.CallEvent("SetStatus4Channel", this, new VMuktiEventArgs(ChannelID, CallStatus.CallHangUp.ToString()));
                            break;

                        case CallStatus.DTMF:
                            //set this as a peroperties not as a status
                            break;

                        case CallStatus.DestUserStratListen:
                            //set this chanales has busy or something like live call ditechion
                            break;

                        case CallStatus.CallHoldByAgent:
                            break;

                        case CallStatus.CallHoldBySys:
                            //VMuktiHelper.CallEvent("TransferCall", this, new VMuktiEventArgs(ChannelID, long.Parse("11111")));
                            break;

                        case CallStatus.CallHangUp:
                            //if (CurrentDialStatus == "Predictive")
                            //{
                            //    string endtime = DateTime.Now.ToString();
                            //    TimeSpan duration = (DateTime.Parse(endtime) - DateTime.Parse(StartTime));
                            //    CallDuration = duration.Seconds;
                                //int tmpID = -1;

                                //new Dialer.DataAccess.ClsUserDataService().User_Save(ref tmpID, LeadID, CalledDate, StartDate, DateTime.Parse(StartTime), long.Parse(CallDuration.ToString()), DispositionID, CurrentCampainID, ConfID, CallNote, IsDNC, IsGlobal, UserID);
                                ////  new Dialer.DataAccess.ClsUserDataService().UpdateCurrentLeadStatus(LeadID, DispositionID);
                                //CallResult = CallStatus.NotInCall;
                                //StartTime = ClsConstants.NullString;
                                //VMuktiHelper.CallEvent("GetNextLead", this, new VMuktiEventArgs(ChannelID));
                            //}
                            break;

                        case CallStatus.CallDispose:
                            if (CurrentDialStatus == "Predictive")
                            {
                                string endtime = DateTime.Now.ToString();
                                TimeSpan duration = (DateTime.Parse(endtime) - DateTime.Parse(StartTime));
                                CallDuration = duration.Seconds;
                            }
                            //VMuktiHelper.CallEvent("SetStatus4Channel", this, new VMuktiEventArgs(ChannelID, CallStatus.ReadyState.ToString()));
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--AutoDialer--:--ClsLeadCollection.cs--:--GetAll--");
                    ClsException.LogError(ex);
                    ClsException.WriteToErrorLogFile(ex);
                }
            }
        }

        public long ListID
        {
            get { return _ListID; }
            set { _ListID = value; }

        }
        
        public long CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }

        public string CampaginPrefix
        {
            get { return _CampaignPrefix; }
            set { _CampaignPrefix = value; }
        }
        
       

        #endregion
       
    }
}
