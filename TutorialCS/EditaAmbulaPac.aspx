<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditaAmbulaPac.aspx.cs" Inherits="Edit" Culture="es-CL" %>
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
                    <div class="header">Gestionar pacientes</div>
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
                <td align="right">Hora:</td>
                <td><asp:TextBox ID="txthora" runat="server" Width="200px" type="time" ></asp:TextBox></td>
            </tr>

            <tr>
                <td align="right">Tipo alta:</td>
                <td><asp:DropDownList ID="cbotipoalta" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>

            <tr>
                <td align="right">Cama Hospitalización</td>
                <td><asp:TextBox ID="txtcamahosp" runat="server" Width="200px"></asp:TextBox></td>
                
            </tr>
            <tr>
                <td align="right">
                                      &nbsp;&nbsp;&nbsp;<br />
                      <br />&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:ImageButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" ImageUrl="~/media/layout/alta-icono.ico"
                       OnClientClick="return confirm('¿Confirma que desea dar de alta al paciente?');"  ToolTip="Dar de alta" />

                </td>
                <td>
                      &nbsp;&nbsp;&nbsp;<br />
                      <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   &nbsp;
                     <asp:ImageButton ID="btndeshacer" runat="server" 
                          ImageUrl="~/media/layout/ingreso-icono.ico" Width="40px" 
                          ToolTip="Ingresar a cama definitiva" 
                          OnClientClick="return confirm('¿Seguro que quiere ingresar este paciente?')" 
                          onclick="btndeshacer_Click" />

                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImageButton1" runat="server" 
                        ImageUrl="~/media/layout/cancelar.ico" OnClick="ButtonCancel_Click" 
                        Width="40px" ToolTip="Cancelar" />
                    <br />
                    <asp:Label ID="lblerror3" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    &nbsp;
                    <br />
                    
                </td>
                
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
