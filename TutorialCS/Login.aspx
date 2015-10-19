<%@ Page Language="C#" MasterPageFile="~/MasterPageInicio.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login"  Title="Gestión camas CBO"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="tablalogin" align="center">
    <table class="login" >
            <tr class="titulo">
                <td></td>
            </tr>
            <tr>
                <td>
                    <table width="400" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td class="style1">&nbsp;Contraseña : </td>
                        <td><asp:TextBox Id="txtPassword" width="150" TextMode="Password" placeholder="Contraseña" runat="server"/><br /></td>
                      </tr>
                      
                      <tr>
                      
                        <td class="style1"> 
                            <br />
                            <br />
                            <br />
                          </td>
                        <td><asp:Button Id="cmdLogin" Text="Iniciar sesión" runat="server" 
                                onclick="cmdLogin_Click" /></td>
                      </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
        <br/>
        <br/>
        <div id="ErrorMessage" runat="server" />
</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 78px;
        }
    </style>
</asp:Content>


