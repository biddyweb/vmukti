<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginRegisterControl.ascx.cs" Inherits="Controls_LoginRegisterControl" %>
<%@ Register Src="~/Controls/StyleButton.ascx" TagName="StyleButton" TagPrefix="uc" %>

<div id="divLoginValidate" class="error" style="width:100%;float:left;display:none;" runat="server" >

</div>

<div id="div1" class="error" style="width:100%;float:left;" runat="server" >
    <asp:Label Visible="false" ID="lblError" style="width:100%;float:left;" runat="server" />
</div>

User Name<br />
<asp:TextBox ID="tbUserName" Width="140px" CssClass="textBox" Height="15px" runat="server" /><br />
Password<br />
<asp:TextBox ID="tbPassword" TextMode="Password" Width="140px" CssClass="textBox" Height="15px" runat="server" /><br />
<div id="divEmailTextbox" style="display:none;">
    E-mail ID<br />
    <asp:TextBox ID="tbEmail" Width="140px" CssClass="textBox" Height="15px" runat="server" />
</div>
<div id="divLoginButtons" style="margin:5px 0px;width:100%;float:left;">
   
   <uc:StyleButton ID="btnRegister" Text="Register"  Width="65px" MarginRight="3px" runat="server" />
    <asp:Button ID="btnLogin" Text="Login" Width="65px" Height="27px" runat="server" MarginRight="3px" CssClass="aspButton"/>
   <%-- <uc:StyleButton ID="btnLogin" Text="Login" Width="65px" runat="server" />--%>
</div>
<div id="divRegisterButtons" style="display:none; margin:5px 0px;width:100%;float:left;">
   <%-- <uc:StyleButton ID="btnOk" Text="OK" OnClientClick="return false;" Width="65px"  MarginRight="3px" runat="server" />--%>
  
    <uc:StyleButton ID="btnCancel" Text="CANCEL" MarginRight="3px" Width="65px" runat="server" />
  
   <asp:Button ID="btnOk" Text="OK"  Width="65px" MarginRight="3px" Height="27px"  runat="server" class="styleButton" CssClass="aspButton"/>
  
</div>


