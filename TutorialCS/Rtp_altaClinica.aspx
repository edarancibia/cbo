<%@ Page Title="Alta Clinica" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rtp_altaClinica.aspx.cs" Inherits="Rtp_altaClinica" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <asp:ImageButton ID="ImageButton1" runat="server" 
        ImageUrl="~/media/layout/print.png" onclick="ImageButton1_Click" />
    <br />

    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="725px">
        <LocalReport ReportPath="Reportes\altaMedica.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>

     <iframe id="frmPrint" name="IframeName" width="500" 
  height="200" runat="server" 
  style="display: none" runat="server"></iframe>

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="DataSet_etiquetaTableAdapters.HOS_ETIQUETATableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox1" Name="FICHA" PropertyName="Text" 
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

