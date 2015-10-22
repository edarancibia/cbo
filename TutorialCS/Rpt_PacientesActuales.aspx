<%@ Page Title="Listado de pacientes hospitalizados" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rpt_PacientesActuales.aspx.cs" Inherits="Rpt_PacientesActuales" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script language="javascript" type="text/javascript">
       function printdiv() {
        var headstr = "<html><head><title></title></head><body>";
        var footstr = "</body>";
        var newstr = $("#ReportViewer1_ctl10").html()
        var popupWin = window.open('', '_blank');
        popupWin.document.write(headstr + newstr + footstr);
        popupWin.print();
        return false;
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ImageButton ID="ImageButton1" runat="server" 
        ImageUrl="~/media/layout/print.png" onclick="ImageButton1_Click" />
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    <br />
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="691px" 
        AsyncRendering="False" InteractivityPostBackMode="AlwaysSynchronous" 
        ShowPrintButton="False" style="margin-right: 0px">
        <LocalReport ReportPath="Reportes\PctesActuales.rdlc">
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
        TypeName="DataSet_Pctes_actualesTableAdapters.HOS_PACIENTES_ACTUALESTableAdapter">
    </asp:ObjectDataSource>
    </asp:Content>

