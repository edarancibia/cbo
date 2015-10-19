<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AltaPac.aspx.cs" Inherits="Edit" Culture="es-CL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gestión pacientes</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" />
    <script>
        $(document).on('ready', function () {
            $('#ButtonOK').on('click', function () {
                
            });
        });
    </script>

</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">Altas</div>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right">Fecha ingreso:</td>
                <td><asp:TextBox ID="txtini2" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
            <td></td>
                <td><asp:TextBox ID="txtfin2" runat="server" Width="200px" Visible="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Cama:</td>
                <td><asp:DropDownList ID="cboCamas3" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Apellido:</td>
                <td><asp:TextBox ID="txtape3" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Nombre:</td>
                <td><asp:TextBox ID="txtnom3" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Tpo alta:</td>
                <td><asp:DropDownList ID="cboalta" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Ultima acción:</td>
                <td><asp:TextBox ID="txtultima" runat="server" Width="200px" Enabled="false"></asp:TextBox></td>
            </tr>

            <tr>
                <td align="right">Hora:</td>
                <td><asp:TextBox ID="txthora" runat="server" Width="200px" type="time" ></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                     <asp:ImageButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" ImageUrl="~/media/layout/icon-ok.ico" 
                       OnClientClick="return confirm('¿Confirma que desea dar de alta al paciente?');"  ToolTip="Aceptar" />

                </td>
                <td>
                      &nbsp;&nbsp;&nbsp;<br />
                      <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" 
                        ImageUrl="~/media/layout/cancelar.ico" OnClick="ButtonCancel_Click" 
                        Width="40px" ToolTip="Cancelar" />

                        &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btndeshacer" runat="server" 
                          ImageUrl="~/media/layout/deshacer-icono-5993-48.png" Width="40px" 
                          ToolTip="Cancelar alta" 
                          OnClientClick="return confirm('¿Seguro que quiere cancelar el alta de este paciente?')" 
                          onclick="btndeshacer_Click" />
                    <br />
                    <asp:Label ID="lblerror3" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblperfil" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="Label"></asp:Label>
                    <br />
                    <asp:Label ID="lblrut3" runat="server" Text="Label"></asp:Label>
                    
                </td>
                
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
