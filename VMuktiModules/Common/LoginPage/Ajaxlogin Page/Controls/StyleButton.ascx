<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StyleButton.ascx.cs" Inherits="Controls_StyleButton" %>
<div id="divButton" class="styleButton" onmouseover="setStyleButton(this, 'over');"  onmouseout="setStyleButton(this, 'out');" runat="server">
    <div class="buttonleft" id="divLeft" runat="server">&nbsp;</div>
    <div class="buttoncontent" id="divText" runat="server"><asp:Label ID="lbText" runat="server" /></div>
    <div class="buttonright" id="divRight" runat="server">&nbsp;</div>
</div>