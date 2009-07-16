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

namespace LoginePage
{
    public partial class ForgotPass : System.Web.UI.Page
    {
        Control forgetPass;
        protected void Page_Load(object sender, EventArgs e)
        {
            forgetPass = LoadControl("RecoverPassword.ascx");
            if (forgetPass == null)
                Response.Write("Control cant be loaded");
            PlaceHolder1.Controls.Add(forgetPass);
        }
    }
}
