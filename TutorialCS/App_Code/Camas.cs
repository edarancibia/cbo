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
using System.Net.Mail;

/// <summary>
/// Metodos de llenado de combos,grillas,metedos insert,update,select.
/// </summary>
public class Camas
{
    public string est,cama,estado,id,nombre,apellidos,rut, pacBuscado,camaBuscada,detmant,rut_pac,nombre2,apat,amat,rut_ver;
    public DataRow r;
    public DataTable dt2;
    public DateTime fechaini;
    SqlDataReader dr;
    public int pacupc,pacH,pacF,pacM,pacE, SecOri, pieza, idMant,camasOcupadasP,sector,Ncama;

    public string para;
    public string asunto;
    public string cuerpo;
    public MailMessage correo;


    #region combos

    public DataTable llenaCamas()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT  A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA FROM SECTOR A, PIEZA B, CAMA C WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) ORDER BY CAMA ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    //TIPOS DE ALTAS
    public DataTable llenaAltas()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_ESTADOCAMAS WHERE (PERFIL=2) AND NOT ID_ESTADOCAMA=5 AND NOT ID_ESTADOCAMA=18 AND NOT DESCRIPCION='PABELLON'", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataTable llenaTipoAseo()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_TIPOASEO", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataTable llenaPrevis()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT RUT_NUM, NOMBRE FROM PREVIS ORDER BY NOMBRE ASC", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    //TIPO DE INGRESO DE PACIENTE
    public DataTable cboIngre()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_TIPOINGRESO", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    //estado cama para modificar reserva
    public DataTable llenaestadoCamas()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_ESTADOCAMAS WHERE PERFIL=1", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    // llena lista de camas para ingreso por traslado
    public DataTable llenaCamasNsec(int nuevo_sector)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT  A.DESCRIP +' '+ B.DESCRIP +' '+ LTRIM(Str(C.NRO_CA, 25))CAMA, C.ID_CAMA FROM SECTOR A, PIEZA B, CAMA C WHERE (A.ESTADO=1) AND (A.COD_SEC=B.COD_SEC AND B.ESTADO=1) AND (B.COD_PIE=C.COD_PIE) AND A.COD_SEC=@nuevo_sector ORDER BY CAMA", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("nuevo_sector", nuevo_sector);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    //lista de sectores
    public DataTable llenaSector()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM SECTOR WHERE ESTADO=1", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataTable llenaSectorTras()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM SECTOR WHERE ESTADO=1 OR COD_SEC=14", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataTable llenaNivel()
    {
        SqlDataAdapter da = new SqlDataAdapter("Select * from hos_nivelescuidado",ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    #endregion

    #region cargaeventos

    //cargar el evento seleccionado(reserva,ingreso,aseo,mantencion,alta,etc)
    public DataRow cargaReserva(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT A.*,RTRIM(LTRIM(B.A_PAT)) + ' '+ RTRIM(LTRIM(B.A_MAT)) AS APELLIDO,RTRIM(LTRIM(B.NOMBRE))as NOMBRE FROM HOS_GESTIONCAMA A,PACIENTE B WHERE ID_EVENTO = @id AND A.RUT_NUM=B.RUT_NUM", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }

    public DataRow cargaTraslado(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT A.*,RTRIM(LTRIM(B.A_PAT)) + ' '+ RTRIM(LTRIM(B.A_MAT)) AS APELLIDO,RTRIM(LTRIM(B.NOMBRE))as NOMBRE,A.OBS FROM HOS_TRASLADOS A,PACIENTE B WHERE ID_TRASLADO = @id AND A.RUT_NUM=B.RUT_NUM", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }

    public DataRow cargaAseo(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_GESTIONCAMA  WHERE ID_EVENTO = @id ", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }

    public DataRow cargaDetMant(int id)
    {
        SqlDataAdapter da = new SqlDataAdapter("select * from hos_det_mant where id_evento=@id", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id",id);
        DataTable dt2 = new DataTable();
        da.Fill(dt2);

        if (dt2.Rows.Count > 0)
        {
            return dt2.Rows[0];
        }

        return null;
    }

    public void cargaDetalleMant(int id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from hos_det_mant where id_evento=@id",con);
            cmd.Parameters.AddWithValue("id",id);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                detmant = dr["obs"] as string;
            }
            cmd.Dispose();
            con.Close();
        }
    }

    #endregion

    #region comprobaciones

    //comprueba si la cama esta ocupada por otro paciente o evento
    public bool ocupada(int cama)
    {
        string sql = "SELECT * FROM HOS_GESTIONCAMA WHERE ID_CAMA=@cama AND ESTADO=1";
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("cama", cama);

            con.Open();

            int cont = Convert.ToInt32(cmd.ExecuteScalar());

            if (cont == 0)
                return false;
            else
                return true;
        }
    }

    public bool ocupada2(int cama)
    {
        string sql = "SELECT * FROM HOS_GESTIONCAMA WHERE (ID_CAMA=@cama AND ESTADO=1)or(ID_CAMA=@cama AND ID_ESTADOCAMA BETWEEN 18 AND 21 AND EVENTOPAC=1)";
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("cama", cama);

            con.Open();

            int cont = Convert.ToInt32(cmd.ExecuteScalar());

            if (cont == 0)
                return false;
            else
                return true;
        }
    }
    //comprueba si la cama esta en aseo
    public bool aseo(int cama)
    {
        string sql = "SELECT * FROM HOS_GESTIONCAMA WHERE ID_CAMA=@cama AND ID_ESTADOCAMA BETWEEN 9 AND 11 AND ESTADO=1";
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("cama", cama);

            con.Open();

            int cont = Convert.ToInt32(cmd.ExecuteScalar());

            if (cont == 0)
                return false;
            else
                return true;
        }
    }

    //verifica si la cama esta disponible para las fechas seleccionadas
    public bool esOcupada(int cama, DateTime fechaini, DateTime fechafin)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_GESTIONCAMA WHERE FECHAINI < @fechafin AND @fechaini < FECHA_FIN AND ID_CAMA=@cama AND EVENTOPAC=1", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            con.Open();
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

    public bool existeFicha(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM HOS_GESTIONCAMA 
                                            WHERE (NRO_FI=@ficha AND ESTADO=1) OR 
                                            ( NRO_FI=@ficha AND ID_ESTADOCAMA=18 and EVENTOPAC=1 and ESTADO=0)", con);
            cmd.Parameters.AddWithValue("ficha",ficha);
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

    #endregion


    public void enviaCorreo()
    {
        //correo.IsBodyHtml = true;

        correo = new MailMessage();
        correo.To.Add(new MailAddress(this.para));
        correo.From = new MailAddress("gestioncamascbo@outlook.com");
        correo.Subject = asunto;
        correo.Body = cuerpo;
        correo.IsBodyHtml = false;

        SmtpClient client = new SmtpClient("smtp.live.com", 587);
        using (client)
        {
            client.Credentials = new System.Net.NetworkCredential("gestioncamascbo@outlook.com", "camas1530");
            client.EnableSsl = true;
            client.Send(correo);
        }
    }

    //deja en 0 todos los eventos de la cama seleccionada (int)
    public void updEstEvento2(int cama, int estado)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0 WHERE ID_CAMA=@cama", con);
            cmd.Parameters.AddWithValue("estado", 0);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //deja en 0 todos los eventos de la cama seleccionada (string)
    public void updEstEvento(string cama, int estado)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0 WHERE ID_CAMA=@cama", con);
            cmd.Parameters.AddWithValue("estado", 0);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }


    //DA POR TERMINADO EL ULTIMO EVENTO DEJANDO TODO EN 0
    public void terminaCamaActual(string id, int cama, int estado, int eventopac)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0,EVENTOPAC=0  WHERE ID_EVENTO=@id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            con.Close();
        }
    }

    #region reservas

    //Verifica si existe paciente
    public bool existePac(string rut)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("select count(*) from paciente where rut_num=@rut",con);
            cmd.Parameters.AddWithValue("rut",rut);
            con.Open();
            int cant = (int)cmd.ExecuteScalar();

            if (cant > 0)
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

    public void buscaXrut(string rut)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("select LTRIM(RTRIM(nombre))nombre,LTRIM(RTRIM(a_pat))apat, LTRIM(RTRIM(a_mat))amat,rut_ver from paciente where rut_num=@rut", con);
            cmd.Parameters.AddWithValue("rut",rut);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                nombre2 = Convert.ToString(dr["nombre"]);
                apat = Convert.ToString(dr["apat"]);
                amat = Convert.ToString(dr["amat"]);
                rut_ver = Convert.ToString(dr["rut_ver"]);
            }

            cmd.Dispose();
            con.Close();
        }
    }

    public void guardaPaciente(string rut_num,string rut_ver,string nombre,string a_pat,string a_mat,string telefono,int sexo)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into paciente(rut_num,rut_ver,a_pat,a_mat,nombre,telefono,sexo)values(@rut_num,@rut_ver,@nombre,@a_pat,@a_mat,@telefono,@sexo)",con);
            cmd.Parameters.AddWithValue("rut_num",rut_num);
            cmd.Parameters.AddWithValue("rut_ver",rut_ver);
            cmd.Parameters.AddWithValue("nombre",nombre);
            cmd.Parameters.AddWithValue("a_pat",a_pat);
            cmd.Parameters.AddWithValue("a_mat",a_mat);
            cmd.Parameters.AddWithValue("telefono",telefono);
            cmd.Parameters.AddWithValue("sexo",sexo);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }


    //crea nueva reserva
    public void guardaReserva(int cama, string rut, int estadoCama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,FECHA,HORA,FECHAINI,FECHA_FIN,ID_ESTADOCAMA,ESTADO,USUARIO,EVENTOPAC,NRO_FI) VALUES(@cama,@rut,@fecha,@hora,@fechaini,@fechafin,@estadocama,@estado,@usuario,@eventopac,@ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estadoCama", estadoCama);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("ficha", ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //crea un nuevo evento con la reserva en estado confirmada o cancelada
    public void confirma(int cama, string rut, int estadocama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA(ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,FECHAINI,FECHA_FIN,ESTADO,EVENTOPAC,USUARIO,NRO_FI) VALUES(@cama,@rut,@estadocama,@fecha,@hora,@fechaini,@fechafin,@estado,@eventopac,@usuario,@ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("estadocama", estadocama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("ficha", ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //coloca los estados de la reserva sin confirmar en 0
    public void terminaReserva(int id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0,EVENTOPAC=0 WHERE ID_EVENTO=@id ", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //cierra la reserva cuando ingresa el paciente
    public void cierraReserva(string id, DateTime fechafin)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0,FECHA_FIN=@fechafin,EVENTOPAC=0 WHERE ID_EVENTO=@id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    #endregion

    #region ingreso

    public void ingresoDirecto(string cama, string rut, int estadocama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario, int tipoingreso, int quirurgico,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,FECHAINI,FECHA_FIN,ESTADO,EVENTOPAC,USUARIO,TIPO_INGRESO,QUIRURGICO,NRO_FI) VALUES(@cama,@rut,@estadocama,@fecha,@hora,@fechaini,@fechafin,@estado,@eventopac,@usuario,@tipoingreso,@quirurgico,@ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("estadocama", estadocama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("tipoingreso", tipoingreso);
            cmd.Parameters.AddWithValue("quirurgico", quirurgico);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    #endregion

    #region Preingreso

    public void buscaxficha(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT A.RUT_NUM,RTRIM(LTRIM(A.A_PAT))+' ' +RTRIM(LTRIM(A.A_MAT)) AS APELLIDO,A.NOMBRE,B.NRO_FI from paciente A, ficha B,FIC_PAC C WHERE B.NRO_FI=@ficha AND B.NRO_FI=C.NRO_FI AND C.RUT_NUM=A.RUT_NUM", con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                rut = Convert.ToString(dr["RUT_NUM"]);
                nombre = Convert.ToString(dr["NOMBRE"]);
                apellidos=Convert.ToString(dr["APELLIDO"]);
            }
            cmd.Dispose();
            con.Close();
        }
    }

    public void guardaPreIngreso(int cama,string rut,int estadocama,DateTime fecha,DateTime fechaini,DateTime fechafin, string hora,int estado,int eventopac,string usuario,int tipoin,int tipopac,DateTime fechahora, int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA(ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,FECHAINI,FECHA_FIN,HORA,ESTADO,EVENTOPAC,USUARIO,TIPO_INGRESO,QUIRURGICO,FECHAHORA,NRO_FI)VALUES(@cama,@rut,@estadocama,@fecha,@fechaini,@fechafin,@hora,@estado,@eventopac,@usuario,@tipoin,@tipopac,@fechahora,@ficha)",con);
            cmd.Parameters.AddWithValue("cama",cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("estadocama",estadocama);
            cmd.Parameters.AddWithValue("fecha",fecha);
            cmd.Parameters.AddWithValue("fechaini",fechaini);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.Parameters.AddWithValue("hora",hora);
            cmd.Parameters.AddWithValue("estado",estado);
            cmd.Parameters.AddWithValue("eventopac",eventopac);
            cmd.Parameters.AddWithValue("usuario",usuario);
            cmd.Parameters.AddWithValue("tipoin", tipoin);
            cmd.Parameters.AddWithValue("tipopac", tipopac);
            cmd.Parameters.AddWithValue("fechahora",fechahora);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public void eliminaPreIngreso(int idevento)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET EVENTOPAC=0 WHERE ID_EVENTO=@idevento",con);
            cmd.Parameters.AddWithValue("idevento",idevento);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public void buscaPieza(int cama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT c.COD_PIE FROM HOS_GESTIONCAMA a,SECTOR b,PIEZA c,CAMA d WHERE a.ID_CAMA=d.ID_CAMA AND d.COD_PIE=c.COD_PIE AND c.COD_SEC=b.COD_SEC AND a.ID_CAMA=@cama",con);
            cmd.Parameters.AddWithValue("cama",cama);

            SqlDataReader dr = cmd.ExecuteReader();
            
            if(dr.Read())
            pieza = Convert.ToInt32(dr["COD_PIE"]);

            dr.Close();
            cmd.Clone();
            con.Close();
        }
    }

    public void ocupadasPieza(int pieza)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)FROM HOS_GESTIONCAMA A, PIEZA B,CAMA C WHERE A.ID_CAMA=C.ID_CAMA AND C.COD_PIE=B.COD_PIE AND B.COD_PIE=@pieza AND A.ESTADO=1",con);
            cmd.Parameters.AddWithValue("pieza",pieza);

            camasOcupadasP = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void insertBloqueoPieza(int cama, string rut, int estadocama, DateTime fecha, DateTime fechaini, DateTime fechafin, string hora, int estado, int eventopac, string usuario, int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA(ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,FECHAINI,FECHA_FIN,HORA,ESTADO,EVENTOPAC,USUARIO,NRO_FI)VALUES(@cama,@rut,@estadocama,@fecha,@fechaini,@fechafin,@hora,@estado,@eventopac,@usuario,@ficha)", con);
            cmd.Parameters.AddWithValue("cama",cama);
            cmd.Parameters.AddWithValue("rut",rut);
            cmd.Parameters.AddWithValue("estadocama",estadocama);
            cmd.Parameters.AddWithValue("fecha",fecha);
            cmd.Parameters.AddWithValue("fechaini",fechaini);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.Parameters.AddWithValue("hora",hora);
            cmd.Parameters.AddWithValue("estado",estado);
            cmd.Parameters.AddWithValue("eventopac",eventopac);
            cmd.Parameters.AddWithValue("usuario",usuario);
            cmd.Parameters.AddWithValue("ficha",ficha);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public bool esBloqueoPieza(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from hos_gestioncama where id_estadocama between 19 and 21 and eventopac=1 and nro_fi=@ficha",con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
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

    public bool esAislaMedico(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from hos_gestioncama where id_estadocama=20 and eventopac=1 and nro_fi=@ficha", con);
            cmd.Parameters.AddWithValue("ficha", ficha);

            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
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

    public bool esAislaEdad(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from hos_gestioncama where id_estadocama=21 and eventopac=1 and nro_fi=@ficha", con);
            cmd.Parameters.AddWithValue("ficha", ficha);

            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
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

    public void eliminaBloqueo(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update hos_gestioncama set eventopac=0 where id_estadocama between 19 and 21 and eventopac=1 and nro_fi=@ficha", con);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public bool esxTrasladar(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from hos_traslados where nro_fi=@ficha and estado=1",con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            int cant = (int)cmd.ExecuteScalar();

            if (cant > 0)
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

    public int obtieneSector(int pieza)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select cod_sec from pieza where cod_pie=@pieza",con);
            cmd.Parameters.AddWithValue("pieza",pieza);

            SqlDataReader dr = cmd.ExecuteReader();
            

            if (dr.Read())
            {
                sector = Convert.ToInt32(dr["cod_sec"]);
                    
            }
            con.Close();
            return sector;
            
        }
    }

    public int obtieneNCama(int idcama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select nro_ca from cama where id_cama=@idcama",con);
            cmd.Parameters.AddWithValue("idcama",idcama);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Ncama = Convert.ToInt32(dr["nro_ca"]);
            }
            con.Close();
            return Ncama;
        }
    }

    public void updhos_camaactualpaciente(int ficha, int sector, int pieza, int cama, DateTime fecha, string hora, string minuto)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"insert into hos_camaactualpaciente(nro_fi,cod_sec,cod_pie,nro_cama,fecha,hora,minuto)values
                                              (@ficha,@sector,@pieza,@cama,@fecha,@hora,@minuto)",con);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.Parameters.AddWithValue("sector",sector);
            cmd.Parameters.AddWithValue("pieza",pieza);
            cmd.Parameters.AddWithValue("cama",cama);
            cmd.Parameters.AddWithValue("fecha",fecha);
            cmd.Parameters.AddWithValue("hora",hora);
            cmd.Parameters.AddWithValue("minuto",minuto);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }

    public void updhos_camaactualpaciente2(int ficha, int sector, int pieza, int cama, DateTime fecha, string hora, string minuto)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"update hos_camaactualpaciente set cod_sec=@sector,cod_pie=@pieza,nro_cama=@cama,fecha=@fecha,
                                            hora=@hora,minuto=@minuto where nro_fi=@ficha", con);
            cmd.Parameters.AddWithValue("ficha", ficha);
            cmd.Parameters.AddWithValue("sector", sector);
            cmd.Parameters.AddWithValue("pieza", pieza);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("minuto", minuto);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }

    public bool tieneCamaActual(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from hos_camaactualpaciente where nro_fi=@ficha",con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            int cant = (int)cmd.ExecuteScalar();
            con.Close();

            if (cant > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

    public void borraCamaActual(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete hos_camaactualpaciente where nro_fi=@ficha",con);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();

            con.Close();
        }
    }

    #endregion

    #region traslado
    //******************************TRASLADO************************************************************
    //crea un nuevo registro con los datos de la reserva y paciente que se traslada
    public void traslado(int sec_ori, int pie_ori, int cama2, string rut, DateTime fecha, string hora, int n_sector, int estado, string usuario, string obs, DateTime fechahora, DateTime fechaini, DateTime fechafin,int tipoingreso,int tipopac,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_TRASLADOS(COD_SEC,NRO_PIE,ID_CAMA,RUT_NUM,FECHA,HORA,NUEVO_SECTOR,ESTADO,USUARIO,OBS,FECHAHORA,FECHAINI,FECHAFIN,TIPOINGRESO,TIPOPAC,NRO_FI) VALUES(@sec_ori,@pie_ori,@cama2,@rut,@fecha,@hora,@n_sector,@estado,@usuario,@obs,@fechahora,@fechaini,@fechafin,@tipoingreso,@tipopac,@ficha)", con);
            cmd.Parameters.AddWithValue("sec_ori", sec_ori);
            cmd.Parameters.AddWithValue("pie_ori", pie_ori);
            cmd.Parameters.AddWithValue("cama2", cama2);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("n_sector", n_sector);
            //cmd.Parameters.AddWithValue("n_pieza",n_pieza);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("obs", obs);
            cmd.Parameters.AddWithValue("fechahora", fechahora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("tipoingreso", tipoingreso);
            cmd.Parameters.AddWithValue("tipopac", tipopac);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }


    //elimina la reserva de un paciente que paso por pabellon y que no regreesa a la misma cama
    public bool eliminaReservaXpabellon(int id)
    {
        string sql = "UPDATE HOS_GESTIONCAMA SET ESTADO=0,EVENTOPAC=0 WHERE ID_EVENTO=@id";
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("id",id);

            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
            return true;
        }
    }

    //obtiene la fecha de ingreso del paciente que vuelve de pabellon para crearle un nuevo ingreso
    public void activaUltIngreso(string rut)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("HOS_BUSCA_FINI"); 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("RUT",rut);

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                fechaini= Convert.ToDateTime(dr["FECHAINI"]);
            }
            cmd.Dispose();
            con.Close();
        }
    }

    //elimina registro de tabla traslados de paciente que vuelve a su cama desde pabellon
    public void eliminaTraslado(string rut)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            con.Open();
            string sql = "UPDATE HOS_TRASLADOS SET ESTADO=0 WHERE RUT_NUM=@RUT AND ESTADO=1";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("RUT",rut);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            con.Close();
        }
    }


    //ingreso por traslado normal
    public void ingresoTraslado(string cama, string rut, int estadocama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario, int tipoingreso, int quirurgico, DateTime fechahora,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,FECHAINI,FECHA_FIN,ESTADO,EVENTOPAC,USUARIO,TIPO_INGRESO,QUIRURGICO,FECHAHORA,NRO_FI) VALUES(@cama,@rut,@estadocama,@fecha,@hora,@fechaini,@fechafin,@estado,@eventopac,@usuario,@tipoingreso,@quirurgico,@fechahora,@ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("estadocama", estadocama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("tipoingreso", tipoingreso);
            cmd.Parameters.AddWithValue("quirurgico", quirurgico);
            cmd.Parameters.AddWithValue("fechahora", fechahora);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //crea aseo despues de traslado
    public void CreaAseoTras(int cama2, int estadocama2, DateTime fecha,DateTime fechaini,DateTime fechafin, string hora, string usuario, int estado, int eventopac)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,ID_ESTADOCAMA,FECHA,FECHAINI,FECHA_FIN,HORA,USUARIO,ESTADO,EVENTOPAC) VALUES(@cama2,@estadocama2,@fecha,@fechaini,@fechafin,@hora,@usuario,@estado,@eventopac)", con);
            cmd.Parameters.AddWithValue("cama2", cama2);
            cmd.Parameters.AddWithValue("estadocama2", estadocama2);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("fechaini",fechaini);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //Crea una nueva reserva al terminar aseo para el paciente que esta en pabellon y regresa a la misma cama
    public void creaReserva(int cama, string rut_num, int estadocama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac,int tipoin,int tipopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA(ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,FECHAINI,FECHA_FIN,ESTADO,EVENTOPAC,TIPO_INGRESO,QUIRURGICO,USUARIO,NRO_FI)VALUES(@cama,@rut_num,@estadocama,@fecha,@hora,@fechaini,@fechafin,@estado,@eventopac,@tipoin,@tipopac,@usuario,@ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut_num", rut_num);
            cmd.Parameters.AddWithValue("estadocama", estadocama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("tipoin",tipoin);
            cmd.Parameters.AddWithValue("tipopac",tipopac);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("ficha", ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //PREGUNTA SI EL PACIENTE ESTA ACTUALMENTE EN PABELLON
    public bool esPabellon(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_GESTIONCAMA WHERE NRO_FI=@ficha and ID_ESTADOCAMA=22 AND ESTADO=1",con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            int cant = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            if (cant > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

    //VERIFICA SI EL PACIENTE HA SIDO CONFIRMADO EN HOSPITALIZACION
    public bool esConfirmado(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_GESTIONCAMA WHERE NRO_FI=@ficha AND ID_ESTADOCAMA=18 AND EVENTOPAC=1",con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            int cant = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            if (cant > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public void cierraTraslado(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_TRASLADOS SET ESTADO=0 WHERE ID_TRASLADO=@id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public void obtieneSecOrigen(int idcama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT b.COD_SEC FROM HOS_GESTIONCAMA a,SECTOR b,PIEZA c,CAMA d WHERE a.ID_CAMA=d.ID_CAMA AND d.COD_PIE=c.COD_PIE AND c.COD_SEC=b.COD_SEC AND a.ID_CAMA=@idcama", con);
            cmd.Parameters.AddWithValue("idcama",idcama);

            dr = cmd.ExecuteReader();

            if (dr.Read())
                SecOri = Convert.ToInt32(dr["COD_SEC"]);
            
            dr.Dispose();
            cmd.Dispose();
            con.Close();
        }
    }

    public bool compruebaReserva(int cama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from hos_gestioncama where id_cama=@cama and id_estadocama between 6 and 7 and eventopac=1 and fechaini=convert(datetime,CONVERT(varchar(10), getdate(), 103),103)",con);
            cmd.Parameters.AddWithValue("cama",cama);

            int cant = (int)cmd.ExecuteScalar();

            if (cant > 0)
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


    //**********************************************************************************************************************
    #endregion

    #region alta

    public void alta(string cama, string rut, int estadocama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac,int tipoin,int tipopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,FECHAINI,FECHA_FIN,ESTADO,EVENTOPAC,TIPO_INGRESO,QUIRURGICO,USUARIO,NRO_FI) VALUES(@cama,@rut,@estadocama,@fecha,@hora,@fechaini,@fechafin,@estado,@eventopac,@tipoin,@tipopac,@usuario,@ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("estadocama", estadocama);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("tipoin",tipoin);
            cmd.Parameters.AddWithValue("tipopac",tipopac);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //verifica que el paciente tenga el alta medica antes de poder darle alta administrativa
    public bool verificaAmedica(string cama, string rut)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_GESTIONCAMA WHERE ID_CAMA=@cama and RUT_NUM=@rut AND ID_ESTADOCAMA=13", con);
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

    public bool verifdbclick(string cama)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from hos_gestioncama where id_cama=@cama and id_estadocama=13 and estado=1",con);
            cmd.Parameters.AddWithValue("cama",cama);

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
            dr.Close();
            con.Close();
          }
      }

    //crea aseo despues del alta administrativa
    public void CreaAseo(string cama, int estadocama2, DateTime fecha,DateTime fechaini,DateTime fechafin, string hora, int estado, int eventopac, string usuario)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,ID_ESTADOCAMA,FECHA,FECHAINI,FECHA_FIN,HORA,ESTADO,EVENTOPAC,USUARIO) VALUES(@cama,@estadocama2,@fecha,@fechaini,@fechafin,@hora,@estado,@eventopac,@usuario)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("estadocama2", estadocama2);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //cancelar alta medica o probable
    public void cancelaAlta(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update hos_gestioncama set id_estadocama=5 where nro_fi=@ficha and estado=1",con);
            cmd.Parameters.AddWithValue("ficha",ficha);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }

    //busca si paciente tiene algun tipo de alta vigente
    public bool buscaAlta(int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from hos_gestioncama where nro_fi=@ficha and id_estadocama between 12 and 13 and estado=1",con);
            cmd.Parameters.AddWithValue("ficha",ficha);

            int cant = (int)cmd.ExecuteScalar();

            if (cant > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            con.Close();
            cmd.Dispose();
        }
    }

    #endregion

    #region mantencion

    public DataRow cargaMantencion(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOS_GESTIONCAMA  WHERE ID_EVENTO=@id ", ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }

    public int guardaMan(int cama, int rut, int estadoCama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,FECHA,HORA,FECHAINI,FECHA_FIN,ID_ESTADOCAMA,ESTADO,USUARIO,EVENTOPAC,NRO_FI) VALUES(@cama,@rut,@fecha,@hora,@fechaini,@fechafin,@estadocama,@estado,@usuario,@eventopac,@ficha)SELECT SCOPE_IDENTITY()", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estadoCama", estadoCama);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("ficha", ficha);

            return Convert.ToInt32(cmd.ExecuteScalar());

            cmd.Dispose();
            con.Close();
        }
    }

    public int iniciaMan(int cama, int rut, int estadoCama, DateTime fecha, string hora, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,FECHA,HORA,FECHAINI,FECHA_FIN,ID_ESTADOCAMA,ESTADO,USUARIO,EVENTOPAC,NRO_FI) VALUES(@cama,@rut,@fecha,@hora,@fechaini,@fechafin,@estadocama,@estado,@usuario,@eventopac,@ficha)SELECT SCOPE_IDENTITY()", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estadoCama", estadoCama);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("ficha", ficha);

            return Convert.ToInt32(cmd.ExecuteScalar());

            cmd.Dispose();
            con.Close();
        }
    }

    public void updMan(int id, int estado, int eventopac,DateTime fechahora)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0, EVENTOPAC=0,FECHAHORA=@FECHAHORA WHERE ID_EVENTO=@id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("fechahora",fechahora);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public void terminaMan(int cama, int rut, int estadoCama, DateTime fecha, string hora, string horafin, DateTime fechaini, DateTime fechafin, int estado, int eventopac, string usuario,int ficha)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,FECHA,HORA,FECHAINI,FECHA_FIN,ID_ESTADOCAMA,ESTADO,USUARIO,EVENTOPAC,NRO_FI) VALUES(@cama,@rut,@fecha,@hora,@fechaini,@fechafin,@estadocama,@estado,@usuario,@eventopac,ficha)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("rut", rut);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("horafin", horafin);
            cmd.Parameters.AddWithValue("fechaini", fechaini);
            cmd.Parameters.AddWithValue("fechafin", fechafin);
            cmd.Parameters.AddWithValue("estadoCama", estadoCama);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.Parameters.AddWithValue("ficha", ficha);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    //cancela mantencion
    public void eliminaMan(int id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM HOS_GESTIONCAMA WHERE ID_EVENTO = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public void guardaDetalleMant(int id, int cama, DateTime fecha, string obs,string usuario)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into hos_det_mant(id_evento,id_cama,fecha,obs,usuario)values(@id,@cama,@fecha,@obs,@usuario)", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("cama",cama);
            cmd.Parameters.AddWithValue("fecha",fecha);
            cmd.Parameters.AddWithValue("obs",obs);
            cmd.Parameters.AddWithValue("usuario",usuario);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    #endregion

    #region aseo

    //cambia el tipo de aseo
    public void cambiaTipoAseo(int idaseo, int tipo2)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ID_ESTADOCAMA=@tipo2 WHERE ID_EVENTO=@idaseo", con);
            cmd.Parameters.AddWithValue("idaseo", idaseo);
            cmd.Parameters.AddWithValue("tipo2", tipo2);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public void terminarAseo(string id, string horafin, string usuario)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET HORAFIN=@horafin, ESTADO=0,EVENTOPAC=0, USUARIO=@usuario WHERE ID_EVENTO=@id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("horafin", horafin);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    #endregion

    #region paginaInicio

    public void buscaPac(string apat)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT RTRIM(LTRIM(b.A_PAT))+' '+ RTRIM(LTRIM(b.A_MAT))+' '+ RTRIM(LTRIM(b.NOMBRE))as paciente,
                                            C.DESCRIP +' '+ D.DESCRIP +' '+ LTRIM(Str(E.NRO_CA, 25))CAMA FROM HOS_GESTIONCAMA A, PACIENTE B, SECTOR C,PIEZA D,CAMA E
                                            WHERE (A.ID_ESTADOCAMA=5 AND A.RUT_NUM=B.RUT_NUM AND A.ID_CAMA=E.ID_CAMA AND E.COD_PIE=D.COD_PIE AND D.COD_SEC=C.COD_SEC 
                                            AND upper(B.A_PAT) like '%' + upper(@apat) + '%' AND A.ESTADO=1) OR(A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.RUT_NUM=B.RUT_NUM 
                                            AND A.ID_CAMA=E.ID_CAMA AND E.COD_PIE=D.COD_PIE AND D.COD_SEC=C.COD_SEC AND upper(B.A_PAT) like '%' + upper(@apat) + '%' AND A.ESTADO=1)", con);
            cmd.Parameters.AddWithValue("apat",apat);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                pacBuscado = Convert.ToString(dr["paciente"]);
                camaBuscada = Convert.ToString(dr["cama"]);
            }

            cmd.Dispose();
            con.Close();
        }
    }

    public void PacUpc()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=4)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=4)OR (A.ID_ESTADOCAMA=18 AND A.EVENTOPAC=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=4)", con);

            pacupc = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void PacH()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=1)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=1)", con);

            pacH = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void PacF()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=2)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=2)", con);

            pacF = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void PacM()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=3)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=3)", con);

            pacM = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }

    public void PacE()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HOS_GESTIONCAMA A, SECTOR B,PIEZA C,CAMA D WHERE (A.ID_ESTADOCAMA=5 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)OR (A.ID_ESTADOCAMA BETWEEN 12 AND 13 AND A.ESTADO=1 AND A.ID_CAMA=D.ID_CAMA AND D.COD_PIE=C.COD_PIE AND C.COD_SEC=B.COD_SEC AND B.COD_SEC=11)", con);

            pacE = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            con.Close();
        }
    }
    #endregion
}