using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Data;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Events.Scheduler;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class Ambulatorio : System.Web.UI.Page
{
    public string est;
    int perfil;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["perfil"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        perfil = Convert.ToInt32(Session["perfil"]);

        if (!((perfil == 1) || (perfil == 2) ||(perfil == 3)))
        {
            Response.Redirect("~/Inicio.aspx");
        }

        if (!IsPostBack)
        {
            cargarCamasYeventos();
        }
    }

    private void cargarCamasYeventos()
    {
        cargaCamas();
        DayPilotScheduler1.DataSource = cargarEventos(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    private DataTable cargarEventos(DateTime start, int days)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT A.ID_AMBULA AS ReservationId,B.RUT_NUM,B.NOMBRE+' '+B.A_PAT AS ReservationName,C.ID_CAMA AS RoomId, A.ID_ESTADOCAMA AS ReservationStatus,A.FECHA_INI as ReservationStart,A.FECHA_FIN AS ReservationEnd FROM HOS_AMBULATORIO A, PACIENTE B,CAMA C,HOS_ESTADOCAMAS D WHERE A.RUT_NUM = B.RUT_NUM AND A.ID_CAMA = C.ID_CAMA AND A.ID_ESTADOCAMA = D.ID_ESTADOCAMA AND NOT A.ID_ESTADOCAMA=8 AND NOT (([FECHA_FIN] <= @start)OR ([FECHA] >= @end))AND A.EVENTOPAC=1 AND A.ID_ESTADOCAMA NOT BETWEEN 9 AND 11", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days));
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
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

        SqlDataAdapter da = new SqlDataAdapter(@"SELECT DISTINCT a.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA, H.ID_ESTADOCAMA AS ESTADO 
                                                FROM SECTOR A, PIEZA B, CAMA C 
                                                LEFT JOIN HOS_AMBULATORIO H ON C.ID_CAMA=H.ID_CAMA AND H.ESTADO=1 
                                                WHERE (A.COD_SEC=B.COD_SEC) AND (B.COD_PIE=C.COD_PIE) AND A.COD_SEC=@sec
                                                ORDER BY C.CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("sec", roomFilter);
        DataTable dt = new DataTable();
        da.Fill(dt);


        foreach (DataRow r in dt.Rows)
        {
            string cama = (string)r["CAMA"];
            string id = Convert.ToString(r["ID_CAMA"]);
            string estado = Convert.ToString(r["ESTADO"]);

            switch (estado)
            {
                case "13":
                    est = "ALTA CLINICA";
                    break;
                case "23":
                    est = "OCUPADA";
                    break;
                case "24":
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

    protected void DayPilotScheduler1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
    {
        switch (e.Command)
        {
            case "refresh":
                cargarCamasYeventos();
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

        //string ficha = Convert.ToString(e.DataItem["NRO_FI"]);

        switch (status)
        {
            case 13:
                e.DurationBarColor = "Brown"; // alta medica
                e.BackgroundColor = "Brown";
                e.FontColor = "#fff";
                e.ToolTip = "";
                break;
            case 23:
                e.DurationBarColor = "green";  // verde,ocupada
                e.ToolTip = "PROCEDIMIENTO";
                e.BackgroundColor = "green";
                e.FontColor = "#fff";
                break;
            case 24:
                e.DurationBarColor = "green";
                e.ToolTip = "TRANSITORIO";
                e.BackgroundColor = "green";
                e.FontColor = "#fff";
                break;
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