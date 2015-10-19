
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Net.Mail;

public partial class Edit : Page
{
    int estadocama;
    string rut2;
    public int perfil;

    private string para;
    private string asunto;
    private string cuerpo;

    private MailMessage correo;
    Camas funcamas = new Camas();
    Ambulatorio objAmbula = new Ambulatorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            lblerror3.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (perfil == null)
            {
                lblerror3.Text = "Debe iniciar sesión";
                lblerror3.Visible = true;
            }

            DataRow dr = objAmbula.cargaEvento(Request.QueryString["id"]);

            if (dr == null)
            {
                throw new Exception("No se pudo cargar el evento.");
            }

            int tipoevento = Convert.ToInt32(dr["ID_ESTADOCAMA"]);

            if (tipoevento >= 6 && tipoevento <= 7)
            {
                noAlta();
            }
            else
            {
                if (tipoevento == 4)
                {
                    noAlta();
                }
                else
                {

                    txtini2.Text = Convert.ToDateTime(dr["FECHA_INI"]).ToShortDateString();
                    txtfin2.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                    estadocama = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
                    txtape3.Text = dr["APELLIDO"] as string;
                    txtnom3.Text = dr["NOMBRE"] as string;
                    string ultimaalta = Convert.ToString(dr["HORA"]);
                    rut2 = dr["RUT_NUM"] as string;
                    Session["camaAm"] = Convert.ToInt32(dr["ID_CAMA"]);
                    Session["tipoinAm"]=Convert.ToInt32(dr["TIPO_INGRESO"]);
                    Session["tipopacAm"] = Convert.ToInt32(dr["TIPO_PAC"]);
                    Session["fichaAm"]=Convert.ToInt32(dr["NRO_FI"]);
                    Session["tipoambu"] = Convert.ToInt32(dr["TIPO_AMBULA"]);
                    Session["camahos"] = Convert.ToInt32(dr["CAMA_HOS"]);
                    Session["rutAm"] = Convert.ToInt32(dr["RUT_NUM"]);

                    //llena combo camas
                    cboCamas3.DataSource = objAmbula.llenaCamasAmbula();
                    cboCamas3.DataTextField = "CAMA";
                    cboCamas3.DataValueField = "ID_CAMA";
                    cboCamas3.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                    cboCamas3.DataBind();

                    //llena cbo altas
                    cbotipoalta.DataSource = objAmbula.llenaAltas();
                    cbotipoalta.DataTextField = "DESCRIPCION";
                    cbotipoalta.DataValueField = "ID_ESTADOCAMA";
                    cbotipoalta.DataBind();

                    txtape3.ReadOnly = true;
                    txtnom3.ReadOnly = true;
                    txtini2.ReadOnly = true;
                    txtfin2.ReadOnly = true;
                    cboCamas3.Enabled = false;
                    txtcamahosp.ReadOnly = true;

                    lblerror3.Visible = false;
                    txthora.Text = DateTime.Now.ToString("HH:mm");

                    nomCama();
                    if (objAmbula.camaDesc != null)
                    {
                        txtcamahosp.Text = objAmbula.camaDesc.ToString();
                    }
                    else
                    {
                        txtcamahosp.Text = "";
                    }
                }
            }
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        ButtonOK.Attributes.Add("onclick", "this.disabled=true;");
        int idambula = Convert.ToInt32(Request.QueryString["id"]);
        int cama = Convert.ToInt32(Session["camaAm"]);
        DateTime fecha = DateTime.Now;
        DateTime fechaini = Convert.ToDateTime(txtini2.Text);
        DateTime fechafin = Convert.ToDateTime(txtfin2.Text);
        string hora = DateTime.Now.ToString("HH:mm");
        int tipoin = Convert.ToInt32(Session["tipoinAm"]);
        int tipopac = Convert.ToInt32(Session["tipopacAm"]);
        string usuario = Convert.ToString(Session["rutusu"]);
        string rut2 = Convert.ToString(Session["rut"]);
        int ficha = Convert.ToInt32(Session["fichaAm"]);
        int tipoambu = Convert.ToInt32(Session["tipoambu"]);
        int camahosp = Convert.ToInt32(Session["camahos"]);
        string rut = Convert.ToString(Session["rutAm"]);
        int perfil2 = Convert.ToInt32(Session["perfil"]);

