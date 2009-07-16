using System;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

using System.Collections.Generic;
using System.Data.SqlClient;


using System.Configuration;


public partial class _Default : System.Web.UI.Page
{

   

    protected void Page_Load(object sender, EventArgs e)
    {
        
       
    }

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    //public static bool  RegisterUser(string userName, string password, string email)
    //{
    //    try
    //    {
    //        User newUser = new User(userName, password, email);
    //        return (newUser.Save() != 0);
           
           
    //    }
    //    catch (Exception ex)
    //    {
    //        return true;
    //    }
    //}

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    //public static KeyValuePair<bool, string> LoginUser(string userName, string password)
    //{
    //    bool authResult = false;
    //    User currentUser = new User();
        
    //    currentUser = currentUser.GetByUserNameAndPassword(userName, password, ref authResult);
    //    if (currentUser == null)
    //    {
    //        if (authResult)
    //        {
    //            return new KeyValuePair<bool, string>(false, "Please enter valid password!");
    //        }
    //        else
    //        {
    //            return new KeyValuePair<bool, string>(false, "Please enter valid user name!");
    //        }
    //    }
    //    else
    //    {
    //        return new KeyValuePair<bool, string>(true, currentUser.DisplayName);
    //    }
    //}

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    //public static KeyValuePair<bool, string> AddBuddy(string userName, string buddyName)
    //{
    //    // bool authResult = false;
    //    User currentUser = new User();
    //    string res = currentUser.AddBuddy(userName, buddyName); 
    //    if (res == "Offline" || res == "Online")
    //    {
    //        return new KeyValuePair<bool, string>(true, res);
            
    //        //else
    //        //{
    //        //    return new KeyValuePair<bool, string>(false, "Please enter valid user name!");
    //        //}
    //    }
    //    else
    //        return new KeyValuePair<bool, string>(false, res);
    //}
}
