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
using System.Data.SqlClient;

namespace ForgatePass
{
    public partial class RecoverPassword : System.Web.UI.UserControl
    {
        SqlConnection cn;
        SqlCommand cmd;
        SqlDataReader dr;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            try{
                cn=new SqlConnection();
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["connectString"].ToString();
                cn.Open();
                }
            catch(Exception ex)
            {
                Response.Write("Unable to connect to data server");
            }
         
        }

        protected void btnGetPass_Click(object sender, EventArgs e)
        {
            string cmdTxt="select Email,Password from UserInfo where DisplayName="+"'"+txtUserName.Text+ "' and Email="+ "'"+txtEmail.Text+"'";
           cmd=new SqlCommand(cmdTxt,cn);
            dr=cmd.ExecuteReader();
            if(!dr.HasRows)
                Response.Write("Your username is not matching with your Email Address or either one");
            else
            {
                while(dr.Read())
                sendMail(txtUserName.Text,txtEmail.Text,dr.GetString(1));
            }
            txtUserName.Text = null;
            txtEmail.Text = null;
        }

        private void sendMail(string usrName, string EmailAdd,string Pass)
        {
            string strFrom = ConfigurationManager.AppSettings["EmailAdd"].ToString();
            string strTo = EmailAdd;
            string strServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
            int intPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            string strPwd = ConfigurationManager.AppSettings["Password"].ToString();
            
            System.Net.Mail.MailMessage mailMsg;

            //mail content
            string strMsg = "";
            strMsg += "your Account information is as...\n";
            strMsg += "\n UserName:" + usrName+ "\n";
            strMsg+= "\n Password:"+ Pass;

             mailMsg = new System.Net.Mail.MailMessage();
             mailMsg.From = new System.Net.Mail.MailAddress(strFrom);
             mailMsg.To.Add(strTo);

             //mail subject
                mailMsg.Subject = "Password Recovery";
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = strMsg;
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.Priority = System.Net.Mail.MailPriority.High;
                mailMsg.IsBodyHtml = true;

                System.Net.Mail.SmtpClient SmtpMail = new System.Net.Mail.SmtpClient(strServer,intPort);
                SmtpMail.Credentials = new System.Net.NetworkCredential(strFrom, strPwd);
                SmtpMail.EnableSsl = true;
                try
                {
                    SmtpMail.Send(mailMsg);
                    Response.Write("your password is sent succesfully to your account");
                }
                 catch(Exception ex)
                {
                    Response.Write(ex.Message);
                    Response.Write("Your mail server is not working");
                }
        }

    }
}