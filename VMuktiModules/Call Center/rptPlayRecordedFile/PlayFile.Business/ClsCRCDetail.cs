using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFile.Common;
using System.Data;

namespace PlayFile.Business
{
    public class ClsCRCDetail : ClsBaseObject
    {
        #region Fields

        //long _ID = ClsConstants.NullLong;
        string _DespositionName = ClsConstants.NullString;
       
        #endregion

        #region Properties

        // public long ID { get { return _ID; } set { _ID = value; } }
        public string DespositionName { get { return _DespositionName; } set { _DespositionName = value; } }
       

        #endregion

        public override bool MapData(DataRow row)
        {
            try
            {
                //ID = GetLong(row, "ID");
                DespositionName = GetString(row, "DespositionName");
                
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
