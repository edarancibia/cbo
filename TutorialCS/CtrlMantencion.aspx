﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="CtrlMantencion.aspx.cs" Inherits="_Default" MasterPageFile="~/MasterPage2.master" Title="Gestión camas CBO" Culture="es-CL"%>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="js/modal.js"></script>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
    <link href='Themes/scheduler_white.css' type="text/css" rel="stylesheet" /> 
	<script type="text/javascript">

	    var modal = new DayPilot.Modal();
	    modal.border = "10px solid #ccc";
	    modal.closed = function () {
	        if (this.result == "OK") {
	            dps.commandCallBack('refresh');
	        }
	    };

	    function createEvent(start, end, resource) {
	        modal.height = 250;
	        modal.showUrl("IniciaMantencion.aspx?start=" + start.toStringSortable() + "&end=" + end.toStringSortable() + "&r=" + resource);
	    }
        
	    function editEvent(id) {
	        modal.height = 300;
	        modal.showUrl("EditaMantencion.aspx?id=" + id);
	    }

	    function afterRender(data) {
	    };

	    function filter(property, value) {
	        if (!dps.clientState.filter) {
	            dps.clientState.filter = {};
	        }
	        if (dps.clientState.filter[property] != value) { // only refresh when the value has changed
	            dps.clientState.filter[property] = value;
	            dps.commandCallBack('filter');
	        }
	    }
	
	</script>
    <style type="text/css">
        .scheduler_white_rowheader 
        {
            background: -webkit-gradient(linear, left top, left bottom, from(#eeeeee), to(#dddddd));
	        background: -moz-linear-gradient(top, #eeeeee 0%, #dddddd);
	        background: -ms-linear-gradient(top, #eeeeee 0%, #dddddd);
	        background: -webkit-linear-gradient(top, #eeeeee 0%, #dddddd);
	        background: linear-gradient(top, #eeeeee 0%, #dddddd);
	        filter: progid:DXImageTransform.Microsoft.gradient(startColorStr="#eeeeee", endColorStr="#dddddd");

        }
        .scheduler_white_rowheader_inner 
        {
        	border-right: 1px solid #ccc;
        }
        .scheduler_white_rowheadercol2
        {
    	    background: White;
        }
        .scheduler_white_rowheadercol2 .scheduler_white_rowheader_inner 
        {
    	    top: 2px;
    	    bottom: 2px;
    	    left: 2px;
    	    background-color: transparent;
	        border-left: 5px solid #1a9d13; /* green */
	        border-right: 0px none;
        }
        .status_dirty.scheduler_white_rowheadercol2 .scheduler_white_rowheader_inner
        {
	        border-left: 5px solid #ea3624; /* red */
        }
        .status_cleanup.scheduler_white_rowheadercol2 .scheduler_white_rowheader_inner
        {
	        border-left: 5px solid #f9ba25; /* orange */
        }
        
    
    </style>	
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div style="margin-bottom: 5px;">
    <h2>Mantenciones en curso</h2>
    SECTOR:
        <asp:DropDownList ID="cbosectorhosp" runat="server" onchange="filter('RoomId', this.value)">
        <asp:ListItem Text="TODOS" Value="0"></asp:ListItem>
        <asp:ListItem Text="SECTOR H" Value="1"></asp:ListItem>
        <asp:ListItem Text="SECTOR F" Value="2"></asp:ListItem>
        <asp:ListItem Text="SECTOR E" Value="11"></asp:ListItem>
        <asp:ListItem Text="SECTOR M" Value="3"></asp:ListItem>
        <asp:ListItem Text="UPC" Value="4"></asp:ListItem>
        </asp:DropDownList>

        &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp
        <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Mantencion2.aspx">Planificación</asp:LinkButton>

                 &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp
         &nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp 
        <asp:Label ID="Label8" runat="server" Text="Ingreso sin confirmar" BackColor="#F781F3" ForeColor="black" Font-Size="9"></asp:Label>&nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label1" runat="server" Text="R. sin confirmar" BackColor="Aqua" ForeColor="Black" Font-Size="9"></asp:Label>&nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label2" runat="server" Text="R. Confirmada" BackColor="Blue" Font-Size="9" 
            ForeColor="White"></asp:Label> &nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label3" runat="server" Text="Ocupada" BackColor="Green" ForeColor="White" Font-Size="9"></asp:Label>  &nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label4" runat="server" Text="Aseo" BackColor="Yellow" Font-Size="9"></asp:Label>&nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label5" runat="server" Text="Alta probable" BackColor="RosyBrown" ForeColor="White" Font-Size="9"></asp:Label>&nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label6" runat="server" Text="Alta médica" BackColor="Brown" ForeColor="White" Font-Size="9"></asp:Label>&nbsp&nbsp&nbsp&nbsp
        <asp:Label ID="Label7" runat="server" Text="Mantención" BackColor="Orange" ForeColor="White" Font-Size="9"></asp:Label>
                                                                   
    </div>
    <div align="center">
    <DayPilot:DayPilotScheduler 
        ID="DayPilotScheduler1" 
        runat="server" 
        
        DataStartField="ReservationStart" 
        DataEndField="ReservationEnd" 
        DataTextField="ReservationName" 
        DataValueField="ReservationId" 
        DataResourceField="RoomId" 
        DataTagFields="ReservationStatus"
        
        ClientObjectName="dps"
        
        CellGroupBy="Month"
        CellDuration="1440"
        Days="1"
        
        HeightSpec="Max"
        Height="500"
        Width="47%"
        HeaderFontSize="8pt"
        EventFontSize="8pt"
        CellWidth="180"

        EventMoveHandling="CallBack"  

		EventResizeHandling="CallBack"

		TimeRangeSelectedHandling="JavaScript"
		TimeRangeSelectedJavaScript="createEvent(start, end, column);" 

		OnCommand="DayPilotScheduler1_Command"

		EventClickHandling="JavaScript"
		EventClickJavaScript="editEvent(e.value());" 

		AfterRenderJavaScript="afterRender(data);" 

        OnBeforeEventRender="DayPilotScheduler1_BeforeEventRender" OnBeforeCellRender="DayPilotScheduler1_BeforeCellRender"
        
        FreeTimeClickHandling=""
        JavaScriptFreeAction=""

		CssOnly="true"
		CssClassPrefix="scheduler_white"
		RowHeaderWidthAutoFit="true"
		EventHeight="50"
		DurationBarVisible="true"
		SyncResourceTree="false"

		OnBeforeResHeaderRender="DayPilotScheduler1_BeforeResHeaderRender"
   
        >
        <HeaderColumns>
            <DayPilot:RowHeaderColumn Title="CAMA" Width="80" />
            <DayPilot:RowHeaderColumn Title="ESTADO" Width="80" />

        </HeaderColumns>
    </DayPilot:DayPilotScheduler>
    </div>
    <br />

</asp:Content>
