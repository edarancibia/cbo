<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IngresoTraslado.aspx.cs" Inherits="Edit" Culture="es-CL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gestión pacientes</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">Ingreso por traslado</div>
                    <br />
                </td>
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
                <td align="right">Inicio:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server" Width="200px" Visible="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Destino:</td>
                <td><asp:DropDownList ID="cboCamas" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right"> Fecha:</td>
                <td><asp:TextBox ID="txtfecha" runat="server" Width="200px" type="date" required ></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"> Hora:</td>
                <td><asp:TextBox ID="txthora" runat="server" Width="200px" type="time" ></asp:TextBox></td>
            </tr>
             <tr>
                <td align="right"> Obs:</td>
                <td><asp:TextBox ID="txtobs" runat="server" Width="200px" Enabled="false" ></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:ImageButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" ImageUrl="~/media/layout/icon-ok.ico" 
                      OnClientClick="return confirm('¿Confirma que desea ingresar el paciente?');" />
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
