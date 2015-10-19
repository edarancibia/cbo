using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Windows.Forms;
using System.Net.Mail;

public partial class New : Page
{
    public SqlDataReader dr;
    string rutsicbo,apat,amat,nombre2,rut2;
    public int perfil;
    Camas funcamas = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        perfil = Convert.ToInt32(Session["perfil"]);

        if(!((perfil == 1) || (perfil == 5)))
        {
            lblperfil.Text = "No tiene privilegios se usuario para esta opción.";
            lblperfil.Visible = true;
        }
        
        if (!IsPostBack)
        {
            DataRow dr = funcamas.cargaMantencion(Request.QueryString["id"]);

            if (dr == null)
            {
                throw new Exception("Error al cargar mantencion.");
            }

            int tipoevento = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
            int estEvento = Convert.ToInt32(dr["ESTADO"]);

            if (tipoevento != 4)
            {
                lblerror.Text = "No tiene privilegios para esta opción.";
                lblerror.Visible = true;
                bloqueaControles();
                //btnfin.Enabled = false;
                lblperfil.Visible = false;
                LinkButton1.Visible = false;
            }
            else if(tipoevento==4 && estEvento==1)
            {
                lblerror.Visible = true;
                btnfin.Enabled = true;
                lblperfil.Visible = false;
                LinkButton1.Visible = false;
                
            }

                TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();

                cbocama.DataSource = funcamas.llenaCamas();
                cbocama.DataTextField = "CAMA";
                cbocama.DataValueField = "ID_CAMA";
                cbocama.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                cbocama.DataBind();
                cbocama.Enabled = false;
                txthoraini.ReadOnly = true;
                txthoraini.Text = Convert.ToString(dr["HORA"]);
                btnfin.Enabled = true;
                lblerror.Visible = false;
                lblperfil.Visible = false;
           
        }

    }

    private void creaAseo(int cama, int estadocama2, DateTime fecha, string hora, int estado, string usuario, int eventopac)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO HOS_GESTIONCAMA (ID_CAMA,ID_ESTADOCAMA,FECHA,HORA,ESTADO,USUARIO,EVENTOPAC) VALUES(@cama,@estadocama2,@fecha,@hora,@estado,@usuario,@eventopac)", con);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("estadocama2", estadocama2);
            cmd.Parameters.AddWithValue("fecha", fecha);
            cmd.Parameters.AddWithValue("hora", hora);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("usuario", usuario);
            cmd.Parameters.AddWithValue("eventopac", eventopac);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    protected bool bloqueaControles()
    {
        TextBoxStart.ReadOnly = true;
        TextBoxEnd.ReadOnly = true;
        txthoraini.ReadOnly = true;
        cbocama.Enabled = false;
        btnfin.Enabled = false;
        return true;
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }
    protected void btnfin_Click(object sender, EventArgs e)
    {
        btnfin.Enabled = false;
        DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
        DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
        DateTime fecha = DateTime.Now;
        int cama = Convert.ToInt32(cbocama.SelectedValue);
        string cama2 = Convert.ToString(cbocama.SelectedValue);
        string hora = txthoraini.Text;
        string horafin = DateTime.Now.ToString("HH:mm");
        int id = Convert.ToInt32(Request.QueryString["id"]);
        string usuario = Convert.ToString(Session["rutusu"]);

        string camaMail = cbocama.SelectedItem.ToString();
        funcamas.para = "aseocbo@gmail.com";
        funcamas.asunto = "Se solicita aseo";
        funcamas.cuerpo = "La cama '" + camaMail + "' requiere aseo. Solicitado por '" + Session["nomusuario"].ToString() + "'. Gestión camas CBO.";

        funcamas.updMan(id, 0, 0,fecha);
        //funcamas.terminaMan(cama,4,4,fecha,hora,horafin,fechaini,fechafin,0,0, usuario);
        //creaAseo(cama,9,fecha,hora,1,usuario,1);
        funcamas.CreaAseo(cama2,9,fecha,fecha,fecha,hora,1,1,usuario);
        funcamas.enviaCorreo();
        Modal.Close(this, "OK");
    }

    protected void btncancelar_Click(object sender, EventArgs e)
    {
        Modal.Close(this, "OK");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(Request.QueryString["id"]);
        funcamas.eliminaMan(id);
        Modal.Close(this, "OK");
    }
}
