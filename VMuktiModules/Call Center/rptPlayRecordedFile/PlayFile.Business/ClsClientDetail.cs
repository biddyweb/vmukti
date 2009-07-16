using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFile.Common;
using System.Data;

namespace PlayFile.Business
{
    public class ClsClientDetail : ClsBaseObject
    {
        #region Fields

        
        long _LeadID = ClsConstants.NullLong;
        long _LeadFieldID = ClsConstants.NullLong;
        string _PropertyValue = ClsConstants.NullString;
       

        #endregion

        #region Properties

       
        public long LeadID { get { return _LeadID; } set { _LeadID = value; } }
        public long LeadFieldID { get { return _LeadFieldID; } set { _LeadFieldID = value; } }
        public string PropertyValue {get { return _PropertyValue; } set { _PropertyValue=value; } }
 
        #endregion

        public override bool MapData(DataRow row)
        {
            try
            {
               
                //LeadID = GetLong(row, "LeadID");
                //LeadFieldID = GetLong(row, "LeadFieldID");
                PropertyValue = GetString(row, "PropertyValue");
               
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
