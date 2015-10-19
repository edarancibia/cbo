<%@ Page Title="Listado de pacientes hospitalizados" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Rpt_PacientesActuales.aspx.cs" Inherits="Rpt_PacientesActuales" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script>
    // Guardando la URL de la imagen del botón imprimir. Esta imagen es una copia de la imagen mostrada en el ReportViewer, guardada en la carpeta de imágenes de la aplicación web.
    var urlImg = '<%=Page.ResolveUrl("~/Content/themes/base/minified/images/Print.gif") %>';

    // Función que se ejecuta una vez se ha terminado de cargar el DOM de la página web en el navegador
    $(document).ready(function () {
        colocarBtnImprimir();    // Colocar el botón de imprimir en la barra de herramientas del ReportViewer
        $("#BtnImprimir").click(imprimirDiv);  // Asignando la función "imprimirDiv" al evento click del botón de impresión
    });

    // Esta función coloca el botón de imprimir en la barra de herramientas del ReportViewer
    function colocarBtnImprimir() {
        var jqoBarraRpt = $('div#ReportViewer1_ctl05>div:first-child');    // Buscando el div que contiene la barra de herramientas del RportViewer

        if (jqoBarraRpt && jqoBarraRpt.length > 0    // Verificando que el DIV barra de herramientas fue encontrado,
        && jqoBarraRpt.find('#BtnImprimir').length <= 0) {    // y verificando que el botón de imprimir no existe ya

            // Colocando el botón de impresión, con una estructura similar a la que tiene el botón original en el ReportViewer
            jqoBarraRpt.append('<table cellpadding="0" cellspacing="0" ToolbarSpacer="true" style="display:inline;width:10px;"><tr><td></td></tr></table><div style="display:inline;font-family:Verdana;font-size:8pt;vertical-align:top;"><table cellpadding="0" cellspacing="0" style="display:inline;"><tr><td height="28px"><div"><div id="BtnImprimir"><table title="Print"><tr><td><img title="Print" src="' + urlImg + '" alt="Print" style="border-style:None;height:16px;width:16px;" /></td></tr></table></div><div disabled="disabled" style="display:none;border:1px transparent Solid;"><table title="Print"><tr><td><input type="image" disabled="disabled" title="Print" src="' + urlImg + '" alt="Print" style="border-style:None;height:16px;width:16px;cursor:default;" /></td></tr></table></div></div></td></tr></table></div>');
        }
    }

    // Función que se encarga de imprimir el reporte
    function imprimirDiv() {
        var divImprimir = $("div[id$='ReportDiv']").parent();    // Obteniendo el DIV que contiene el reporte a imprimir
        var newWin = window.open();    // Abriendo una nueva ventana del navegador
        newWin.document.open();    // Abriendo el documento de la nueva ventana, para escribir su contenido
        newWin.document.write('<html><head><style type="text/css">' + getAllStyleSheetsAsText() + '</style></head><body>' + divImprimir.html() + '</body>');
        newWin.document.close();
        newWin.print();
        newWin.close();
    }

    function getAllStyleSheetsAsText() {
        var cssText = '';
        var sheets = document.styleSheets;
        for (var c = 0; c < sheets.length; c++) {
            var sheet = sheets[c];
            if ((sheet.ownerNode || sheet.owningElement).id.endsWith('_ReportControl_styles')) {
                var rules = sheet.rules || sheet.cssRules;
                for (var r = 0; r < rules.length; r++) {
                    var cssRule = rules[r];
                    if ($.browser.msie) {
                        var cssText = cssText + cssRule.selectorText + '{' + cssRule.style.cssText.toLowerCase() + '}';
                    } else {
                        var cssText = cssText + cssRule.cssText;
                    }
                }
            }
        }
        return cssText;
    }
</script>
<style type="text/css">
#BtnImprimir {
    border: 1px solid transparent;
}
#BtnImprimir:hover {
    border: 1px solid rgb(51,102,153);
    background-color: rgb(221,238,247);
    cursor:pointer;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    <br />
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="803px" AsyncRendering="False" InteractivityPostBackMode="AlwaysSynchronous" ShowPrintButton="False">
        <LocalReport ReportPath="Reportes\PctesActuales.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="DataSet_Pctes_actualesTableAdapters.HOS_PACIENTES_ACTUALESTableAdapter">
    </asp:ObjectDataSource>
    </asp:Content>

