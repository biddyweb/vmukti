using System;
using System.Text;
using System.Data;
using VMuktiAPI;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;


namespace VMuktiAPI
{
    public static class ClsLogging
    {
        public static void WriteToTresslog(StringBuilder LogMessage)
        {
            try
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Categories.Clear();
                logEntry.Categories.Add("Trace");
                //logEntry.Priority = 5;
                //logEntry.Severity = TraceEventType.Information;
                logEntry.Message = LogMessage.ToString();
                logEntry.TimeStamp = logEntry.TimeStamp.ToLocalTime();
                Logger.Write(logEntry);                
                VMuktiAPI.VMuktiHelper.CallEvent("SendConsoleMessage", null, new VMuktiEventArgs(LogMessage.ToString()));
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "WriteToTresslog()", "ClsLogging.cs"); 
            }
        }
    }
}
