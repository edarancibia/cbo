<%@ Page Title="Ambulatorio" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Ambulatorio.aspx.cs" Inherits="Ambulatorio" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

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
	    modal.showUrl("PreIngresoAmbula.aspx?start=" + start.toStringSortable() + "&end=" + end.toStringSortable() + "&r=" + resource);
	    }

	    function editEvent(id, ID_ESTADOCAMA) {
	        modal.height = 300;
	        modal.showUrl("EditaAmbulaPac.aspx?id=" + id);
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Pacientes ambulatorios</h2>
    <div style="margin-bottom: 5px;" align="center">
    SECTOR:
        <asp:DropDownList ID="cbosectorhosp" runat="server" onchange="filter('room', this.value)">
        <asp:ListItem Text="SELECCIONE" Value="0"></asp:ListItem>
        <asp:ListItem Text="PROCEDIMIENTOS" Value="15"></asp:ListItem>
        <asp:ListItem Text="TRANSITORIO" Value="16"></asp:ListItem>
        </asp:DropDownList>
    </div>
<br />
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
        Width="50%"
        HeaderFontSize="8pt"
        EventFontSize="8pt"
        CellWidth="220"

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
</asp:Content>

