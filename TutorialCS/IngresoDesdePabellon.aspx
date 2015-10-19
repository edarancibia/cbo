<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IngresoDesdePabellon.aspx.cs" Inherits="Edit" Culture="es-CL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gesti�n pacientes</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">Ingreso pabellon</div>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right">Inicio:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Fin:</td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Cama:</td>
                <td><asp:DropDownList ID="cboCamas" runat="server" Width="200px"></asp:DropDownList></td>
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
                <td align="right">Tipo paciente:</td>
                <td><asp:DropDownList ID="cbotipopac" runat="server" Width="200px">
                    <asp:ListItem Value="1">QUIRUGICO</asp:ListItem>
                    <asp:ListItem Value="2">MEDICO</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">
                    Hora:</td>
                <td>
                    <asp:TextBox ID="txthora" runat="server" Width="200px" TextMode="Time"></asp:TextBox>
                    <td>&nbsp;</td>
                </td>
            </tr>
            <tr>
                <td align="right">Tipo ingreso:</td>
                <td><asp:DropDownList ID="cbotipoingre" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:ImageButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" ImageUrl="~/media/layout/icon-ok.ico" 
                      OnClientClick="return confirm('�Confirma que desea ingresar el paciente?');" />
                      &nbsp&nbsp&nbsp&nbsp
                    <asp:ImageButton ID="ImageButton1" runat="server" 
                        ImageUrl="~/media/layout/cancelar.ico" OnClick="ButtonCancel_Click" 
                        Width="40px" />
                    <br />
                    <br />
                    <asp:Label ID="lblerror2" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    &nbsp;<asp:Label ID="lblperfil" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="Label"></asp:Label>
                    <br />
                    <asp:Label ID="lblrut2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
