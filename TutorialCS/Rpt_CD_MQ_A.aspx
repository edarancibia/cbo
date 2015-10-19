<%@ Page Title="Resumen del Censo Diario" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rpt_CD_MQ_A.aspx.cs" Inherits="Rpt_CD_MQ_A" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="908px" 
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="Reportes\RCD_MQ.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>

