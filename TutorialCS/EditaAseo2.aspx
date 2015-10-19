<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditaAseo2.aspx.cs" Inherits="EditaAseo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Terminar aseo</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
    <script>
        function ocultar(combo) {
            
            if (document.getElementById('<%= cbotipoaseo2.ClientID %>').value == 15) {
                txtobs.style.display = "block";
                lblobs.style.display = "block";
            } else {
                txtobs.style.display = "none";
                lblobs.style.display = "none";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table border="0" cellspacing="4" cellpadding="0">
         <tr>
            <td align="right"></td>
              <td>
                 <div class="header">Finalizar aseo</div><br />
            </td>
         </tr>

         <tr>
           <td align="right"">Cama:</td>
           <td><asp:DropDownList ID="cboCamas3" runat="server" Width="200px"></asp:DropDownList></td><br />
         </tr>

         <tr>
           <td align="right">Tipo:</td>
           <td><asp:DropDownList ID="cbotipoaseo2" runat="server" Width="200px" onchange="ocultar(this)"></asp:DropDownList></td>
         </tr>
         
         <tr>
           <td align="right">Hora inicio:</td>
           <td><asp:TextBox ID="txtini2" runat="server" Width="200px" MaxLength="5"></asp:TextBox></td>
         </tr>
         
         <tr>
           <td align="right">Fin estimado:</td>
           <td><asp:TextBox ID="txtfinest" runat="server" Width="200px" MaxLength="5"></asp:TextBox></td>
         </tr> 
         <tr>
            <td><asp:Label ID="lblobs" runat="server" Text="Detalle:" style="display:none"></asp:Label></td>
            <td ><asp:TextBox ID="txtobs" runat="server" Width="200" style="display:none"></asp:TextBox></td>
         </tr>
         <tr>
            <td align="right">
               <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
             <asp:Button ID="bntcambiatipo" runat="server" onclick="Button2_Click" 
                    Text="Cambiar tipo" Width="90px"  OnClientClick="return confirm('¿Confirma que desea cambiar el tipo de aseo?');"/>
                    </ContentTemplate>
                    </asp:UpdatePanel>
            </td>
            <td>

                &nbsp&nbsp
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Finalizar" Width="90px" OnClientClick="return confirm('¿Confirma que desea dar por terminado el aseo?');"/>
                &nbsp&nbsp
                <asp:Button ID="bntcancela" runat="server" onclick="Button3_Click" 
                    Text="Salir" Width="90px" />
             </td>
         </tr>
         
         <tr>
          <td>&nbsp;</td>
          <td>
           <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/media/layout/ajax-loader.gif" />
                        <br />Enviando notificación a mantención...
                    </ProgressTemplate>
           </asp:UpdateProgress>
          <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
                  ForeColor="Red"></asp:Label>&nbsp;
              <asp:Label ID="lblperfil" runat="server" Font-Bold="True" ForeColor="Red" 
                  Text="Label"></asp:Label>
             &nbsp;<asp:Label ID="lblcama" runat="server" Text="Label"></asp:Label>
             </td>
         </tr>
           
      </table>
    </div>
    </form>
</body>
</html>
