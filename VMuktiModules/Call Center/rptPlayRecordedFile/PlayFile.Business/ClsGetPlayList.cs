using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PlayFile.DataAccess;

namespace PlayFile.Business
{
    public class ClsGetPlayList : ClsBaseObject
    {
        public static System.Data.DataSet GetPlayList()
        {
            try
            {
                return (new PlayFile.DataAccess.ClsPlayList().PlayList());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
