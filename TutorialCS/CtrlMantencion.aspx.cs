
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

        if (!((perfil == 1) || (perfil == 5)))
        {
            Response.Redirect("Inicio.aspx");
        }

        // prevent invalid ViewState errors in Firefox
        
        if (Request.Browser.Browser == "Firefox") Response.Cache.SetNoStore();

        DayPilotScheduler1.Separators.Clear();
        DayPilotScheduler1.Separators.Add(DateTime.Today, Color.Red);

        if (!IsPostBack)
        {
            DayPilotScheduler1.StartDate = DateTime.Now;
            cargarCamasYmantencion();
            DateTime firstOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DayPilotScheduler1.SetScrollX(firstOfMonth);
        }
    }

    private void cargarCamasYmantencion()
    {
        cargaCamas();
        DayPilotScheduler1.DataSource = cargarMantencion(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private void cargarCamasYmantencionFiltrado()
    {
        cargarCamasFiltrado();
        DayPilotScheduler1.DataSource = cargarMantencion(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private DataTable cargarMantencion(DateTime start, int days)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT A.ID_EVENTO AS ReservationId,A.ID_CAMA AS RoomId,A.HORA + ' '+ C.OBS as ReservationName,A.ID_ESTADOCAMA as ReservationStatus,FECHAINI AS ReservationStart,A.FECHA_FIN as ReservationEnd FROM HOS_GESTIONCAMA A, HOS_ESTADOCAMAS B ,HOS_DET_MANT C where ((A.ID_ESTADOCAMA=4 AND a.ESTADO=1 AND A.ID_ESTADOCAMA = B.ID_ESTADOCAMA AND A.ID_EVENTO=C.ID_EVENTO)OR(A.ID_ESTADOCAMA=15 AND A.ESTADO=1 AND A.ID_ESTADOCAMA = B.ID_ESTADOCAMA AND A.ID_EVENTO=C.ID_EVENTO)) OR(a.ID_ESTADOCAMA=4 AND a.ESTADO=0 AND a.EVENTOPAC=1 AND A.ID_ESTADOCAMA = B.ID_ESTADOCAMA AND A.ID_EVENTO=C.ID_EVENTO)", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days));
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    private void cargarCamasFiltrado()
    {
        DayPilotScheduler1.Resources.Clear();
        string roomFilter = "0";
        if (DayPilotScheduler1.ClientState["filter"] != null)
        {
            roomFilter = (string)DayPilotScheduler1.ClientState["filter"]["RoomId"];
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
                    est = "ALTA CLINICA";
                    break;
                case "15":
                    est = "SOLICITA MANTENCIÓN";
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
        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) ORDER BY CAMA", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
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
                    est = "SOLICITA MANTENCIÓN";
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
                cargarCamasYmantencionFiltrado();
                break;
            case "filter":
                cargarCamasYmantencionFiltrado();
                break;
        }
    }


    protected void DayPilotScheduler1_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs e)
    {
        e.InnerHTML = String.Format("{0} ({1:d} - {2:d})", e.Text, e.Start, e.End);
        int status = Convert.ToInt32(e.Tag["ReservationStatus"]);
        //string obs = e.Text["ReservationName"].ToString();

        switch (status)
        {
            case 6: // nueva
                    e.DurationBarColor = "cyan";
                    //e.ToolTip = "Nueva";

                break;
            case 7:  // confirmada
                    e.DurationBarColor = "blue";
                    //e.ToolTip = "Confirmada";

                break;
            case 5: // checkin
                    e.DurationBarColor = "green";  // azul
                    //e.ToolTip = "Ingresado";
                break;
            case 2: // checked out
                e.DurationBarColor = "gray";
                e.ToolTip = "Egresado";
                break;
            case 8:
                e.DurationBarColor = "black";
                e.ToolTip = "Cancelada";
                break;

            case 4://mantencion
                e.DurationBarColor = "orange";
                e.ToolTip = "Mantención";
                e.BackgroundColor="orange";
                e.FontColor = "#fff";
                break;

            case 12:
                e.DurationBarColor = "Brown";//alta probable
               
                break;

            case 13:
                e.DurationBarColor = "RosyBrown";//alta medica
                
                break;
            case 15:
                e.DurationBarColor = "orange"; 
                e.ToolTip = "Mantención";
                e.BackgroundColor = "orange";
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
