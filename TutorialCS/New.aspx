<%@ Page Language="C#" AutoEventWireup="true" CodeFile="New.aspx.cs" Inherits="New" Culture="es-CL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Nueva reserva</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
    <script type="text/javascript">
        function validar() {
            var boton = document.getElementById("bntvalida");
            boton.click();
        }

        function ValidarTextBox() {
            var text = document.getElementById("<%=txtnombre.ClientID%>");
            var text2 = document.getElementById("<%=txtape.ClientID%>");
            if (text.value == '') {
                text.focus();
                alert("Ingrese nombre");
                return false;
            }

            if (text2.value == '') {
                text2.focus();
                alert("Ingrese apellido");
            }
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
                    <div class="header">Nueva reserva</div>
                </td>
            </tr>
            <tr>
                <td align="right">Inicio:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server" Width="200" Enabled="false"></asp:TextBox></td>
            </tr>
            <tr>
            <td></td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server" Width="200" Visible="false"></asp:TextBox></td>
                
            </tr>
            <tr>
                <td align="right">Cama:</td>
                <td><asp:DropDownList ID="cbocama" runat="server" Width="200"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Rut:</td>
                <td><asp:TextBox ID="txtrut" runat="server" MaxLength="8" Width="150" 
                        ontextchanged="txtrut_TextChanged" onblur="validar()"></asp:TextBox>
                    <asp:Button ID="btnvalida" runat="server" onclick="btnvalida_Click" style="Display:none"/>
                    -
                    <asp:TextBox ID="txtdig" runat="server" Width="30" MaxLength="1"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td align="right">Apellido paterno:</td>
                <td><asp:TextBox ID="txtape" runat="server" Width="200"></asp:TextBox></td>
            </tr>
             <tr>
                <td align="right">Apellido materno:</td>
                <td><asp:TextBox ID="txtape2" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Nombre:</td>
                <td><asp:TextBox ID="txtnombre" runat="server" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Teléfono:</td>
                <td><asp:TextBox ID="txttelefono" runat="server" MaxLength="10" Width="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td><asp:DropDownList ID="cboprev" runat="server" Width="200" Visible="false"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td><asp:TextBox ID="txtinter" runat="server" Width="200" Visible="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Sexo :</td>
                <td><asp:RadioButton ID="RadioButton1" runat="server" GroupName="sexo" 
                        Text="Masculino" Checked="True"/><asp:RadioButton ID="RadioButton2"
                        runat="server" GroupName="sexo" Text="Femenino" /></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:ImageButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" ImageUrl="~/media/layout/icon-ok.ico"
                      OnClientClick="return ValidarTextBox();" />
                      &nbsp &nbsp
                    <asp:ImageButton ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" ImageUrl="~/media/layout/cancelar.ico" />
                    <br />
                    <br />
                    <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    &nbsp;<br />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
