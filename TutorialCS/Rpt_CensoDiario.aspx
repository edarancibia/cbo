<%@ Page Title="Censo diario" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rpt_CensoDiario.aspx.cs" Inherits="Rpt_CensoDiario" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
    Desde:
        <asp:TextBox ID="txtfini" runat="server" type="date"></asp:TextBox>
        &nbsp&nbsp&nbsp&nbsp
    Hasta:
        <asp:TextBox ID="txtffin" runat="server" type="date"></asp:TextBox>
        &nbsp&nbsp&nbsp
        <asp:Button ID="Button1" runat="server" Text="Aceptar" 
            onclick="Button1_Click" />
    </div>
        <rsweb:ReportViewer ID="ReportViewer2" runat="server" Width="611px">
        </rsweb:ReportViewer>
    </asp:Content>

