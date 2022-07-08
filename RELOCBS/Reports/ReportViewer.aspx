<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="RELOCBS.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script src='<%=ResolveUrl("~/Scripts/crviewer/crv.js")%>'type="text/javascript">--%>
</head>
<body style="width:100%;height:100%">
    <form id="form1" runat="server">
        <div>
            <%--<CR:CrystalReportViewer ID="CRViewer" runat="server"  Width="100%" Height="100%"  AutoDataBind="True" ToolPanelView="None" EnableParameterPrompt="false"  />--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="1000px" Width="1000px" BackColor="#fdb913"></rsweb:ReportViewer>
        </div>
        
    </form>
</body>
</html>
