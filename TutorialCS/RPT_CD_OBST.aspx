<%@ Page Title="Resumen del Censo Diario" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="RPT_CD_OBST.aspx.cs" Inherits="RPT_CD_OBST" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div>
    Fecha: 
    <asp:TextBox ID="txtfecha" runat="server" type="date"></asp:TextBox>
    <asp:Button ID="btnok" runat="server" Text="Aceptar" /> 
</div>
    <br />
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="890px">
        <LocalReport ReportPath="Reportes\RCD_OBST.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>

</asp:Content>

