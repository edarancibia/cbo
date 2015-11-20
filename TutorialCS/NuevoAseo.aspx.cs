using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

public partial class EditaAseo : System.Web.UI.Page
{
    
    public DateTime horafin, horaini,horaact2;
    public int tipo, perfil, idaseo, tipo2, idcama2;
    Camas funcamas = new Camas();

    private string para;
    private string asunto;
    private string cuerpo;
    private MailMessage correo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblerror.Visible = false;
            perfil = Convert.ToInt32(Session["perfil"]);

            if (!((perfil == 1) || (perfil == 4)))
            {
                lblerror.Text = "No tiene privilegios de aseo.";
                lblerror.Visible = true;
                Button1.Enabled = false;
                cbotipoaseo2.Enabled = false;
            }
            

            //llena combo camas
            cboCamas3.DataSource = funcamas.llenaCamas();
            cboCamas3.DataTextField = "CAMA";
            cboCamas3.DataValueField = "ID_CAMA";
            cboCamas3.SelectedValue = Request.QueryString["r"];
            cboCamas3.DataBind();
            cboCamas3.Enabled = false;

            //llena cbotipoaseo
            cbotipoaseo2.DataSource = funcamas.llenaTipoAseo();
            cbotipoaseo2.DataTextField = "DESCRIPCION";
            cbotipoaseo2.DataValueField = "ID_ESTADOCAMA";
            //cbotipoaseo2.SelectedValue = Convert.ToString(dr["ID_ESTADOCAMA"]);
            cbotipoaseo2.DataBind();
            //cbotipoaseo2.Enabled = false;

            Session["camaaseo"] = Convert.ToInt32(cboCamas3.SelectedValue);
            txtini2.Text = DateTime.Now.ToString("HH:mm");
            calcfinest();
        }

        txtfinest.ReadOnly = true;
        txtini2.ReadOnly = true;
        lblerror.Visible = false;
    }

    private void calcfinest()
    {
        tipo = Convert.ToInt32(cbotipoaseo2.SelectedValue);
        horaini = Convert.ToDateTime(txtini2.Text);

        if (tipo == 9)
        {
            horafin = horaini.AddMinutes(40);
            txtfinest.Text = horafin.ToString("HH:mm");
        }

        if (tipo == 10)
        {
            horafin = horaini.AddMinutes(120);
            txtfinest.Text = horafin.ToString("HH:mm");
        }

        if (tipo == 11)
        {
            horafin = horaini.AddMinutes(10);
            txtfinest.Text = horafin.ToString("HH:mm");
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime fecha = DateTime.Now;
        tipo2 = Convert.ToInt32(cbotipoaseo2.SelectedValue);
        string hora = DateTime.Now.ToString("HH:mm");
        string usuario = Convert.ToString(Session["rutusu"]);
        string camaAseo = Convert.ToString(Session["camaaseo"]);

        funcamas.CreaAseo(camaAseo,tipo2,fecha,fecha,fecha,hora,1,1,usuario);
        Modal.Close(this,"OK");
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }
}