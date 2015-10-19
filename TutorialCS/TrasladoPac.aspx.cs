using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Net.Mail;
using System.Threading;
using System.Web.UI.WebControls;

public partial class Edit : System.Web.UI.Page
{
    public string rut2;
    int cama;
    public int tipoingreso, perfil;

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
            lblperfil.Visible = false;
            lblerror2.Visible = false;

            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 3)))
            {
                lblperfil.Text = "No tiene privilegios para hacer un traslado.";
                bloqueacontroles();
                //lblperfil.Visible = false;
                lblerror2.Visible = false;
                lblrut.Visible = false;
                ButtonOK.Enabled = false;
            }

            DataRow dr = funcamas.cargaReserva(Request.QueryString["id"]);
            //cama = Request.QueryString["ID_CAMA"];

            if (dr == null)
            {
                throw new Exception("La reserva no fue encontrada.");
            }

            int tipoevento = Convert.ToInt32(dr["ID_ESTADOCAMA"]);

                if (tipoevento == 5 || tipoevento==22) /*Si el paciente esta actualente en la cama*/
                {

                    TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                    TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                    rut2 = Convert.ToString(dr["RUT_NUM"]);
                    txtape.Text = dr["APELLIDO"] as string;
                    txtnom.Text = dr["NOMBRE"] as string;
                    rut2 = dr["RUT_NUM"] as string;
                    lblrut.Text = rut2;
                    tipoingreso = Convert.ToInt32(dr["TIPO_INGRESO"]);
                    Session["tipoingreso"] = tipoingreso;
                    Session["tipopac"] = Convert.ToInt32(dr["QUIRURGICO"]);
                    Session["ficha2"] = Convert.ToInt32(dr["NRO_FI"]);

                    //llena combo camas
                    cboCamas.DataSource = funcamas.llenaCamas();
                    cboCamas.DataTextField = "CAMA";
                    cboCamas.DataValueField = "ID_CAMA";
                    cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                    cboCamas.DataBind();

                    //llena combo cama destino
                    cbocamas2.DataSource = funcamas.llenaSectorTras();
                    cbocamas2.DataTextField = "DESCRIP";
                    cbocamas2.DataValueField = "COD_SEC";
                    cbocamas2.DataBind();

                    TextBoxStart.ReadOnly = true;
                    TextBoxEnd.ReadOnly = true;
                    txtape.ReadOnly = true;
                    txtnom.ReadOnly = true;
                    lblerror2.Visible = false;
                    lblrut.Visible = false;
                    lblperfil.Visible = false;
                    lblrut.Visible = false;;
                    cboCamas.Enabled = false;

                    txthora.Text = DateTime.Now.ToString("HH:mm");
                    cama = Convert.ToInt32(dr["ID_CAMA"]);
                      
                }
                else
                {
                    /*si el paciente tiene alta puede trasladarse*/
                    if (Convert.ToInt32(dr["ID_ESTADOCAMA"]) >= 12 && Convert.ToInt32(dr["ID_ESTADOCAMA"]) <= 13)
                    {
                        TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                        TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                        rut2 = Convert.ToString(dr["RUT_NUM"]);
                        txtape.Text = dr["APELLIDO"] as string;
                        txtnom.Text = dr["NOMBRE"] as string;
                        rut2 = dr["RUT_NUM"] as string;
                        lblrut.Text = rut2;
                        tipoingreso = Convert.ToInt32(dr["TIPO_INGRESO"]);
                        Session["tipoingreso"] = tipoingreso;
                        Session["tipopac"] = Convert.ToInt32(dr["QUIRURGICO"]);
                        Session["ficha2"] = Convert.ToInt32(dr["NRO_FI"]);

                        //llena combo camas
                        cboCamas.DataSource = funcamas.llenaCamas();
                        cboCamas.DataTextField = "CAMA";
                        cboCamas.DataValueField = "ID_CAMA";
                        cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                        cboCamas.DataBind();

                        //llena combo sector destino
                        cbocamas2.DataSource = funcamas.llenaSectorTras();
                        cbocamas2.DataTextField = "DESCRIP";
                        cbocamas2.DataValueField = "COD_SEC";
                        cbocamas2.DataBind();

                        txtape.ReadOnly = true;
                        txtnom.ReadOnly = true;
                        lblerror2.Visible = false;
                        lblrut.Visible = false;
                        lblperfil.Visible = false;
                        cboCamas.Enabled = false;
                        txthora.Text = DateTime.Now.ToString("HH:mm");
                    }
                    else
                    {
                        /*no esta ingresado en sala*/
                        bloqueacontroles();
                        lblerror2.Text = "NO PUEDE TRASLADAR ESTE PACIENTE.";
                        lblperfil.Visible = false;
                        ButtonOK.Enabled = false;
                    }
                }
                lblperfil.Visible = false;
                lblrut.Visible = false;
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        DateTime fecha = DateTime.Now;
        DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
        DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
        DateTime fechahora = DateTime.Now;    
        int cama2 =Convert.ToInt32(cboCamas.SelectedValue);
        string id = Request.QueryString["id"];
        string rut = lblrut.Text;
        string usuario = Convert.ToString(Session["rutusu"]);
        string hora = Convert.ToString(Request["txthora"]);
        int n_sector = Convert.ToInt32(cbocamas2.SelectedValue);
        int tipopac = Convert.ToInt32(Request.QueryString["QUIRURGICO"]);
        string obs = txtobs.Text;
        string horaaseo = DateTime.Now.ToString("HH:mm");   
        //int sec_ori=Convert.ToInt32(Request.QueryString["COD_SEC"]);
        int pie_ori = Convert.ToInt32(Request.QueryString["NRO_PIE"]);
        int tipoin = Convert.ToInt32(Session["tipoingreso"]);
        int tipopac2 = Convert.ToInt32(Session["tipopac"]);
        int ficha = Convert.ToInt32(Session["ficha2"]);

        string camaMail = cboCamas.SelectedItem.ToString();
        funcamas.para = "aseocbo@gmail.com";
        funcamas.asunto = "Cama que requiere aseo";
        funcamas.cuerpo = "La cama '" + camaMail + "' requiere aseo. Gestión camas CBO.";

        funcamas.obtieneSecOrigen(cama2);
        int sec_ori = funcamas.SecOri;

        if (n_sector == 14)
        {
            funcamas.terminaCamaActual(id, cama, 0, 0);
            updEstEvento(cama, 0, 0);
            funcamas.CreaAseoTras(cama2, 11, fecha, fecha, fecha, hora, usuario, 1, 1);
            funcamas.traslado(sec_ori, pie_ori, cama2, rut, fecha, hora, n_sector, 0, usuario, obs, fechahora, fechaini, fechafin, tipoin, tipopac2, ficha);
            funcamas.enviaCorreo();
            ButtonOK.Enabled = false;
            Modal.Close(this, "OK");

        }
        else
        {
            if (funcamas.esBloqueoPieza(ficha))
            {
                funcamas.eliminaBloqueo(ficha);
            }

            funcamas.terminaCamaActual(id, cama, 0, 0);
            updEstEvento(cama, 0, 0);
            funcamas.CreaAseoTras(cama2, 11, fecha, fecha, fecha, hora, usuario, 1, 1);
            funcamas.traslado(sec_ori, pie_ori, cama2, rut, fecha, hora, n_sector, 1, usuario, obs, fechahora, fechaini, fechafin, tipoin, tipopac2, ficha);
            funcamas.enviaCorreo();
            ButtonOK.Enabled = false;
            Modal.Close(this, "OK");
        }
    }

    //coloca en 0 los estados anteriores de todos los eventos de la cama seleccionada
    private void updEstEvento(int cama, int estado,int eventopac)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE HOS_GESTIONCAMA SET ESTADO=0,EVENTOPAC=0 WHERE ID_CAMA=@cama AND ESTADO=1 and eventopac=1", con);
            cmd.Parameters.AddWithValue("estado", 0);
            cmd.Parameters.AddWithValue("cama", cama);
            cmd.Parameters.AddWithValue("eventopac",eventopac);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    private bool esAseo(int cama)
    {
        string sql = "SELECT * FROM HOS_GESTIONCAMA WHERE ID_CAMA=@cama AND ID_ESTADOCAMA BETWEEN 9 AND 11 AND ESTADO=1";
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("cama", cama);

            con.Open();

            int cont = Convert.ToInt32(cmd.ExecuteScalar());

            if (cont == 0)
                return false;
            else
                return true;
        }
    }

    private bool bloqueacontroles()
    {
        TextBoxStart.ReadOnly = true;
        TextBoxEnd.ReadOnly = true;
        txtape.ReadOnly = true;
        txtnom.ReadOnly = true;
        //cboCamas.Enabled = false;
        //ButtonOK.Enabled = false;
        return true;
    }

    protected void ButtonCancel_Click(object sender, ImageClickEventArgs e)
    {
        Modal.Close(this, "OK");
    }

    protected void btnpab_Click(object sender, ImageClickEventArgs e)
    {
        int ficha = Convert.ToInt32(Session["ficha2"]);
        DateTime fecha = DateTime.Now;

        if (funcamas.esPabellon(ficha))
        {
            lblerror2.Text = "El paciente ya está en pabellón";
            lblerror2.Visible = true;
        }
        else
        {
            if (funcamas.esConfirmado(ficha))
            {
                lblerror2.Text = "Primero debe confirmar el ingreso";
                lblerror2.Visible = true;
            }
            else
            {
                
                DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
                DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
                int cama2 = Convert.ToInt32(cboCamas.SelectedValue);
                int id = Convert.ToInt32(Request.QueryString["id"]);
                string rut = lblrut.Text;
                string usuario = Convert.ToString(Session["rutusu"]);
                string hora = Convert.ToString(Request["txthora"]);
                int tipoin = Convert.ToInt32(Session["tipoingreso"]);
                int tipopac2 = Convert.ToInt32(Session["tipopac"]);

                funcamas.terminaReserva(id);
                funcamas.creaReserva(cama2, rut, 22, fecha, hora, fechaini, fechafin, 1, 1, tipoin, tipopac2, usuario, ficha);
                Modal.Close(this, "OK");
            }
        }
    }
    protected void btnpab2_Click(object sender, ImageClickEventArgs e)
    {
        int ficha = Convert.ToInt32(Session["ficha2"]);

        if (funcamas.esPabellon(ficha))
        {
            DateTime fecha = DateTime.Now;
            DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
            DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
            int cama2 = Convert.ToInt32(cboCamas.SelectedValue);
            int id = Convert.ToInt32(Request.QueryString["id"]);
            string rut = lblrut.Text;
            string usuario = Convert.ToString(Session["rutusu"]);
            string hora = Convert.ToString(Request["txthora"]);
            int tipoin = Convert.ToInt32(Session["tipoingreso"]);
            int tipopac2 = Convert.ToInt32(Session["tipopac"]);

            funcamas.terminaReserva(id);
            funcamas.creaReserva(cama2, rut, 5, fecha, hora, fechaini, fechafin, 1, 1, tipoin, tipopac2, usuario, ficha);
            Modal.Close(this, "OK");

        }
        else
        {

            lblerror2.Text = "El paciente no ha salido a pabellón";
            lblerror2.Visible = true;
        }
    }
}