        if (tipoambu == 2)
        {
            lblerror3.Text = "No puede dar de alta a un paciente Transitorio";
        }
        else
        {

            if (Convert.ToInt32(cbotipoalta.SelectedValue) == 2)
            {
                if (perfil2 == 3)
                {
                    lblerror3.Text = "No tiene privilegios para dar un alta administrativa.";
                    lblerror3.Visible = true;
                }
                else
                {
                    if (objAmbula.verificaAltaMedAmbu(cama, rut))
                    {
                        objAmbula.cierraAmbula(idambula);
                        objAmbula.guardaPreIngreso(ficha, cama, rut, 2, fecha, hora, 0, 0, fechaini, fechafin, tipoin, tipopac, usuario, tipoambu, camahosp);
                        Modal.Close(this, "OK");
                    }
                    else
                    {
                        lblerror3.Text = "El paciente necesita alta clínica antes de poder obtener el alta administrativa.";
                        lblerror3.Visible = true;
                    }

                }
            }
            else
            {
                if (perfil2 == 2)
                {
                    lblerror3.Text = "No tiene privilegios para dar un alta clínica.";
                    lblerror3.Visible = true;
                }
                else
                {
                    objAmbula.cierraAmbula(idambula);
                    objAmbula.guardaPreIngreso(ficha, cama, rut, 13, fecha, hora, 1, 1, fechaini, fechafin, tipoin, tipopac, usuario, tipoambu, camahosp);
                    Modal.Close(this, "OK");
                }
            }
        }

    }

    private bool noAlta()
    {
        lblerror3.Text = "No es posible dar de alta este paciente.";
        lblerror3.Visible = true;
        return true;
    }


    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this, "OK");
    }

    protected void btndeshacer_Click(object sender, ImageClickEventArgs e)
    {
        if (perfil == 3)
        {
            lblerror3.Text = "No tiene privilegios para realizar esta acción";
            lblerror3.Visible = true;
        }
        else
        {
            int idambula2 = Convert.ToInt32(Request.QueryString["id"]);
            int cama = Convert.ToInt32(Session["camaAm"]);
            DateTime fecha = DateTime.Now;
            DateTime fechaini = Convert.ToDateTime(txtini2.Text);
            DateTime fechafin = Convert.ToDateTime(txtfin2.Text);
            string hora = DateTime.Now.ToString("HH:mm");
            int tipoin = Convert.ToInt32(Session["tipoinAm"]);
            int tipopac = Convert.ToInt32(Session["tipopacAm"]);
            string usuario = Convert.ToString(Session["rutusu"]);
            string rut2 = Convert.ToString(Session["rut"]);
            int ficha = Convert.ToInt32(Session["fichaAm"]);
            int tipoambu = Convert.ToInt32(Session["tipoambu"]);
            int camahosp = Convert.ToInt32(Session["camahos"]);
            string rut = Convert.ToString(Session["rutAm"]);

            if (funcamas.ocupada2(camahosp))
            {
                lblerror3.Text = "La cama de aún no está disponible";
                lblerror3.Visible = true;
            }
            else
            {
                objAmbula.cierraAmbula(idambula2);
                funcamas.guardaPreIngreso(camahosp, rut, 18, fecha, fechaini, fechafin, hora, 0, 1, usuario, tipoin, tipopac, fecha, ficha);
                Modal.Close(this, "OK");
            }
        }
    }

    private void nomCama()
    {
        objAmbula.descCama(Convert.ToInt32(Session["camahos"]));
    }
}
