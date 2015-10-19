
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
        if (!IsPostBack)
        {
            lblerror.Visible = false;
            lblperfil.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 5)))
            {
                lblperfil.Text = "No tiene privilegios para programar una mantención.";
                lblperfil.Visible = true;
                ButtonOK.Enabled = false;
                cbocama.Enabled = false;
                lblerror.Visible = false;
            }

            TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();

            lblerror.Visible = false;
            
            cbocama.DataSource = funcamas.llenaCamas();
            cbocama.DataTextField = "CAMA";
            cbocama.DataValueField = "ID_CAMA";
            cbocama.SelectedValue = Request.QueryString["r"];
            cbocama.DataBind();
            cbocama.Enabled = false;
            txthoraini.Text = DateTime.Now.ToString("HH:mm");

        }

    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
        DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
        DateTime fecha = DateTime.Now;
        int cama = Convert.ToInt32(cbocama.SelectedValue);
        string hora = txthoraini.Text;
        string usuario = Convert.ToString(Session["rutusu"]);
        string obs = txtdet.Text;

        if (funcamas.ocupada2(cama))
        {
            lblerror.Text = "No puede hacer mantencion mientras la cama esté ocupada.";
            lblerror.Visible = true;
        }
        else
        {
            if (esOcupada(cama, fechaini, fechafin))
            {
                lblerror.Text = "LA CAMA NO ESTA DISPONIBLE PARA LA FECHA INDICADA";
                lblerror.Visible = true;
            }
            else
            {
                int obtenido = funcamas.guardaMan(cama, 4, 4, fecha, hora, fechaini, fechafin, 0, 1, usuario,0);
                //funcamas.guardaMan(cama, 4, 4, fecha, hora, fechaini, fechafin, 0, 1, usuario);
                if (obtenido > 0)
                {
                    funcamas.guardaDetalleMant(obtenido, cama, fecha, obs, usuario);
                    Modal.Close(this, "OK");
                }

            }
        }
    
    }

    private bool esOcupada(int cama, DateTime fechaini, DateTime fechafin)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM HOS_GESTIONCAMA WHERE FECHAINI < @fechafin AND @fechaini < FECHA_FIN AND ID_CAMA=@cama", con);
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



    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }
    protected void txtrut_TextChanged(object sender, EventArgs e)
    {

    }
 
}
