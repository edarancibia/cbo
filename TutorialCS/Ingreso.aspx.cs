
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
            }

            DataRow dr = funcamas.cargaReserva(Request.QueryString["id"]);
            
            if (dr == null)
            {
                throw new Exception("La reserva no fue encontrada.");
            }

            Session["idevento"] = Convert.ToInt32(dr["ID_EVENTO"]);
            estadocama = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
            Session["estadocama"] = estadocama;

            TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
            txtape.Text = dr["APELLIDO"] as string;
            txtnom.Text = dr["NOMBRE"] as string;
            rut2 = dr["RUT_NUM"] as string;
            int cama2 = Convert.ToInt32(dr["ID_CAMA"]);

            //llena combo camas
            cboCamas.DataSource = funcamas.llenaCamas();
            cboCamas.DataTextField = "CAMA";
            cboCamas.DataValueField = "ID_CAMA";
            cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
            cboCamas.DataBind();

            TextBoxStart.ReadOnly = true;
            TextBoxEnd.ReadOnly = true;
            cboCamas.Enabled = false;
            txtape.ReadOnly = true;
            txtnom.ReadOnly = true;

            lblerror2.Visible = false;
            lblrut2.Visible = false;
            lblrut2.Text = rut2;
            txthora.Text = DateTime.Now.ToString("HH:mm");
            LinkButton1.Visible = false;
            lblobs.Visible = false;
            txtobs.Visible = false;

            //txthora.Text = DateTime.Now.ToString("HH:mm");

            if (Convert.ToDateTime(TextBoxStart.Text) > DateTime.Now)
            {
                ButtonOK.Enabled = false;
            }
            if (funcamas.ocupada(cama2))
            {
                //lblerror2.Text = "La cama no esta disponible para un ingreso.";
                //lblerror2.Visible = true;
                ButtonOK.Enabled = false;
            }

            if (funcamas.aseo(cama2))
            {
                lblerror2.Text = "La cama no esta disponible para un ingreso porque esta en aseo.";
                lblerror2.Visible = true;
                ButtonOK.Enabled = false;
            }

            if (estadocama == 18)
            {
                Session["tipoin4"] = Convert.ToInt32(dr["TIPO_INGRESO"]);
                Session["tipopac4"] = Convert.ToInt32(dr["QUIRURGICO"]);
                Session["ficha"] = Convert.ToInt32(dr["NRO_FI"]);
            }

            if (estadocama == 4)
            {
                lblobs.Visible = true;
                txtobs.Visible = true;

                int idevento = Convert.ToInt32(Session["idevento"]);
                funcamas.cargaDetalleMant(idevento);
                txtobs.Text = funcamas.detmant;
            }

            if ((estadocama == 5)||(estadocama==19)) 
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
       int quirurgico = Convert.ToInt32(Session["tipopac4"]);
       int tipoingreso = Convert.ToInt32(Session["tipoin4"]);
       int ficha = Convert.ToInt32(Session["ficha"]); 
       
       funcamas.updEstEvento(cama, estado);
       funcamas.cierraReserva(id, fechafin);
       funcamas.ingresoDirecto(cama, rut, 5, fecha, hora, fechaini, fechafin, 1, 1, usuario, tipoingreso, quirurgico,ficha);
       Modal.Close(this, "OK");
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }

    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        int id =Convert.ToInt32(Session["idevento"]);
        funcamas.eliminaReservaXpabellon(id);
        Modal.Close(this, "OK");
    }
}
