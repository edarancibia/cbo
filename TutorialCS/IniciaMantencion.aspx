<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IniciaMantencion.aspx.cs" Inherits="New" Culture="es-CL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Nueva mantencion</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
    <script type="text/javascript">
        function validar() {
            var boton = document.getElementById("bntvalida");
            boton.click();
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 267px;
        }
    </style>
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td class="style1">
                    <div class="header">Iniciar mantención</div>
                </td>
            </tr>
            <tr>
                <td align="right">Inicio:</td>
                <td class="style1"><asp:TextBox ID="TextBoxStart" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Fin:</td>
                <td class="style1"><asp:TextBox ID="TextBoxEnd" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Cama:</td>
                <td class="style1"><asp:DropDownList ID="cbocama" runat="server" Width="200"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Hora inicio:</td>
                <td class="style1"><asp:TextBox ID="txthoraini" runat="server" Width="200" MaxLength="5"></asp:TextBox></td>
            </tr>
            <tr>
            <td>Detalle:</td>
            <td><asp:TextBox ID="txtobs" runat="server" Width="200"></asp:TextBox></td>              
            </tr>
            <tr>
                <td align="right">&nbsp;</td>
                <td class="style1">
                    <br />
                    <asp:Button ID="bntini" runat="server" Text="Iniciar" Width="75px" 
                        onclick="bntini_Click" OnClientClick="return confirm('¿Confirma que desea comenzar la mantención?');" />&nbsp&nbsp
                    &nbsp&nbsp
                    <asp:Button ID="btncancelar" runat="server" Text="Cancelar" Width="75px" 
                        onclick="btncancelar_Click" />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td class="style1">
                      &nbsp &nbsp&nbsp &nbsp
                    <br />
                    <br />
                    <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                        &nbsp
                    <asp:Label ID="lblperfil" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
