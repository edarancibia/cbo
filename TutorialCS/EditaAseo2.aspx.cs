using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

public partial class EditaAseo : System.Web.UI.Page
{
    
    public DateTime horafin, horaini,horaact2;
    public int tipo, perfil, idaseo, tipo2, idcama;
    Camas funcamas = new Camas();

    private string para;
    private string asunto;
    private string cuerpo;
    private MailMessage correo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblerror.Visible = false;
            lblperfil.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 4)))
            {
                lblperfil.Text = "No tiene privilegios de aseo.";
                lblperfil.Visible = true;
                bntcambiatipo.Enabled = false;
                Button1.Enabled = false;
                cbotipoaseo2.Enabled = false;
            }
            
            DataRow dr = funcamas.cargaAseo(Request.QueryString["id"]);

            if (dr == null)
            {
                throw new Exception("El aseo no fue encontrado.");
            }

            //llena combo camas
            cboCamas3.DataSource = funcamas.llenaCamas();
            cboCamas3.DataTextField = "CAMA";
            cboCamas3.DataValueField = "ID_CAMA";
            cboCamas3.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
            cboCamas3.DataBind();
            cboCamas3.Enabled = false;

            //llena cbotipoaseo
            cbotipoaseo2.DataSource = funcamas.llenaTipoAseo();
            cbotipoaseo2.DataTextField = "DESCRIPCION";
            cbotipoaseo2.DataValueField = "ID_ESTADOCAMA";
            //cbotipoaseo2.SelectedValue = Convert.ToString(dr["ID_ESTADOCAMA"]);
            cbotipoaseo2.DataBind();
            //cbotipoaseo2.Enabled = false;

            int idcama2 = Convert.ToInt32(dr["ID_CAMA"]);
            txtini2.Text = Convert.ToString(dr["HORA"]);
            calcfinest();
            lblcama.Text = idcama2.ToString();
            lblcama.Visible = false;
        }

        txtfinest.ReadOnly = true;
        txtini2.ReadOnly = true;
        lblerror.Visible = false;
    }

    private void creaMant(int idcama, int tipo2,string rut,DateTime fecha,DateTime fechaini,DateTime fechafin, string hora,int estado,string usuario,int eventopac)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,ID_ESTADOCAMA,RUT_NUM,FECHA,FECHAINI,FECHA_FIN,HORA,ESTADO,USUARIO,EVENTOPAC)VALUES(@idcama,@tipo2,@rut,@fecha,@fechaini,@fechafin,@hora,@estado,@usuario,@eventopac)", con);
            cmd.Parameters.AddWithValue("idcama", idcama);
            cmd.Parameters.AddWithValue("tipo2", tipo2);
            cmd.Parameters.AddWithValue("rut",rut);
            cmd.Parameters.AddWithValue("fecha",fecha);
            cmd.Parameters.AddWithValue("fechaini",fechaini);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.Parameters.AddWithValue("hora",hora);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario",usuario);
            cmd.Parameters.AddWithValue("eventopac",eventopac);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }


    private void calcfinest()
    {
        tipo = Convert.ToInt32(cbotipoaseo2.SelectedValue);
        horaini = Convert.ToDateTime(txtini2.Text);

        if (tipo == 9)
        {
            horafin = horaini.AddMinutes(40);
            txtfinest.Text = horafin.ToString("HH:mm");
        }

        if (tipo == 10)
        {
            horafin = horaini.AddMinutes(120);
            txtfinest.Text = horafin.ToString("HH:mm");
        }

        if (tipo == 11)
        {
            horafin = horaini.AddMinutes(10);
            txtfinest.Text = horafin.ToString("HH:mm");
        }
    }

    
    //comprueba si el paciente que avando la cama esta en pabellon
    protected void buscaPabellon()
    {
       using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
       {
           int cama = Convert.ToInt32(cboCamas3.SelectedValue);
           con.Open();
           SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_TRASLADOS where ID_CAMA=@cama AND NUEVO_SECTOR=13 and ESTADO=1", con);
           cmd.Parameters.AddWithValue("cama",cama);

           SqlDataReader dr = cmd.ExecuteReader();

           if (dr.Read())
           {
               string rutpac=Convert.ToString(dr["RUT_NUM"]);
               DateTime fecha = DateTime.Now;
               string hora = DateTime.Now.ToString("HH:mm");
               DateTime fechafin = DateTime.Now;
               string usuario = Convert.ToString(Session["rutusu"]);
               int tipoin = Convert.ToInt32(Session["tipoingreso"]);
               int tipopac = Convert.ToInt32(Session["tipopac"]);
               int ficha = Convert.ToInt32(Session["ficha"]);

               funcamas.creaReserva(cama,rutpac,17,fecha,hora,fecha,fechafin,0,1,tipoin,tipopac,usuario,ficha);
           }
           cmd.Dispose();
           con.Close();
       }    
    }

    protected void bntcancelar_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //termina el aseo
        DateTime finest = Convert.ToDateTime(txtfinest.Text);

        string id = Request.QueryString["id"];
        string horafin = DateTime.Now.ToString("HH:mm");
        string usuario = Convert.ToString(Session["rutusu"]);

        funcamas.terminarAseo(id, horafin, usuario);
        //buscaPabellon();
        Modal.Close(this, "OK");

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //cambia el tipo de aseo
        bntcambiatipo.Enabled = false;
        idaseo = Convert.ToInt32(Request.QueryString["id"]);
        tipo2 = Convert.ToInt32(cbotipoaseo2.SelectedValue);
        string id = Request.QueryString["id"];
        string horafin = DateTime.Now.ToString("HH:mm");
        string usuario = Convert.ToString(Session["rutusu"]);
        idcama = Convert.ToInt32(lblcama.Text);
        DateTime fecha = DateTime.Now;
        string hora = DateTime.Now.ToString("HH:mm");
        DateTime fechaini = DateTime.Now;
        DateTime fechafin = DateTime.Now;
        string obs = txtobs.Text;

        string camaMail = cboCamas3.SelectedItem.ToString();
        funcamas.para = "mantencioncbo@gmail.com";
        funcamas.asunto = "Se solicita asistencia";
        funcamas.cuerpo = "La cama '" + camaMail + "' requiere mantención. Detalle: '" + obs + "'  Solicitado por '" + Session["nomusuario"].ToString() + "'. Gestión camas CBO.";

        if (tipo2 == 15) //solicita mantencion
        {
            funcamas.terminarAseo(id, horafin, usuario);
            
            int obtenido = funcamas.iniciaMan(idcama, 4, 4, fecha, hora, fechaini, fechafin, 1, 1, usuario,0);

            if (obtenido > 0)
            {
                funcamas.guardaDetalleMant(obtenido, idcama, fecha, obs, usuario);
                funcamas.enviaCorreo();
                //Modal.Close(this, "OK");
            }
        }
        else
        {
            funcamas.cambiaTipoAseo(idaseo, tipo2);
        }
        Modal.Close(this, "OK");

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Modal.Close(this, "OK");
    }
}