<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IngresoDirecto.aspx.cs" Inherits="Edit" Culture="es-CL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gestión pacientes</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
    <script type="text/javascript">
      $(document).ready(function(){
          $('.txthora').mask('00:00');
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
                    <div class="header">Ingreso</div>
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
                <td align="right">Tipo paciente:</td>
                <td><asp:DropDownList ID="cbotipopac" runat="server" Width="200px">
                    <asp:ListItem Value="1">QUIRURGICO</asp:ListItem>
                    <asp:ListItem Value="2">MEDICO</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Rut:</td>
                <td><asp:TextBox ID="txtrut" runat="server" Width="200px" MaxLength="8"></asp:TextBox>
                <asp:Button ID="btnvalidarut" runat="server" onclick="btnvalida_Click" style="Display:none"/></td>
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
                <td align="right">Previsión:</td>
                <td><asp:DropDownList ID="cboprev" runat="server" Width="200"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Telefono:</td>
                <td><asp:TextBox ID="txttelefono" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Intervencion:</td>
                <td><asp:TextBox ID="txtinter" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Procedencia:</td>
               <td><asp:DropDownList ID="cbotipoingre" runat="server" Width="200"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">
                    Hora:</td>
                <td>
                    <asp:TextBox ID="txthora" runat="server" Width="200px" type="time"></asp:TextBox>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <br />
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
                    &nbsp;<asp:Label ID="lblperfil" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="lblrut2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
