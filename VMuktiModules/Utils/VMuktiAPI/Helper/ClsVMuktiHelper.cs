using System;
using System.Collections.Generic;
using System.Text;
using VMuktiService;

namespace VMuktiAPI
{
    public static partial class VMuktiHelper
    {

        public static double RectSuggestHeight = 25.0;
        public static bool IsDraggingPOD;
        public static bool IsRectSuggestAdded;
        
        public static List<VMuktiEvents> VMEvents = new List<VMuktiEvents>();

        public static List<NetPeerServer> VMuktiServers = new List<NetPeerServer>();

        public static void CallEvent(string eventName, object sender, VMuktiEventArgs e)
        {
            foreach (VMuktiEvents ve in VMEvents)
            {
                if (ve.EventName.Equals(eventName))
                {
                    ve.FireVMuktiEvent(sender, e);
                    break;
                }
            }
        }

        public static void ExceptionHandler(Exception ex, string _FunctionName, string _FileName)
        {
            StringBuilder sb1;
            ex.Data.Add("My Key", _FunctionName + "--:--" + _FileName + "--:--" + ex.Message + " :--:--");
            System.Text.StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine();
            sb.AppendLine("StackTrace : " + ex.StackTrace);
            sb.AppendLine();
            sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
            sb.AppendLine();
            sb1 = VMuktiAPI.VMuktiHelper.CreateTressInfo();
            sb.Append(sb1.ToString());
            VMuktiAPI.ClsLogging.WriteToTresslog(sb);
        }

        //public static void TraceHandler(string msg)
        //{
        //    StringBuilder sb1;
        //    System.Text.StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(msg);
        //    sb.AppendLine();
        //    sb1 = VMuktiAPI.VMuktiHelper.CreateTressInfo();
        //    sb.Append(sb1.ToString());
        //    VMuktiAPI.ClsLogging.WriteToTresslog(sb);
        //}


        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            ClsPeer peer = VMuktiAPI.VMuktiInfo.CurrentPeer;
            sb.Append("User Is : ");
            sb.Append(peer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : ");
            sb.Append(peer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : ");
            sb.Append(peer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : ");
            sb.Append(VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }

        public static VMuktiEvents RegisterEvent(string eventName)
        {
            bool isEventExists = false;
            int i = 0;
        
            for (i = 0; i < VMEvents.Count; i++)
            {
                if (VMEvents[i].EventName.Equals(eventName))
                {
                    isEventExists = true;
                    break;
                }
            }
            if (!isEventExists)
            {
                VMEvents.Add(new VMuktiEvents(eventName));
                return VMEvents[VMEvents.Count - 1];
            }
            else
            {
                return VMEvents[i];
            }

        }

        public static void UnRegisterEvent(string eventName)
        {
            for(int i = 0 ;i<VMEvents.Count;i++)
            {
                if (VMEvents[i].EventName == eventName)
                {
                    VMEvents.RemoveAt(i);
                    break;
                }
            }

        }

        public static string[] StartAServer()
        {
            try
            {
                VMuktiServers.Add(new NetPeerServer("net.tcp://" + Environment.MachineName + ":2500/" + VMuktiServers.Count.ToString()));
                VMuktiServers[VMuktiServers.Count - 1].AddEndPoint("net.tcp://" + Environment.MachineName + ":2500/" + Convert.ToString(VMuktiServers.Count - 1));
                VMuktiServers[VMuktiServers.Count - 1].Name = "net.tcp://" + Environment.MachineName + ":2500/" + Convert.ToString(VMuktiServers.Count - 1);
                VMuktiServers[VMuktiServers.Count - 1].OpenServer();

                return new string[] { "net.tcp://" + Environment.MachineName + ":2500/" + Convert.ToString(VMuktiServers.Count - 1), VMuktiAPI.VMuktiInfo.CurrentPeer.MeshID + Convert.ToString(VMuktiServers.Count - 1) };
            }
            catch
            {
                return null;
            }
        }

        public static void StopAServer(string URI)
        {
            for (int i = 0; i < VMuktiServers.Count; i++)
            {
                if (VMuktiServers[i].Name == URI)
                {
                    VMuktiServers[i].CloseServer();
                    break;
                }
            }
        }
    }

    #region Add Events
    public class VMuktiEvents
    {
        public string EventName = "";
        public delegate void VMuktiEventHandler(object sender, VMuktiEventArgs e);
        public event VMuktiEventHandler VMuktiEvent;

        public VMuktiEvents(string eventName)
        {
            EventName = eventName;
        }

        public void FireVMuktiEvent(object sender, VMuktiEventArgs e)
        {
            if (VMuktiEvent != null)
            {
                VMuktiEvent(sender, e);
            }
        }


    }
    #endregion

    # region VMukti Event Args
    public class VMuktiEventArgs : EventArgs
    {
        public List<object> _args = new List<object>();
        public VMuktiEventArgs(params object[] parameters)
        {
            _args.AddRange(parameters);
            
        }
    }
    #endregion

    public class ModuleServer
    {
    }
}
