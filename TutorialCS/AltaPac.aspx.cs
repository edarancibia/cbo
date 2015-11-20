
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

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            lblerror3.Visible = false;
            lblperfil.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (perfil == null)
            {
                lblerror3.Text = "Debe iniciar sesión";
                lblerror3.Visible = true;
            }

            if(perfil > 3)
            {
                lblperfil.Text = "No tiene privilegios para dar de alta.";
                lblperfil.Visible = true;
                ButtonOK.Enabled = false;
                cboalta.Enabled = false;
            }

            DataRow dr = funcamas.cargaReserva(Request.QueryString["id"]);

            if (dr == null)
            {
                throw new Exception("La reserva no fue encontrada.");
            }

            int tipoevento = Convert.ToInt32(dr["ID_ESTADOCAMA"]);

            if (tipoevento >= 6 && tipoevento <= 7)
            {
                noAlta();
                lblperfil.Visible = false;
            }
            else
            {
                if (tipoevento == 4)
                {
                    noAlta();
                    lblperfil.Visible = false;
                }
                else
                {

                    txtini2.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                    txtfin2.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                    estadocama = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
                    txtape3.Text = dr["APELLIDO"] as string;
                    txtnom3.Text = dr["NOMBRE"] as string;
                    string ultimaalta = Convert.ToString(dr["HORA"]);
                    txtultima.Text = ultimaalta;
                    rut2 = dr["RUT_NUM"] as string;
                    int cama2 = Convert.ToInt32(dr["ID_CAMA"]);
                    Session["tipoin3"]=Convert.ToInt32(dr["TIPO_INGRESO"]);
                    Session["tipopac3"] = Convert.ToInt32(dr["QUIRURGICO"]);
                    Session["ficha3"]=Convert.ToInt32(dr["NRO_FI"]);

                    //llena combo camas
                    cboCamas3.DataSource = funcamas.llenaCamas();
                    cboCamas3.DataTextField = "CAMA";
                    cboCamas3.DataValueField = "ID_CAMA";
                    cboCamas3.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                    cboCamas3.DataBind();

                    //llena combo altas
                    cboalta.DataSource = funcamas.llenaAltas();
                    cboalta.DataTextField = "DESCRIPCION";
                    cboalta.DataValueField = "ID_ESTADOCAMA";
                    cboalta.DataBind();

                    txtape3.ReadOnly = true;
                    txtnom3.ReadOnly = true;
                    txtini2.ReadOnly = true;
                    txtfin2.ReadOnly = true;
                    cboCamas3.Enabled = false;

                    lblerror3.Visible = false;
                    lblrut3.Visible = false;
                    lblrut3.Text = rut2;
                    //lblperfil.Visible = false;
                    txthora.Text = DateTime.Now.ToString("HH:mm");

                    
                }
            }
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
            ButtonOK.Attributes.Add("onclick", "this.disabled=true;");
            DateTime fechaini = Convert.ToDateTime(txtini2.Text);
            DateTime fechafin = Convert.ToDateTime(txtfin2.Text);
            DateTime fecha = DateTime.Now;
            string hora = Request["txthora"];
            string hora2 = DateTime.Now.ToString("HH:mm");
            string cama = cboCamas3.SelectedValue;
            string id = Request.QueryString["id"];
            string usuario = Convert.ToString(Session["rutusu"]);
            string rut = lblrut3.Text;
            int estadocama = Convert.ToInt32(cboalta.SelectedValue);
            int estadocama2 = 9;
            int perfil2 = Convert.ToInt32(Session["perfil"]);
            int tipoin = Convert.ToInt32(Session["tipoin3"]);
            int tipopac = Convert.ToInt32(Session["tipopac3"]);
            int ficha = Convert.ToInt32(Session["ficha3"]);
            
            string camaMail = cboCamas3.SelectedItem.ToString();
            funcamas.para = "aseocbo@gmail.com";
            funcamas.asunto = "Cama que requiere aseo";
            funcamas.cuerpo = "La cama '" + camaMail + "' requiere aseo. Gestión camas CBO.";
            //correo.IsBodyHtml = true;
        
            if (Convert.ToInt32(cboalta.SelectedValue) == 2)
            {
                if (perfil2 == 3)
                {
                    lblerror3.Text = "No tiene privilegios para dar un alta administrativa.";
                    lblerror3.Visible = true;
                }
                else
                {
                    if (funcamas.verificaAmedica(cama, rut)) //comprueba que tenga alta medica
                    {
                        if (funcamas.esBloqueoPieza(ficha))//comprueba si tiene camas bloqueadas por aislamiento o exclusiva
                        {
                            funcamas.eliminaBloqueo(ficha);
                        }
                        
                        //si es alta administrativa la cama queda en aseo
                        funcamas.updEstEvento(cama, 0);
                        cierraReserva(id, fechafin);
                        funcamas.alta(cama, rut, estadocama, fecha, hora, fechaini, fechafin, 0, 0,tipoin,tipopac, usuario,ficha);
                        funcamas.CreaAseo(cama, estadocama2, fecha,fecha,fecha, hora2, 1, 1, usuario);
                        funcamas.enviaCorreo();
                        ButtonOK.Enabled = false;
                        Modal.Close(this, "OK");
                    }
                    else
                    {
                        lblerror3.Text = "El paciente necesita alta clínica antes de poder obtener el alta administrativa.";
                        lblerror3.Visible = true;
                    }
                    
                }
            }else{
                if (perfil2 == 2)
                {
                    lblerror3.Text = "No tiene privilegios para realizar esta acción.";
                    lblerror3.Visible = true;
                }
                else if (Convert.ToInt32(cboalta.SelectedValue) == 13)
                {
                    //alta clinica
                    if (funcamas.verifdbclick(cama))
                    {
                        lblerror3.Text = "Ya se dio el alta";
                        lblerror3.Visible = true;
                    }
                    else
                    {
                        funcamas.updEstEvento(cama, 0);
                        cierraReserva(id, fechafin);
                        funcamas.alta(cama, rut, estadocama, fecha, hora, fechaini, fechafin, 1, 1, tipoin, tipopac, usuario, ficha);

                        camaMail = cboCamas3.SelectedItem.ToString();
                        funcamas.para = "admisioncbo@gmail.com";
                        funcamas.asunto = "Nueva alta clínica";
                        funcamas.cuerpo = "El paciente de la cama '" + camaMail + "' tiene el ya tiene el alta clínica. Gestión camas CBO.";
                        ButtonOK.Enabled = false;
                        funcamas.enviaCorreo();
                        Modal.Close(this, "OK");
                        Response.Redirect("~/Rtp_altaClinica.aspx");
                    }
                }
                else
                {
                    //alta probable
                    funcamas.updEstEvento(cama, 0);
                    cierraReserva(id, fechafin);
                    funcamas.alta(cama, rut, estadocama, fecha, hora, fechaini, fechafin, 1, 1, tipoin, tipopac, usuario, ficha);
                    ButtonOK.Enabled = false;
                    Modal.Close(this, "OK");
                }
            }
    }


    private void cierraReserva(string id,DateTime fechafin)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0,FECHA_FIN=@fechafin,EVENTOPAC=0 WHERE ID_EVENTO=@id",con);
            cmd.Parameters.AddWithValue("id",id);
            cmd.Parameters.AddWithValue("fechafin",fechafin);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    private bool noAlta()
    {
        lblerror3.Text = "No es posible dar de alta este paciente.";
        lblerror3.Visible = true;
        lblrut3.Visible = false;
        lblperfil.Visible = false;
        cboalta.Enabled = false;
        return true;
    }


    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this, "OK");
    }

    protected void btndeshacer_Click(object sender, ImageClickEventArgs e)
    {
        int ficha = Convert.ToInt32(Session["ficha3"]);

        if (funcamas.buscaAlta(ficha))
        {
            funcamas.cancelaAlta(ficha);
            Modal.Close(this, "OK");
        }
    }
    protected void btnimprimir_Click(object sender, ImageClickEventArgs e)
    {
        
    }
}
