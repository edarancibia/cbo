<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreIngresoPac.aspx.cs" Inherits="Edit" Culture="es-CL" %>
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
                    <div class="header">Ingreso a admisi�n</div>
                    <br />
                    <br />
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
                <td align="right">Fecha ingreso:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server" Width="200px" Visible="false"></asp:TextBox></td>
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
                <td></td>
                
                <td><asp:RadioButton ID="RadioButton1" runat="server" GroupName="aislamiento" Text="Bloqueo exclusiva" /></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:RadioButton ID="RadioButton2" runat="server" GroupName="aislamiento" text="Aislamiento m�dico"/></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:RadioButton ID="RadioButton3" runat="server" GroupName="aislamiento" Text="Aislamiento edad"/></td>
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
                    <br />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
