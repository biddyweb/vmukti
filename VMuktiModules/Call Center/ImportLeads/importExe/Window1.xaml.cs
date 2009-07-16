using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using ImportLeads.Business;
using System.Data.SqlClient;
using System.IO;
using VMuktiAPI;
using ImportLeads.DataAccess;
using System.Xml;

namespace importExe
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

       // string[] args = new string [8];


        //1)cmbFormat
        List<string> cmbFormat = new List<string>();
        List<string> ZipNotinDB = new List<string>();

        //2)cmbListType
        string cmbListType =null ;
        int cmbListTypeIndex;

        //3)lstLists
        //string paraListID =null;
        int lstListsValue;
        string lstListskey;

        //lstFilterType
        List<string> FilterType = new List<string>();

        //5)cmbCountry
        int cmbCountry;

        //6)txtFile
        string FileName = null;

        //string[] FilterType = new string[2];

        //txtDNC
        string txtDNCvalue = null;
        bool txtDNCEna;

        //8)chkGlobalDNC
        bool chkGlobalDNC;
       
       

        OleDbConnection conn;

        int noOfRowsProcessed = 0;
        int badNumbers = 0;
        int repeatNumbers = 0;
        int NoAreacode = 0;
        int NotMatchedZipcodes = 0;
        string sheetName;


        DataTable dtBulkZipCode;
        DataTable dtLead;
        DataTable dtTrendWestLead;
        DataTable dtLeadDetail;
        DataTable dtLocation;
        DataTable dtTimezone;
        DataTable dtCountry;
        DataTable dtTimezone1;
        DataRow[] drCollection;
        DataTable dtPhoneNumber;
        DataTable TempData;
        DataSet dsLeadFormat;
        DataSet dsLeadFields;
        string[] str = null;
        int ListID = 0;
        int CountryCode = 0;


        string lblNoOfProcessed;
        string lblRecordsInserted;
        string lblDuplicateLeads;
        string lblNoAreaCode;
        string lblBadLeads;
        string lblNotmatchedZipCodes;

        public delegate void DelImportLead();
        DelImportLead objDelImportLead = null;

        public Window1()
        {
            try
            {
                InitializeComponent();

                this.Visibility = Visibility.Hidden;

                objDelImportLead = new DelImportLead(import_leads_start);
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(startImporting));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Lowest;
                t.Start();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Window1()", "ImportExe:Window1.xaml.cs");
            }
            //import_leads_start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        
        private void import_leads_start()
        {
            try
            {
               
                //string[] args = Environment.GetCommandLineArgs();
                //MessageBox.Show(args.Length.ToString());

                //args[0] = string.Empty;

                //for (int i = 1; i < args.Length; i++)
                //{
                //    args[0] +=" "+ args[i];
                //}

                XmlDocument mydoc = new XmlDocument();
                mydoc.Load(AppDomain.CurrentDomain.BaseDirectory + "\\Config_ImportLeads.xml");
                //MessageBox.Show(args[0]);

                //cmbFormat
                XmlNodeList myNodes = mydoc.SelectNodes("//cmbFormat/Item");
                foreach (XmlNode nd in myNodes)
                {
                    cmbFormat.Add(nd.SelectSingleNode("@value").InnerText);
                }

                //cmbListType
                XmlNode cmbListTypeNode = mydoc.SelectSingleNode("//cmbListType");
                cmbListType = cmbListTypeNode.SelectSingleNode("item1").SelectSingleNode("@value").InnerText;
                cmbListTypeIndex = int.Parse(cmbListTypeNode.SelectSingleNode("item1").SelectSingleNode("@key").InnerText);


                //lstLists
                XmlNode lstListsNode = mydoc.SelectSingleNode("//lstLists/Item1");
                if (lstListsNode != null)
                {
                    lstListskey = lstListsNode.SelectSingleNode("@key").InnerText;
                    lstListsValue = int.Parse(lstListsNode.SelectSingleNode("@value").InnerText);
                }
       
                //lstFilterType
                XmlNodeList lstFilterTypeNodes = mydoc.SelectNodes("//lstFilterType/Item");
                if (lstFilterTypeNodes != null)
                {
                    foreach (XmlNode nd in lstFilterTypeNodes)
                    {
                        FilterType.Add(nd.SelectSingleNode("@value").InnerText);
                    }
                }

                //cmbCountry
                XmlNode cmbCountryNode = mydoc.SelectSingleNode("//cmbCountry/Item");
                cmbCountry = int.Parse(cmbCountryNode.SelectSingleNode("@value").InnerText);
               
                //txtFile
                XmlNode FileNameNode = mydoc.SelectSingleNode("//FileName/Item");
                FileName = FileNameNode.SelectSingleNode("@value").InnerText;
                
                //txtDNC
                XmlNode txtDNCNode = mydoc.SelectSingleNode("//txtDNC/Item");
                txtDNCvalue = txtDNCNode.SelectSingleNode("@value").InnerText;
                txtDNCEna = bool.Parse(txtDNCNode.SelectSingleNode("@key").InnerText);
               
                //chkGlobalDNC
                XmlNode chkGlobalDNCNode = mydoc.SelectSingleNode("//chkGlobalDNC/Item");
                chkGlobalDNC = bool.Parse(chkGlobalDNCNode.SelectSingleNode("@key").InnerText);
                
                //conn string
                VMuktiAPI.VMuktiInfo.MainConnectionString = mydoc.SelectSingleNode("//connectionString").InnerText;
                
                VMuktiAPI.VMuktiInfo.CurrentPeer.ID = int.Parse(mydoc.SelectSingleNode("//VmuktiID").InnerText);

                SqlConnection con = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                SqlCommand cmd = new SqlCommand("select CountryCode from Country where CountryCode=" + cmbCountry, con);
                con.Open();
                CountryCode = Int32.Parse(cmd.ExecuteScalar().ToString());
                //CountryCode = Int32.Parse(cmbCountry.ToString());
                //CountryCode =Int32.Parse("44");
                con.Close();
                //cmbListType = args[1];
                //paraListID = args[2];
                //FileName = args[3];
                //FilterType = args[4].Split(',');
                //cmbCountry = args[5];
                //txtDNC = args[6];
                //chkGlobalDNC = args[7];

                //CountryCode = int.Parse(cmbCountry);
                fillTable();
                import_leads();

                this.Close();

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "import_leads_start()", "ImportExe:Window1.xaml.cs");
            }
        }

        void startImporting()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelImportLead);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "startImporting()", "ImportExe:Window1.xaml.cs");            
            }
        }


        void import_leads()
        {
            try
            {
                if (lstListsValue == -1)
                {
                    System.Windows.MessageBox.Show("Select from List");
                }
                //else if (FilterType  -1)
                //{
                //    MessageBox.Show("Select Filter Type");
                //}
                else
                {

                    //progress bar
                    //proimport.Visibility = Visibility.Visible;

                    //proimport.Minimum = 0;
                    //proimport.Value = 0;
                    //proimport.Maximum = 20;
                    //proimport.Minimum = 0;
                    //proTimer = new DispatcherTimer();
                    //proTimer.Interval = new TimeSpan(1000);
                    //proTimer.Tick += new EventHandler(proTimer_Tick);
                    //proTimer.Start();
                    //proTimer.IsEnabled = true;


                    //end code



                    //CountryCode = int.Parse(((ComboBoxItem)cmbCountry.SelectedItem).Tag.ToString());

                    // Getting Fields For A Format //
                    if (cmbListTypeIndex == 1)
                    {
						#region For DNC List

                        //parameters for exe ( process to import leads)

                        //string CountryCode = CountryCode;
                        //int ListType = int.Parse(cmbListType);
                        //end parameter
                        if (cmbFormat[1].ToUpper() == "CSV" || cmbFormat[1].ToUpper() == "EXCEL")
                        {
                            if (cmbFormat[1].ToUpper() == "CSV")
                            {
                                conn = new OleDbConnection(csvConnection());
                            }
                            else if (cmbFormat[1].ToUpper() == "EXCEL")
                            {
                                if (FileName.Contains(".xlsx"))
                                {
                                conn = new OleDbConnection();
                                conn.ConnectionString = ExcelConnection();
                            }
                                else
                                {
                                    conn = new OleDbConnection();
                                    conn.ConnectionString = ExcelConnection2003();
                                } 
                                //conn = new OleDbConnection();
                                // conn.ConnectionString = ExcelConnection();
                            }
                            
                            conn.Open();
                            DataTable WorksheetName = new DataTable();

                            WorksheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            sheetName = (string)WorksheetName.Rows[0][WorksheetName.Columns[2].ColumnName];
                            conn.Close();
                            Int64 count = ClsList.GetMaxIDLeads();
                            // Int64 MaxLocationID = ClsList.GetMaxLocationID();

                            //commment :  string[] str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');

                            createDataTable();
                            DataSet dsTimeZone = ClsList.TimeZone_GetAll();
                            dtTimezone = dsTimeZone.Tables[0];



                            DataSet ds1 = ClsList.PhoneNumbers_GetAll(lstListsValue);
                            dtPhoneNumber = ds1.Tables[0];

                            if (txtDNCvalue.Length > 0)
                            {
                                //SqlConnection conn = new SqlConnection("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir");
                                noOfRowsProcessed = 1;
                                string str2 = Convert.ToString(txtDNCvalue.Replace("-", ""));

                                Int64 phonenum = 0;
                                try
                                {
                                    phonenum = String.IsNullOrEmpty(str2) ? 0 : Int64.Parse(str2);
                                    //if (Int16.Parse(phonenum.ToString().Substring(0, CountryCode.ToString().Length)) != CountryCode)
                                    //{
                                    //    phonenum = Int64.Parse((Math.Pow(10 * CountryCode, (phonenum.ToString().Length))).ToString()) + phonenum;
                                    //}
                                }
                                catch (Exception exp)
                                {
                                    badNumbers++;
                                }

                                DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phonenum);
                                if (drRepeat.Length > 0)
                                {
                                    repeatNumbers++;
                                }
                                else
                                {
                                    SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                    SqlCommand cmd = new SqlCommand("select getdate()", conn1);
                                    conn1.Open();
                                    SqlDataReader dr = cmd.ExecuteReader();
                                    dr.Read();
                                    dtLead.Rows.Add(count, phonenum, cmbFormat[0], Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, true, 1, lstListsValue, 1, 0, "Fresh", chkGlobalDNC == true ? true : false);
                                    conn1.Close();
                                }
                            }
                            else if (FileName.Length > 0 && lstListsValue > 0)
                            {


                                TempData = new DataTable();
                                OleDbDataAdapter da;
                                if (cmbFormat[1].ToUpper() == "CSV")
                                {
                                    string onlyFileName = FileName.Substring(FileName.LastIndexOf('\\') + 1);
                                    da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + onlyFileName + "]", csvConnection());//D:\\sample\\newcsv.csv
                                }
                                else
                                {
                                    if (FileName.Contains(".xlsx"))
                                    {
                                    da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection());
                                    }
                                    else
                                    {
                                        da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection2003());
                                    }
                                }

                                //da.Fill(TempData);
                                //da.Dispose();
                                //noOfRowsProcessed = TempData.Rows.Count;


                                string dainfo = da.ToString();
                                da.Fill(dtTrendWestLead);
                                da.Dispose();
                                noOfRowsProcessed = dtTrendWestLead.Rows.Count;
                                NAR(da);

                                if ((dtTrendWestLead.Columns.Count - 1) != dsLeadFields.Tables[0].Rows.Count)
                                {
                                    System.Windows.MessageBox.Show("Total no of ExcelSheet Columns not match with LeadFormat Columns");
                                    goto jump2;
                                }
                                else if ((dsLeadFields.Tables[0].Select("CustomFieldID=1")).Count() == 0)
                                {
                                    System.Windows.MessageBox.Show("Lead Format Must Have Phone Number Field Map with Excel Column");
                                    goto jump2;
                                }
                                //DataSet StateCountry = ClsList.StateCountry_GetAll();

                                ds1 = ClsList.PhoneNumbers_GetAll(lstListsValue);
                                dtPhoneNumber = ds1.Tables[0];

                                DataRow[] drStateCountry = null;

                                DataTable dtTemp = new DataTable();


                                Int64 globalCounter = ClsList.GetMaxIDTrendWestID();
                                count = ClsList.GetMaxIDLeads();
                                Int64 MaxLocationID = 0;

                                Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();



                                int co = dtTrendWestLead.Rows.Count - 1;
                                noOfRowsProcessed = dtTrendWestLead.Rows.Count;

                                DataRow[] drPhoneColumn = dsLeadFields.Tables[0].Select("CustomFieldID=1");
                                if (drPhoneColumn.Length == 0)
                                {
                                    System.Windows.MessageBox.Show("Lead Format has no Phone Number Field");
                                }
                                int PhoneCol = int.Parse(drPhoneColumn[0][0].ToString());

                                DataRow[] drStateColumn = dsLeadFields.Tables[0].Select("CustomFieldID=2");
                                int StateCol = 0;
                                if (drStateColumn.Length == 0)
                                {
                                    //System.Windows.MessageBox.Show("Lead Format has no State Field");
                                }
                                else
                                {
                                    StateCol = int.Parse(drStateColumn[0][0].ToString());
                                }
                                //int StateCol = int.Parse(drStateColumn[0][0].ToString());

                                DataRow[] drZipColumn = dsLeadFields.Tables[0].Select("CustomFieldID=3");
                                if (drZipColumn.Length == 0)
                                {
                                    System.Windows.MessageBox.Show("Lead Format has no ZipCode Field");
                                }
                                int ZipCol = int.Parse(drZipColumn[0][0].ToString());
                                int counter = 0;
                                int notmatchedZipcode = 0;

                                DataTable ZipcodeNotinDB = new DataTable();
                                ZipcodeNotinDB.Columns.Add("zipcod");
                                int Zipcount = 0;

                                SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                SqlCommand cmd = new SqlCommand("select getdate()", conn1);
                                conn1.Open();
                                SqlDataReader dr = cmd.ExecuteReader();
                                dr.Read();

                                for (int j = co; j >= 0; j--)
                                {
                                    string zipcod1 = Convert.ToString(dtTrendWestLead.Rows[j][ZipCol].ToString());

                                    Int64 finalzipcode1 = String.IsNullOrEmpty(zipcod1) ? 0 : Int64.Parse(zipcod1);
                                    dtBulkZipCode.Rows.Add(finalzipcode1);
                                }
                                SqlBulkCopy sqlBulkBulkZipCode = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);

                                sqlBulkBulkZipCode.DestinationTableName = "ZipCodeTemp";
                                sqlBulkBulkZipCode.ColumnMappings.Add("ZipCode", "ZipCode");
                                sqlBulkBulkZipCode.BulkCopyTimeout = 1000;
                                sqlBulkBulkZipCode.WriteToServer(dtBulkZipCode);
                                sqlBulkBulkZipCode.Close();

                                DataSet DataFromLocation1 = ClsList.GetLocationDetail();
                                DataTable dtforlocation = new DataTable();
                                dtforlocation = DataFromLocation1.Tables[0];

                                DataSet ds = ClsList.AreaCode_GetAll(); // .AreaCode_GetAll();
                                dtTemp = ds.Tables[0];

                                for (int i = co; i >= 0; i--)
                                //for (int i = 0; i < dtTrendWestLead.Rows.Count; i++)
                                    
                                {

                                    int varcount = 0;
                                    Int64 phonenum = 0;
                                    Int64 ZipCodeID = 0;

                                    string str2 = Convert.ToString(dtTrendWestLead.Rows[i][PhoneCol].ToString().Replace("-", ""));

                                    string zipcod = Convert.ToString(dtTrendWestLead.Rows[i][ZipCol].ToString());
                                    Int64 finalzipcode = String.IsNullOrEmpty(zipcod) ? 0 : Int64.Parse(zipcod);

                                    //ZipCodeID = ClsList.GetZipCodeID(finalzipcode);
                                    //DataSet DataFromLocation = ClsList.GetLocationID(finalzipcode);

                                    try
                                    {
                                        drStateCountry = dtforlocation.Select("ZipCode=" + finalzipcode);
                                        //DataTable dt = new DataTable();
                                        //dt = DataFromLocation.Tables[0];
                                        //drStateCountry = dt.Select("ZipCode=" + finalzipcode);
                                    }
                                    catch (Exception exp)
                                    {
                                        badNumbers++;
                                        dtTrendWestLead.Rows.RemoveAt(i);
                                        continue;
                                    }
                                    try
                                    {
                                        phonenum = String.IsNullOrEmpty(str2) ? 0 : Int64.Parse(str2);

                                    }
                                    catch (Exception exp)
                                    {
                                        if (exp.Message.ToLower().Contains("input string") == true)
                                        {
                                            System.Windows.MessageBox.Show("Phone Column format is not matching");
                                            goto jump2;
                                        }
                                        badNumbers++;
                                        dtTrendWestLead.Rows.RemoveAt(i);
                                        continue;
                                    }
                                    DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phonenum);
                                    if (drRepeat.Length > 0)
                                    {
                                        repeatNumbers++;
                                        dtTrendWestLead.Rows.RemoveAt(i);
                                        continue;
                                    }
                                    dtPhoneNumber.Rows.Add(phonenum);

                                    int condition = 0;

                                    //string FieldValues = "";
                                    DataRow drLocation1 = null;
                                    DataRow[] AreaCod = null;



                                    //DataSet ds = ClsList.AreaCode_GetAll(); // .AreaCode_GetAll();
                                    //dtTemp = ds.Tables[0];
                                    // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");
                                    //int loopcount = drCollection.Length;
                                    int loopcount = dtTemp.Rows.Count;

                                    //AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(0, 3) + "'");

                                    AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");
                                    if (AreaCod.Count() > 0)
                                    {
                                        condition = 1;
                                    }
                                    else
                                    {
                                        condition = 0;
                                        break;
                                    }

                                    if (condition == 1)
                                    {
                                        DataRow[] t1;
                                        t1 = dtTimezone.Select("TimezoneName = '" + dtTemp.Rows[0][1].ToString() + "'");

                                        Int64 gotIt = Int64.Parse(t1[0][1].ToString());

                                        try
                                        {
                                            if (drStateCountry.Length > 0)
                                            {
                                                string zipcod1 = null;
                                                if (co == i)
                                                { 
                                                    MaxLocationID = Int64.Parse(drStateCountry[0][0].ToString());
                                                }
                                                else
                                                {
                                                    if (dtTrendWestLead.Rows.Count > i + 1)
                                                    {
                                                        zipcod1 = Convert.ToString(dtTrendWestLead.Rows[i + 1][ZipCol].ToString());
                                                        if (zipcod == zipcod1)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            MaxLocationID = Int64.Parse(drStateCountry[0][0].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MaxLocationID = Int64.Parse(drStateCountry[0][0].ToString());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MaxLocationID = 0;
                                                //System.Windows.MessageBox.Show(zipcod);
                                                if(ZipNotinDB.Count()>0)
                                                //if (Zipcount > 0)
                                                { 
                                                    if(ZipNotinDB.Contains(zipcod))
                                                    //if ((ZipcodeNotinDB.Rows[Zipcount - 1][0]).ToString() == zipcod)
                                                    { 
                                                    }
                                                    else
                                                    {
                                                        ZipNotinDB.Add(zipcod);
                                                    }
                                                }
                                                else
                                                {
                                                    ZipNotinDB.Add(zipcod);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.MessageBox.Show(ex.ToString());
                                        }


                                        if (MaxLocationID == 0)
                                        {
                                            NotMatchedZipcodes++;
                                    }
                                    else
                                    {
                                            //SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                            //SqlCommand cmd = new SqlCommand("select getdate()", conn1);
                                            //conn1.Open();
                                            //SqlDataReader dr = cmd.ExecuteReader();

                                            //dr.Read();

                                            dtLead.Rows.Add(count, phonenum, cmbFormat[0], Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, true, 1, lstListsValue, MaxLocationID, 0, "Fresh", chkGlobalDNC == true ? true : false); 
                                            for (int cnt = 1; cnt <= dsLeadFields.Tables[0].Rows.Count; cnt++)
                                            {
                                                Int64 tempCustomFieldID = Convert.ToInt64(dsLeadFields.Tables[0].Rows[cnt - 1][1].ToString());
                                                if (tempCustomFieldID > 3)
                                                {
                                                    dtLeadDetail.Rows.Add(MaxLeadDetail, count, tempCustomFieldID, dtTrendWestLead.Rows[i][cnt].ToString());
                                                    MaxLeadDetail++;
                                                }

                                            }

                                            //dtLead.Rows.Add(count, phonenum, str[0], 1, null, null, false, 1, false, 0, int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()), MaxLocationID, 0, "Fresh", false);
                                            globalCounter++;
                                        count++;
                                        //conn1.Close();
                                    }

                                    // provalue += 1;
                                }

                                //progress.Kill();

                            }
                                conn1.Close();
                            }
                             
                            if (cmbFormat[1].ToUpper() == "EXCEL" || cmbFormat[1].ToUpper() == "CSV")
                            {
                                dtLead.DefaultView.Sort = "ID";

                                //    /*  SqlBulkCopy sqlBulk = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                //    //SqlBulkCopy sqlBulk = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir", SqlBulkCopyOptions.FireTriggers);
                                //    sqlBulk.DestinationTableName = "TrendWestLead";
                                //    sqlBulk.ColumnMappings.Add("ID", "ID");
                                //    sqlBulk.ColumnMappings.Add("LeadID", "LeadID");
                                //    sqlBulk.ColumnMappings.Add("lead", "VendorLeadID");
                                //    sqlBulk.ColumnMappings.Add("First Name", "FirstName");
                                //    sqlBulk.ColumnMappings.Add("Last Name", "LastName");
                                //    sqlBulk.ColumnMappings.Add("Address", "Address");
                                //    sqlBulk.ColumnMappings.Add("City", "City");
                                //    sqlBulk.ColumnMappings.Add("State", "State");
                                //    sqlBulk.ColumnMappings.Add("Zip", "Zip");
                                //    sqlBulk.ColumnMappings.Add("Control", "Control");
                                //    sqlBulk.ColumnMappings.Add("Event", "Event");
                                //    sqlBulk.ColumnMappings.Add("Work1", "[Work]");
                                //    sqlBulk.ColumnMappings.Add("Program Code", "ProgramCode");
                                //    sqlBulk.ColumnMappings.Add("Site", "Site");
                                //    sqlBulk.ColumnMappings.Add("Email", "Email");
                                //    sqlBulk.BulkCopyTimeout = 1000;
                                //    sqlBulk.WriteToServer(dtTrendWestLead);
                                //    sqlBulk.Close();*/

                                SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                                sqlBulkLocation.DestinationTableName = "Location";
                                sqlBulkLocation.BulkCopyTimeout = 1000;
                                sqlBulkLocation.WriteToServer(dtLocation);
                                sqlBulkLocation.Close();

                                //SqlBulkCopy sqlBulkLead = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                ////SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                                //sqlBulkLead.DestinationTableName = "Leads";
                                //sqlBulkLead.BulkCopyTimeout = 1000;
                                //sqlBulkLead.WriteToServer(dtLead);
                                //sqlBulkLead.Close();

                                SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                                sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
                                sqlBulkLeadDetail.BulkCopyTimeout = 1000;
                                sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
                                sqlBulkLeadDetail.Close();

                                string showStatusOfRecord = "";
                                lblNoOfProcessed = noOfRowsProcessed.ToString() + " Records Processed.\n";
                                showStatusOfRecord += lblNoOfProcessed;
                                lblBadLeads = badNumbers.ToString() + " Bad Records Found.\n";
                                showStatusOfRecord += lblBadLeads;
                                lblRecordsInserted = dtLead.Rows.Count.ToString() + " Records Inserted.\n";
                                showStatusOfRecord += lblRecordsInserted;
                                lblDuplicateLeads = repeatNumbers.ToString() + " Records Repeated.\n";
                                showStatusOfRecord += lblDuplicateLeads;
                                lblNoAreaCode = NoAreacode.ToString() + " Records Are Having Wrong AreaCode\n";
                                showStatusOfRecord += lblNoAreaCode;
                                lblNotmatchedZipCodes = NotMatchedZipcodes.ToString() + " Zipcodes not matched with server database.\n";
                                showStatusOfRecord += lblNotmatchedZipCodes;
                                System.Windows.MessageBox.Show(showStatusOfRecord);
                                if (ZipNotinDB.Count() > 0)
                                {
                                    WriteZipFile();
                                }
                                sendMail(noOfRowsProcessed, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreacode);

                                badNumbers = 0;
                                noOfRowsProcessed = 0;
                                repeatNumbers = 0;
                                NoAreacode = 0;

                                SqlBulkCopy sqlBulk1 = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                                // SqlBulkCopy sqlBulk1 = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                                sqlBulk1.DestinationTableName = "Leads";
                                sqlBulk1.ColumnMappings.Add("ID", "ID");
                                sqlBulk1.ColumnMappings.Add("PhoneNo", "PhoneNo");
                                sqlBulk1.ColumnMappings.Add("LeadFormatID", "LeadFormatID");
                                sqlBulk1.ColumnMappings.Add("CreatedDate", "CreatedDate");
                                sqlBulk1.ColumnMappings.Add("CreatedBy", "CreatedBy");
                                sqlBulk1.ColumnMappings.Add("DeletedDate", "DeletedDate");
                                sqlBulk1.ColumnMappings.Add("DeletedBy", "DeletedBy");
                                sqlBulk1.ColumnMappings.Add("IsDeleted", "IsDeleted");
                                sqlBulk1.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
                                sqlBulk1.ColumnMappings.Add("ModifiedBy", "ModifiedBy");
                                sqlBulk1.ColumnMappings.Add("DNCFlag", "DNCFlag");
                                sqlBulk1.ColumnMappings.Add("DNCBy", "DNCBy");
                                sqlBulk1.ColumnMappings.Add("ListID", "ListID");
                                sqlBulk1.ColumnMappings.Add("LocationID", "LocationID");
                                sqlBulk1.ColumnMappings.Add("RecycleCount", "RecycleCount");
                                sqlBulk1.ColumnMappings.Add("Status", "Status");
                                sqlBulk1.ColumnMappings.Add("IsGlobalDNC", "IsGlobalDNC");
                                sqlBulk1.BulkCopyTimeout = 1000;
                                sqlBulk1.WriteToServer(dtLead);
                                sqlBulk1.Close();

                            }//end if
                        }
                        else
                        {
                            
                            ClsTextFileInsert insText = new ClsTextFileInsert(FileName);
                            int[] TotalFields = InsertDNCTextFile();//insText.TotalCount(Convert.ToInt64(strformat[0]), ListID, strTimezone, filterType, TotalTimeZone,CountryCode);

                            if (TotalFields.Length == 1)
                            {
                                //System.Windows.MessageBox.Show("Phone Number Column is missing");
                                System.Windows.MessageBox.Show("Three Columns Phone Number, State and ZipCode must be in your importing File");
                            }
                            else
                            {
                                
                                //lblNoOfProcessed = TotalFields[0].ToString() + " Records Processed.";
                                //lblBadLeads = TotalFields[1].ToString() + " Bad Records Found.";
                                //lblRecordsInserted = TotalFields[2].ToString() + " Records Inserted.";
                                //lblDuplicateLeads= TotalFields[3].ToString() + " Records Repeated.";
                                //lblNoAreaCode = TotalFields[4].ToString() + " Records Are Having Wrong AreaCode";
                                //Array.Clear(TotalFields, 0, TotalFields.Length);
                                string showStatusOfRecord = "";
                                lblNoOfProcessed = TotalFields[0].ToString() + " Records Processed.\n";
                                showStatusOfRecord += lblNoOfProcessed;
                                lblBadLeads = TotalFields[1].ToString() + " Bad Records Found.\n";
                                showStatusOfRecord += lblBadLeads;
                                lblRecordsInserted = TotalFields[2].ToString() + " Records Inserted.\n";
                                showStatusOfRecord += lblRecordsInserted;
                                lblDuplicateLeads = TotalFields[3].ToString() + " Records Repeated.\n";
                                showStatusOfRecord += lblDuplicateLeads;
                                lblNoAreaCode = TotalFields[4].ToString() + " Records Are Having Wrong AreaCode.\n";
                                showStatusOfRecord += lblNoAreaCode;
                                lblNotmatchedZipCodes = TotalFields[5].ToString() + " Zipcodes not matched with server database.\n";
                                showStatusOfRecord += lblNotmatchedZipCodes;
                                System.Windows.MessageBox.Show(showStatusOfRecord);
                                if (ZipNotinDB.Count > 0)
                                {
                                    WriteZipFile();
                                }
                                sendMail(TotalFields[0], TotalFields[1], TotalFields[2], TotalFields[3], TotalFields[4]);
                                badNumbers = 0;
                                noOfRowsProcessed = 0;
                                repeatNumbers = 0;
                                NoAreacode = 0;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region For Calling List
                        string strTimezone = "";
                        int TotalTimeZone = 0;

                        /*   if (chkTimeZone != null)
                           {
                               for (int j = 0; j < chkTimeZone.Length; j++)
                               {
                                   if (chkTimeZone[j].IsChecked == true)
                                   {
                                       TotalTimeZone = TotalTimeZone + 1;
                                       if (strTimezone == "")
                                           strTimezone = "'" + chkTimeZone[j].Content.ToString() + "'";
                                       else
                                           strTimezone = strTimezone + ",'" + chkTimeZone[j].Content.ToString() + "'";
                                   }
                               }
                           }
                           if (strTimezone.Length <= 0)
                           {
                               MessageBox.Show("Select Item");
                               goto jump2;
                           }*/

                        ListID = lstListsValue;
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (cmbFormat[1].ToUpper() == "EXCEL" || cmbFormat[1].ToUpper() == "CSV")
                        {
                            #region Connection For CSV And Excel
                            if (cmbFormat[1].ToUpper() == "CSV")
                            {
                                conn = new OleDbConnection(csvConnection());
                            }
                            else
                            {
                                if(FileName.Contains(".xlsx"))
                                {
                                conn = new OleDbConnection();
                                conn.ConnectionString = ExcelConnection();
                                }
                                else
                                {
                                    conn = new OleDbConnection();
                                    conn.ConnectionString = ExcelConnection2003();
                                }
                            }
                            #endregion
                            
                            conn.Open();
                            DataTable WorksheetName = new DataTable();
                            //conn.Close();
                            WorksheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            sheetName = (string)WorksheetName.Rows[0][WorksheetName.Columns[2].ColumnName];
                            conn.Close();
                            if (FileName.Length <= 0 || lstListsValue <= 0)
                            {
                                goto jump2;
                            }
                            createDataTable();

                            OleDbDataAdapter da;
                            if (cmbFormat[1].ToUpper() == "CSV")
                            {
                                string onlyFileName = FileName.Substring(FileName.LastIndexOf('\\') + 1);
                                da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + onlyFileName + "]", csvConnection());//D:\\sample\\newcsv.csv
                            }
                            else
                            {
                                if (FileName.Contains(".xlsx"))
                                {
                                da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection());
                                }
                                else
                                {
                                    da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection2003());
                                }
                            }
                            //OleDbDataAdapter da = new OleDbDataAdapter("select * from  Sheet1$", ExcelConnection());
                            string dainfo = da.ToString();
                            da.Fill(dtTrendWestLead);
                            da.Dispose();
                            NAR(da);

                            if ((dtTrendWestLead.Columns.Count - 1) != dsLeadFields.Tables[0].Rows.Count)
                            {
                                System.Windows.MessageBox.Show("Total no of ExcelSheet Columns not match with LeadFormat Columns");
                                goto jump2;
                            }
                            else if ((dsLeadFields.Tables[0].Select("CustomFieldID=1")).Count() == 0)
                            {
                                System.Windows.MessageBox.Show("Lead Format Must Have Phone Number Field Map with Excel Column");
                                goto jump2;
                            }
                            //DataSet StateCountry = ClsList.StateCountry_GetAll();

                            DataSet ds1 = ClsList.PhoneNumbers_GetAll(lstListsValue);
                            dtPhoneNumber = ds1.Tables[0];

                            DataRow[] drStateCountry = null;

                            DataTable dtTemp = new DataTable();
                            Int64 globalCounter = ClsList.GetMaxIDTrendWestID();
                            Int64 count = ClsList.GetMaxIDLeads();
                            Int64 MaxLocationID = 0;
                            //Int64 MaxLocationID = ClsList.GetMaxLocationID();
                            Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();

                            //  OleDbDataAdapter da = new OleDbDataAdapter("select 1 as A1,1 as A2,*,1 As Work1,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection());

                            int co = dtTrendWestLead.Rows.Count - 1;
                            noOfRowsProcessed = dtTrendWestLead.Rows.Count;

                            DataRow[] drPhoneColumn = dsLeadFields.Tables[0].Select("CustomFieldID=1");
                            if (drPhoneColumn.Length == 0)
                            {
                                System.Windows.MessageBox.Show("Lead Format has no Phone Number Field");
                            }
                            int PhoneCol = int.Parse(drPhoneColumn[0][0].ToString());

                            DataRow[] drStateColumn = dsLeadFields.Tables[0].Select("CustomFieldID=2");
                            int StateCol=0;
                            if (drStateColumn.Length == 0)
                            {
                                //System.Windows.MessageBox.Show("Lead Format has no State Field");
                            }
                            else
                            {
                                StateCol = int.Parse(drStateColumn[0][0].ToString());
                            }
                            //int StateCol = int.Parse(drStateColumn[0][0].ToString());
                            DataRow[] drZipColumn = dsLeadFields.Tables[0].Select("CustomFieldID=3");
                            if (drZipColumn.Length == 0)
                            {
                                System.Windows.MessageBox.Show("Lead Format has no ZipCode Field");
                            }
                            int ZipCol = int.Parse(drZipColumn[0][0].ToString());
                            int counter = 0;

                            SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                            SqlCommand cmd = new SqlCommand("select getdate()", conn1);
                            conn1.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            dr.Read();

                            #region Get LocationDetail From DB

                            for(int j=co;j>=0;j--)
                            {
                                string zipcod1 = Convert.ToString(dtTrendWestLead.Rows[j][ZipCol].ToString());

                                Int64 finalzipcode1 = String.IsNullOrEmpty(zipcod1) ? 0 : Int64.Parse(zipcod1);
                                dtBulkZipCode.Rows.Add(finalzipcode1);
                            }
                            SqlBulkCopy sqlBulkBulkZipCode = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                            
                            sqlBulkBulkZipCode.DestinationTableName = "ZipCodeTemp";
                            sqlBulkBulkZipCode.ColumnMappings.Add("ZipCode", "ZipCode");
                            sqlBulkBulkZipCode.BulkCopyTimeout = 1000;
                            sqlBulkBulkZipCode.WriteToServer(dtBulkZipCode);
                            sqlBulkBulkZipCode.Close();

                            DataSet DataFromLocation1 = ClsList.GetLocationDetail();
                            DataTable dtforlocation=new DataTable();
                            dtforlocation = DataFromLocation1.Tables[0];

                            DataSet ds2 = ClsList.AreaCode_GetAll();
                            dtTemp = ds2.Tables[0];
                            #endregion

                            for (int i = co; i >= 0; i--)
                            {
                                int varcount = 0;
                                Int64 phonenum = 0;
                                Int64 ZipCodeID = 0;
                                

                                string str2 = Convert.ToString(dtTrendWestLead.Rows[i][PhoneCol].ToString().Replace("-", ""));
                                string zipcod = Convert.ToString(dtTrendWestLead.Rows[i][ZipCol].ToString());

                                Int64 finalzipcode = String.IsNullOrEmpty(zipcod) ? 0 : Int64.Parse(zipcod);

                                #region Checking For Zipcode not in Location Table And BadNumbers
                                try
                                {
                                    drStateCountry = dtforlocation.Select("ZipCode=" + finalzipcode);
                                    if (drStateCountry.Length > 0)
                                    {
                                        MaxLocationID = Int64.Parse(drStateCountry[0][0].ToString());
                                    }
                                    else
                                    {
                                        MaxLocationID = 0;
                                        if (ZipNotinDB.Count() > 0)
                                        {
                                            if (ZipNotinDB.Contains(zipcod))
                                            {
                                            }
                                            else
                                            {
                                                ZipNotinDB.Add(zipcod);
                                            }
                                        }
                                        else
                                        {
                                            ZipNotinDB.Add(zipcod);
                                        }
                                        NotMatchedZipcodes++;
                                        dtTrendWestLead.Rows.RemoveAt(i);
                                        continue;
                                    }
                                }
                                catch (Exception exp)
                                {
                                    badNumbers++;
                                    dtTrendWestLead.Rows.RemoveAt(i);
                                    continue;
                                }
                                try
                                {
                                    phonenum = String.IsNullOrEmpty(str2) ? 0 : Int64.Parse(str2);
                                }
                                catch (Exception exp)
                                {
                                    if (exp.Message.ToLower().Contains("input string") == true)
                                    {
                                        System.Windows.MessageBox.Show("Phone Column format is not matching");
                                        goto jump2;
                                    }
                                    badNumbers++;
                                    dtTrendWestLead.Rows.RemoveAt(i);
                                    continue;
                                }
                                #endregion

                                #region Checking For Repeatation of Inserted Number
                                DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phonenum);
                                if (drRepeat.Length > 0)
                                {
                                    repeatNumbers++;
                                    dtTrendWestLead.Rows.RemoveAt(i);
                                    continue;
                                }
                                dtPhoneNumber.Rows.Add(phonenum);
                                #endregion

                                #region Filtering And Inserting into Temp
                                Int64 FieldID = 0;
                                int condition = 0;

                                string FieldValues = "";
                                DataRow[] AreaCod = null;

                                try
                                {
                                    #region Filtering
                                    AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");
                                    if (AreaCod.Count() > 0)
                                    {
                                        for (int cntFilter = 0; cntFilter < FilterType.Count; cntFilter++)
                                        {
                                            string[] strFilterValues = FilterType[cntFilter].Split(',');
                                            FieldValues = strFilterValues[2].Replace('~', ',');
                                            FieldID = Convert.ToInt64(strFilterValues[1].ToString());
                                            string Operator = strFilterValues[3].ToString();
                                            #region Filter TimeZOne
                                            if (FieldID == 4)
                                            {
                                                if (Operator.ToLower() == "in")
                                                {
                                                    if (FieldValues.Contains(AreaCod[0].ItemArray[1].ToString()) == true)
                                                    {
                                                        condition = 1;
                                                    }
                                                    else
                                                    {
                                                        condition = 0;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (FieldValues.Contains(AreaCod[0].ItemArray[1].ToString()) == false)
                                                    {
                                                        condition = 1;
                                                    }
                                                    else
                                                    {
                                                        condition = 0;
                                                        break;
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Filter AreaCode
                                            else if (FieldID == 6)
                                            {
                                                if (Operator.ToLower() == "in")
                                                {
                                                    if (FieldValues.Contains(dtTrendWestLead.Rows[i][0].ToString()) == true)
                                                    {
                                                        condition = 1;
                                                    }
                                                    else
                                                    {
                                                        condition = 0;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (FieldValues.Contains(dtTrendWestLead.Rows[i][0].ToString()) == false)
                                                    {
                                                        condition = 1;
                                                    }
                                                    else
                                                    {
                                                        condition = 0;
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Filter State
                                            else if (FieldID == 2)
                                            {
                                                if (Operator.ToLower() == "in")
                                                {
                                                    if (FieldValues.Contains(dtTrendWestLead.Rows[i][StateCol].ToString()) == true)
                                                    {
                                                        condition = 1;
                                                    }
                                                    else
                                                    {
                                                        condition = 0;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (FieldValues.Contains(dtTrendWestLead.Rows[i][StateCol].ToString()) == false)
                                                    {
                                                        condition = 1;
                                                    }
                                                    else
                                                    {
                                                        condition = 0;
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Filter Other Fields
                                            else
                                            {

                                                try
                                                {
                                                    DataRow[] drCustColum = dsLeadFields.Tables[0].Select("CustomFieldID=" + FieldID);
                                                    int CustCol = int.Parse(drCustColum[0][0].ToString());
                                                    if (Operator.ToLower() == "in")
                                                    {
                                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][CustCol].ToString()) == true)
                                                        {
                                                            condition = 1;
                                                        }
                                                        else
                                                        {
                                                            condition = 0;
                                                            break;
                                                        }
                                                    }

                                                    else
                                                    {
                                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][CustCol].ToString()) == false)
                                                        {
                                                            condition = 1;
                                                        }
                                                        else
                                                        {
                                                            condition = 0;
                                                            break;
                                                        }

                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    VMuktiHelper.ExceptionHandler(ex, "Filtering loop", "Window1.xaml.cs(ImportEXE)");
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        dtTrendWestLead.Rows.RemoveAt(i);
                                        NoAreacode++;
                                        continue;
                                    }
                                    #endregion

                                    #region Inserting 
                                    if (condition == 0 && FilterType.Count>0)
                                    {
                                        #region Counter For Wrong AreaCode
                                        try
                                        {
                                            dtTrendWestLead.Rows.RemoveAt(i);
                                            if (FieldID >= 0)
                                            //if (FieldID >= 1)
                                            {
                                                NoAreacode++;
                                            }
                                            continue;
                                        }
                                        catch (Exception ex)
                                        {
                                            VMuktiHelper.ExceptionHandler(ex, "Counter For Wrong AreaCode for XLS", "Importing(Window1.xaml.cs)");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Insert into Temp Leaddetail and Leads
                                        dtLead.Rows.Add(count, phonenum, cmbFormat[0], Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, false, 0, lstListsValue, MaxLocationID, 0, "Fresh", false);

                                        for (int cnt = 1; cnt <= dsLeadFields.Tables[0].Rows.Count; cnt++)
                                        {
                                            Int64 tempCustomFieldID = Convert.ToInt64(dsLeadFields.Tables[0].Rows[cnt - 1][1].ToString());
                                                if (tempCustomFieldID > 3)
                                            {
                                                dtLeadDetail.Rows.Add(MaxLeadDetail, count, tempCustomFieldID, dtTrendWestLead.Rows[i][cnt].ToString());
                                                MaxLeadDetail++;
                                            }

                                        }
                                        #endregion
                                        globalCounter++;
                                        count++;
                                    }
                                    #endregion

                                jump1: ;
                                }
                                catch (Exception ex)
                                {
                                    VMuktiHelper.ExceptionHandler(ex, "import_leads()", "ImportExe:Window1.xaml.cs");            

                                }
                                #endregion
                            }
                            conn1.Close();
                        }
                        else
                        {
                            ClsTextFileInsert insText = new ClsTextFileInsert(FileName);
                            int[] TotalFields = InsertTextFile();//insText.TotalCount(Convert.ToInt64(strformat[0]), ListID, strTimezone, filterType, TotalTimeZone,CountryCode);

                            if (TotalFields.Length == 1)
                            {
                                //System.Windows.MessageBox.Show("Phone Number Column is missing");
                                System.Windows.MessageBox.Show("Two Columns Phone Number and ZipCode must be in your importing File");
                            }
                            else
                            {
                                
                                //lblNoOfProcessed = TotalFields[0].ToString() + " Records Processed.";
                                //lblBadLeads = TotalFields[1].ToString() + " Bad Records Found.";
                                //lblRecordsInserted = TotalFields[2].ToString() + " Records Inserted.";
                                //lblDuplicateLeads= TotalFields[3].ToString() + " Records Repeated.";
                                //lblNoAreaCode = TotalFields[4].ToString() + " Records Are Having Wrong AreaCode";
                                //Array.Clear(TotalFields, 0, TotalFields.Length);
                                string showStatusOfRecord = "";
                                lblNoOfProcessed = TotalFields[0].ToString() + " Records Processed.\n";
                                showStatusOfRecord += lblNoOfProcessed;
                                lblBadLeads = TotalFields[1].ToString() + " Bad Records Found.\n";
                                showStatusOfRecord += lblBadLeads;
                                lblRecordsInserted = TotalFields[2].ToString() + " Records Inserted.\n";
                                showStatusOfRecord += lblRecordsInserted;
                                lblDuplicateLeads = TotalFields[3].ToString() + " Records Repeated.\n";
                                showStatusOfRecord += lblDuplicateLeads;
                                lblNoAreaCode = TotalFields[4].ToString() + " Records Are Having Wrong AreaCode\n";
                                showStatusOfRecord += lblNoAreaCode;
                                lblNotmatchedZipCodes = TotalFields[5].ToString() + " ZipCodes are not matched with database.\n";
                                showStatusOfRecord += lblNotmatchedZipCodes;
                                System.Windows.MessageBox.Show(showStatusOfRecord);
                                if (ZipNotinDB.Count() > 0)
                                {
                                    WriteZipFile();
                                }
                                sendMail(TotalFields[0], TotalFields[1], TotalFields[2], TotalFields[3], TotalFields[4]);
                                badNumbers = 0;
                                noOfRowsProcessed = 0;
                                repeatNumbers = 0;
                                NoAreacode = 0;
                            }

                        }
                        if (cmbFormat[1].ToUpper() == "EXCEL" || cmbFormat[1].ToUpper() == "CSV")
                        {
                            dtLead.DefaultView.Sort = "ID";

                            SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                            //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                            sqlBulkLocation.DestinationTableName = "Location";
                            sqlBulkLocation.BulkCopyTimeout = 1000;
                            sqlBulkLocation.WriteToServer(dtLocation);
                            sqlBulkLocation.Close();

                            SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                            //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                            sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
                            sqlBulkLeadDetail.BulkCopyTimeout = 1000;
                            sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
                            sqlBulkLeadDetail.Close();

                            string showStatusOfRecord = "";
                            lblNoOfProcessed = noOfRowsProcessed.ToString() + " Records Processed.\n";
                            showStatusOfRecord += lblNoOfProcessed;
                            lblBadLeads = badNumbers.ToString() + " Bad Records Found.\n";
                            showStatusOfRecord += lblBadLeads;
                            lblRecordsInserted = dtLead.Rows.Count.ToString() + " Records Inserted.\n";
                            showStatusOfRecord += lblRecordsInserted;
                            lblDuplicateLeads = repeatNumbers.ToString() + " Records Repeated.\n";
                            showStatusOfRecord += lblDuplicateLeads;
                            lblNoAreaCode = NoAreacode.ToString() + " Records Are Having Wrong AreaCode\n";
                            showStatusOfRecord += lblNoAreaCode;
                            lblNotmatchedZipCodes = NotMatchedZipcodes.ToString()+" ZipCodes are not matched with database.\n";
                            showStatusOfRecord += lblNotmatchedZipCodes;
                            System.Windows.MessageBox.Show(showStatusOfRecord);
                            if (ZipNotinDB.Count() > 0)
                            {
                                WriteZipFile();
                            }
                            sendMail(noOfRowsProcessed, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreacode);


                            badNumbers = 0;
                            noOfRowsProcessed = 0;
                            repeatNumbers = 0;
                            NoAreacode = 0;

                            SqlBulkCopy sqlBulk1 = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                            // SqlBulkCopy sqlBulk1 = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                            sqlBulk1.DestinationTableName = "Leads";
                            sqlBulk1.ColumnMappings.Add("ID", "ID");
                            sqlBulk1.ColumnMappings.Add("PhoneNo", "PhoneNo");
                            sqlBulk1.ColumnMappings.Add("LeadFormatID", "LeadFormatID");
                            sqlBulk1.ColumnMappings.Add("CreatedDate", "CreatedDate");
                            sqlBulk1.ColumnMappings.Add("CreatedBy", "CreatedBy");
                            sqlBulk1.ColumnMappings.Add("DeletedDate", "DeletedDate");
                            sqlBulk1.ColumnMappings.Add("DeletedBy", "DeletedBy");
                            sqlBulk1.ColumnMappings.Add("IsDeleted", "IsDeleted");
                            sqlBulk1.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
                            sqlBulk1.ColumnMappings.Add("ModifiedBy", "ModifiedBy");
                            sqlBulk1.ColumnMappings.Add("DNCFlag", "DNCFlag");
                            sqlBulk1.ColumnMappings.Add("DNCBy", "DNCBy");
                            sqlBulk1.ColumnMappings.Add("ListID", "ListID");
                            sqlBulk1.ColumnMappings.Add("LocationID", "LocationID");
                            sqlBulk1.ColumnMappings.Add("RecycleCount", "RecycleCount");
                            sqlBulk1.ColumnMappings.Add("Status", "Status");
                            sqlBulk1.ColumnMappings.Add("IsGlobalDNC", "IsGlobalDNC");
                            sqlBulk1.BulkCopyTimeout = 1000;
                            sqlBulk1.WriteToServer(dtLead);
                            sqlBulk1.Close();

                        }//end if
                        #endregion
                    }//end 

                jump2: ;
                }//else


            }//end try
            catch (Exception ex)
            {
                //VMuktiHelper.ExceptionHandler(ex, "import_leads(Outer)", "ImportExe:Window1.xaml.cs");
            }
        }


        private string csvConnection()
        {
            try
            {
                string csvConnection;
                //string[] FilePath = FileName.Split('\\');
                string wholeFilePath = FileName.Substring(0, FileName.LastIndexOf("\\")) + "\\";
                //string wholeFilePath = "";
                //for (int i = 0; i < FilePath.Length - 1; i++)
                //{
                //    wholeFilePath += FilePath[i] + "\\";
                //}
                csvConnection = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + wholeFilePath + @";Extended Properties=""Text;HDR=Yes;FMT=Delimited""";
                return csvConnection;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "csvConnection()", "ImportExe:Window1.xaml.cs");
                return null;
            }
        }
       
        private string ExcelConnection()
        {
            try
            {
                //return
                //    @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                //    @"Data Source=" + FileName + ";" +
                //    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                //    @"Excel 8.0;" + Convert.ToChar(34).ToString();

                return @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    @"Data Source=" + FileName + ";" +
                    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                    @"Excel 12.0;" + Convert.ToChar(34).ToString();
                //string path = "c:\testInput\Data\Combisep\2004\Processed\OM0001122-GOP";
                //string fileToQuery = "OM0001122-GOP_Combisep_outFile.csv"
                //string sql = "SELECT * FROM " + fileToQuery;
                //System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection( "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";" + "Extended Properties='text;FMT=Delimited(,);HDR=YES'");
                //return @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                //@"Data Source=" + fileName + ";" +
                //@"Extended Properties=""text;HDR=NO;FMT=Delimited""";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ExcelConnection()", "ImportExe:Window1.xaml.cs");
                return null;
            }

        }
        
        private string ExcelConnection2003()
        {
            try
            {
                return
                    @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    @"Data Source=" + FileName + ";" +
                    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                    @"Excel 8.0;" + Convert.ToChar(34).ToString();

                //return @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                //    @"Data Source=" + FileName + ";" +
                //    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                //    @"Excel 12.0;" + Convert.ToChar(34).ToString();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ExcelConnection2003()", "ImportExe:Window1.xaml.cs");
                return null;
            }

        }
        
        public void createDataTable()
        {
            try
            {
                dtBulkZipCode = new DataTable();
                dtBulkZipCode.Columns.Add("ZipCode");
                dtBulkZipCode.Columns[0].DataType = typeof(Int64);
                dtBulkZipCode.Rows.Clear();

                //dtLeads = new DataTable();
                dtTrendWestLead = new DataTable();
                //dtTrendWestLead.Columns.Add("ID");
                //dtTrendWestLead.Columns[0].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("LeadID");
                //dtTrendWestLead.Columns[1].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("lead");
                //dtTrendWestLead.Columns[2].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("First Name");
                //dtTrendWestLead.Columns.Add("Last Name");
                //dtTrendWestLead.Columns.Add("Address");
                //dtTrendWestLead.Columns.Add("City");
                //dtTrendWestLead.Columns.Add("State");
                //dtTrendWestLead.Columns.Add("Zip");
                //dtTrendWestLead.Columns.Add("Control");
                //dtTrendWestLead.Columns.Add("Event");
                //dtTrendWestLead.Columns.Add("Work1");
                //dtTrendWestLead.Columns[11].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("Program Code");
                //dtTrendWestLead.Columns[12].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("Site");
                //dtTrendWestLead.Columns.Add("Email");
                //dtTrendWestLead.Columns.Add("AreaCod");

                dtLeadDetail = new DataTable();
                dtLeadDetail.Columns.Add("ID");
                dtLeadDetail.Columns[0].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("LeadID");
                dtLeadDetail.Columns[1].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("LeadFieldID");
                dtLeadDetail.Columns[2].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("PropertyValue");



                dtLead = new DataTable();
                dtLead.Columns.Add("ID");
                dtLead.Columns.Add("PhoneNo");
                dtLead.Columns.Add("LeadFormatID");
                dtLead.Columns.Add("CreatedDate");
                dtLead.Columns.Add("CreatedBy");
                dtLead.Columns.Add("DeletedDate");
                dtLead.Columns.Add("DeletedBy");
                dtLead.Columns.Add("IsDeleted");
                dtLead.Columns.Add("ModifiedDate");
                dtLead.Columns.Add("ModifiedBy");
                dtLead.Columns.Add("DNCFlag");
                dtLead.Columns.Add("DNCBy");
                dtLead.Columns.Add("ListID");
                dtLead.Columns.Add("LocationID");
                dtLead.Columns.Add("RecycleCount");
                dtLead.Columns.Add("Status");
                dtLead.Columns.Add("IsGlobalDNC");

                dtLocation = new DataTable();
                dtLocation.Columns.Add("ID");
                dtLocation.Columns[0].DataType = typeof(Int64);
                dtLocation.Columns.Add("TimeZoneID");
                dtLocation.Columns[1].DataType = typeof(Int64);
                dtLocation.Columns.Add("CountryID");
                dtLocation.Columns[2].DataType = typeof(Int64);
                dtLocation.Columns.Add("StateID");
                dtLocation.Columns[3].DataType = typeof(Int64);
                dtLocation.Columns.Add("AreaCodeID");
                dtLocation.Columns[4].DataType = typeof(Int64);
                dtLocation.Columns.Add("ZipCodeID");
                dtLocation.Columns[5].DataType = typeof(Int64);


                dtTrendWestLead.Rows.Clear();
                dtLead.Rows.Clear();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "createDataTable()", "ImportExe:Window1.xaml.cs");
            }
        }

        private void NAR(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch
            {
            }
            finally
            {
                o = null;
            }
        }

        
        private int[] InsertTextFile()
        {
            int[] ResultArray;
            try
            {
                DataSet dsCustomeFieldDetails = new DataSet();
                int varcount = 0;
                //DataSet StateCountry = ClsList.StateCountry_GetAll();

                DataTable dtTemp = new DataTable();
                
                ClsImportLeadsDataService clsReterive = new ClsImportLeadsDataService();

                dsCustomeFieldDetails = clsReterive.ReteriveCustomeFieldDetails(Convert.ToInt64(cmbFormat[0]));

                // dsCustomeFieldDetails = ClsList.GetLeadFields(Convert.ToInt64(str[0]));

                DataRow[] drPhoneNumber = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 1");
                if (drPhoneNumber.Length == 0)
                {
                    System.Windows.MessageBox.Show("Lead Format has no Phone Number Field");
                }
                int phoneStartpos = Convert.ToInt32(drPhoneNumber[0]["StartPosition"]);
                int phonelen = Convert.ToInt32(drPhoneNumber[0]["Length"]);

                DataRow[] drStateID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 2");
                int StateStartpos = 0;
                int Statelen = 0;
                if (drStateID.Length == 0)
                {
                    //System.Windows.MessageBox.Show("Lead Format has no State Field");
                }
                else
                {
                    StateStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
                    Statelen = Convert.ToInt32(drStateID[0]["Length"]);
                }
                //int StateStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
                //int Statelen = Convert.ToInt32(drStateID[0]["Length"]);

                DataRow[] drZipID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 3");
                if (drZipID.Length == 0)
                {
                    System.Windows.MessageBox.Show("Lead Format has no ZipCode Field");
                }
                int ZipStartpos = Convert.ToInt32(drZipID[0]["StartPosition"]);
                int Ziplen = Convert.ToInt32(drZipID[0]["Length"]);
                

                if (drPhoneNumber.Count() > 0)
                {
                    FileStream fs1 = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None, 2 << 18, FileOptions.SequentialScan);

                    DataTable dtTimezone = new DataTable();

                    DataRow[] drStateCountry = null;

                    DataSet dsTimeZone = ClsList.TimeZone_GetAll();
                    dtTimezone = dsTimeZone.Tables[0];

                    Int64 LeadMaxcount = ClsList.GetMaxIDLeads();
                    Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();
                    Int64 MaxLocationID = ClsList.GetMaxLocationID();

                    createDataTable();
                    int TotalColm = dsCustomeFieldDetails.Tables[0].Rows.Count;

                    
                    StreamReader sr1 = new StreamReader(fs1);
                   
                    SqlConnection conn = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    SqlCommand cmd = new SqlCommand("select getdate()", conn);
                    conn.Open();
                    SqlDataReader dr1;
                    int Rowcnt = 0;
                    //string Row = "";
                    int badNumbers = 0;
                    int notmatchedZipcode = 0;
                    // Byte badNumberFlag = 0;
                    // Byte RepeatFlag =0;
                    Byte AreaCodeFlag = 0;
                    int repeatNumbers = 0;
                    int NoAreaCode = 0;
                    DataTable dtPhoneNumber = new DataTable();
                    DataSet ds1 = ClsList.PhoneNumbers_GetAll(ListID);
                    dtPhoneNumber = ds1.Tables[0];
                    ListID = lstListsValue;

                    try 
                    {
                        while (sr1.EndOfStream != true)
                        {
                            try
                            {
                                string Row1 = sr1.ReadLine();
                                string ZipCode1 = Row1.Substring(ZipStartpos - 1, Ziplen).Replace(" ", "");
                                Int64 finalzipcode1 = String.IsNullOrEmpty(ZipCode1) ? 0 : Int64.Parse(ZipCode1.Substring(0, 5));
                                dtBulkZipCode.Rows.Add(finalzipcode1);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "BulkZip", "Windows1.xaml.cs");
                    }

                    SqlBulkCopy sqlBulkBulkZipCode = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);

                    sqlBulkBulkZipCode.DestinationTableName = "ZipCodeTemp";
                    sqlBulkBulkZipCode.ColumnMappings.Add("ZipCode", "ZipCode");
                    sqlBulkBulkZipCode.BulkCopyTimeout = 1000;
                    sqlBulkBulkZipCode.WriteToServer(dtBulkZipCode);
                    sqlBulkBulkZipCode.Close();

                    DataSet DataFromLocation1 = ClsList.GetLocationDetail();
                    DataTable dtforlocation = new DataTable();
                    dtforlocation = DataFromLocation1.Tables[0];


                    Int64 FieldID1 = 0;
                    string FieldValues1 = "";
                    for (int cntFilter1 = 0; cntFilter1 < FilterType.Count; cntFilter1++)
                    {
                        //string[] strFilterValues = ((ListBoxItem)lstFilterType.SelectedItems[cntFilter]).Tag.ToString().Split(',');
                        string[] strFilterValues1 = FilterType[cntFilter1].Split(',');
                        FieldValues1 = strFilterValues1[2].Replace('~', ',');
                        FieldID1 = Convert.ToInt64(strFilterValues1[1].ToString());
                        string Operator = strFilterValues1[3].ToString();
                    }
                    //DataSet ds2 = ClsList.AreaCode_GetAll(FieldValues1, "in");
                    DataSet ds2 = ClsList.AreaCode_GetAll();
                    dtTemp = ds2.Tables[0];
                    fs1.Close();
                    sr1.Close();
                    fs1 = null;
                    sr1 = null;
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None, 2 << 18, FileOptions.SequentialScan);
                    StreamReader sr = new StreamReader(fs);
                    try
                    {
                        while (sr.EndOfStream != true)
                        //for (Rowcnt = 0; Rowcnt < FileValueArray.Count(); Rowcnt++)
                        {
                            Rowcnt = Rowcnt + 1;
                            string Row = sr.ReadLine();
                            //FileValueArray.ElementAt(Rowcnt);

                            Int64 phoneNumber;
                            try
                            {
                                phoneNumber = Convert.ToInt64(Row.Substring(phoneStartpos - 1, phonelen).Replace("-", "").Replace(" ", ""));
                                //if (Int16.Parse(phoneNumber.ToString().Substring(0, CountryCode.ToString().Length)) != CountryCode)
                                //{
                                //    phoneNumber = Int64.Parse((Math.Pow(10 * CountryCode, (phoneNumber.ToString().Length))).ToString()) + phoneNumber;
                                //}
                            }
                            catch (Exception exp)
                            {
                                badNumbers++;

                                //  badNumberFlag = 1;
                                continue;
                            }

                            DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phoneNumber);
                            if (drRepeat.Length > 0)
                            {
                                repeatNumbers++;
                                //  RepeatFlag = 1;
                                continue;
                            }
                            dtPhoneNumber.Rows.Add(phoneNumber);

                            dr1 = cmd.ExecuteReader();
                            dr1.Read();

                            string ZipCode = Row.Substring(ZipStartpos - 1, Ziplen).Replace(" ", "");
                            Int64 finalzipcode = String.IsNullOrEmpty(ZipCode) ? 0 : Int64.Parse(ZipCode.Substring(0, 5));

                            try
                            {
                                //DataSet DataFromLocation = ClsList.GetLocationID(finalzipcode);
                                //DataTable dt = new DataTable();
                                //dt=DataFromLocation.Tables[0];
                                //drStateCountry = dt.Select("ZipCode=" + finalzipcode);

                                drStateCountry = dtforlocation.Select("ZipCode=" + finalzipcode);
                                //drStateCountry = StateCountry.Tables[0].Select("zipcode='" + ZipCode.Substring(0, 5) + "'");
                            }
                            catch (Exception ex)
                            {
                                badNumbers++;
                                dr1.Close();
                                continue;
                            }

                            Int64 FieldID = 0;
                            int condition = 0;

                            string FieldValues = "";
                            DataRow drLocation1 = null;
                            DataRow[] AreaCod = null;

                            try
                            {
                                for (int cntFilter = 0; cntFilter < FilterType.Count; cntFilter++)
                                {
                                    string[] strFilterValues = FilterType[cntFilter].Split(',');
                                    FieldValues = strFilterValues[2].Replace('~', ',');
                                    FieldID = Convert.ToInt64(strFilterValues[1].ToString());
                                    string Operator = strFilterValues[3].ToString();

                                    if (FieldID == 4)
                                    {
                                        if (Operator.ToLower() == "in")
                                        {
                                            //DataSet ds = ClsList.AreaCode_GetAll(FieldValues, "in");
                                            //dtTemp = ds.Tables[0];


                                            // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");


                                            //int loopcount = drCollection.Length;
                                            int loopcount = dtTemp.Rows.Count;
                                            if (FieldValues.Contains(AreaCod[0].ItemArray[1].ToString()) == true)
                                            {
                                            AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(0, 3) + "'");

                                            if (AreaCod.Count() > 0)
                                            {

                                                //varcount++;
                                                condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }
                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //DataSet ds = ClsList.AreaCode_GetAll(FieldValues, "not in");
                                            //dtTemp = ds.Tables[0];


                                            // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");



                                            //int loopcount = drCollection.Length;
                                            int loopcount = dtTemp.Rows.Count;
                                            if (FieldValues.Contains(AreaCod[0].ItemArray[1].ToString()) == false)
                                            {
                                            AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(0, 3) + "'");

                                            if (AreaCod.Count() > 0)
                                            {

                                                //varcount++;
                                                condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }
                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }


                                        }
                                    }
                                    else if (FieldID == 6)
                                    {
                                        if (Operator.ToLower() == "in")
                                        {
                                            if (FieldValues.Contains(phoneNumber.ToString().Substring(1, 3)) == true)
                                            {
                                                //DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
                                                //dtTemp = ds.Tables[0];
                                                AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");

                                                if (AreaCod.Count() > 0)
                                                {

                                                    //varcount++;
                                                    condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }


                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (FieldValues.Contains(phoneNumber.ToString().Substring(1, 3)) == false)
                                            {
                                                //DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
                                                //dtTemp = ds.Tables[0];
                                                AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");

                                                if (AreaCod.Count() > 0)
                                                {

                                                    //varcount++;
                                                    condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }


                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }

                                        }

                                    }
                                    else if (FieldID == 2)
                                    {

                                        if (Operator.ToLower() == "in")
                                        {
                                            if (FieldValues.Contains(Row.Substring(StateStartpos, Statelen)) == true)
                                            {
                                                //DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
                                                //dtTemp = ds.Tables[0];
                                                AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
                                                if (AreaCod.Count() > 0)
                                                {

                                                    condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }


                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (FieldValues.Contains(Row.Substring(StateStartpos, Statelen)) == false)
                                            {
                                                //DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
                                                //dtTemp = ds.Tables[0];
                                                AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
                                                if (AreaCod.Count() > 0)
                                                {

                                                    //varcount++;
                                                    condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }
                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] drCustColum = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID=" + FieldID);
                                        int CustStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
                                        int Custlen = Convert.ToInt32(drStateID[0]["Length"]);

                                        if (Operator.ToLower() == "in")
                                        {
                                            if (FieldValues.Contains(Row.Substring(CustStartpos, Custlen)) == true)
                                            {
                                                //DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
                                                //dtTemp = ds.Tables[0];
                                                AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
                                                if (AreaCod.Count() > 0)
                                                {

                                                    //varcount++;
                                                    condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }


                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }

                                        }
                                        else
                                        {
                                            if (FieldValues.Contains(Row.Substring(CustStartpos, Custlen)) == false)
                                            {
                                                //DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
                                                //dtTemp = ds.Tables[0];
                                                AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
                                                if (AreaCod.Count() > 0)
                                                {

                                                    //varcount++;
                                                    condition = 1;

                                                }
                                                else
                                                {
                                                    condition = 0;
                                                }


                                            }
                                            else
                                            {
                                                condition = 0;
                                                break;
                                            }

                                        }

                                    }
                                }
                            }
                            catch (Exception exp)
                            {
                                System.Windows.MessageBox.Show(exp.Message.ToString());
                            }

                            if (condition == 1)
                            {
                                DataRow[] t1;
                                DataSet ds = ClsList.TimeZone_GetAll();
                                dtTimezone = ds.Tables[0];
                                t1 = dtTimezone.Select("TimezoneName = '" + dtTemp.Rows[0][1].ToString() + "'");

                                Int64 gotIt = Int64.Parse(t1[0][1].ToString());

                                //Int64 SstID = Int64.Parse(drStateCountry[0][0].ToString());
                               

                                //Validation from Location Table 
                                //Kanhaiya


                                try
                                {
                                if (drStateCountry.Length > 0)
                                {
                                        MaxLocationID = Int64.Parse(drStateCountry[0][0].ToString());
                                }
                                else
                                {
                                        MaxLocationID = 0;
                                        if (ZipNotinDB.Count() > 0)
                                        {
                                            if (ZipNotinDB.Contains(ZipCode))
                                            {
                                            }
                                            else
                                            {
                                                ZipNotinDB.Add(ZipCode);
                                            }
                                        }
                                        else
                                        {
                                            ZipNotinDB.Add(ZipCode);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                //End validation From Location Table


                                if (MaxLocationID == 0)
                                {
                                    notmatchedZipcode++;
                                }
                                else
                                {
                                dtLead.Rows.Add(LeadMaxcount, phoneNumber, Convert.ToInt64(cmbFormat[0]), Convert.ToDateTime(dr1[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr1.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, false, 0, ListID, MaxLocationID, 0, "Fresh", false);
                                int Colcnt = 0;

                                for (Colcnt = 0; Colcnt < TotalColm; Colcnt++)
                                {
                                    Int64 fieldID = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]); ;
                                    int StartPositionTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
                                    int LengthTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

                                    // string Row = FileValueArray.ElementAt(Rowcnt);
                                    string Value = Row.Substring(StartPositionTmp - 1, LengthTmp);

                                    dtLeadDetail.Rows.Add(MaxLeadDetail, LeadMaxcount, fieldID, Value);
                                    MaxLeadDetail = MaxLeadDetail + 1;
                                }

                                    //MaxLocationID = MaxLocationID + 1;
                                LeadMaxcount = LeadMaxcount + 1;
                                Row = "";
                                }

                            }
                            else
                            {
                                NoAreaCode++;
                            }
                            dr1.Close();
                            
                        } //While End
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "InsertTextFile()", "ImportExe:Window1.xaml.cs");
                    }
                    finally
                    {
                        sr.Close();
                        fs.Close();
                        sr = null;
                        fs = null;
                        GC.Collect(0, GCCollectionMode.Forced);

                        //GC.SuppressFinalize(this);
                    }



                    SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                    sqlBulkLocation.DestinationTableName = "Location";
                    sqlBulkLocation.BulkCopyTimeout = 1000;
                    sqlBulkLocation.WriteToServer(dtLocation);
                    sqlBulkLocation.Close();


                    SqlBulkCopy sqlBulkLead = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                    sqlBulkLead.DestinationTableName = "Leads";
                    sqlBulkLead.BulkCopyTimeout = 1000;
                    sqlBulkLead.WriteToServer(dtLead);
                    sqlBulkLead.Close();


                    SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                    sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
                    sqlBulkLeadDetail.BulkCopyTimeout = 1000;
                    sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
                    sqlBulkLeadDetail.Close();


                    //ResultArray = new int[] { Rowcnt, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreaCode };
                    ResultArray = new int[] { Rowcnt, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreaCode, notmatchedZipcode };

                    dtLocation = null;
                    dtLead = null;
                    dtLeadDetail = null;

                    return ResultArray;
                }
                else
                {
                    ResultArray = new int[] { 0 };
                    return ResultArray;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "InsertTextFile(Outer)", "ImportExe:Window1.xaml.cs");
                ResultArray = new int[] { 0 };
                return ResultArray;
            }
        }

        private int[] InsertDNCTextFile()
        {
            ListID = lstListsValue;
            int[] ResultArray;
            int Rowcnt = 0;
            //string Row = "";
            int badNumbers = 0;
            // Byte badNumberFlag = 0;
            // Byte RepeatFlag =0;
            int repeatNumbers = 0;
            int NoAreaCode = 0;
            int notmatchedZipcode = 0;
            try
            {
                DataSet dsCustomeFieldDetails = new DataSet();
                int varcount = 0;
                //DataSet StateCountry = ClsList.StateCountry_GetAll();

                DataTable dtTemp = new DataTable();
                
                ClsImportLeadsDataService clsReterive = new ClsImportLeadsDataService();

                dsCustomeFieldDetails = clsReterive.ReteriveCustomeFieldDetails(Convert.ToInt64(cmbFormat[0]));

                // dsCustomeFieldDetails = ClsList.GetLeadFields(Convert.ToInt64(str[0]));

                DataRow[] drPhoneNumber = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 1");

                if (drPhoneNumber.Length == 0)
                {
                    System.Windows.MessageBox.Show("LeadFormat has no phone Number Field");
                }
                int phoneStartpos = Convert.ToInt32(drPhoneNumber[0]["StartPosition"]);
                int phonelen = Convert.ToInt32(drPhoneNumber[0]["Length"]);

                DataRow[] drStateID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 2");
                int StateStartpos =0;
                int Statelen = 0;
                if (drStateID.Length == 0)
                {
                    //System.Windows.MessageBox.Show("LeadFormat has no state field");
                }
                else
                {
                    StateStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
                    Statelen = Convert.ToInt32(drStateID[0]["Length"]);
                }
                //int StateStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
                //int Statelen = Convert.ToInt32(drStateID[0]["Length"]);
                

                DataRow[] drZipID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 3");

                if (drZipID.Length == 0)
                {
                    System.Windows.MessageBox.Show("LeadFormat has no ZipCode Field");
                }
                int ZipStartpos = Convert.ToInt32(drZipID[0]["StartPosition"]);
                int Ziplen = Convert.ToInt32(drZipID[0]["Length"]);
                

                if (drPhoneNumber.Count() > 0)
                {
                    FileStream fs1 = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None, 2 << 18, FileOptions.SequentialScan);
                    DataTable dtTimezone = new DataTable();

                    DataRow[] drStateCountry = null;

                    DataSet dsTimeZone = ClsList.TimeZone_GetAll();
                    dtTimezone = dsTimeZone.Tables[0];

                    Int64 LeadMaxcount = ClsList.GetMaxIDLeads();
                    Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();
                    Int64 MaxLocationID = ClsList.GetMaxLocationID();

                    createDataTable();
                    int TotalColm = dsCustomeFieldDetails.Tables[0].Rows.Count;
                    StreamReader sr1 = new StreamReader(fs1);
                    SqlConnection conn = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    SqlCommand cmd = new SqlCommand("select getdate()", conn);
                    conn.Open();
                    SqlDataReader dr1;
                    Byte AreaCodeFlag = 0;
                    DataTable dtPhoneNumber = new DataTable();
                    DataSet ds1 = ClsList.PhoneNumbers_GetAll(ListID);
                    dtPhoneNumber = ds1.Tables[0];

                    try
                    {
                        while (sr1.EndOfStream != true)
                        {
                            try
                            {
                                string Row1 = sr1.ReadLine();
                                string ZipCode1 = Row1.Substring(ZipStartpos - 1, Ziplen).Replace(" ", "");
                                Int64 finalzipcode1 = String.IsNullOrEmpty(ZipCode1) ? 0 : Int64.Parse(ZipCode1.Substring(0, 5));
                                dtBulkZipCode.Rows.Add(finalzipcode1);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "BulkZip", "Windows1.xaml.cs");
                    }

                    SqlBulkCopy sqlBulkBulkZipCode = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);

                    sqlBulkBulkZipCode.DestinationTableName = "ZipCodeTemp";
                    sqlBulkBulkZipCode.ColumnMappings.Add("ZipCode", "ZipCode");
                    sqlBulkBulkZipCode.BulkCopyTimeout = 1000;
                    sqlBulkBulkZipCode.WriteToServer(dtBulkZipCode);
                    sqlBulkBulkZipCode.Close();

                    DataSet DataFromLocation1 = ClsList.GetLocationDetail();
                    DataTable dtforlocation = new DataTable();
                    dtforlocation = DataFromLocation1.Tables[0];

                    DataSet ds2 = ClsList.AreaCode_GetAll();
                    dtTemp = ds2.Tables[0];

                    fs1.Close();
                    sr1.Close();
                    fs1 = null;
                    sr1 = null;
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None, 2 << 18, FileOptions.SequentialScan);
                    StreamReader sr = new StreamReader(fs);

                    try
                    {
                        while (sr.EndOfStream != true)
                        //for (Rowcnt = 0; Rowcnt < FileValueArray.Count(); Rowcnt++)
                        {
                            Rowcnt = Rowcnt + 1;
                            string Row = sr.ReadLine();
                            //FileValueArray.ElementAt(Rowcnt);

                            Int64 phoneNumber;
                            try
                            {
                                phoneNumber = Convert.ToInt64(Row.Substring(phoneStartpos - 1, phonelen).Replace("-", "").Replace(" ", ""));
                            }
                            catch (Exception exp)
                            {
                                badNumbers++;

                                //  badNumberFlag = 1;
                                continue;
                            }

                            DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phoneNumber);
                            if (drRepeat.Length > 0)
                            {
                                repeatNumbers++;
                                //  RepeatFlag = 1;
                                continue;
                            }
                            dtPhoneNumber.Rows.Add(phoneNumber);

                            dr1 = cmd.ExecuteReader();
                            dr1.Read();


                                string ZipCode = Row.Substring(ZipStartpos - 1, Ziplen).Replace(" ", "");
                            Int64 finalzipcode = String.IsNullOrEmpty(ZipCode) ? 0 : Int64.Parse(ZipCode.Substring(0, 5));
                            try
                            {
                                //DataSet DataFromLocation = ClsList.GetLocationID(finalzipcode);
                                //DataTable dt = new DataTable();
                                //dt = DataFromLocation.Tables[0];
                                //drStateCountry = dt.Select("ZipCode=" + finalzipcode);
                                drStateCountry = dtforlocation.Select("ZipCode=" + finalzipcode);
                            }
                            catch (Exception ex)
                            {
                                badNumbers++;
                                dr1.Close();
                                continue;
                            }
                            int condition = 0;
                            DataRow drLocation1 = null;
                            DataRow[] AreaCod = null;
                            DataSet ds = ClsList.AreaCode_GetAll(); // .AreaCode_GetAll();
                            dtTemp = ds.Tables[0];
                            int loopcount = dtTemp.Rows.Count;

                            AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(0, 3) + "'");
                            if (AreaCod.Count() > 0)
                            {
                                condition = 1;
                            }
                            else
                            {
                                condition = 0;
                            }

                            if (condition == 1)
                            {
                                DataRow[] t1;

                                t1 = dtTimezone.Select("TimezoneName = '" + dtTemp.Rows[0][1].ToString() + "'");

                                Int64 gotIt = Int64.Parse(t1[0][1].ToString());


                                
                                //Int64 SstID = Int64.Parse(drStateCountry[0][0].ToString());
                               

                                //Validation from Location Table 
                                //Kanhaiya

                                try
                                {
                                if (drStateCountry.Length > 0)
                                {
                                        MaxLocationID = Int64.Parse(drStateCountry[0][0].ToString());
                                }
                                else
                                {
                                        MaxLocationID = 0;
                                        if (ZipNotinDB.Count() > 0)
                                        {
                                            if (ZipNotinDB.Contains(ZipCode))
                                            {
                                            }
                                            else
                                            {
                                                ZipNotinDB.Add(ZipCode);
                                            }
                                        }
                                        else
                                        {
                                            ZipNotinDB.Add(ZipCode);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }


                                if (MaxLocationID == 0)
                                {
                                    notmatchedZipcode++;
                                }
                                else
                                {

                                dtLead.Rows.Add(LeadMaxcount, phoneNumber, Convert.ToInt64(cmbFormat[0]), Convert.ToDateTime(dr1[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr1.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, true, 1, ListID, MaxLocationID, 0, "Fresh", false);
                                int Colcnt = 0;

                                for (Colcnt = 0; Colcnt < TotalColm; Colcnt++)
                                {
                                    Int64 fieldID = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]); ;
                                    int StartPositionTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
                                    int LengthTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

                                    // string Row = FileValueArray.ElementAt(Rowcnt);
                                    string Value = Row.Substring(StartPositionTmp - 1, LengthTmp);

                                    dtLeadDetail.Rows.Add(MaxLeadDetail, LeadMaxcount, fieldID, Value);
                                    MaxLeadDetail = MaxLeadDetail + 1;
                                }

                                MaxLocationID = MaxLocationID + 1;
                                LeadMaxcount = LeadMaxcount + 1;
                                Row = "";
                                }

                            }
                            else
                            {
                                NoAreaCode++;
                            }
                            dr1.Close();
                        } //While End
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "InsertDNCTextFile()", "ImportExe:Window1.xaml.cs");
                    }
                    finally
                    {
                        sr.Close();
                        fs.Close();
                        sr = null;
                        fs = null;
                        GC.Collect(0, GCCollectionMode.Forced);

                        //GC.SuppressFinalize(this);
                    }

                    SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                    sqlBulkLocation.DestinationTableName = "Location";
                    sqlBulkLocation.BulkCopyTimeout = 1000;
                    sqlBulkLocation.WriteToServer(dtLocation);
                    sqlBulkLocation.Close();

                    SqlBulkCopy sqlBulkLead = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                    sqlBulkLead.DestinationTableName = "Leads";
                    sqlBulkLead.BulkCopyTimeout = 1000;
                    sqlBulkLead.WriteToServer(dtLead);
                    sqlBulkLead.Close();

                    SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir", SqlBulkCopyOptions.FireTriggers);
                    sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
                    sqlBulkLeadDetail.BulkCopyTimeout = 1000;
                    sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
                    sqlBulkLeadDetail.Close();

                    ResultArray = new int[] { Rowcnt, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreaCode,notmatchedZipcode };
                    dtLocation = null;
                    dtLead = null;
                    dtLeadDetail = null;
                    return ResultArray;
                }
                else
                {
                    ResultArray = new int[] { 0 };
                    return ResultArray;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "InsertDNCTextFile(Outer)", "ImportExe:Window1.xaml.cs");
                ResultArray = new int[] { 0 };
                return ResultArray;
            }
        }

        private void fillTable()
        {
            try
            {
                dsLeadFields = new DataSet();
                //str = cmbFormat.Split(',');

                dsLeadFields = ClsList.GetLeadFields(Convert.ToInt64(cmbFormat[0]));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fillTable()", "ImportExe:Window1.xaml.cs");
            }
        }

        private void WriteZipFile()
        {
            try
            {
                //System.Windows.MessageBox.Show("Some Zipcode is not available in server database. You can save such zipcodes \n and directly insert into database using BulkLocationUpdate Module");
                System.Windows.MessageBox.Show("Some Zipcode is not available in server database. You can save such zipcodes and directly insert into database using BulkLocationUpdate Module");
                System.Windows.Forms.DialogResult sfdRes;
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Zipcode.txt";
                sfdRes = sfd.ShowDialog();

                if (sfdRes == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    { 
                        File.Delete(sfd.FileName);
                    }
                    //File.Move(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src.zip", sfd.FileName);
                    StreamWriter fw = File.CreateText(sfd.FileName);
                    fw.Write("Zipcodes that are not available in Database:-");
                    fw.WriteLine();
                    foreach (string drPrintString in ZipNotinDB)
                    {
                        fw.Write(drPrintString);
                        fw.Write(",");
                    }
                    fw.Flush();
                    fw.Close();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "WriteZipFile()", "ImportExe:Window1.xaml.cs");
            }

        }

        private void sendMail(int noOfRowsProcessed,int lblBadLeads,int lblRecordsInserted,int lblDuplicateLeads,int NoAreacode)
        {
            try
            {
                string strFrom = "vmuktiuser@vmukti.com";
                string strTo = "kanhaiya@vmukti.com";
                string strServer = "smtp.gmail.com";
                int intPort = 587;
                string strPwd = "vmukti";

                string strMsg = noOfRowsProcessed.ToString() + " Records Processed.\n";
                strMsg += lblBadLeads.ToString() + " Bad Records Found.\n";
                strMsg += lblRecordsInserted.ToString() + " Records Inserted.\n";
                strMsg += lblDuplicateLeads.ToString() + " Records Repeated.\n";
                strMsg += NoAreacode.ToString() + " Records Are Having Wrong AreaCode\n";

                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                mailMsg.From = new System.Net.Mail.MailAddress(strFrom);
                mailMsg.To.Add(strTo);

                mailMsg.Subject = "ImportLead Information From VMukti";
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = strMsg;
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.Priority = System.Net.Mail.MailPriority.High;
                mailMsg.IsBodyHtml = true;

                System.Net.Mail.SmtpClient SmtpMail = new System.Net.Mail.SmtpClient(strServer,intPort);
                SmtpMail.Credentials = new System.Net.NetworkCredential(strFrom, strPwd);
                SmtpMail.EnableSsl = true;
                SmtpMail.Send(mailMsg);
            }
            catch (Exception ex)
            {
                //VMuktiHelper.ExceptionHandler(ex, "sendMail()", "ImportExe:Window1.xaml.cs");
                System.Windows.MessageBox.Show("If you are not receiving any mail regarding importing lead details then might be some Network problem in your system");
            }

        }

    }
}
