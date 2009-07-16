using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using VMuktiAPI;
using VMuktiService;
using System.Web.Mail;
using System.Net.Mail;
using System.Text.RegularExpressions;

public partial class Controls_LoginRegisterControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // btnOk.OnClientClick = string.Format("onRegisterUser('{0}', '{1}', '{2}', '{3}');", tbUserName.ClientID, tbPassword.ClientID, tbEmail.ClientID, divLoginValidate.ClientID);
            btnOk.Click += new EventHandler(btnOk_Click);
            btnRegister.OnClientClick = string.Format("showRegister(true,'{0}');", lblError.ClientID);

            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnCancel.OnClientClick = string.Format("showRegister(false,'{0}');", lblError.ClientID);
        }
        catch (Exception ex)
        {
        }
    }

    void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (tbUserName.Text == "")
            {
                lblError.Visible = true;
                lblError.Text = "Username Can't left blank";
                return;
            }
            if (tbPassword.Text == "")
            {
                lblError.Visible = true;
                lblError.Text = "Password Can't left blank";
                return;
            }
            if (tbEmail.Text == "")
            {
                lblError.Visible = true;
                lblError.Text = "Email Can't left blank";
                return;
            }

            //string MatchEmailPattern1 = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"+ @"((([0-1]?[0-9]{1,2}|2{1}[0-5]{2})\.([0-1]?[0-9]{1,2}|2{1}[0-5]{2})\."
            //                            + @"([0-1]?[0-9]{1,2}|2{1}[0-5]{2})\.([0-1]?[0-9]{1,2}|2{1}[0-5]{2})){1}|" + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            //explanation of MatchEmailPattern1 http://www.codeproject.com/KB/recipes/EmailRegexValidator.aspx

            string MatchEmailPattern = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                        + @"[a-zA-Z]{2,}))$";
            // explanation of this validator http://www.cambiaresearch.com/c4/bf974b23-484b-41c3-b331-0bd8121d5177/Parsing-Email-Addresses-with-Regular-Expressions.aspx


            Match myMatch = Regex.Match(tbEmail.Text.Trim(), MatchEmailPattern, RegexOptions.IgnoreCase);
            if (!myMatch.Success)
            {
                lblError.Visible = true;
                lblError.Text = "Please enter a valid Email ID";
                return;
            }
            else
            {
                string to = tbEmail.Text;
                string subject = "Welcome";
                string body = "Welcome to Vmukti";
                int status = sendMail(to, subject, body);
                Response.Redirect("VMukti.Presentation.xbap" + "?" + encodestring("uname") + "=" + encodestring(tbUserName.Text) + "&" + encodestring("pass") + "=" + encodestring(tbPassword.Text) + "&" + encodestring("email") + "=" + encodestring(tbEmail.Text));
            }
        }
        catch (Exception ex)
        {
        }
    }

    void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (tbUserName.Text == "")
            {
                lblError.Visible = true;
                lblError.Text = "UserName cant be blank";
                return;
            }
            if (tbPassword.Text == "")
            {
                lblError.Visible = true;
                lblError.Text = "Password cant be blank";
                return;
            }
            Response.Redirect("VMukti.Presentation.xbap" + "?" + encodestring("uname") + "=" + encodestring(tbUserName.Text) + "&" + encodestring("pass") + "=" + encodestring(tbPassword.Text));

        }
        catch (Exception ex)
        {

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

    public static int sendMail(string To, string Subject, string Body)
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
            smp.Credentials = new System.Net.NetworkCredential(strFrom, strPwd);
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

}
