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
    public string estado;
    public string est;
    public int perfil;

    protected void Page_Load(object sender, EventArgs e)
    {
        perfil = Convert.ToInt32(Session["perfil"]);

        if (!((perfil == 1) || (perfil == 4)||(perfil == 2)))
        {
            Response.Redirect("Inicio.aspx");
        }

        // prevent invalid ViewState errors in Firefox
        
        if (Request.Browser.Browser == "Firefox") Response.Cache.SetNoStore();

        DayPilotScheduler1.Separators.Clear();
        DayPilotScheduler1.Separators.Add(DateTime.Today, Color.Red);

        if (!IsPostBack)
        {

            cargarCamasYaseosSFiltro();
        }
    }

    private void cargarCamasYaseos()
    {
        cargaCamas();
        DayPilotScheduler1.DataSource = cargarAseo(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private void cargarCamasYaseosSFiltro()
    {
        cargaCamasSfiltro();
        DayPilotScheduler1.DataSource = cargarAseo(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private DataTable cargarAseo(DateTime start, int days)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT A.ID_EVENTO AS ReservationId,A.ID_CAMA AS RoomId,A.HORA as ReservationName,A.ID_ESTADOCAMA as ReservationStatus,FECHAINI AS ReservationStart,A.FECHA_FIN as ReservationEnd FROM HOS_GESTIONCAMA A, HOS_ESTADOCAMAS B where A.ID_ESTADOCAMA between 9 and 11 AND A.ID_ESTADOCAMA = B.ID_ESTADOCAMA AND A.ESTADO=1", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days));
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }


    private void cargaCamasSfiltro()
    {
        DayPilotScheduler1.Resources.Clear();

        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) and NOT C.ID_CAMA=144 ORDER BY CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
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
                    est = "ASEO CORTO";
                    break;
                case "10":
                    est = "ASEO TERMINAL";
                    break;
                case "11":
                    est = "ASEO TRASLADO";
                    break;
                case "12":
                    est = "ALTA PROBABLE";
                    break;
                case "13":
                    est = "ALTA CLINICA";
                    break;
                case "15":
                    est = "SOLICITA MANTENCIÓN";
                    break;
                case "22":
                    est = "EN PABELLON";
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


    private void cargaCamas()
    {
        DayPilotScheduler1.Resources.Clear();
        //beds = Convert.ToInt32(cbosectorhosp.SelectedValue);
        string roomFilter = "0";
        
        if (DayPilotScheduler1.ClientState["filter"] != null)
        {
            roomFilter = (string)DayPilotScheduler1.ClientState["filter"]["room"];
        }

        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND A.COD_SEC=@beds and NOT C.ID_CAMA=144 ORDER BY CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("beds",roomFilter);
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
                    est = "ASEO CORTO";
                    break;
                case "10":
                    est = "ASEO TERMINAL";
                    break;
                case "11":
                    est = "ASEO TRASLADO";
                    break;
                case "12":
                    est = "ALTA PROBABLE";
                    break;
                case "13":
                    est = "ALTA CLINICA";
                    break;
                case "15":
                    est = "SOLICITA MANTENCIÓN";
                    break;
                case "22":
                    est = "EN PABELLON";
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
        
        SqlDataAdapter da = new SqlDataAdapter("SELECT count(*) as count FROM HOS_GESTIONCAMA WHERE NOT (([FECHA_FIN] <= @start) OR ([FECHA] >= @end)) AND ID_CAMA = @cama AND ID_EVENTO <> @id", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", end);
        da.SelectCommand.Parameters.AddWithValue("cama", cama);
        DataTable dt = new DataTable();
        da.Fill(dt);

        int count = Convert.ToInt32(dt.Rows[0]["count"]);
        return count == 0;
    }


    

    protected void DayPilotScheduler1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
    {
        switch (e.Command)
        {
            case "refresh":
                cargarCamasYaseos();
                break;
            case "filter":
                cargarCamasYaseos();
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

        cargarCamasYaseos();
        DayPilotScheduler1.UpdateWithMessage(message);
    }

    protected void DayPilotScheduler1_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs e)
    {
        e.InnerHTML = String.Format("{0} ({1:d} - {2:d})", e.Text, e.Start, e.End);
        int status = Convert.ToInt32(e.Tag["ReservationStatus"]);
        string interv = Convert.ToString(e.DataItem["ReservationName"]);

        switch (status)
        {
            case 9:  // aseo corto
                    e.DurationBarColor = "yellow";
                    //e.ToolTip = "Confirmada";
                    e.ToolTip = interv;
                    e.BackgroundColor = "yellow";
                break;
            case 10: // aseo terminal
                    e.DurationBarColor = "yellow";  
                    //e.ToolTip = "Ingresado";
                    e.ToolTip = interv;
                    e.BackgroundColor = "yellow";
                break;
            case 11:// aseo traslado
                e.DurationBarColor = "yellow";
                e.ToolTip = interv;
                e.BackgroundColor = "yellow";
                break;
                
            default:
                throw new ArgumentException("Estado inesperado.");
        }

        e.InnerHTML = e.InnerHTML + String.Format("<br /><span style='color:gray'>{0}</span>", e.ToolTip);

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
