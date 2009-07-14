using System;
using ScriptDesigner.DataAccess;
using VMuktiAPI;
namespace ScriptDesigner.Business
{
    public class ClsDispositionCollection : ClsBaseCollection<ClsDisposition>
    {
        public static ClsDispositionCollection GetAll(Int64 DespListID)
        {
            try
            {
                ClsDispositionCollection obj = new ClsDispositionCollection();
                obj.MapObjects(new ClsQuestionAnsDataService().Disposition_GetAll(DespListID));
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAll", "ClsDispositionCollection.cs");
                return null;
            }
        }

    }
}
