<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreIngresoPacEdita.aspx.cs" Inherits="Edit" Culture="es-CL" %>
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
                    <div class="header">Modificar ingreso</div>
                    <br />
                    <br />
                    <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Cancelar ingreso</asp:LinkButton>&nbsp&nbsp
                    <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">Crear ingreso</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td align="right">Ficha:</td>
                <td><asp:TextBox ID="txtficha" runat="server" Width="200px" MaxLength="6"></asp:TextBox></td>
                <asp:Button ID="btnvalidaficha" runat="server" onclick="btnvalidaficha_Click" style="Display:none"/>
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
                <td align="right">Cama:</td>
                <td><asp:DropDownList ID="cboCamas" runat="server" Width="200px"></asp:DropDownList></td>
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
                <td align="right">Tipo paciente:</td>
                <td><asp:DropDownList ID="cbotipopac" runat="server" Width="200px">
                    <asp:ListItem Value="1">MEDICO</asp:ListItem>
                    <asp:ListItem Value="2">QUIRUGICO</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
 
            <tr>
                <td align="right">Tipo ingreso:</td>
                <td><asp:DropDownList ID="cbotipoingre" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>

            <tr>
             <td><asp:Label ID="lblmant" runat="server" Text="Mantención:"></asp:Label></td>
             <td><asp:TextBox ID="txtobsmant" runat="server" Width="200" Enabled="false"></asp:TextBox></td>
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
                    <br />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
