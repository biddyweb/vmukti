using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFile.Common;
using System.Data;

namespace PlayFile.Business
{
    public class ClsCampaignDetails : ClsBaseObject
    {
        #region Fields

        //long _ID = ClsConstants.NullLong;
        string _Name = ClsConstants.NullString;
       

        #endregion

        #region Properties

        //public long ID { get { return _ID; } set { _ID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
       

        #endregion

        public override bool MapData(DataRow row)
        {
            try
            {
                //ID = GetLong(row, "ID");
                Name = GetString(row, "Name");
               
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
