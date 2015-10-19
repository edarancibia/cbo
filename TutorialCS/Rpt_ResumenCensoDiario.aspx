<%@ Page Title="Resumen Censo Diario" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rpt_ResumenCensoDiario.aspx.cs" Inherits="Rpt_ResumenCensoDiario" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div>
    Nivel de cuidado:
    <asp:DropDownList ID="cboarea" runat="server" 
        onselectedindexchanged="cboarea_SelectedIndexChanged">

    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;
    Fecha:
    <asp:TextBox ID="txtfecha" runat="server" type="date"></asp:TextBox>
    &nbsp;
    <asp:Button ID="btnok" runat="server" Text="Aceptar" onclick="btnok_Click"/>
</div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="402px" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="962px">
        <LocalReport ReportPath="Reportes\ResumenCensoDiario.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>

