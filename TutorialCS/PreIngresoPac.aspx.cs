
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

public partial class Edit : Page
{
    int estadocama,cama,perfil,estadoBloqueo;
    Camas funcamas = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            
            lblerror2.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 2)))
            {
              lblerror2.Text = "No tiene privilegios para hacer un ingreso.";
              ButtonOK.Enabled = false;
              cboCamas.Enabled = false;
              
            }
            

            TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();

            //llena combo camas
            cboCamas.DataSource = funcamas.llenaCamas();
            cboCamas.DataTextField = "CAMA";
            cboCamas.DataValueField = "ID_CAMA";
            cboCamas.SelectedValue = Request.QueryString["r"];
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
            txtficha.Focus();

            if(funcamas.ocupada(cama))
            {
                lblerror2.Text = "La cama no está disponible";
                lblerror2.Visible = true;
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

        if (RadioButton1.Checked || RadioButton2.Checked==true || RadioButton3.Checked==true)
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
                    lblerror2.Text = "El paciente fue trasladado pero no se le ha asignado nueva cama. Vaya al menú 'Traslados' y luego a 'Pacientes por trasladar', y búsquelo en su sector de origen.";
                    lblerror2.Visible = true;
                }
                else
                {
                    funcamas.buscaPieza(cama);
                    funcamas.ocupadasPieza(funcamas.pieza);

                    if (funcamas.camasOcupadasP > 0)
                    {
                        lblerror2.Text = "No es posible bloquear esta pieza.";
                        lblerror2.Visible = true;
                    }
                    else
                    {
                        funcamas.buscaPieza(cama);
                        int pieza = funcamas.pieza;
                        funcamas.obtieneSector(pieza);

                        string hora2 = Convert.ToString(DateTime.Now.Hour);
                        string minuto = Convert.ToString(DateTime.Now.Minute);

                        if (funcamas.tieneCamaActual(ficha))
                        {
                            funcamas.borraCamaActual(ficha);
                        }

                        funcamas.obtieneNCama(cama);
                        funcamas.updhos_camaactualpaciente(ficha, funcamas.sector, pieza, funcamas.Ncama, fecha, hora2, minuto);

                        camasPieza(funcamas.pieza);
                        funcamas.guardaPreIngreso(cama, rut2, 18, fecha, fechaini, fechafin, hora, 0, 1, usuario, tipoin, tipopac, fecha, ficha);
                        Modal.Close(this, "OK");
                    }
                }
            }

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
                    funcamas.buscaPieza(cama);
                    int pieza = funcamas.pieza;
                    funcamas.obtieneSector(pieza);
                    
                    string hora2 = Convert.ToString(DateTime.Now.Hour);
                    string minuto = Convert.ToString(DateTime.Now.Minute);

                    if (funcamas.tieneCamaActual(ficha))
                    {
                        funcamas.borraCamaActual(ficha);
                    }

                    funcamas.obtieneNCama(cama);
                    funcamas.updhos_camaactualpaciente(ficha,funcamas.sector,pieza,funcamas.Ncama,fecha,hora2,minuto);
                    funcamas.guardaPreIngreso(cama, rut2, 18, fecha, fechaini, fechafin, hora, 0, 1, usuario, tipoin, tipopac, fecha, ficha);
                    Modal.Close(this, "OK");
                }
            }
        }
    }

    //crea un evento ingreso para cada cama de la pieza selccionadada
    private void camasPieza(int pieza)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT b.DESCRIP,c.NRO_CA,c.ID_CAMA FROM HOS_GESTIONCAMA A, PIEZA B,CAMA C WHERE A.ID_CAMA=C.ID_CAMA AND C.COD_PIE=B.COD_PIE AND B.COD_PIE=@pieza and not C.ID_CAMA=@cama", con);
            cmd.Parameters.AddWithValue("pieza", pieza);
            cmd.Parameters.AddWithValue("cama",(int)Session["idcama"]);

            SqlDataReader dr = cmd.ExecuteReader();

            if (RadioButton1.Checked == true)
                estadoBloqueo = 19;

            if (RadioButton2.Checked == true)
                estadoBloqueo = 20;

            if (RadioButton3.Checked == true)
                estadoBloqueo = 21;

            while (dr.Read())
            {
                DateTime fecha = DateTime.Now;
                DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
                DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
                string hora = DateTime.Now.ToString("HH:mm");
                
                int cama2 = (int)dr["ID_CAMA"];
                funcamas.insertBloqueoPieza(cama2, Convert.ToString(Session["rut"]), estadoBloqueo, fecha, fechaini, fechafin, hora, 0, 1, Convert.ToString(Session["rutusu"]), Convert.ToInt32(txtficha.Text));

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
