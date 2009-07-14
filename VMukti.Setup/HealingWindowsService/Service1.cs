using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace HealingWindowsService
{
    public partial class Service1 : ServiceBase
    {

        System.Timers.Timer tNetAvailability = new System.Timers.Timer(30000);
        bool startBS = false;
        string currentSuperNodeIP = "";
       

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                //read the configuration.xaml file to retrieve bootstrap ip -- saloni
                System.Xml.XmlDocument ConfDocIPAdd = new System.Xml.XmlDocument();
                string[] strLine = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Installation.txt");
                ConfDocIPAdd.Load(strLine[0] + "Configuration.xml");

                //reading bootstrap ip -- saloni
                System.Xml.XmlNodeList xmlNodes = null;
                xmlNodes = ConfDocIPAdd.GetElementsByTagName("BootStrpIP");
                currentSuperNodeIP = xmlNodes[0].Attributes["Value"].Value.ToString();

                EventLog el12 = new EventLog("Application", Environment.MachineName, "Healing Service");
                el12.WriteEntry("Bootstrap IP is  " + currentSuperNodeIP + " at " + DateTime.Now);
                el12.Close();


                //timers to check for connection to bootstrap at regular interval -- saloni
                tNetAvailability.Elapsed += new System.Timers.ElapsedEventHandler(tNetAvailability_Elapsed);
                tNetAvailability.Start();
                EventLog el2 = new EventLog("Application", Environment.MachineName, "Healing Service");
                el2.WriteEntry("Timer started at " + DateTime.Now);
                el2.Close();
            }
            catch (Exception exp)
            {
                EventLog el2 = new EventLog("Application", Environment.MachineName, "Healing Service");
                el2.WriteEntry("EXP  " + exp.Message + " at " + DateTime.Now);
                if (exp.InnerException != null)
                {
                    el2.WriteEntry("INNER EXP  " + exp.InnerException.Message + DateTime.Now);
                }
                el2.Close();
            }

        }

        protected override void OnStop()
        {
            try
            {
                EventLog el10 = new EventLog("Application", Environment.MachineName, "Healing Service");
                el10.WriteEntry("Stopping the HealingVMuktiService at  " + DateTime.Now);
                el10.Close();

                //stop the timer on stopping healing service -- saloni
                tNetAvailability.Stop();
                tNetAvailability = null;
                
            }
            catch (Exception exp)
            {
                EventLog el = new EventLog("Application", Environment.MachineName, "Healing Service");
                el.WriteEntry("exp in ONSTOP of HealingService " + DateTime.Now);
                el.Close();
            }
        }

        //timers elapsed event fired at every 30 secs to check for availability of bootstrap -- saloni
        void tNetAvailability_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {    
            //variable set to check whether to start/restart the bootstrap if any error occured -- saloni
            if (startBS)
            {
                EventLog el = new EventLog("Application", Environment.MachineName, "Healing Service");
                el.WriteEntry("Checking whether BS is running " + DateTime.Now);
                el.Close();


                //retrieving the machine name and the bootstrap service name to start/restart -- saloni
                ServiceController controller = new ServiceController();
                controller.ServiceName = "VMuktiBootstrap";
                controller.MachineName = currentSuperNodeIP;

                try
                {
                    //retrieving the status of bootstrap whether already started and having error or not started at all -- saloni
                    string status = controller.Status.ToString();

                    EventLog elstat = new EventLog("Application", Environment.MachineName, "Healing Service");
                    elstat.WriteEntry("VMuktiBootstrap service status is  " + status + " at " + DateTime.Now);
                    elstat.Close();

                    //if running, stop the bootstrap service and start again as such not able to connect -- saloni
                    if (status == "Running")
                    {
                        controller.Stop();
                        controller.Start();

                        EventLog el1 = new EventLog("Application", Environment.MachineName, "Healing Service");
                        el1.WriteEntry("VMuktiBootstrap service is RESTART at " + DateTime.Now);
                        el1.Close();
                    }
                    //else start the bootstrap service as such was stopped due to some unavoidable error -- saloni
                    else
                    {
                        controller.Start();
                        EventLog el2 = new EventLog("Application", Environment.MachineName, "Healing Service");
                        el2.WriteEntry("VMuktiBootstrap service is STARTED at " + DateTime.Now);
                        el2.Close();
                    }

                    //set the variable again to false -- saloni
                    startBS = false;
                }
                catch (Exception exp)
                {
                    EventLog elexp = new EventLog("Application", Environment.MachineName, "Healing Service");
                    elexp.WriteEntry("EXP is  " + exp.Message + " at " + DateTime.Now);
                    elexp.Close();
                }
            }
            else
            {
                /// Url of bootstrap to check for network connection,wcf service in faulted state or error in bootstrap -- saloni
                Uri objuri = new Uri("http://" + currentSuperNodeIP + ":" + "80/HttpBootstrap");                
                System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(objuri); 
                System.Net.WebResponse objResp;
                try
                {
                    //Pinging the boostrap -- saloni
                    objResp = objWebReq.GetResponse();
                    objResp.Close();
                }
                catch (Exception ex)
                {
                    //Unable to connect to bootstrap so set the variable to true to convey to start/restart bootstrap at another interval of timer -- saloni
                    startBS = true;
                    EventLog el4 = new EventLog("Application", Environment.MachineName, "Healing Service");
                    el4.WriteEntry("Exception  " + ex.Message + DateTime.Now);
                    if (ex.InnerException != null)
                    {
                        el4.WriteEntry("InnerException  " + ex.InnerException.Message + DateTime.Now);
                    }
                    el4.Close();
                }
            }
        }
    }
}
