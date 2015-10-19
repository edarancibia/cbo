using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

public partial class Edit : Page
{
    int estadocama,cama,perfil,estadoBloqueo;
    Camas funcamas = new Camas();
    Ambulatorio objAmbula = new Ambulatorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            
            lblerror2.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (perfil == 3)
            {
                lblerror2.Text = "No tiene privilegios para hacer un ingreso.";
                ButtonOK.Enabled = false;
                cboCamas.Enabled = false;
                lblerror2.Visible = true;
            }
            else
            {
                TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
                TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();

                //llena combo camas ambulatoria
                cboCamas.DataSource = objAmbula.llenaCamasAmbula();
                cboCamas.DataTextField = "CAMA";
                cboCamas.DataValueField = "ID_CAMA";
                cboCamas.SelectedValue = Request.QueryString["r"];
                cboCamas.DataBind();
                cboCamas.Enabled = false;

                //llena combo camas hospitalizacion
                cbocamahosp.DataSource = funcamas.llenaCamas();
                cbocamahosp.DataTextField = "CAMA";
                cbocamahosp.DataValueField = "ID_CAMA";
                cbocamahosp.DataBind();

                //llena combo tipo ingreso
                cbotipoingre.DataSource = funcamas.cboIngre();
                cbotipoingre.DataTextField = "DESCRIPCION";
                cbotipoingre.DataValueField = "ID_TIPOINGRESO";
                cbotipoingre.DataBind();
                txtnom.ReadOnly = true;
                txtape.ReadOnly = true;
                TextBoxStart.ReadOnly = true;
                TextBoxEnd.ReadOnly = true;
                Session["idcama"] = Convert.ToInt32(cboCamas.SelectedValue);
                cama = Convert.ToInt32(Session["idcama"]);
                lblerror2.Visible = false;
                ButtonOK.Enabled = false;
                txtficha.Focus();

                if (funcamas.ocupada(cama))
                {
                    lblerror2.Text = "La cama no está disponible";
                    lblerror2.Visible = true;
                }
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
        int tipoin;
        int tipopac;
        string usuario = Convert.ToString(Session["rutusu"]);
        string rut2 = Convert.ToString(Session["rut"]);
        int ficha = Convert.ToInt32(txtficha.Text);
        int tipoambu;
        int camahosp;
        int estadocama;

        if (RadioButton4.Checked == false && RadioButton5.Checked == false)
        {
            lblerror2.Text = "Seleccione tipo de paciente";
            lblerror2.Visible = true;
        }
        else
        {
            if (funcamas.existeFicha(ficha))
            {
                lblerror2.Text = "Este paciente ya esta ingresado";
                lblerror2.Visible = true;
            }
            else
            {
                if (funcamas.esxTrasladar(ficha))
                {
                    lblerror2.Text = "El paciente fue trasladado pero no se le ha asignado nueva cama. Vaya al menú 'Traslados' y luego a 'Traslados pendientes', y búsquelo en su sector de origen.";
                    lblerror2.Visible = true;
                }
                else
                {

                    if (RadioButton4.Checked == true)
                    {

                        if (cama > 155)
                        {
                            lblerror2.Text = "No puedo ingresar un paciente transitorio en una cama de procedimientos";
                            lblerror2.Visible = true;
                        }
                        else
                        {
                            tipoambu = 1;
                            estadocama = 23;
                            camahosp = 0;
                            tipoin = 0;
                            tipopac = 0;

                            funcamas.buscaPieza(camahosp);
                            int pieza = funcamas.pieza;
                            funcamas.obtieneSector(pieza);

                            string hora2 = Convert.ToString(DateTime.Now.Hour);
                            string minuto = Convert.ToString(DateTime.Now.Minute);

                            funcamas.obtieneNCama(camahosp);
                            funcamas.updhos_camaactualpaciente(ficha,funcamas.sector,pieza,funcamas.Ncama,fecha,hora2,minuto);

                            objAmbula.guardaPreIngreso(ficha, cama, rut2, estadocama, fecha, hora, 1, 1, fechaini, fechafin, tipoin, tipopac, usuario, tipoambu, camahosp);
                            Modal.Close(this, "OK");
                        }
                    }
                    else
                    {
                        if (cama < 156)
                        {
                            lblerror2.Text = "No puedo ingresar un paciente de procedimientos en una cama de transitoria";
                            lblerror2.Visible = true;
                        }
                        else
                        { 
                        
                            tipoambu = 2;
                            estadocama = 24;
                            camahosp = Convert.ToInt32(cbocamahosp.SelectedValue);
                            tipoin = Convert.ToInt32(cbotipoingre.SelectedValue);
                            tipopac=Convert.ToInt32(cbotipopac.SelectedValue);

                            funcamas.buscaPieza(camahosp);
                            int pieza = funcamas.pieza;
                            funcamas.obtieneSector(pieza);

                            string hora2 = Convert.ToString(DateTime.Now.Hour);
                            string minuto = Convert.ToString(DateTime.Now.Minute);

                            //funcamas.obtieneNCama(camahosp);
                            //funcamas.updhos_camaactualpaciente(ficha,funcamas.sector,pieza,funcamas.Ncama,fecha,hora2,minuto);

                            objAmbula.guardaPreIngreso(ficha, cama, rut2, estadocama, fecha, hora, 1, 1, fechaini, fechafin, tipoin, tipopac, usuario, tipoambu, camahosp);
                            Modal.Close(this, "OK");
                        }
                    }
                }
            }
        }
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

}
