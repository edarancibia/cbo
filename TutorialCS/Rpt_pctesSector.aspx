<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rpt_pctesSector.aspx.cs" Inherits="Rpt_pctesSector" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <div>
    SECTOR: 
    <asp:DropDownList ID="cbosector" runat="server" 
            onselectedindexchanged="cbosector_SelectedIndexChanged">
    </asp:DropDownList>
        <asp:Button ID="btnok" runat="server" Text="Aceptar" onclick="btnok_Click" />
    </div>
    <br />
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="752px" 
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="Reportes\Pctes_sector.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="DataSet_PCTES_SECTORTableAdapters.HOS_PCTES_SECTORTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="cbosector" Name="COD_SEC" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

