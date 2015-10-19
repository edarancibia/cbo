
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

    protected void Page_Load(object sender, EventArgs e)
    {


        // prevent invalid ViewState errors in Firefox
        
        if (Request.Browser.Browser == "Firefox") Response.Cache.SetNoStore();

        DayPilotScheduler1.Separators.Clear();
        DayPilotScheduler1.Separators.Add(DateTime.Today, Color.Red);

        if (!IsPostBack)
        {
            cargarCamasYeventosSFiltro();
        }
    }

    private void cargarCamasYeventos()
    {
        cargaCamas();
        DayPilotScheduler1.DataSource = cargarEventos(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private void cargarCamasYeventosSFiltro()
    {
        cargaCamasSfiltro();
        DayPilotScheduler1.DataSource = cargarEventos(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private DataTable cargarEventos(DateTime start, int days)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.ID_TRASLADO AS ReservationId,B.RUT_NUM,B.NOMBRE+' '+B.A_PAT+' '+ B.A_MAT AS ReservationName,C.ID_CAMA AS RoomId,A.ESTADO AS ReservationStatus, A.FECHAINI as ReservationStart,A.FECHAFIN AS ReservationEnd, A.NUEVO_SECTOR AS N_SECTOR FROM HOS_TRASLADOS A, PACIENTE B,CAMA C,HOS_ESTADOCAMAS D WHERE A.RUT_NUM = B.RUT_NUM AND  C.ID_CAMA=A.ID_CAMA AND A.ESTADO=1", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days));
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    private void cargaCamasSfiltro()
    {
        DayPilotScheduler1.Resources.Clear();

        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) ORDER BY CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
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
                    est = "ALTA MEDICA";
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

        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND A.COD_SEC=@beds ORDER BY CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
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
                    est = "ALTA MEDICA";
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


    protected void DayPilotScheduler1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
    {
        switch (e.Command)
        {
            case "refresh":
                cargarCamasYeventosSFiltro();
                break;
            case "filter":
                cargarCamasYeventos();
                break;
        }
    }

    
    protected void DayPilotScheduler1_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs e)
    {
        e.InnerHTML = String.Format("{0} ({1:d} - {2:d})", e.Text, e.Start, e.End);
        int status = Convert.ToInt32(e.Tag["ReservationStatus"]);
        //string interv = Convert.ToString(e.DataItem["INTERVENSION"]);
        int nsector = Convert.ToInt32(e.DataItem["N_SECTOR"]);

        switch (status)
        {
 
            case 1:
                if (nsector == 13)
                {
                    e.DurationBarColor = "MAGENTA"; // mantencion
                    e.ToolTip = "PABELLON";
                }
                else
                {
                    e.DurationBarColor = "MAGENTA"; // mantencion
                    e.ToolTip = "TRASLADO";
                }
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
            case "":
                e.CssClass = "status_disponible";
                break;
        }
    }
}
