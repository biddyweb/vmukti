using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using ScriptRender.Business;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using VMuktiAPI;
using VMuktiService;
using System.Text;

namespace ScriptRender.Presentation
{

    //public enum ModulePermissions
    //{
    //    View = 1
    //}

    public partial class CtlScriptRender : System.Windows.Controls.UserControl
    {

        //ModulePermissions[] _MyPermissions;
        public ArrayList al = new ArrayList();
        string str;
        string strModPath;
        string filename;
        string destination;
        string filePath;
        public static IHTTPFileTransferService clientHttpFileTransfer = null;
        object obj1 = null;

        public CtlScriptRender()
        {
            this.InitializeComponent();

            //_MyPermissions = MyPermissions;
            //FncPermissionsReview();
  
           
           // DownloadAndExtractZipFile();
            FncLoadScript();
            //VMuktiAPI.VMuktiHelper.RegisterEvent("SetLeadIDScript").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(CtlScriptRender_LoadScript);
            //this.Unloaded += new RoutedEventHandler(CtlScriptRender_Unloaded);

        }

        //void FncPermissionsReview()
        //{
        //    this.Visibility = Visibility.Hidden;

        //    for (int i = 0; i < _MyPermissions.Length; i++)
        //    {
        //        if (_MyPermissions[i] == ModulePermissions.View)
        //        {
        //            this.Visibility = Visibility.Visible;
        //        }
        //    }
        //}
       
        int DownloadAndExtractZipFile()
        {
            #region Download and Extract Zip file
            try
            {





                str = ScriptName;
                str += "_Script";

                Assembly ass = Assembly.GetEntryAssembly();
                // this maybe something like "using Vmuktiapi"
                filename = str + ".zip";

                #region Download Zip File using WCF FileTranserService

                try
                {
                    BasicHttpClient bhcFts = new BasicHttpClient();
                    clientHttpFileTransfer = (IHTTPFileTransferService)bhcFts.OpenClient<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                    clientHttpFileTransfer.svcHTTPFileTransferServiceJoin();
                    DownloadRequest request = new DownloadRequest();
                    RemoteFileInfo rfi = new RemoteFileInfo();

                    request.FileName = filename;
                    request.FolderWhereFileIsStored = "Scripts";
                    rfi = clientHttpFileTransfer.svcHTTPFileTransferServiceDownloadFile(request);

                

                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts"));
                    }
                    destination = ass.Location.Replace("VMukti.Presentation.exe", @"Scripts");

                 
                    filePath = destination + "\\" + filename;

                    System.IO.Stream inputStream = rfi.FileByteStream;


                    using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, FileShare.ReadWrite))
                    {
                        int chunkSize = 2048;
                        byte[] buffer = new byte[chunkSize];

                        do
                        {
                            // read bytes from input stream
                            int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                            if (bytesRead == 0) break;

                            // write bytes to output stream
                            writeStream.Write(buffer, 0, bytesRead);

                        } while (true);
                        writeStream.Close();
                    }
                    //}
                }
                catch (Exception ex)
                {
                    return -1;
                    MessageBox.Show(ex.Message+"1");
                    
                }

                #endregion

                #region Downloading ZipFile Using WebClient  -----Commented


                #endregion

                #region Extracting

                if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules")))
                {
                    Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules"));
                }



              
                try
                {
                    str = ScriptName;
                    str += "_Script";

                    Assembly ass2 = Assembly.GetEntryAssembly();

                    strModPath = ass2.Location.Replace("VMukti.Presentation.exe", @"ScriptModules");

                
                    
                  
                   
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }





            
                ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();

