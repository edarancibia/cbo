
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Windows.Forms;

public partial class New : Page
{
    public SqlDataReader dr;
    string rutsicbo,apat,amat,nombre2,rut2;
    public int perfil;
    Camas funcamas = new Camas();
 
    protected void Page_Load(object sender, EventArgs e)
    {
        perfil = Convert.ToInt32(Session["perfil"]);

        if (!((perfil == 1) || (perfil == 5)))
        {
            lblperfil.Text = "No tiene privilegios se usuario para esta opción.";
            lblperfil.Visible = true;
            bntini.Enabled = false;
        }
        
        if (!IsPostBack)
        {
          TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
          TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();
          cbocama.DataSource = funcamas.llenaCamas();
          cbocama.DataTextField = "CAMA";
          cbocama.DataValueField = "ID_CAMA";
          cbocama.SelectedValue = Request.QueryString["r"];
          cbocama.DataBind();
          cbocama.Enabled = false;
          txthoraini.ReadOnly = true;
          txthoraini.Text = DateTime.Now.ToString("HH:mm");
          lblerror.Visible = false;
          lblperfil.Visible = false;

          int cama2 = Convert.ToInt32(cbocama.SelectedValue);
          if(aseo(cama2))
          {
              lblerror.Text = "La habitación no está disponible para mantención";
              lblerror.Visible = true;
              bntini.Enabled = false;
          }

          if (funcamas.ocupada2(cama2))
          {
              lblerror.Text = "La habitación no está disponible para mantención";
              lblerror.Visible = true;
              bntini.Enabled = false;
          }          
        }
    }

    private void updMan(int id, int estado, int eventopac)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0, EVENTOPAC=0 WHERE ID_EVENTO=@id", con);
            cmd.Parameters.AddWithValue("id",id);
            cmd.Parameters.AddWithValue("estado",estado);
            cmd.Parameters.AddWithValue("eventopac",eventopac);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }


    private bool aseo(int cama2)
    {
        string sql = "SELECT * FROM HOS_GESTIONCAMA WHERE ID_CAMA=@cama2 AND ID_ESTADOCAMA BETWEEN 9 AND 11 AND ESTADO=1";
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("cama2", cama2);

            con.Open();

            int cont = Convert.ToInt32(cmd.ExecuteScalar());

            if (cont == 0)
                return false;
            else
                return true;
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
        return true;
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }

    protected void bntini_Click(object sender, EventArgs e)
    {

        DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
        DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
        DateTime fecha = DateTime.Now;
        int cama = Convert.ToInt32(cbocama.SelectedValue);
        string hora = txthoraini.Text;
        int id = Convert.ToInt32(Request.QueryString["id"]);
        string usuario = Convert.ToString(Session["rutusu"]);
        string obs = txtobs.Text;

        funcamas.updEstEvento2(cama, 0);

        int obtenido = funcamas.iniciaMan(cama, 4, 4, fecha, hora, fechaini, fechafin, 1, 1, usuario,0);
      
        if (obtenido > 0)
        {
            funcamas.guardaDetalleMant(obtenido, cama, fecha, obs, usuario);
            Modal.Close(this, "OK");
        }
    }
    protected void btncancelar_Click(object sender, EventArgs e)
    {
        Modal.Close(this, "OK");
    }
}
