<%@ Page Title="Gestion Camas CBO" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Resultado.aspx.cs" Inherits="Resultado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 <link href="media/Estilos.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="resultado" align="center">
<table border="1">
 <tr>
   <td><strong>PACIENTE</strong></td>
   <td><strong>CAMA</strong></td>
 </tr>
 <tr>
  <td><asp:Label ID="lblpac" runat="server" Text="Label"></asp:Label></td>
  <td><asp:Label ID="lblcama" runat="server" Text="Label"></asp:Label></td>
 </tr>
</table>


</div>
</asp:Content>

