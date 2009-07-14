using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Mail;
using System.Net.Mail;
using System.Data.Sql;
using System.Data.SqlClient;


namespace LoginePage
{
    public partial class _Default : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        string ip, ip1;
        string Query1 = "select DisplayName from UserInfo";

        protected void Page_Load(object sender, EventArgs e)
        {

            //btnOk.OnClientClick = string.Format("onRegisterUser('{0}', '{1}', '{2}', '{3}');", txtUserName.ClientID, .ClientID, tbEmail.ClientID, divLoginValidate.ClientID);
            //btnRegister.OnClientClick = string.Format("onLoginUser('{0}', '{1}', '{2}');", tbUserName.ClientID, tbPassword.ClientID, divLoginValidate.ClientID);
       
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectString"].ToString();
            con = new SqlConnection();
            con.ConnectionString = conStr;
            cmd = new SqlCommand();
            cmd.Connection = con;
            //lblEmailWn.Text = "";
            if (!IsPostBack)
            {
                lblEmail.Visible = false;
                txtEmail.Visible = false;
                btnOk.Visible = false;
                btnCancel.Visible = false;
                txtUserName.Focus();
            }            
          
        }
       
        string encodestring(string str)
        {
            try
            {
                int j = 0;

                Byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                Byte[] ans = new Byte[encodedBytes.Length];
                foreach (Byte b in encodedBytes)
                {

                    int i = Convert.ToInt32(b);

                    if ((i >= 65 && i <= 90) || (i >= 97 && i <= 122))
                    {
                        i += 4;
                        if ((i > 90 && i <= 97) || (i > 122 && i <= 129))
                            i = i - 26;

                    }
                    else if (i >= 48 && i <= 57)
                    {
                        i += 7;
                        if (i > 57)
                            i = i - 10;
                    }
                    else
                    {
                        if (i == 61)
                            i = 44;
                        else if (i == 44)
                            i = 61;
                    }

                    ans[j++] = Convert.ToByte(i);
                }
                return System.Text.ASCIIEncoding.ASCII.GetString(ans);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        static string DeCodeString(string str)
        {
            try
            {
                int j = 0;

                Byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                Byte[] ans = new Byte[encodedBytes.Length];
                foreach (Byte b in encodedBytes)
                {

                    int i = Convert.ToInt32(b);

                    if ((i >= 65 && i <= 90) || (i >= 97 && i <= 122))
                    {
                        i -= 4;
                        if ((i < 65 && i >= 61) || (i < 97 && i >= 93))
                            i = i + 26;

                    }
                    else if (i >= 48 && i <= 57)
                    {
                        i -= 7;
                        if (i < 48)
                            i = i + 10;
                    }

                    else
                    {
                        if (i == 61)
                            i = 44;
                        else if (i == 44)
                            i = 61;
                    }

                    ans[j++] = Convert.ToByte(i);
                }
                return System.Text.ASCIIEncoding.ASCII.GetString(ans);
            }
            catch (Exception exp)
            {

                return null;
            }
        }

        protected void btnRegister_Click(object sender, ImageClickEventArgs e)
        {
            lblEmail.Visible = true;
            txtEmail.Visible = true;
            btnOk.Visible = true;
            btnCancel.Visible = true;
            btnLogin.Visible = false;
            btnRegister.Visible = false;
            txtPassWord.Focus();
        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
           // ip1 = Request.Url.AbsoluteUri;
           //// "http://localhost:1421/Default.aspx";
           // if (ip1.Contains("Default.aspx"))
           // {
           //     ip = ip1.Replace("Default.aspx", "VMukti.Presentation.xbap");
           // }
           // else
           // {
           //     ip = ip1.Insert(ip1.Length, "VMukti.Presentation.xbap");
           // }
            
            //ip=ip1.Split('/')[0];
            //ip = ip1.Replace("http://", "").Substring(0, ip1.Replace("http://", "").IndexOf('/')).Split(':')[0];
            Response.Redirect("VMukti.Presentation.xbap" + "?" + encodestring("uname") + "=" + encodestring(txtUserName.Text) + "&" + encodestring("pass") + "=" + encodestring(txtPassWord.Text));
           // Response.Redirect(ip + "?" + encodestring("uname") + "=" + encodestring(txtUserName.Text) + "&" + encodestring("pass") + "=" + encodestring(txtPassWord.Text));
        }

        protected void btnCancel_Click1(object sender, ImageClickEventArgs e)
        {
            lblLength.Visible = false;
            //lblEmailWn.Text = "";
            txtUserName.Enabled = true;
            txtPassWord.Enabled = true;
            btnLogin.Visible = true;
            btnRegister.Visible = true;
            txtEmail.Visible = false;
            lblEmail.Visible = false;
            btnOk.Visible = false;
            btnCancel.Visible = false;
        }

    //    protected void btnOk_Click(object sender, ImageClickEventArgs e)
      //  {
           //  string to ="sanghvi.dhaval86@gmail.com";
           //  string subject ="Welcome"; 
            // string body ="Welcome to Vmukti"; 
            // int status = sendMail(to,subject,body);
            // if (status == 1)
       //          Response.Write("your mail has been sent successfully");
            // else
           //      Response.Write("Sorry"); 

            //if (txtEmail.Text.Length == 0)
            //{
            //    lblEmailWn.Text = "Please Enter E-mail Address";

            //}
            //else
            //{
            //    ip1 = Request.Url.AbsoluteUri;
            //    if (ip1.Contains("Default.aspx"))
            //    {
            //        ip = ip1.Replace("Default.aspx", "VMukti.Presentation.xbap");

            //    }
            //    else
            //    {
            //        ip = ip1.Insert(ip1.Length, "VMukti.Presentation.xbap");
            //    }
                // ip = ip1.Replace("http://", "").Substring(0, ip1.Replace("http://", "").IndexOf('/')).Split(':')[0];
               
      //      }
        public int sendMail(string To, string Subject, string Body)
        {
            try
            {
                string strFrom = ConfigurationManager.AppSettings["EmailAdd"].ToString();
                string strServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                int intPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                string strPwd = ConfigurationManager.AppSettings["Password"].ToString();
            
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add(To);
                msg.From = new MailAddress(strFrom, "VMukti User", System.Text.Encoding.UTF8);
                msg.Subject = Subject;
                msg.Body = Body;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Priority = System.Net.Mail.MailPriority.High;
                SmtpClient smp = new SmtpClient();
                smp.UseDefaultCredentials = false;
                smp.Credentials = new System.Net.NetworkCredential(strFrom,strPwd);
                smp.Port = intPort;
                smp.Host = strServer;
                smp.EnableSsl = true;
                smp.Send(msg);
                return (1);
            }
            catch (Exception ex)
            {
                return (0);
            }
        }

        protected void btnOk_Click(object sender, ImageClickEventArgs e)
        {
            if (txtPassWord.Text.Length < 6)
            {
                lblLength.Visible = true;
            }
            else
            {
            int flag = 0;
            DataSet ds = new DataSet();
            con.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Query1;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);
            con.Close();
            for (int i = 0; i <ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() == txtUserName.Text)
                {
                    flag = 1;
                        lblLength.Text = "";
                        lblLength.Text = "This User Id is Already Registered";
                        lblLength.Visible = true;
                        //Response.Write("This User Id is Already Registered");
                }
            }
                //string Password = txtPassWord.Text;
                //int len = Password.Length;
                //if (len < 6)
                //{
                //    flag = 1;
                //    lblLength.Visible = true;
                //}
            
            if (flag == 0)
            {
                lblLength.Visible = false;         
                string to = txtEmail.Text;
                string subject = "Welcome";
                string body = "Welcome to Vmukti";
                int status = sendMail(to, subject, body);
                if (status == 1)
                    {
                        //Response.Write("your mail has been sent successfully");
                        Response.Redirect("VMukti.Presentation.xbap" + "?" + encodestring("uname") + "=" + encodestring(txtUserName.Text) + "&" + encodestring("pass") + "=" + encodestring(txtPassWord.Text) + "&" + encodestring("email") + "=" + encodestring(txtEmail.Text));
                    }
                else
                    {
                        //Response.Write("Sorry");
            Response.Redirect("VMukti.Presentation.xbap" + "?" + encodestring("uname") + "=" + encodestring(txtUserName.Text) + "&" + encodestring("pass") + "=" + encodestring(txtPassWord.Text) + "&" + encodestring("email") + "=" + encodestring(txtEmail.Text));            
                    }
                }
            }
        }


        }
}

