using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Data;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Events.Scheduler;
using System.Windows.Forms;

/// <summary>
/// Descripción breve de Inicio
/// </summary>
public class Inicio
{
    public int ocuh, disph,ocuF,dispF,ocuE,dispE,ocuM,dispM;

    //cuenta las camas ocupadas de cada sector
    public void ocupH()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=5) AND (a.COD_SEC=1)OR(A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA BETWEEN 12 AND 13) AND (a.COD_SEC=1))AS OCUPH", con);

            ocuh = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }


    public void ocupF()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=2)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=2)", con);

            ocuF = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void ocupM()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=3)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=3)", con);

            ocuM = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void ocupE()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)", con);

            ocuE = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }
    //***************************************************************************************************************

    //cuenta las camas disponibles de cada sector

    public void dispH()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=1)",con);

            disph = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }
    /*
    public void dispF()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=2)", con);

            disph = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void dispM()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=3)", con);

            disph = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void dispE()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=11)", con);

            disph = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }*/
}