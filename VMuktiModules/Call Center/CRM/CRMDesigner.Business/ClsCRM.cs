using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using CRMDesigner.Common;

namespace CRMDesigner.Business
{
    public class ClsCRM : ClsBaseObject
    {
        #region Fields
        private int _ID = ClsConstants.NullInt;
        private string _CRMName = ClsConstants.NullString;
        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string CRMName
        {
            get { return _CRMName; }
            set { _CRMName = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            ID = GetInt(row, "ID");
            CRMName = GetString(row, "CRMName");

            return base.MapData(row);
        }
        #endregion
    }
}
