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
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Descripción breve de Pinicio
/// </summary>
public class Pinicio
{
    public int ocuh, disph, ocuF, disF, ocuE, disE, ocuM, disM,penH,penF,penM,penE,penU;
    public string nomPac;

    //cuenta las camas ocupadas de cada sector
    public void ocupH()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT(SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C 
                                             LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA 
                                            WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND 
                                            (h.ID_ESTADOCAMA=5 AND h.ESTADO=1) AND (a.COD_SEC=1)OR  (A.ESTADO=1) AND 
                                            (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=22 AND h.ESTADO=1) 
                                            AND (a.COD_SEC=1) OR(A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND 
                                            (h.ID_ESTADOCAMA BETWEEN 12 AND 13 AND h.ESTADO=1) AND (a.COD_SEC=1)OR (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) 
                                            AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=18 AND h.EVENTOPAC=1) AND (a.COD_SEC=1))AS OCUPH", con);

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
            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=5 AND h.ESTADO=1) AND (a.COD_SEC=2)OR(A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=22 AND h.ESTADO=1) AND (a.COD_SEC=2) OR(A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA BETWEEN 12 AND 13 AND h.ESTADO=1) AND (a.COD_SEC=2)OR (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=18 AND h.EVENTOPAC=1) AND (a.COD_SEC=2))AS OCUPF", con);

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
            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=5 AND h.ESTADO=1) AND (a.COD_SEC=3)OR(A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=22 AND h.ESTADO=1) AND (a.COD_SEC=3) OR(A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA BETWEEN 12 AND 13 AND h.ESTADO=1) AND (a.COD_SEC=3)OR (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND (h.ID_ESTADOCAMA=18 AND h.EVENTOPAC=1) AND (a.COD_SEC=3))AS OCUPM", con);

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
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)OR (A.ID_ESTADOCAMA=22 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11) OR(A.ID_ESTADOCAMA=22 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)OR (A.ID_ESTADOCAMA=18 AND A.EVENTOPAC=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)", con);

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
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C 
                                            LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 AND NOT (h.ID_ESTADOCAMA BETWEEN 9 AND 11)
                                            WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND 
                                            (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=1)", con);

            disph = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }
    
    public void dispF()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C 
                                            LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 AND NOT (h.ID_ESTADOCAMA BETWEEN 9 AND 11)
                                            WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) 
                                            AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=2)", con);

            disF = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void dispM()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C 
                                            LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 AND NOT (h.ID_ESTADOCAMA BETWEEN 9 AND 11)
                                            WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) 
                                            AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=3)", con);

            disM = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void dispE()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM SECTOR A, PIEZA B, CAMA C 
                                            LEFT JOIN HOS_GESTIONCAMA H ON C.ID_CAMA=H.ID_CAMA AND h.ESTADO=1 AND NOT (h.ID_ESTADOCAMA BETWEEN 9 AND 11)
                                            WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) 
                                            AND (h.ID_ESTADOCAMA is NULL AND a.COD_SEC=11)", con);

            disE = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }
    //***********************************************************************************

    public string buscaNomPac(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT LTRIM(RTRIM(b.NOMBRE )) +' '+ LTRIM(RTRIM(b.A_PAT))+ ' '+ LTRIM(RTRIM(b.A_MAT)) as PACIENTE FROM FIC_PAC a,PACIENTE b WHERE NRO_FI=@ficha AND a.RUT_NUM=b.RUT_NUM", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("ficha",ficha);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                nomPac = Convert.ToString(dr["PACIENTE"]);
            }

            return nomPac;
            cmd.Dispose();
            con.Close();
        }
    }

    public int pendientesH()
    {
      using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
      {
         con.Open();

         SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM HOS_TRASLADOS A, SECTOR B WHERE A.NUEVO_SECTOR=B.COD_SEC AND A.ESTADO=1 AND a.NUEVO_SECTOR=1)CANT", con);
         cmd.CommandType = CommandType.Text;    
         SqlDataReader dr = cmd.ExecuteReader();

          if (dr.Read())
          {
            penH = (int)dr["CANT"];
          }
          else
          {
            penH = 0;
          }

           con.Close();
           return penH;
         }
    }

    public int pendientesF()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM HOS_TRASLADOS A, SECTOR B WHERE A.NUEVO_SECTOR=B.COD_SEC AND A.ESTADO=1 AND a.NUEVO_SECTOR=2)CANT", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                penF = (int)dr["CANT"];
            }
            else
            {
                penF = 0;
            }

            con.Close();
            return penF;
        }
    }

    public int pendientesM()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM HOS_TRASLADOS A, SECTOR B WHERE A.NUEVO_SECTOR=B.COD_SEC AND A.ESTADO=1 AND a.NUEVO_SECTOR=3)CANT", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                penM = (int)dr["CANT"];
            }
            else
            {
                penM = 0;
            }

            con.Close();
            return penM;
        }
    }

    public int pendientesE()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM HOS_TRASLADOS A, SECTOR B WHERE A.NUEVO_SECTOR=B.COD_SEC AND A.ESTADO=1 AND a.NUEVO_SECTOR=11)CANT", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                penE = (int)dr["CANT"];
            }
            else
            {
                penE = 0;
            }

            con.Close();
            return penE;
        }
    }

    public int pendientesUPC()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT(SELECT COUNT(*) FROM HOS_TRASLADOS A, SECTOR B WHERE A.NUEVO_SECTOR=B.COD_SEC AND A.ESTADO=1 AND a.NUEVO_SECTOR=4)CANT", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                penU = (int)dr["CANT"];
            }
            else
            {
                penU = 0;
            }

            con.Close();
            return penU;
        }
    }
}