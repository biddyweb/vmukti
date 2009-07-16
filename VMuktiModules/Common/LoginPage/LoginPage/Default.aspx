<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LoginePage._Default"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>VMukti</title>
    
    <style type="text/css">
       
        .tablelogin
        {
            height: 92px;
            top: 190px;
            left: 54px;
            position: relative;
            width: 246px;
        }
        .formstyle
        {
        	background:  url(images/new_index-000001.png);
            margin-right: auto;
            margin-left: auto;
        }
        .href
        {
        	 color:#ff6600;
        }
        
        #form1
        {
            height: 589px;
            width: 982px;
        }
               
       
       
        .style16
        {
            font-weight:bold;
            font-size:large;
            font-size: x-large;
            color: #FFFFFF;
            position: relative;
            text-align: center;
            height: 50px;
        }
       
        .style21
        {
            width: 44px;
          
            height: 28px;
        }
        .style22
        {
           
            width: 129px;
            height: 28px;
        }
        .style23
        {
            height: 30px;
            width: 44px;
          
        }
        .style24
        {
            height: 30px;
          
            width: 129px;
        }
        .style25
        {
            height: 32px;
            width: 44px;
           
        }
        .style26
        {
            height: 32px;
           
            width: 129px;
        }
               
       
        .TableValidation
        {
            width: 27%;
            top: -59px;
            left: 52px;
            position: relative;
            height: 82px;
        }
       
       
        .TableHelp
        {
            position: relative;
            text-align: justify;
            margin-top: -400px;
            margin-left: 320px;
        }
       
       
        .TableHeight
        {
             height: 30px;
        }
       
       
        .fntHelp
        {
            font-size: small;
            font-weight: bold;
            font-style: normal;
            color: #000000;
        }
       
       
        .btnLogin
        {
            top: -130px;
            left: 115px;
            position: relative;
            height: 40px;
            width: 63px;
        }
        .btnOk
        {
            top: -126px;
            left: 115px;
            position: relative;
            height: 40px;
            width: 63px;
        }
        .btnRegister
        {
            top: -129px;
            left: 140px;
            position: relative;
            height: 40px;
            width: 63px;
        }
        .btnCancle
        {
            top: -126px;
            left: 140px;
            position: relative;
            height: 40px;
            width: 63px;
        }
        .ForrgetPassword
        {
            bottom: -40px;
            left: 200px;
            position: relative;
        }
        </style>
    
    <link href="vmukti.css" rel="stylesheet" type="text/css" />
    
