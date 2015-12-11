
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Text;

public partial class Edit : Page
{
    int estadocama,cama;
    Camas funcamas = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            /*
            lblerror2.Visible = false;
            lblperfil.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 2)))
            {
              lblperfil.Text = "No tiene privilegios para hacer un ingreso.";
              lblperfil.Visible = true;
              ButtonOK.Enabled = false;
              cboCamas.Enabled = false;
              
            }
            */

            DataRow dr = funcamas.cargaReserva(Request.QueryString["id"]);
            Session["idevento"] = Convert.ToInt32(dr["ID_EVENTO"]);
            Session["ficha"] = Convert.ToInt32(dr["nro_fi"]);
            int ideve1 = Convert.ToInt32(Session["idevento"]);
            int estado = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
            txtobs.Text = dr["OBS"] as string;

            if ((estado == 18)||(estado == 6)||(estado==7)||(estado==5)||(estado==22)||(estado >= 12 && estado <= 13))
            {
                TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                txtape.Text = dr["APELLIDO"] as string;
                txtnom.Text = dr["NOMBRE"] as string;
                txtobs.Text = dr["OBS"] as string;
                txtficha.Text = dr["NRO_FI"] as string;

                //llena combo camas
                cboCamas.DataSource = funcamas.llenaCamas();
                cboCamas.DataTextField = "CAMA";
                cboCamas.DataValueField = "ID_CAMA";
                cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                cboCamas.DataBind();
                cboCamas.Enabled = false;

                //llena combo tipo ingreso
                cbotipoingre.DataSource = funcamas.cboIngre();
                cbotipoingre.DataTextField = "DESCRIPCION";
                cbotipoingre.DataValueField = "ID_TIPOINGRESO";
                cbotipoingre.DataBind();
                cbotipoingre.Enabled = true;
                txtnom.ReadOnly = true;
                txtape.ReadOnly = true;
                TextBoxStart.ReadOnly = true;
                TextBoxEnd.ReadOnly = true;
                Session["idcama"] = Convert.ToInt32(cboCamas.SelectedValue);
                cama = Convert.ToInt32(Session["idcama"]);
                lblerror2.Visible = false;
                ButtonOK.Enabled = false;
                txtficha.ReadOnly = true;
                lblmant.Visible = false;
                txtobsmant.Visible = false;
                //LinkButton2.Visible = false;
            }

            else if (estado == 4)
            {
                int idevento = Convert.ToInt32(Session["idevento"]);
                funcamas.cargaDetalleMant(idevento);
                txtobsmant.Text = funcamas.detmant;
                LinkButton1.Visible = false;
                lblerror2.Visible = false;
                ButtonOK.Enabled = false;
                LinkButton2.Visible = false;
            }
            else
            {
                cbotipoingre.Enabled = true;
                txtnom.ReadOnly = true;
                txtape.ReadOnly = true;
                TextBoxStart.ReadOnly = true;
                TextBoxEnd.ReadOnly = true;
                LinkButton1.Visible = false;
                lblerror2.Visible = false;
                ButtonOK.Enabled = false;
                cbotipoingre.Enabled = false;
                cbotipopac.Enabled = false;
                txtficha.ReadOnly = true;
                LinkButton2.Visible = false;
                txtobs.Text = dr["OBS"] as string;
            }

            

            if(funcamas.ocupada(cama))
            {
                lblerror2.Text = "La cama no está disponible";
                lblerror2.Visible = true;
                txtobs.Text = dr["OBS"] as string;
            }
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        int cama = Convert.ToInt32(Session["idcama"]);
        DateTime fecha = DateTime.Now;
        DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
        DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
        string hora = DateTime.Now.ToString("HH:mm");
        int tipoin = Convert.ToInt32(cbotipoingre.SelectedValue);
        int tipopac = Convert.ToInt32(cbotipopac.SelectedValue);
        string usuario = Convert.ToString(Session["rutusu"]);
        string rut2 = Convert.ToString(Session["rut"]);
        int ficha = Convert.ToInt32(txtficha.Text);

        funcamas.guardaPreIngreso(cama,rut2,18,fecha,fechaini,fechafin,hora,0,1,usuario,tipoin,tipopac,fecha,ficha,txtobs.Text);
        Modal.Close(this, "OK");
    }

    protected void btnvalidaficha_Click(object sender, EventArgs e)
    {
        int fic = Convert.ToInt32(txtficha.Text);
        funcamas.buscaxficha(fic);
        txtape.Text = funcamas.apellidos;
        txtnom.Text = funcamas.nombre;
        Session["rut"] = funcamas.rut;
        ButtonOK.Enabled = true;
    }
    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        int ficha2 = Convert.ToInt32(Session["ficha"]);
        int ideve2 = Convert.ToInt32(Session["idevento"]);
        funcamas.eliminaPreIngreso(ideve2);
        
        if (funcamas.esBloqueoPieza(ficha2))//comprueba si tiene camas bloqueadas por aislamiento o exclusiva
        {
            funcamas.eliminaBloqueo(ficha2);
        }

        Modal.Close(this, "OK");
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        funcamas.terminaReserva(Convert.ToInt32(Session["idevento"]));
        Modal.Close(this,"OK");
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {

        funcamas.updObs(Convert.ToInt32(Session["idevento"]),txtobs.Text);
        Modal.Close(this, "OK");
    }
}
