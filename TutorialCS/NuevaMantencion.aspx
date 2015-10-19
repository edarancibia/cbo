<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NuevaMantencion.aspx.cs" Inherits="New" Culture="es-CL" %>
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
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">Nueva mantención</div>
                </td>
            </tr>
            <tr>
                <td align="right">Inicio:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Fin:</td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Cama:</td>
                <td><asp:DropDownList ID="cbocama" runat="server" Width="200"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Hora inicio:</td>
                <td><asp:TextBox ID="txthoraini" runat="server" Width="200" MaxLength="5"></asp:TextBox></td>
            </tr>
            
             <tr>
                <td align="right">Detalle:</td>
                <td><asp:TextBox ID="txtdet" runat="server" Width="200"></asp:TextBox></td>
            </tr>

            <tr>
                <td align="right"></td>
                <td>
                    <asp:ImageButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" ImageUrl="~/media/layout/icon-ok.ico"
                      OnClientClick="return confirm('¿Confirma que desea programar la mantención?');" />
                      &nbsp &nbsp&nbsp &nbsp
                    <asp:ImageButton ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" ImageUrl="~/media/layout/cancelar.ico" />
                    <br />
                    <br />
                    <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    &nbsp;<asp:Label ID="lblperfil" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="Label"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
