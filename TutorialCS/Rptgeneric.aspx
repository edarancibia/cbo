<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rptgeneric.aspx.cs" Inherits="Rptgeneric" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<div>
    TIPO :
    <asp:DropDownList ID="DropDownList1" runat="server">
    <asp:ListItem Value="6">RESERVAS S/C</asp:ListItem>
    <asp:ListItem Value="7">RESERVAS CONFIRMADAS</asp:ListItem>
    <asp:ListItem Value="5">INGRESOS</asp:ListItem>
    <asp:ListItem Value="2">ALTAS</asp:ListItem>
    </asp:DropDownList>
    &nbsp&nbsp&nbsp&nbsp
    DESDE :
    <td><asp:TextBox ID="txtfechaini" runat="server" Width="200px" type="Date"></asp:TextBox></td>

    &nbsp&nbsp&nbsp&nbsp
    HASTA :
    <td><asp:TextBox ID="txtfechafin" runat="server" Width="200px" type="Date"></asp:TextBox></td>
    &nbsp&nbsp&nbsp&nbsp
    <asp:Button ID="bntok" runat="server" Text="Aceptar" onclick="bntok_Click" />
    <rsweb:ReportViewer ID="ReportViewer2" runat="server" Width="740px" 
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="Reportes\Reporte_generico.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="DataSet_RTPGENERICOTableAdapters.HOS_RPT_GENERICOTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownList1" Name="tipo" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="txtfechaini" Name="fechaini" 
                PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="txtfechafin" Name="fechafin" 
                PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>

</div>
</asp:Content>

