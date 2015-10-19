using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// Descripción breve de Ambulatorio
/// </summary>
public class Ambulatorio
{
    public string camaDesc;

    public DataRow cargaEvento(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT A.*,RTRIM(LTRIM(B.A_PAT)) + ' '+ RTRIM(LTRIM(B.A_MAT)) AS APELLIDO,RTRIM(LTRIM(B.NOMBRE))as NOMBRE FROM HOS_AMBULATORIO A,PACIENTE B WHERE ID_AMBULA = @id AND A.RUT_NUM=B.RUT_NUM", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }


    //TIPOS DE ALTAS
    public DataTable llenaAltas()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_ESTADOCAMAS WHERE (PERFIL=2) AND NOT ID_ESTADOCAMA=5 AND NOT ID_ESTADOCAMA=18 AND NOT DESCRIPCION='PABELLON'  AND not ID_ESTADOCAMA=12", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataTable llenaCamasAmbula()
    {
        SqlDataAdapter da = new SqlDataAdapter(@"SELECT  B.DESCRIP +' '+LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA 
                                                FROM SECTOR A, PIEZA B, CAMA C 
                                                WHERE (A.COD_SEC=B.COD_SEC ) AND (B.COD_PIE=C.COD_PIE) AND(a.COD_SEC BETWEEN 15 AND 16) 
                                                ORDER BY CAMA DESC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public void guardaPreIngreso(int ficha, int cama, string rut, int estadocama, DateTime fecha, string hora, int estado, int eventopac, DateTime fechaini, DateTime fechafin, int tipoin, int tipopac, string usuario,int tipo_ambula,int cama_hos)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"INSERT INTO HOS_AMBULATORIO(NRO_FI,ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,ESTADO,EVENTOPAC,FECHA_INI,
                                            FECHA_FIN,TIPO_INGRESO,TIPO_PAC,USUARIO,TIPO_AMBULA,CAMA_HOS)VALUES
                                            (@ficha,@cama,@rut,@estadocama,@fecha,@hora,@estado,@eventopac,@fechaini,@fechafin,@tipoin,
                                            @tipopac,@usuario,@tipo_ambula,@cama_hos)", con);
            cmd.Parameters.AddWithValue("ficha", ficha);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("estadocama", estadocama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("tipoin", tipoin);
            cmd.Parameters.AddWithValue("tipopac", tipopac); 
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("tipo_ambula",tipo_ambula);
            cmd.Parameters.AddWithValue("cama_hos",cama_hos);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public bool existeAmbulatorio(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM HOS_AMBULATORIO WHERE NRO_FI=@ficha AND ESTADO=1)", con);
            cmd.Parameters.AddWithValue("ficha", ficha);
            con.Open();

            int cant = (int)cmd.ExecuteScalar();

            if (cant > 0)
                return true;
            else
                return false;

            cmd.Dispose();
            con.Close();
        }
    }

    public void cierraAmbula(int idambula)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("update hos_ambulatorio set estado=0,eventopac=0 where id_ambula=@idambula",con);
            cmd.Parameters.AddWithValue("idambula",idambula);
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //verifica que el paciente tenga el alta medica antes de poder darle alta administrativa
    public bool verificaAltaMedAmbu(int cama, string rut)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_AMBULATORIO WHERE ID_CAMA=@cama and RUT_NUM=@rut AND ID_ESTADOCAMA=13", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }

            cmd.Dispose();
            con.Close();
        }
    }

    public string descCama(int idcama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SUBSTRING(E.DESCRIP,1,4) +' '+ LTRIM(Str(F.NRO_CA, 25))CAMA FROM PIEZA E, CAMA F WHERE F.COD_PIE=E.COD_PIE AND F.ID_CAMA=@idcama",con);
            cmd.Parameters.AddWithValue("idcama",idcama);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                camaDesc = Convert.ToString(dr["CAMA"]);
            }

            con.Close();

            return camaDesc;
        }
    }
}