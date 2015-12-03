<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="plano.aspx.cs" Inherits="plano" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style>
    table td {
    width: 70px;
    height: 50px;
    overflow: hidden;
    white-space: nowrap;
    text-align: center;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server"> 
<div>
<asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <table border="1">
        <tr>
            <td>H 26</td>
            <td>H 27</td>
            <td>H 28</td>
            <td>H 29</td>
            <td>H 30</td>
            <td>H 31</td>
            <td>H 32</td>
            <td>H 33</td>
            <td>H 34</td>
            <td>H 35</td>
        </tr>
        <tr>
            <td colspan="10">Pasillo</td>
        </tr>
        <tr>
            <td>H 39</td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td>H 38</td>
        </tr>
    </table>
</div>
</asp:Content>

