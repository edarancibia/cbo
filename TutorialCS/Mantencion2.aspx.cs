
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Data;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Events.Scheduler;
using System.Windows.Forms;

public partial class _Default : System.Web.UI.Page 
{
    public string estado, ocupacion;
    public string est;
    public int beds;

    protected void Page_Load(object sender, EventArgs e)
    {


        // prevent invalid ViewState errors in Firefox
        
        if (Request.Browser.Browser == "Firefox") Response.Cache.SetNoStore();

        DayPilotScheduler1.Separators.Clear();
        DayPilotScheduler1.Separators.Add(DateTime.Today, Color.Red);

        if (!IsPostBack)
        {

            DayPilotScheduler1.StartDate = DateTime.Now;
            LoadResourcesAndEventsSF();

            // scroll to this month
            DateTime firstOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DayPilotScheduler1.SetScrollX(firstOfMonth);

            ocupacionActual();
            lblocupa.Text = "Ocupaci�n actual "+ ocupacion +"%";
            lblocupa.Visible = false;
            //lblocupa.Text = hoy.ToShortDateString();
        }
    }

    private void LoadResourcesAndEvents()
    {
        LoadResources();
        DayPilotScheduler1.DataSource = dbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private void LoadResourcesAndEventsSF()
    {
        cargaCamasSfiltro();
        DayPilotScheduler1.DataSource = dbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    protected void DayPilotScheduler1_EventMove(object sender, DayPilot.Web.Ui.Events.EventMoveEventArgs e)
    {
        string id = e.Value;
        DateTime start = e.NewStart;
        DateTime end = e.NewEnd;
        int cama = Convert.ToInt32(e.NewResource);

        string message = null;

        if (!dbIsFree(id, start, end, cama))
        {
            message = "La reserva no puede sobreponer una reservar existente.";
        }
        else if (e.OldEnd <= DateTime.Today)
        {
            message = "Esta reserva no se puede volver a cambiar.";
        }
        else if (e.OldStart < DateTime.Today)
        {
            if (e.OldResource != e.NewResource)
            {
                message = "La cama no se puede volver a cambiar.";
            }
            else
            {
                message = "La fecha de inicio de la reserva no se puede cambiar.";
            }
        }
        else if (e.NewStart < DateTime.Today)
        {
            message = "La reserva no se puede mover al pasado.";
        }
        else
        {
            dbUpdateEvent(id, start, end, cama);
            //message = "Reservation moved.";
        }

        LoadResourcesAndEvents();
        DayPilotScheduler1.UpdateWithMessage(message);
    }
    
    private DataTable dbGetEvents(DateTime start, int days)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.ID_EVENTO AS ReservationId,B.RUT_NUM,B.NOMBRES+' '+B.APELLIDOS AS ReservationName,C.ID_CAMA AS RoomId, A.ID_ESTADOCAMA AS ReservationStatus,A.FECHAINI as ReservationStart,A.FECHA_FIN AS ReservationEnd, B.INTERVENSION AS INTERVENSION FROM HOS_GESTIONCAMA A, HOS_PACIENTE B,CAMA C,HOS_ESTADOCAMAS D WHERE A.RUT_NUM = B.RUT_NUM AND A.ID_CAMA = C.ID_CAMA AND A.ID_ESTADOCAMA = D.ID_ESTADOCAMA AND NOT A.ID_ESTADOCAMA=8 AND NOT (([FECHA_FIN] <= @start)OR ([FECHA] >= @end)) AND A.EVENTOPAC=1 AND A.ID_ESTADOCAMA NOT BETWEEN 9 AND 11", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days));
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    private void dbUpdateEvent(string id, DateTime start, DateTime end, int cama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET FECHA = @start, FECHA_FIN = @end, ID_CAMA = @cama WHERE ID_EVENTO = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.ExecuteNonQuery();
        }
    }

    private bool dbIsFree(string id, DateTime start, DateTime end, int cama)
    {
        // event with the specified id will be ignored

        SqlDataAdapter da = new SqlDataAdapter("SELECT count(*) as count FROM HOS_GESTIONCAMA WHERE NOT (([FECHA_FIN] <= @start) OR ([FECHAINI] >= @end)) AND ID_CAMA = @cama AND ID_EVENTO <> @id AND EVENTOPAC=1", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", end);
        da.SelectCommand.Parameters.AddWithValue("cama", cama);
        DataTable dt = new DataTable();
        da.Fill(dt);

        int count = Convert.ToInt32(dt.Rows[0]["count"]);
        return count == 0;
    }

    private void ocupacionActual()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("select 100 *(select COUNT(*) FROM HOS_GESTIONCAMA where (ID_ESTADOCAMA=5 AND ESTADO=1) or (ID_ESTADOCAMA BETWEEN 12 and 13 AND ESTADO=1))/ (Select Count(*) FROM SECTOR A, PIEZA B, CAMA C WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE)) AS OCUPA", con);
            con.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    ocupacion = Convert.ToString(rd["OCUPA"]);
                }
                lblocupa.Text = "Ocupaci�n actual " + ocupacion + " %";
            }

        }
    }

    private void cargaCamasSfiltro()
    {
        DayPilotScheduler1.Resources.Clear();

        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) and NOT C.ID_CAMA=144 ORDER BY CAMA", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);


        foreach (DataRow r in dt.Rows)
        {
            string cama = (string)r["CAMA"];
            string id = Convert.ToString(r["ID_CAMA"]);
            string estado = Convert.ToString(r["ESTADO"]);

            switch (estado)
            {
                case "4":
                    est = "MANTENCION";
                    break;
                case "5":
                    est = "OCUPADA";
                    break;
                case "6":
                    est = "DISPONIBLE";
                    break;
                case "7":
                    est = "DISPONIBLE";
                    break;
                case "8":
                    est = "DISPONIBLE";
                    break;
                case "9":
                    est = "ASEO";
                    break;
                case "10":
                    est = "ASEO";
                    break;
                case "11":
                    est = "ASEO";
                    break;
                case "12":
                    est = "ALTA PROBABLE";
                    break;
                case "13":
                    est = "ALTA CLINICA";
                    break;
                case "15":
                    est = "SOLICITA MANTENCI�N";
                    break;
                case "22":
                    est = "OCUPADA";
                    break;
                default:
                    est = "DISPONIBLE";
                    break;
            }

            Resource res = new Resource(cama, id);
            res.DataItem = r;
            res.Columns.Add(new ResourceColumn(est));
            DayPilotScheduler1.Resources.Add(res);
        }
    }


    private void LoadResources()
    {
        DayPilotScheduler1.Resources.Clear();
        string roomFilter = "0";

        if (DayPilotScheduler1.ClientState["filter"] != null)
        {
            roomFilter = (string)DayPilotScheduler1.ClientState["filter"]["room"];
        }

        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND A.COD_SEC=@beds and NOT C.ID_CAMA=144 ORDER BY CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("beds", roomFilter);
        DataTable dt = new DataTable();
        da.Fill(dt);

        foreach (DataRow r in dt.Rows)
        {
            string cama = (string)r["CAMA"];
            string id = Convert.ToString(r["ID_CAMA"]);
            string estado = Convert.ToString(r["ESTADO"]);
            //int beds = Convert.ToInt32(r["CODSECTOR"]);

            switch (estado)
            {
                case "1":
                    est = "CERRADA";
                    break;
                case "2":
                    est= "DISPONIBLE";
                    break;
                case "3":
                    est = "ASEO";
                    break;
                case "4":
                    est = "MANTENCION";
                    break;
                case "5":
                    est = "OCUPADA";
                    break;
                case "6":
                    est = "DISPONIBLE";
                    break;
                case "8":
                    est = "DISPONIBLE";
                    break;
                case "9":
                    est = "ASEO";
                    break;
                case "10":
                    est="ASEO";
                    break;
                case "11":
                    est = "ASEO";
                    break;
                case "12":
                    est = "ALTA PROBABLE";
                    break;
                case "13":
                    est = "ALTA CLINICA";
                    break;
                case "15":
                    est = "SOLICITA MANTENCI�N";
                    break;
                case null:
                    est="DISPONIBLE";
                    break;
                default:
                    est = "DISPONIBLE";
                    break;
            }
            

            //int beds = Convert.ToInt32(r["RoomSize"]);
            //string bedsFormatted = (beds == 1) ? "1 bed" : String.Format("{0} beds", beds);

            Resource res = new Resource(cama, id);
            res.DataItem = r;
            //res.Columns.Add(new ResourceColumn(bedsFormatted));
            res.Columns.Add(new ResourceColumn(est));

            DayPilotScheduler1.Resources.Add(res);
        }

    }

    protected void DayPilotScheduler1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
    {
        switch (e.Command)
        {
            case "refresh":
                LoadResourcesAndEventsSF();
                break;
            case "filter":
                LoadResourcesAndEvents();
                break;
        }
    }

    
    protected void DayPilotScheduler1_EventResize(object sender, DayPilot.Web.Ui.Events.EventResizeEventArgs e)
    {
        string id = e.Value;
        DateTime start = e.NewStart;
        DateTime end = e.NewEnd;
        int cama = Convert.ToInt32(e.Resource);

        string message = null;

        if (!dbIsFree(id, start, end, cama))
        {
            message = "No se puede sobreponer a una reserva existente.";
        }
        else if (e.OldEnd <= DateTime.Today)
        {
            message = "La reserva no se puede cambiar mas.";
        }
        else if (e.OldStart != e.NewStart)
        {
            if (e.OldStart < DateTime.Today)
            {
               message = "La fecha de inicio no se puede cambiar.";
            }
            else if (e.NewStart < DateTime.Today)
            {
                message = "La reserva no se puede cambiar al pasado.";
            }
        }
        else
        {
            dbUpdateEvent(id, start, end, cama);
            //message = "Reservation updated.";
        }

        LoadResourcesAndEventsSF();
        DayPilotScheduler1.UpdateWithMessage(message);
    }

    protected void DayPilotScheduler1_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs e)
    {
        e.InnerHTML = String.Format("{0} ({1:d} - {2:d})", e.Text, e.Start, e.End);
        int status = Convert.ToInt32(e.Tag["ReservationStatus"]);
        string interv = Convert.ToString(e.DataItem["INTERVENSION"]);

        switch (status)
        {
            case 6: // nueva
                e.DurationBarColor = "cyan"; //nueva reserva
                //e.ToolTip = "Nueva";
                e.ToolTip = interv;
                e.BackgroundColor = "cyan";
                break;
            case 7:  // confirmada
                 e.DurationBarColor = "blue"; //reserva confirmada
                 //e.ToolTip = "Confirmada";
                 e.ToolTip = interv;
                 e.BackgroundColor = "blue";
                 e.FontColor = "#fff";
                break;
            case 5: // checkin
                 e.DurationBarColor = "green";  // verde,ocupada
                 //e.ToolTip = "Ingresado";
                 e.ToolTip = interv;
                 e.BackgroundColor = "green";
                 e.FontColor = "#fff";
                break;
            case 2: // checked out
                e.DurationBarColor = "gray";
                e.ToolTip = "Egresado";
                break;
            case 8:
                e.DurationBarColor = "black";
                e.ToolTip = "Cancelada";
                break;

            case 12:
                e.DurationBarColor = "RosyBrown"; //alta probable
                e.ToolTip = interv;
                e.BackgroundColor = "RosyBrown";
                e.FontColor = "#fff";
                break;
            case 13:
                e.DurationBarColor = "Brown"; // alta medica
                e.ToolTip = interv;
                e.BackgroundColor = "Brown";
                e.FontColor = "#fff";
                break;

            case 4:
                e.DurationBarColor = "orange"; // alta medica
                e.ToolTip = "MANTENCION";
                e.BackgroundColor = "orange";
                e.FontColor = "#fff";
                break;
            case 15:
                e.DurationBarColor = "orange"; // alta medica
                e.ToolTip = "MANTENCION";
                e.BackgroundColor = "orange";
                e.FontColor = "#fff";
                break;
            case 17:
                e.DurationBarColor = "blue"; // alta medica
                e.ToolTip = "EN PABELL�N";
                e.BackgroundColor = "blue";
                e.FontColor = "#fff";
                break;
            default:
                throw new ArgumentException("Estado inesperado.");
        }

        e.InnerHTML = e.InnerHTML + String.Format("<br /><span style='color:#fff'>{0}</span>", e.ToolTip);

        //int paid = Convert.ToInt32(e.DataItem["ReservationPaid"]);
        //string paidColor = "#aaaaaa";

        //e.Areas.Add(new Area().Bottom(10).Right(4).Html("<div style='color:" + paidColor + "; font-size: 8pt;'>Paid: " + interv + "%</div>").Visibility(AreaVisibility.Visible));
        //e.Areas.Add(new Area().Left(4).Bottom(8).Right(4).Height(2).Html("<div style='background-color:" + paidColor + "; height: 100%; width:" + interv + ></div>").Visibility(AreaVisibility.Visible));
    }

    protected void DayPilotScheduler1_BeforeCellRender(object sender, DayPilot.Web.Ui.Events.BeforeCellRenderEventArgs e)
    {
        if (e.IsBusiness)
        {
            e.BackgroundColor = "#ffffff";
        }
        else
        {
            e.BackgroundColor = "#ebebeb";
        }
    }

    protected void DayPilotScheduler1_BeforeResHeaderRender(object sender, BeforeResHeaderRenderEventArgs e)
    {
        string estado = Convert.ToString(e.DataItem["ESTADO"]);
        switch (estado)
        {
            case "4":
                e.CssClass = "status_mantencion";
                break;
            case "7":
                e.CssClass = "status_disponible";
                break;
            case "8":
                e.CssClass = "status_disponible";
                break;
            case "9":
                e.CssClass = "status_aseo";
                break;
            case "10":
                e.CssClass = "status_aseo";
                break;
            case "11":
                e.CssClass = "status_aseo";
                break;
            case "6":
                e.CssClass = "status_disponible";
                break;
            case "12":
                e.CssClass = "status_altaprobable";
                break;
            case "13":
                e.CssClass = "status_altamedica";
                break;
            case "15":
                e.CssClass = "status_mantencion";
                break;
            case "":
                e.CssClass = "status_disponible";
                break;
        }
    }

}