</head>
<body bgcolor="White" background="images/background.jpg"  >
    <form id="form1" runat="server" class="formstyle">
    &nbsp;<table class="tablelogin">
        <tr>
            <td class="style25" >
                <asp:Label ID="Label1" runat="server" Text="UserName"></asp:Label>
            </td>
            <td  class="style26">
                <asp:TextBox ID="txtUserName" runat="server" Width="158px" TabIndex="1"></asp:TextBox>
            </td>
           
        </tr>
        <tr>
            <td class="style23" >
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            </td>
            <td class="style24">
                <asp:TextBox ID="txtPassWord" runat="server" TextMode="Password" Width="158px" 
                    TabIndex="2" ></asp:TextBox>
            </td>
            <%--<td>
            </td>
            <td >
                &nbsp;</td>--%>
        </tr>
        <tr>
            <td class="style21">
                <asp:Label ID="lblEmail" runat="server" Text="E-Mail"></asp:Label>
            </td>
            <td class="style22">
            
                
                <asp:TextBox ID="txtEmail" runat="server" Width="158px" TabIndex="5"   
                    ></asp:TextBox>
                </td>
         
        </tr>
        </table>
        
  
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
    <br />
    <br />
    <br />
    <br />
    <br />
        
    <br />
    <asp:ImageButton ID="btnLogin" runat="server" 
        ImageUrl="~/images/login copy.png" onclick="btnLogin_Click" 
             TabIndex="3" CssClass="btnLogin" />
        
        <asp:ImageButton ID="btnRegister" runat="server" CausesValidation="False" 
                    ImageUrl="~/images/register-1.png" onclick="btnRegister_Click" 
                   
                    TabIndex="4" CssClass="btnRegister" />
        

           
        
    <br />
    <asp:ImageButton ID="btnOk" runat="server" ImageUrl="~/images/ok-1.png"   onclick="btnOk_Click"
        
        
       
        TabIndex="6" CssClass="btnOk" />
    <asp:ImageButton ID="btnCancel" runat="server" CausesValidation="False" 
        ImageUrl="~/images/cancel-1.png"  onclick="btnCancel_Click1"
        
        
       
        TabIndex="7" CssClass="btnCancle" />
    <br />
    
    
        
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <table class="TableHelp">
    <tr>
            <td align="left" class="style16" id="Td1">
                Welcome to VMukti
             </td>
        </tr> 
      <tr >
            <td class="TableHeight">
               <ul><li class="fntHelp">  First time user please download and install&nbsp;&nbsp;
                <a class="href" 
                    href="VMuktiClient 1.0.zip" 
                    target="_blank" tabindex="11">VMukti client setup</a></li></ul> </td>
        </tr>
        <tr >
            <td >
                <ul><li class="fntHelp">Register yourself</li></ul> </td>
        </tr>
        <tr>
            <td >
              <ul><li class="fntHelp">Login to VMukti </li></ul></td>
        </tr>
        <tr>
            <td >
              <ul><li class="fntHelp">Add VMukti users as your buddies using add&nbsp; buddy interface</li></ul></td>
        </tr>
        <tr>
            <td>
              <ul><li class="fntHelp">To start Collaboration, drag widget from the left panel and 
                  drop buddy from the right panel</li></ul></td>
        </tr>
      
    </table>
    <br />
    

    <br />
    <br />
    <table class="TableValidation">
    <tr>
            <td>
            <center>
            <asp:HyperLink ID="hLinkForget" runat="server" Font-Bold="True" 
        NavigateUrl="~/ForgotPass.aspx" >Forgot Password?</asp:HyperLink>
        </center>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtUserName" ErrorMessage="*Please Enter UserName" 
                    TabIndex="15"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtEmail" ErrorMessage="*Please Enter E-Mail" 
                    TabIndex="15"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtPassWord" ErrorMessage="*Please Enter Password" 
                    TabIndex="8"></asp:RequiredFieldValidator>
                    </td>
        </tr>
        <tr>
                <asp:Label ID="lblLength" runat="server" 
                    Text="*Password Should Be Greater Than Five Characters" Visible="False" 
                    ForeColor="Red"></asp:Label>       
        </tr>
        <tr>
            <td>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="txtEmail" 
                    ErrorMessage="*Please Enter Valid E-Mail Address" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    TabIndex="9"></asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    
    
   
    <br />
    <br />
    <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label 
        ID="lblRights" runat="server" 
        Text="Copyright 2008 @ VMukti " TabIndex="12"></asp:Label>&nbsp;|&nbsp; Contact :
    <a href="mailto:contact@vmukti.com" tabindex="13">contact@vmukti.com</a>|<a href="http://sourceforge.net"><img src="http://sflogo.sourceforge.net/sflogo.php?group_id=162339&amp;type=3" width="125" height="37" border="0" alt="SourceForge.net Logo" /></a>
    &nbsp;|
    <asp:HyperLink ID="hpCodePlex" runat="server" 
        NavigateUrl="http://codeplex.com/vmukti" ImageUrl="images/codeplex.jpg">CodePlex</asp:HyperLink>
    &nbsp;| 
    </form>
<%-- <script type="text/javascript">
var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
var pageTracker = _gat._getTracker("UA-3491317-1");
pageTracker._initData();
pageTracker._trackPageview();
</script>--%>

<!-- Start Quantcast tag -->
<script type="text/javascript" src="http://edge.quantserve.com/quant.js"></script>
<script type="text/javascript">_qacct="p-4a88Of0l918iQ";quantserve();</script>
<noscript>
<a href="http://www.quantcast.com/p-4a88Of0l918iQ" target="_blank"><img src="http://pixel.quantserve.com/pixel/p-4a88Of0l918iQ.gif" style="display: none;" border="0" height="1" width="1" alt="Quantcast"/></a>
</noscript>
<!-- End Quantcast tag -->

</body>
</html>
