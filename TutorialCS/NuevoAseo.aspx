<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NuevoAseo.aspx.cs" Inherits="EditaAseo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Iniciar aseo</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table border="0" cellspacing="4" cellpadding="0">
         <tr>
            <td align="right"></td>
              <td>
                 <div class="header">Iniciar aseo</div><br />
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
                </td>
                <td>

                &nbsp
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Iniciar" Width="90px"/>
                &nbsp
                <asp:Button ID="bntcancela" runat="server" onclick="Button3_Click" 
                    Text="Salir" Width="90px" />
             </td>
         </tr>
         
         <tr>
          <td>&nbsp;</td>
          <td>
             <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" 
                  ForeColor="Red"></asp:Label>&nbsp;
              &nbsp;</td>
         </tr>
           
      </table>
    </div>
    </form>
</body>
</html>
