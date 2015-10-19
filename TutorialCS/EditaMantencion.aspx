<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditaMantencion.aspx.cs" Inherits="New" Culture="es-CL" %>
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
                    <div class="header">Gestión de mantención programada<br />
                        <br />
                        <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="Small" 
                            onclick="LinkButton1_Click">Cancelar mantencion</asp:LinkButton>
                    </div>
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
                <td align="right">&nbsp;
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:Button ID="btnfin" runat="server" Text="Finalizar" Width="75px" 
                        onclick="btnfin_Click" OnClientClick="return confirm('¿Confirma que desea terminar la mantención?');" />
                       </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td class="style1">
                    <br />
                    &nbsp&nbsp

                        &nbsp&nbsp
                    <asp:Button ID="btncancelar" runat="server" Text="Salir" Width="75px" 
                        onclick="btncancelar_Click" />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td class="style1">
                      &nbsp &nbsp&nbsp &nbsp
                    <br />
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/media/layout/ajax-loader.gif" />
                        <br />Enviando notificación a Aseo...
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                    <br />
                    <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                        &nbsp
                    <asp:Label ID="lblperfil" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
