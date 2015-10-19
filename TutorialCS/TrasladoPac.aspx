<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrasladoPac.aspx.cs" Inherits="Edit" Culture="es-CL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="es-cl" >
<head runat="server">
    <title>Nuevo evento</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
            <script type="text/javascript">
                $(document).ready(function () {
                    $('.txtfecha').mask('dd-mm-aaaa');
                })
            </script>
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">Traslado</div>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right">Fecha ingreso:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server" Width="200px" Visible="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Cama:</td>
                <td><asp:DropDownList ID="cboCamas" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Fecha:</td>
                <td><asp:TextBox ID="txtfecha" runat="server" Width="200px" type="Date" required></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Hora:</td>
                <td><asp:TextBox ID="txthora" runat="server" Width="200px" type="time" required></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Apellido:</td>
                <td><asp:TextBox ID="txtape" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Nombre:</td>
                <td><asp:TextBox ID="txtnom" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Destino:</td>
                <td><asp:DropDownList ID="cbocamas2" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Obs:</td>
                <td><asp:TextBox ID="txtobs" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                      </asp:UpdatePanel>           
                        </td>
                <td>
               
                        &nbsp&nbsp&nbsp<br />

                     <br />&nbsp&nbsp&nbsp&nbsp<asp:ImageButton ID="ButtonOK" runat="server" 
                            OnClick="ButtonOK_Click" ImageUrl="~/media/layout/icon-ok.ico" 
                      OnClientClick="return confirm('¿Seguro que desea trasladar al paciente?');" 
                            ToolTip="Acepta traslado" /> 
                        
                        &nbsp&nbsp&nbsp&nbsp&nbsp
                     &nbsp <asp:ImageButton ID="ButtonCancel" runat="server" 
                            ImageUrl="~/media/layout/cancelar.ico" OnClick="ButtonCancel_Click" 
                            ToolTip="Cancelar" />
                      
                      &nbsp&nbsp       
                            <asp:ImageButton ID="btnpab" runat="server" 
                            ImageUrl="~/media/layout/cirujano.ico" ToolTip="Va a Pabellón" 
                            onclick="btnpab_Click" OnClientClick="return confirm('¿Seguro que desea trasladar al paciente a Pabellón?');"/>   
                            
                      &nbsp&nbsp
                        <asp:ImageButton ID="btnpab2" runat="server" ToolTip="Regresa de Pabellón" ImageUrl="~/media/layout/back-2-1.ico"
                            OnClientClick="return confirm('¿El paciente regresa de Pabellón?');" 
                            onclick="btnpab2_Click"/>         
                        <br />

                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/media/layout/ajax-loader.gif" />

                        
                        <br />Enviando notificación a Aseo...
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:Label ID="lblerror2" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblperfil" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="Label"></asp:Label>
                    <asp:Label ID="lblrut" runat="server" Text="Label"></asp:Label>

                    
                </td>
            </tr>
            <tr>
                <td align="right">&nbsp;</td>
                <td>
               
                     &nbsp;</td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
