
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

public partial class Edit : Page
{
    int estadocama;
    string rutsicbo, apat, amat, nombre2, rut2;
    public int perfil;
    public string usuario;
    Camas funcamas = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {

                TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
                TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();

                //llena combo camas
                cboCamas.DataSource = funcamas.llenaCamas();
                cboCamas.DataTextField = "CAMA";
                cboCamas.DataValueField = "ID_CAMA";
                cboCamas.SelectedValue = Request.QueryString["r"];
                cboCamas.DataBind();
                cboCamas.Enabled = false;

                //llena combo prevision
                cboprev.DataSource = funcamas.llenaPrevis();
                cboprev.DataTextField = "NOMBRE";
                cboprev.DataValueField = "RUT_NUM";
                cboprev.DataBind();

                //llena combo procecencia
                cbotipoingre.DataSource = funcamas.cboIngre();
                cbotipoingre.DataTextField = "DESCRIPCION";
                cbotipoingre.DataValueField = "ID_TIPOINGRESO";
                cbotipoingre.DataBind();
                cbotipoingre.Enabled = true;
                
                //lblerror2.Visible = false;
                lblrut2.Visible = false;
                //lblrut2.Text = rut2;

                txtape.ReadOnly = true;
                txtnom.ReadOnly = true;
                txtinter.ReadOnly = true;
                txttelefono.ReadOnly = true;

                usuario = Convert.ToString(Session["rutusu"]);
                perfil = Convert.ToInt32(Session["perfil"]);
                
                if (!((perfil == 1) || (perfil == 3)))
                {
                    lblperfil.Text = "No tiene privilegios para hacer un ingreso.";
                    bloqueacontroles();
                    lblperfil.Visible = true;
                }
                
                lblerror2.Visible = false;
                lblperfil.Visible = false;
                txthora.Text = DateTime.Now.ToString("HH:mm");
                int cama2 = Convert.ToInt32(cboCamas.SelectedValue);

                if (Convert.ToDateTime(TextBoxStart.Text) > DateTime.Now)
                {
                    bloqueacontroles();
                    ButtonOK.Enabled = false;
                }

                if (funcamas.ocupada(cama2))
                {
                    lblerror2.Text = "La cama no esta disponible para un ingreso porque esta ocupada por otro paciente.";
                    bloqueacontroles();
                }

                if (funcamas.aseo(cama2))
                {
                    lblerror2.Text = "La cama no esta disponible para un ingreso porque esta en aseo.";
                    bloqueacontroles();
                    lblerror2.Visible = true;
                    lblperfil.Visible = false;
                }

                if (estadocama == 5)
                {
                    ButtonOK.Enabled = false;
                    bloqueacontroles();
                    lblperfil.Visible = false;
                }
                txtrut.Focus();
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
       usuario = Convert.ToString(Session["rutusu"]);
       string rut = txtrut.Text;
       int estado = 0;
       string apellidos = txtape.Text;
       string nombre = txtnom.Text;
       int prevision = Convert.ToInt32(cboprev.SelectedValue);
       string telefono = txttelefono.Text;
       string intervencion = txtinter.Text;
       int tipoingreso = Convert.ToInt32(cbotipoingre.SelectedValue);
       int quirurgico = Convert.ToInt32(cbotipopac.SelectedValue);

       funcamas.updEstEvento(cama, estado);
       funcamas.guardaPacIngreso(rut, apellidos, nombre, prevision, telefono, intervencion);
       funcamas.ingresoDirecto(cama, rut, 5, fecha, hora, fechaini, fechafin, 1, 1, usuario, tipoingreso, quirurgico);
       Modal.Close(this, "OK");
            
    }

    private void buscaRut(string rut2)
    {
        rut2 = txtrut.Text;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT APELLIDOS,NOMBRES FROM HOS_PACIENTE WHERE RUT_NUM=@rut2", con);
            cmd.Parameters.AddWithValue("rut2", rut2);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //rut2 = dr["RUT_NUM"].ToString();
                    apat = dr["APELLIDOS"].ToString();
                    //amat = dr["A_MAT"].ToString();
                    nombre2 = dr["NOMBRES"].ToString();

                    txtape.Text = apat;
                    txtnom.Text = nombre2;
                }
            }
            else
            {
                txtape.Focus();
            }
            cmd.Dispose();
            con.Close();
        }
    }

    private bool bloqueacontroles()
    {
        lblerror2.Visible = false;
        lblrut2.Visible = false;
        ButtonOK.Enabled = false;
        cboCamas.Enabled = false;
        cboprev.Enabled = false;
        TextBoxStart.ReadOnly = true;
        TextBoxEnd.ReadOnly = true;
        txtape.ReadOnly = true;
        txtinter.ReadOnly = true;
        txtnom.ReadOnly = true;
        txtrut.ReadOnly = true;
        txttelefono.ReadOnly = true;
        cbotipoingre.Enabled = false;
        cbotipopac.Enabled = false;
        return true;
    }


    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }

    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this);
    }

    protected void btnvalida_Click(object sender, EventArgs e)
    {
        buscaRut(rut2);

        txtape.ReadOnly = false;
        txtnom.ReadOnly = false;
        txtinter.ReadOnly = false;
        txttelefono.ReadOnly = false;
    }
}
