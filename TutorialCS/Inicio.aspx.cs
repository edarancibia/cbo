using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DayPilot;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Data;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Events.Scheduler;

public partial class Inicio : System.Web.UI.Page
{
    Camas funcamas = new Camas();
    Pinicio ini = new Pinicio();
    public string est, ocupacion, sensoD, pacientes, pacientesUpc, iocup;
    public int fichaHistorial;

    protected void Page_Load(object sender, EventArgs e)
    {
        ocupacionActual();
        sensoDiario();
        pacHosp();
        pacUpcActuales();
        camasDisponible();
        //txtbuscardor.Focus();
        this.txtficha.Attributes.Add("OnKeyPress", "return AcceptNum(event)");
        indiceOcup();

        penh();
    }

    private void ocupacionActual()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("select 100 *(select COUNT(*) FROM HOS_GESTIONCAMA where (ID_ESTADOCAMA=5 AND ESTADO=1) or (ID_ESTADOCAMA=22 AND ESTADO=1)or (ID_ESTADOCAMA BETWEEN 12 and 13 AND ESTADO=1)OR(ID_ESTADOCAMA=18 AND EVENTOPAC=1))/ (Select Count(*) FROM SECTOR A, PIEZA B, CAMA C WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND NOT C.ID_CAMA BETWEEN 144 AND 145) AS OCUPA", con);
            con.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    ocupacion = Convert.ToString(rd["OCUPA"]);
                }
                lblocupa.Text = "Ocupación actual Total: " + ocupacion + " %";
            }
            cmd.Dispose();
            con.Close();
            rd.Close();
        }
    }

    private void indiceOcup()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(@"select 100 *(SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B ,PIEZA C,CAMA D
                                              WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND NOT B.COD_SEC=4)
                                              OR (A.ID_ESTADOCAMA=22 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND NOT B.COD_SEC=4)
                                              OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND NOT B.COD_SEC=4)
                                              OR (A.ID_ESTADOCAMA=18 AND A.EVENTOPAC=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND NOT B.COD_SEC=4))/ 
                                              (Select Count(*) FROM SECTOR A, PIEZA B, CAMA C WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE)) AS OCUPA", con);
            con.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    iocup = Convert.ToString(rd["OCUPA"]);
                }
            }
            lblindice.Text = "Indice Ocupacional M-Q: " + iocup + " %";
            cmd.Dispose();
            con.Close();
            rd.Close();
        }
    }

    private void pacHosp()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("select COUNT(*)AS SENSO FROM HOS_GESTIONCAMA where (ID_ESTADOCAMA=5 AND ESTADO=1) or (ID_ESTADOCAMA=22 AND ESTADO=1)or (ID_ESTADOCAMA BETWEEN 12 and 13 AND ESTADO=1)OR(ID_ESTADOCAMA=18 AND EVENTOPAC=1)", con);
            con.Open();
            SqlDataReader rd2 = cmd.ExecuteReader();

            if(rd2.HasRows)
            {
                while (rd2.Read())
                {
                    pacientes = Convert.ToString(rd2["SENSO"]);
                }
                lblcantPac.Text = "Total pacientes hospitalizados: " + pacientes;
            }
            cmd.Dispose();
            rd2.Close();
            con.Close();
        }
    }

    private void sensoDiario()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("select top 1 * from HOS_OCUPACION order by fecha desc ", con);
            con.Open();
            SqlDataReader rd2 = cmd.ExecuteReader();

            if (rd2.HasRows)
            {
                while (rd2.Read())
                {
                    sensoD = Convert.ToString(rd2["SENSO_DIARIO"]);
                }
                lblsenso.Text = "Censo diario: " + sensoD;
            }
            cmd.Dispose();
            rd2.Close();
            con.Close();
        }
    }

    private void pacUpcActuales()
    {
        funcamas.PacUpc();
        lblupc.Text = "Pacientes UPC: " + funcamas.pacupc.ToString();

        ini.ocupH();
        lblsech.Text = "Camas ocupadas: " + ini.ocuh.ToString();

        ini.ocupF();
        lblsecf.Text = "Camas ocupadas: " + ini.ocuF.ToString();

        ini.ocupM();
        lblsecm.Text = "Camas ocupadas: " + ini.ocuM.ToString();

        ini.ocupE();
        lblsece.Text = "Camas ocupadas: " + ini.ocuE.ToString(); ;
    }

    private void camasDisponible()
    {
        ini.dispH();
        lbldish.Text = "Camas disponibles: " + ini.disph.ToString();

        ini.dispF();
        lbldisf.Text = "Camas disponibles: " + ini.disF.ToString();

        ini.dispE();
        lbldise.Text = "Camas disponibles: " + ini.disE.ToString();

        ini.dispM();
        lbldism.Text = "Camas disponibles: " + ini.disM.ToString();
    }


    protected void btnbuscar_Click(object sender, EventArgs e)
    {

        funcamas.buscaPac(txtbuscardor.Text);
        Session["buscapac"] = funcamas.pacBuscado;
        //Label1.Text = Convert.ToString(Session["buscapac"]);
        Session["buscacama"] = funcamas.camaBuscada;
        Response.Redirect("~/Resultado.aspx");
    }
    protected void btnHistorial_Click(object sender, EventArgs e)
    {
        fichaHistorial = Convert.ToInt32(txtficha.Text);
        Session["fichah"] = fichaHistorial;
        Response.Redirect("~/Historial.aspx");
    }

    //TRASLADOS PENDIENTES

    private void penh()
    {
        ini.pendientesH();
        lblpenh.Text = "Sector H: " + ini.penH.ToString();

        ini.pendientesF();
        lblpenf.Text = "Sector F: " + ini.penF.ToString();

        ini.pendientesM();
        lblpenm.Text = "Sector M: " + ini.penM.ToString();

        ini.pendientesE();
        lblpene.Text = "Sector E: " + ini.penE.ToString();

        ini.pendientesUPC();
        lblpenu.Text = "UPC: " + ini.penU.ToString();
    }
}