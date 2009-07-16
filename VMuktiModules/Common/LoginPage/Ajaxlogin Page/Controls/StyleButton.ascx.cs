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

public partial class Controls_StyleButton : System.Web.UI.UserControl
{
    public string Text
    {
        get
        {
            return lbText.Text;
        }
        set
        {
            lbText.Text = value;
        }
    }

    public string OnClientClick
    {
        get
        {
            
            return divButton.Attributes["onclick"];
        }
        set
        {
            divButton.Attributes["onclick"] = value;
        }
    }

    public string Width
    {
        get
        {
            return divText.Style["width"];
        }
        set
        {
            divText.Style["width"] = value;
        }
    }

    public string MarginLeft
    {
        get
        {
            return divLeft.Style["margin-left"];
        }
        set
        {
            divLeft.Style["margin-left"] = value;
        }
    }

    public string MarginRight
    {
        get
        {
            return divRight.Style["margin-right"];
        }
        set
        {
            divRight.Style["margin-right"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
   
}
