
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

public partial class Edit : Page
{
    public int estadocama, perfil;
    string rut2;
    Camas funcamas = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {

            lblerror2.Visible = false;
            lblperfil.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 3)))
            {
                lblperfil.Text = "No tiene privilegios para hacer un ingreso.";
                lblperfil.Visible = true;
                ButtonOK.Enabled = false;
                cboCamas.Enabled = false;
                cbotipopac.Enabled = false;
                cbotipoingre.Enabled = false;
            }

            DataRow dr = funcamas.cargaReserva(Request.QueryString["id"]);
            DataRow dr2 = funcamas.cargaReserva(Request.QueryString["ID_ESTADOCAMA"]);
            
            if (dr == null)
            {
                throw new Exception("La reserva no fue encontrada.");
            }

            TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
            estadocama = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
            txtape.Text = dr["APELLIDOS"] as string;
            txtnom.Text = dr["NOMBRES"] as string;
            rut2 = dr["RUT_NUM"] as string;
            int cama2 = Convert.ToInt32(dr["ID_CAMA"]);

            //llena combo camas
            cboCamas.DataSource = funcamas.llenaCamas();
            cboCamas.DataTextField = "CAMA";
            cboCamas.DataValueField = "ID_CAMA";
            cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
            cboCamas.DataBind();

            //llena combo procecencia
            cbotipoingre.DataSource = funcamas.cboIngre();
            cbotipoingre.DataTextField = "DESCRIPCION";
            cbotipoingre.DataValueField = "ID_TIPOINGRESO";
            cbotipoingre.DataBind();
            cbotipoingre.Enabled = true;

            TextBoxStart.ReadOnly = true;
            TextBoxEnd.ReadOnly = true;
            cboCamas.Enabled = false;
            txtape.ReadOnly = true;
            txtnom.ReadOnly = true;

            lblerror2.Visible = false;
            lblrut2.Visible = false;
            lblrut2.Text = rut2;
            txthora.Text = DateTime.Now.ToString("HH:mm");

            //txthora.Text = DateTime.Now.ToString("HH:mm");

            if (Convert.ToDateTime(TextBoxStart.Text) > DateTime.Now)
            {
                ButtonOK.Enabled = false;
            }
            if (funcamas.ocupada(cama2))
            {
                lblerror2.Text = "La cama no esta disponible para un ingreso porque esta ocupada por otro paciente.";
                lblerror2.Visible = true;
                ButtonOK.Enabled = false;
            }

            if (aseo(cama2))
            {
                lblerror2.Text = "La cama no esta disponible para un ingreso porque esta en aseo.";
                lblerror2.Visible = true;
                cbotipopac.Enabled = false;
                ButtonOK.Enabled = false;
            }


            if (estadocama == 5) 
            {
                ButtonOK.Enabled = false;
            }
  

        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {

       DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
       DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
       DateTime fecha = DateTime.Now;
       string hora = txthora.Text;
       string cama = cboCamas.SelectedValue;
       string id = Request.QueryString["id"];
       string usuario = Convert.ToString(Session["rutusu"]);
       string rut = lblrut2.Text;
       int estado = 0;
       int quirurgico = Convert.ToInt32(cbotipopac.SelectedValue);
       int tipoingreso = Convert.ToInt32(cbotipoingre.SelectedValue);

       updEstEvento(cama, estado);
       cierraReserva(id, fechafin);
       ingreso(cama, rut, 5, fecha, hora, fechaini, fechafin, 1, 1, usuario, tipoingreso, quirurgico);
       Modal.Close(this, "OK");
       
    }

    private void ingreso(string cama,string rut,int estadocama,DateTime fecha,string hora,DateTime fechaini,DateTime fechafin,int estado,int eventopac,string usuario,int tipoingreso,int quirurgico)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,RUT_NUM,ID_ESTADOCAMA,FECHA,HORA,FECHAINI,FECHA_FIN,ESTADO,EVENTOPAC,USUARIO,TIPO_INGRESO,QUIRURGICO) VALUES(@cama,@rut,@estadocama,@fecha,@hora,@fechaini,@fechafin,@estado,@eventopac,@usuario,@tipoingreso,@quirurgico)",con);
            cmd.Parameters.AddWithValue("cama",cama);
            cmd.Parameters.AddWithValue("rut",rut);
            cmd.Parameters.AddWithValue("estadocama",estadocama);
            cmd.Parameters.AddWithValue("fecha",fecha);
            cmd.Parameters.AddWithValue("hora",hora);
            cmd.Parameters.AddWithValue("fechaini",fechaini);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.Parameters.AddWithValue("estado",estado);
            cmd.Parameters.AddWithValue("eventopac",eventopac);
            cmd.Parameters.AddWithValue("usuario",usuario);
            cmd.Parameters.AddWithValue("tipoingreso",tipoingreso);
            cmd.Parameters.AddWithValue("quirurgico", quirurgico);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    private void cierraReserva(string id,DateTime fechafin)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0,FECHA_FIN=@fechafin,EVENTOPAC=0 WHERE ID_EVENTO=@id",con);
            cmd.Parameters.AddWithValue("id",id);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    private bool aseo(int cama)
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


    private void updEstEvento(string cama, int estado)
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


    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }

    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this);
    }
}
