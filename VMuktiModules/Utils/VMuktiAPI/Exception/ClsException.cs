/*
* 1videoConference -- An open source video conferencing platform.
*
* Copyright (C) 2007 - 2008, Adiance Technologies Pvt. Ltd.
*
* Hardik Sanghvi <hardik.sanghvi @adiance.com>
*
* See http://www.1videoconference.org for more information about
* the 1videoConference project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.
*
* This program is free software, distributed under the terms of
* the GNU General Public License Version 2. See the LICENSE file
* at the top of the source tree.
*/
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VMuktiAPI
{
    /// <summary>
    /// Summary description for Exception
    /// </summary>
    public static class ClsException
    {
        public static StringBuilder sb1;
        public static void LogError(Exception ex)
        {
            //try
            //{
                //System.Threading.Thread thrdError = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(PunchError));
                //thrdError.Priority = System.Threading.ThreadPriority.Lowest;
                //thrdError.IsBackground = true;
                //thrdError.Start(ex);
                //thrdError.Join();
            //}
            //catch (Exception exc)
            //{
                //VMuktiHelper.ExceptionHandler(exc, "LogError()", "clsException.cs");
                //ClsException.WriteToErrorLogFile(exc);
              
            //}

        }

        //private static void PunchError(object objException)
        //{
            //try
            //{
                //Exception localobjException  = objException as Exception;
                //WriteToDatabase(localobjException);
                //WriteToErrorLogFile(localobjException);
                //WriteToEventLog(localobjException);

            //}
            //catch (Exception exc)
            //{
            //    VMuktiHelper.ExceptionHandler(exc, "PunchError()", "clsException.cs");
            //    ClsException.WriteToErrorLogFile(exc);
            //}

        //}

        //private static void WriteToDatabase(Exception ex)
        //{
        //    try
        //    {
        //        SqlParameter ParameterMsg = new SqlParameter("@Msg", System.Data.SqlDbType.VarChar);
        //        if (ex.Message != null)
        //        {
        //            ParameterMsg.Value = ex.Message;
        //        }
        //        else
        //        {
        //            ParameterMsg.Value = "Nill";
        //        }

        //        SqlParameter ParameterStackTrace = new SqlParameter("@StackTrace", System.Data.SqlDbType.VarChar);
        //        if (ex.StackTrace != null)
        //        {
        //            ParameterStackTrace.Value = ex.StackTrace;
        //        }
        //        else
        //        {
        //            ParameterStackTrace.Value = "Nill";
        //        }

        //        SqlParameter ParameterAdditionalInfo = new SqlParameter("@AdditionalInfo", System.Data.SqlDbType.VarChar);
        //        if (ex.InnerException != null && ex.InnerException.Message != null)
        //        {
        //            ParameterAdditionalInfo.Value = ex.InnerException.Message;
        //        }
        //        else
        //        {
        //            ParameterAdditionalInfo.Value = "Nill";
        //        }


        //        SqlParameter ParameterSource = new SqlParameter("@Source", System.Data.SqlDbType.VarChar);
        //        if (ex.Source != null)
        //        {
        //            ParameterSource.Value = ex.Source;
        //        }
        //        else
        //        {
        //            ParameterSource.Value = "Nill";
        //        }

        //        SqlParameter ParameterLocation = new SqlParameter("@Location", System.Data.SqlDbType.VarChar);
        //        if (ex.Data != null && ex.Data["My Key"] != null)
        //        {
        //            ParameterLocation.Value = ex.Data["My Key"].ToString();
        //        }
        //        else
        //        {
        //            ParameterLocation.Value = "Nill";
        //        }

        //        SqlParameter ParameterDate = new SqlParameter("@Date", System.Data.SqlDbType.DateTime);
        //        ParameterDate.Value = DateTime.Now;

        //        //SqlHelper.ExecuteNonQuery(Database.conStr, CommandType.StoredProcedure, "sp_Exception_Add", ParameterStackTrace, ParameterAdditionalInfo, ParameterMsg, ParameterSource, ParameterLocation, ParameterDate);
        //    }
        //    catch (Exception exc)
        //    {
        //        VMuktiHelper.ExceptionHandler(exc, "WritetoDatabase()", "clsException.cs");
        //        ClsException.WriteToErrorLogFile(exc);
        //    }

        //}

        public static void WriteToErrorLogFile(Exception ex)
        {
            //try
            //{
                //FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\errlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //StreamWriter sw = new StreamWriter(fs);
                //while (fs.ReadByte() != -1)
                //{ }
                //sw.Write("Title: Error" + sw.NewLine);
                //sw.Write("Error Message: " + ex.Message + sw.NewLine);
                //sw.Write("StackTrace is as follow: " + sw.NewLine + ex.StackTrace + sw.NewLine);
                //sw.Write("Date - Time: " + DateTime.Now.ToString() + sw.NewLine);
                //sw.Write("Location: " + ex.Data["My Key"].ToString() + sw.NewLine);
                //sw.Write("===========================================================================================" + sw.NewLine);
                    //sw.Close();
                //fs.Close();
            //}
            //catch (Exception exc)
            //{
                //VMuktiHelper.ExceptionHandler(exc, "WriteToErrorLogFile()", "clsException.cs");
                //ClsException.WriteToLogFile(exc.Message);
            //}

        }

        //private static void WriteToEventLog(Exception ex)
        //{
        //    try
        //    {
        //        EventLog objEventLog = new EventLog();
        //        if (!(EventLog.SourceExists("VMukti")))
        //        {
        //            EventLog.CreateEventSource("VMukti", "VMukti");
        //        }
        //        objEventLog.Source = "VMukti";
        //        objEventLog.WriteEntry("Error Message: " + ex.Message + "\r\n\nStackTrace is as follow:" + Char.ConvertFromUtf32(13) + ex.StackTrace, EventLogEntryType.Error);
        //    }
        //    catch (Exception exc)
        //    {
        //        VMuktiHelper.ExceptionHandler(exc, "WriteToEventLog()", "clsException.cs");
        //        ClsException.WriteToErrorLogFile(exc);
        //    }

        //}

        public static void WriteToLogFile(string ex)
        {
            try
            {
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\VmuktiSysInfo.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                while (fs.ReadByte() != -1)
                { }
                sw.Write("Title: Error" + sw.NewLine);
                sw.Write("Error Message: " + ex + sw.NewLine);
                sw.Write("StackTrace is as follow: " + sw.NewLine + sw.NewLine);
                sw.Write("Date - Time: " + DateTime.Now.ToString() + sw.NewLine);
                sw.Write("===========================================================================================");
                sw.Close();
                fs.Close();
            }
            catch (Exception exc)
            {
                ClsException.LogError(exc);
            }

        }
    }
}
