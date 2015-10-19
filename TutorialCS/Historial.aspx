<%@ Page Title="Historial estadía clínica" Language="C#" MasterPageFile="~/MasterPageInicio.master" AutoEventWireup="true" CodeFile="Historial.aspx.cs" Inherits="Historial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style type="text/css">
    body
    {
        font-family: Arial;
        font-size: 10pt;
    }
    td
    {
        cursor: pointer;
    }
    .hover_row
    {
        background-color: #A1DCF2;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div>
    <asp:Label ID="lblpac" runat="server" Text="Label" Font-Bold="True" 
        ForeColor="Blue"></asp:Label><br /><br />

    <asp:GridView ID="GridView1" runat="server" Width="692px" HeaderStyle-BackColor="#3AC0F2">
    </asp:GridView>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=GridView1] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
</div>
<br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br />
</asp:Content>

