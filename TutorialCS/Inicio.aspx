<%@ Page Title="Gestión camas CBO" Language="C#" MasterPageFile="~/MasterPageInicio.master" AutoEventWireup="true" CodeFile="Inicio.aspx.cs" Inherits="Inicio" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript" src="js/modal.js"></script>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
    <link href='Themes/scheduler_white.css' type="text/css" rel="stylesheet" /> 

    <style type="text/css">
    .buscador
    {
        float: right;
        font-size: 8px;
    }
    
    .txt
    {
        border:1px solid #456879;
	    border-radius:10px;
	    height: 20px;
	    width: 170px;
    }
    

     .myButton {
	    background-color:#599bb3;
	    -moz-border-radius:31px;
	    -webkit-border-radius:31px;
	    border-radius:30px;
	    border:1px solid #29668f;
	    display:inline-block;
	    cursor:pointer;
	    color:#ffffff;
	    font-family:Arial;
	    font-size:16px;
	    padding:5px 10px;
	    text-decoration:none;
	    text-shadow:0px 1px 0px #3d768a;
        }
    .myButton:hover {
	    background-color:#408c99;
    }
    .myButton:active {
	    position:relative;
	    top:1px;
     }
        
    #buscador
    {
        
        color: blue;
    }
    </style>

    <script type="text/javascript" language="javascript">

        function ValidarTextBox() {
            var text = document.getElementById("<%=txtficha.ClientID%>");
            if (text.value == '') {
                text.focus();
                alert("Ingrese número de ficha");
                return false;
            }

        }

        function AcceptNum(evt) {

            var nav4 = window.Event ? true : false;

            var key = nav4 ? evt.which : evt.keyCode;

            return (key <= 13 || (key >= 48 && key <= 57) || key == 44);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<br />
<div>
    <asp:Label ID="labelpendientes" runat="server" Text="Traslados pendientes: " ForeColor="Red" Font-Bold="true"></asp:Label>
     &nbsp;&nbsp;
    <asp:Label ID="lblpenh" runat="server" Text="Label" ForeColor="Red"></asp:Label>
     &nbsp;&nbsp;
    <asp:Label ID="lblpenf" runat="server" Text="Label" ForeColor="Red"></asp:Label>
     &nbsp;&nbsp;
    <asp:Label ID="lblpenm" runat="server" Text="Label" ForeColor="Red"></asp:Label>
     &nbsp;&nbsp;
    <asp:Label ID="lblpene" runat="server" Text="Label" ForeColor="Red"></asp:Label>
     &nbsp;&nbsp;
    <asp:Label ID="lblpenu" runat="server" Text="Label" ForeColor="Red"></asp:Label>
</div>
<br />
<div id="buscador">
    <asp:Label ID="Label1" runat="server" Text="Buscador de pacientes:"></asp:Label>
    <asp:TextBox ID="txtbuscardor" runat="server" placeholder="Buscar apellido" 
        class="txt"></asp:TextBox>
    <asp:Button ID="btnbuscar" runat="server" Text="Buscar" class="myButton"
        onclick="btnbuscar_Click" />

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="Label2" runat="server" Text="Trazabilidad paciente:" ></asp:Label>
    <asp:TextBox ID="txtficha" runat="server" placeholder="Ingrese ficha" onkeydown="return jsDecimals(event);"
        class="txt"></asp:TextBox>
    <asp:Button ID="btnHistorial" runat="server" Text="Buscar"  class="myButton" OnClientClick="return ValidarTextBox();"
        onclick="btnHistorial_Click"/>
</div>
<br />
 <div class="tabla" align="center">
 <table class="tabla">
 <tr>
    <td colspan="2"><strong>Resumen General</strong></td>
 </tr>
 <tr>
    <td><asp:Label ID="lblocupa" runat="server" Text="Label"></asp:Label></td>
    <td><asp:Label ID="lblcantPac" runat="server" Text="Label"></asp:Label></td>
 </tr>
 <tr>
    <td colspan="2" align="center">
     <asp:Label ID="lblindice" runat="server" Text="Label"></asp:Label></td>
 </tr>
 <tr>
    <td><asp:Label ID="lblsenso" runat="server" Text="Label"></asp:Label></td>
    <td><asp:Label ID="lblupc" runat="server" Text="Label"></asp:Label></td>
 </tr>
 <tr>
    <td colspan="2">Sector H</td>
 </tr>
 <tr>
   <td><asp:Label ID="lblsech" runat="server" Text="Label"></asp:Label></td>
   <td><asp:Label ID="lbldish" runat="server" Text="Label"></asp:Label></td> 
 </tr>
 <tr>
    <td colspan="2">Sector F</td>
 </tr>    
 <tr>
   <td><asp:Label ID="lblsecf" runat="server" Text="Label"></asp:Label></td>
   <td><asp:Label ID="lbldisf" runat="server" Text="Label"></asp:Label></td>
 </tr>
 <tr>
    <td colspan="2">Sector E</td>
 </tr>
 <tr>
    <td><asp:Label ID="lblsece" runat="server" Text="Label"></asp:Label></td>
    <td><asp:Label ID="lbldise" runat="server" Text="Label"></asp:Label> </td>
 </tr>
 <tr>
    <td colspan="2"> Maternidad</td>
 </tr>
 <tr>
   <td><asp:Label ID="lblsecm" runat="server" Text="Label"></asp:Label></td>
   <td><asp:Label ID="lbldism" runat="server" Text="Label"></asp:Label></td>
  </tr>
 </table>

 </div>

 <br />
<br />
<br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br />
</asp:Content>

