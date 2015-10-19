
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

public partial class Edit : Page
{
    public int estadocama, perfil, cama2, idcama,sector,nuevo_sector;
    string rut2;
    SqlDataReader rd;
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

            DataRow dr = funcamas.cargaTraslado(Request.QueryString["id"]);
            
            if (dr == null)
            {
                throw new Exception("La reserva no fue encontrada.");
            }

            TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(dr["FECHAFIN"]).ToShortDateString();
            //estadocama = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
            txtape.Text = dr["APELLIDO"] as string;
            txtnom.Text = dr["NOMBRE"] as string;
            rut2 = dr["RUT_NUM"] as string;
            nuevo_sector = Convert.ToInt32(dr["NUEVO_SECTOR"]);
            Session["tipoingreso2"] = Convert.ToInt32(dr["TIPOINGRESO"]);
            Session["tipopac2"] = Convert.ToInt32(dr["TIPOPAC"]);
            Session["ficha3"] = Convert.ToInt32(dr["NRO_FI"]);
            txtobs.Text=Convert.ToString(dr["OBS"]);

            //llena combo camas
            cboCamas.DataSource = funcamas.llenaCamasNsec(nuevo_sector);
            cboCamas.DataTextField = "CAMA";
            cboCamas.DataValueField = "ID_CAMA";
            cboCamas.DataBind();

            txtfecha.Text = DateTime.Now.ToString("dd/mm/yyyy");
            txthora.Text = DateTime.Now.ToString("HH:mm");
            TextBoxStart.ReadOnly = true;
            TextBoxEnd.ReadOnly = true;
            txtape.ReadOnly = true;
            txtnom.ReadOnly = true;

            lblerror2.Visible = false;
            lblrut2.Visible = false;
            lblrut2.Text = rut2;

            if (Convert.ToInt32(dr["NUEVO_SECTOR"]) == 13)
            {
                ButtonOK.Enabled = false;
                lblerror2.Text = "Para ingresar a este paciente vaya al menu de Ingresos.";
                lblerror2.Visible = true;
            }
  
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {

       DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
       DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
       DateTime fecha = Convert.ToDateTime(txtfecha.Text);
       string hora = txthora.Text;
       string cama = cboCamas.SelectedValue;
       string id = Request.QueryString["id"];
       string usuario = Convert.ToString(Session["rutusu"]);
       string rut = lblrut2.Text;
       int estado = 0;
       int tipoingreso = Convert.ToInt32(Session["tipoingreso2"]);
       int quirurgico = Convert.ToInt32(Session["tipopac2"]);
       cama2 = Convert.ToInt32(cboCamas.SelectedValue);
       DateTime fechahora = DateTime.Now;
       int ficha = Convert.ToInt32(Session["ficha3"]); 
        
       if ((funcamas.ocupada2(cama2))||(funcamas.compruebaReserva(cama2)))
       {
           lblerror2.Text = "La cama no esta disponible para un ingreso porque esta ocupada por otro paciente.";
           lblerror2.Visible = true;
       }

       else if (funcamas.aseo(cama2))
       {
           lblerror2.Text = "La cama no esta disponible para un ingreso porque esta en aseo.";
           lblerror2.Visible = true;
       }
       else
       {

           funcamas.buscaPieza(cama2);
           int pieza = funcamas.pieza;
           funcamas.obtieneSector(pieza);

           string hora2 = Convert.ToString(DateTime.Now.Hour);
           string minuto = Convert.ToString(DateTime.Now.Minute);

           funcamas.obtieneNCama(cama2);
           funcamas.updhos_camaactualpaciente2(ficha, funcamas.sector, pieza, funcamas.Ncama, fecha, hora2, minuto);

           funcamas.updEstEvento(cama, estado);
           funcamas.cierraTraslado(id);
           funcamas.ingresoTraslado(cama, rut, 5, fecha, hora, fechaini, fechafin, 1, 1, usuario, tipoingreso,quirurgico,fechahora,ficha);
           Modal.Close(this, "OK");      
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