               // if (!Directory.Exists(strModPath + "\\" + str  ))
                {
                    fz.ExtractZip(destination + "\\" + filename, strModPath+ "\\"+str, null);
                   
                }
                
              

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DownloadAndExtractZipFile", "CtlScriptRender.xaml.cs");
                return -1;
            }
            #endregion
            return 0;
        }


        string ScriptName = ClsScriptRender.GetZipName(VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID);

       /* void DownloadAndExtractZipFile()
        {
            #region Download and Extract Zip file
            try
            {
                str = ClsScriptRender.GetZipName(VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID);
                str += "_Script";
                Assembly ass = Assembly.GetEntryAssembly();

                filename = str + ".zip";

                #region Download Zip File using WCF FileTranserService

                try
                {
                    BasicHttpClient bhcFts = new BasicHttpClient();
                    clientHttpFileTransfer = (IHTTPFileTransferService)bhcFts.OpenClient<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                    clientHttpFileTransfer.svcHTTPFileTransferServiceJoin();

                    DownloadRequest request = new DownloadRequest();
                    RemoteFileInfo rfi = new RemoteFileInfo();
                    request.FileName = filename;
                    request.FolderWhereFileIsStored = "Scripts";
                    rfi = clientHttpFileTransfer.svcHTTPFileTransferServiceDownloadFile(request);


                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts"));
                    }
                    destination = ass.Location.Replace("VMukti.Presentation.exe", @"Scripts");

                    if (!File.Exists((destination + "\\" + filename)))
                    {
                    filePath = destination + "\\" + filename;

                    System.IO.Stream inputStream = rfi.FileByteStream;


                        using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, FileShare.ReadWrite))
                    {
                        int chunkSize = 2048;
                        byte[] buffer = new byte[chunkSize];

                        do
                        {
                            // read bytes from input stream
                            int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                            if (bytesRead == 0) break;

                            // write bytes to output stream
                            writeStream.Write(buffer, 0, bytesRead);

                        } while (true);

                        writeStream.Close();
                    }
                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                #endregion

                #region Downloading ZipFile Using WebClient  -----Commented

                //try
                //{
                //    Uri u = new Uri("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/VMukti/Scripts/" + filename);
                //    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts")))
                //    {
                //        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts"));
                //    }
                //    destination = ass.Location.Replace("VMukti.Presentation.exe", @"Scripts");

                //    if (File.Exists((destination + "\\" + filename)))
                //    {
                //        File.Delete((destination + "\\" + filename));
                //    }

                //    if (!File.Exists(destination + "\\" + filename))
                //    {
                //        WebClient wc = new WebClient();
                //        wc.DownloadFile(u, destination + "\\" + filename);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                #endregion

                #region Extracting

                if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules")))
                {
                    Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules"));
                }

                strModPath = ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules");
                FastZip fz = new FastZip();

                if (!Directory.Exists(strModPath + "\\" + filename.Split('.')[0]))
                {
                    fz.ExtractZip(destination + "\\" + filename, strModPath, null);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }


        */
        //public void ClosePod()
        //{
        //    try
        //    {
        //        MessageBox.Show("Close pod called for ScriptRender");
        //        cnvLayoutRoot.Children.Clear();

        //    }
        //    catch (Exception exp)
        //    {

        //        exp.Data.Add("My Key", "ClosePod()--:--ctlScriptRender.xaml.cs--:--" + exp.Message + " :--:--");
        //        ClsException.LogError(exp);
        //        //ClsException.WriteToErrorLogFile(exp);

        //    }
        //}

        public void ShowDirectory(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles("*.dll"))
            {
                int hj = al.Add(file.FullName);
            }
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                ShowDirectory(subDir);
            }
        }

        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}

        //void CtlScriptRender_LoadScript(object sender, VMuktiAPI.VMuktiEventArgs e)
        void FncLoadScript()
        {
            try
            {
                Assembly ass3 = Assembly.GetEntryAssembly();
                if (Directory.Exists(ass3.Location.Replace("VMukti.Presentation.exe", @"Scripts")))
                {
                    Directory.Delete(ass3.Location.Replace("VMukti.Presentation.exe", @"Scripts"), true);
                }
            }
            catch (Exception e)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "FncLoadScript", "CtlScriptRender.xaml.cs");
            }
            try
            {
                Assembly ass1 = Assembly.GetEntryAssembly();
                if (Directory.Exists(ass1.Location.Replace("VMukti.Presentation.exe", @"ScriptModules")))
                {
                    Directory.Delete(ass1.Location.Replace("VMukti.Presentation.exe", @"ScriptModules"), true);
                }
            }
            catch (Exception e)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "FncLoadScript", "CtlScriptRender.xaml.cs");
            }

             #region Loading ReferencedAssemblies
            try
            {


                if (DownloadAndExtractZipFile() == -1)
                {
                    return;
                }

                al.Clear();
              

                DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + str);
                ShowDirectory(dirinfomodule);

                

                for (int j = 0; j < al.Count; j++)
                {
                    string[] arraysplit = al[j].ToString().Split('\\');
                  //  if (arraysplit[arraysplit.Length - 1].ToString() == ScriptName + "_Script" + g + ".Presentation.dll")
                    if (arraysplit[arraysplit.Length - 1].ToString().Contains( ".Presentation.dll"))
                    {
                        
                        Assembly a;
                        Assembly assbal;
                        AssemblyName[] anbal;
                        AssemblyName[] an;

                        a = Assembly.LoadFrom(al[j].ToString());
                        an = a.GetReferencedAssemblies();
                        for (int alcount = 0; alcount < al.Count; alcount++)
                        {
                            string strsplit = al[alcount].ToString();
                            string[] strold = strsplit.Split('\\');
                            string strnew = strold[strold.Length - 1].Substring(0, strold[strold.Length - 1].Length - 4);

                            for (int asscount = 0; asscount < an.Length; asscount++)
                            {
                                if (an[asscount].Name == strnew)
                                {
                                    assbal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[alcount].ToString()).GetName());
                                    anbal = assbal.GetReferencedAssemblies();
                                    for (int andal = 0; andal < al.Count; andal++)
                                    {
                                        string strsplitdal = al[andal].ToString();
                                        string[] strolddal = strsplitdal.Split('\\');
                                        string strnewdal = strolddal[strolddal.Length - 1].Substring(0, strolddal[strolddal.Length - 1].Length - 4);

                                        for (int asscountdal = 0; asscountdal < anbal.Length; asscountdal++)
                                        {
                                            if (anbal[asscountdal].Name == strnewdal)
                                            {
                                                System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[andal].ToString()).GetName());
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        System.Type[] t1;
                        t1 = a.GetTypes();

                        #region CreatingObject

                        for (int k = 0; k < t1.Length; k++)
                        {
                            if (t1[k].Name == "MainPage")
                            {
                                try
                                {
                                    obj1 = Activator.CreateInstance(t1[k], null);
                                    ((UserControl)obj1).SetValue(Canvas.LeftProperty, 0.0);
                                    ((UserControl)obj1).SetValue(Canvas.TopProperty, 0.0);

                                    cnvLayoutRoot.Children.Add((UIElement)obj1);
                                }
                                catch (Exception exp)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncLoadScript", "CtlScriptRender.xaml.cs");
                                }
                            }
                        }
                        #endregion

                    }
                }



            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncLoadScript", "CtlScriptRender.xaml.cs");
            }
            #endregion
        }

        public void ClosePod()
        {

            MethodInfo mi = obj1.GetType().GetMethod("ClosePod");
            if (mi != null)
            {
                mi.Invoke(obj1, null);
            }
        }
    }
}

