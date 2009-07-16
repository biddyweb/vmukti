<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecoverPassword.ascx.cs" Inherits="ForgatePass.RecoverPassword" %>
<p>
    <asp:Label ID="lblUserName" runat="server" Text="Enter User Name:"
    style="position:absolute; top: 102px; left: 33px; height: 2px; bottom: 402px;" 
        Font-Bold="True"/>
</p>
<asp:Label ID="Label1" runat="server" Text="Recover Password" 
style="position:absolute; top: 54px; left: 118px;" Font-Bold="True"/>

<p>

<asp:Label ID="lblEmail" runat="server" Text="Enter Email:"
style="position:absolute; left: 33px; height: 18px; width: 106px; top: 138px;" 
        Font-Bold="True"/>

</p>
 
  
  <p>
      <asp:TextBox ID="txtUserName" runat="server" 
          style="z-index: 1; left: 192px; top: 102px; position: absolute"/>
</p>

  
 <p>
    <asp:TextBox ID="txtEmail" runat="server"
    style="z-index:1;position:absolute; top: 135px; left: 190px; right: 635px;" />
 </p>
 
 <p>
     &nbsp;</p>
 
 <p>
     &nbsp;</p>
 
 <p>
 
 <asp:Button ID="btnGetPass" runat="server" Text="Get Password"
 style="z-index:1; position:absolute; top: 187px; left: 131px;" 
         onclick="btnGetPass_Click" />
 </p>
<p>
<asp:RequiredFieldValidator ID="rqrdEmailAddress" runat="server" 
    ErrorMessage="Email Address should not be blank"
    
        style="z-index:1;position:absolute; top: 134px; left: 362px; height: 19px; width: 280px;" 
        ControlToValidate="txtEmail"/>
    
</p>
<p>
     <asp:RequiredFieldValidator ID="rqrdUsrName" runat="server"        
        style="z-index:1; position:absolute; top: 105px; left: 367px; height: 17px; width: 276px; right: 310px;"  
        ErrorMessage="User Name should not be blank" 
         ControlToValidate="txtUserName"/>
</p>
<asp:RegularExpressionValidator ID="reguEmailAddress" runat="server" 
    ErrorMessage="EnterValid Email Address"
    
    style="z-index:1;position:absolute; top: 136px; left: 359px; width: 290px; right: 304px;" 
    ControlToValidate="txtEmail" 
    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"/>

