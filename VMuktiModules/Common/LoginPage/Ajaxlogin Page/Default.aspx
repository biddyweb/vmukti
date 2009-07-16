<%@ Page Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Free Desktop Sharing, Video, Audio Conference, Integrated Chat" %>
<%--<%@ Register Src="~/Controls/DragPanelControl.ascx" TagName="DragDropControl" TagPrefix="ddc" %>
<%@ Reference Control="~/Controls/ctlPOD.ascx" %>
<%@ Register Assembly="wb.Presentation" Namespace="wb.Presentation"  TagPrefix="wb"%>
<%@ Register Assembly="CoAuthering.Presentation" Namespace="CoAuthering.Presentation" TagPrefix="wCoAuth" %>--%>
<asp:Content ID="phCenterContent" ContentPlaceHolderID="CenterContent" runat="server">
  <%--  <ddc:DragDropControl ID="ddControl" runat="server"  />--%>
    <div style="display:none;">
        <asp:LinkButton ID="lnkHiddenLink" runat="server" />
        <asp:HiddenField ID="hdnPosition" runat="server" />
    </div>
</asp:Content>

