
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Windows.Forms;

public partial class New : Page
{
    public SqlDataReader dr;
    public SqlCommand cmd;
    string rutsicbo,apat,amat,nombre2,rut2;
    public int perfil;
    Camas funcamas = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            perfil = Convert.ToInt32(Session["perfil"]);

            if (Session["perfil"] == null)
            {
                lblerror.Visible = true;
                lblerror.Text = "Debe iniciar sesión";
            }

            if (!((perfil == 1) || (perfil == 2)))
            {
                lblerror.Text = "No tiene privilegios para hacer una reserva.";
                bloqueacontroles();
                lblerror.Visible = true;
            }
            else
            {
                lblerror.Visible = false;
            }

            TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();

            txtrut.Focus();
            
            cbocama.DataSource = funcamas.llenaCamas();
            cbocama.DataTextField = "CAMA";
            cbocama.DataValueField = "ID_CAMA";
            cbocama.SelectedValue = Request.QueryString["r"];
            cbocama.DataBind();
            cbocama.Enabled = false;

            cboprev.DataSource = funcamas.llenaPrevis();
            cboprev.DataTextField = "NOMBRE";
            cboprev.DataValueField = "RUT_NUM";
            cboprev.DataBind();
            txtape.ReadOnly = true;
            txtape.ReadOnly = true;
            txttelefono.ReadOnly = true;
            txtinter.ReadOnly = true;
            txtnombre.ReadOnly = true;
            ButtonOK.Enabled = false;
            txtdig.ReadOnly = true;
        }

    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
        DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
        DateTime fecha = DateTime.Now;
        int cama = Convert.ToInt32(cbocama.SelectedValue);
        string rut = txtrut.Text;
        //string rutRes = Convert.ToString(Session["rutRes"]);
        string apellidos = txtape.Text;
        string nombre = txtnombre.Text;
        string telefono = txttelefono.Text;
        int prevision = Convert.ToInt32(cboprev.SelectedValue);
        string intervension = txtinter.Text;
        string hora = DateTime.Now.ToString("HH:mm");
        string usuario = Convert.ToString(Session["rutusu"]);
        int sexo;

        if (RadioButton1.Checked)
        {
            sexo = 1;
        }
        else
        {
            sexo = 0;
        }
        //si la cama esta ocupada la nueva reserva queda en estado 0
        //if (funcamas.ocupada(cama))
        //{
            if (funcamas.esOcupada(cama, fechaini, fechafin))
            {
               lblerror.Text = "LA CAMA SELECCIONADA NO ESTA DISPONIBLE PARA LA FECHA INDICADA";
               lblerror.Visible = true;
            }
            else
            {
                if (funcamas.existePac(rut))
                {
                    funcamas.guardaReserva(cama, rut, 6, fecha, hora, fechaini, fechafin, 0, 1, usuario,0);
                    Modal.Close(this, "OK");
                }
                else
                {
                    funcamas.guardaPaciente(rut,txtdig.Text,nombre,apellidos,txtape2.Text,telefono,sexo);
                    funcamas.guardaReserva(cama, rut, 6, fecha, hora, fechaini, fechafin, 0, 1, usuario,0);
                    Modal.Close(this, "OK");
                }
             }        
            
          //}
    }

    private void updEstEvento(int cama, int estado)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0 WHERE ID_CAMA=@cama AND ESTADO=1", con);
            cmd.Parameters.AddWithValue("estado", estado);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    protected bool bloqueacontroles()
    {
        TextBoxStart.ReadOnly = true;
        TextBoxEnd.ReadOnly = true;
        txtape.ReadOnly = true;
        txtinter.ReadOnly = true;
        txtnombre.ReadOnly = true;
        txtrut.ReadOnly = true;
        txttelefono.ReadOnly = true;
        cbocama.Enabled = false;
        cboprev.Enabled = false;
        ButtonOK.Enabled = false;
        return true;
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }

    protected void txtrut_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btnvalida_Click(object sender, EventArgs e)
    {
        if (funcamas.existePac(txtrut.Text))
        {
            funcamas.buscaXrut(txtrut.Text);
            txtnombre.Text = funcamas.nombre2;
            txtape.Text = funcamas.apat;
            txtape2.Text = funcamas.amat;
            txtdig.Text = funcamas.rut_ver;
        }
        else
        {
            txtdig.ReadOnly = false;
            lblerror.Text="El paciente no está registrado, debe guardarlo.";
            lblerror.Visible=true;
            txtnombre.Text = "";
            txtdig.Text = "";
            txtape.Text = "";
            txtape2.Text = "";
            //Session["rutRes"] = Convert.ToString(txtrut.Text);
        }
        txtape.ReadOnly = false;
        txtnombre.ReadOnly = false;
        txtinter.ReadOnly = false;
        txttelefono.ReadOnly = false;
        ButtonOK.Enabled = true;
    }
}
