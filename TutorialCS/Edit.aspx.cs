
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Collections;

public partial class Edit : Page
{
    public string rut2;
    public string fechaini1, fechafin1;
    public int perfil;
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
                lblerror2.Text = "No tiene privilegios para modificar una reserva.";
                bloqueacontroles();
                lblerror2.Visible = true;
            }

            DataRow dr = funcamas.cargaReserva(Request.QueryString["id"]);

            if (dr == null)
            {
                throw new Exception("La reserva no fue encontrada.");
            }

            int tipoevento = Convert.ToInt32(dr["ID_ESTADOCAMA"]);
            Session["idreserva"] = Convert.ToInt32(dr["ID_EVENTO"]);
            Session["rutnumRes"] = Convert.ToString(dr["rut_num"]);

            if (tipoevento == 15 || tipoevento == 4)
            {
                bloqueacontroles();
            }
            else
            {

                    if (tipoevento == 5 || tipoevento == 18)
                    {
                        /*EL PACIENTE ESTA ACTUALMENTE EN SALA*/

                        TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                        TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                        rut2 = Convert.ToString(dr["RUT_NUM"]);
                        txtape.Text = dr["APELLIDO"] as string;
                        txtnom.Text = dr["NOMBRE"] as string;
                        rut2 = dr["RUT_NUM"] as string;;

                        //llena combo camas
                        cboCamas.DataSource = funcamas.llenaCamas();
                        cboCamas.DataTextField = "CAMA";
                        cboCamas.DataValueField = "ID_CAMA";
                        cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                        cboCamas.DataBind();

                        TextBoxStart.ReadOnly = true;
                        TextBoxEnd.ReadOnly = true;
                        cboCamas.Enabled = false;
                        cboestado.Enabled = false;
                        txtape.ReadOnly = true;
                        txtnom.ReadOnly = true;
                        ButtonOK.Enabled = false;
                        lblerror2.Visible = false;
                    }
                    else
                    {

                        if (Convert.ToInt32(dr["ID_ESTADOCAMA"]) >= 12 && Convert.ToInt32(dr["ID_ESTADOCAMA"]) <= 13)
                        {
                            lblerror2.Text = "Paciente con alta.";
                            lblerror2.Visible = true;
                            ButtonOK.Enabled = false;

                        }
                        else
                        {

                            TextBoxStart.Text = Convert.ToDateTime(dr["FECHAINI"]).ToShortDateString();
                            TextBoxEnd.Text = Convert.ToDateTime(dr["FECHA_FIN"]).ToShortDateString();
                            rut2 = Convert.ToString(dr["RUT_NUM"]);
                            txtape.Text = dr["APELLIDO"] as string;
                            txtnom.Text = dr["NOMBRE"] as string;
                            rut2 = dr["RUT_NUM"] as string;;

                            //llena combo camas
                            cboCamas.DataSource = funcamas.llenaCamas();
                            cboCamas.DataTextField = "CAMA";
                            cboCamas.DataValueField = "ID_CAMA";
                            cboCamas.SelectedValue = Convert.ToString(dr["ID_CAMA"]);
                            cboCamas.DataBind();

                            //llena combo estados
                            cboestado.DataSource = funcamas.llenaestadoCamas();
                            cboestado.DataTextField = "DESCRIPCION";
                            cboestado.DataValueField = "ID_ESTADOCAMA";
                            cboestado.SelectedValue = Convert.ToString(dr["ID_ESTADOCAMA"]);
                            cboestado.DataBind();
                            txtape.ReadOnly = true;
                            txtnom.ReadOnly = true;
                            lblerror2.Visible = false;
                        }
                    }
            }
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
       DateTime fecha = DateTime.Now;
       DateTime fechaini = Convert.ToDateTime(TextBoxStart.Text);
       DateTime fechafin = Convert.ToDateTime(TextBoxEnd.Text);
       int cama = Convert.ToInt32(cboCamas.SelectedValue);
       int id = Convert.ToInt32(Session["idreserva"]);
       string usuario = Convert.ToString(Session["rutusu"]);
       string hora = DateTime.Now.ToString("HH:mm");
       string rut = Convert.ToString(Session["rutnumRes"]);
       int estadoReserva = Convert.ToInt32(cboestado.SelectedValue);

       if (estadoReserva == 7 || estadoReserva == 6)
       {
           funcamas.terminaReserva(id);
           funcamas.confirma(cama, rut, estadoReserva, fecha, hora, fechaini, fechafin, 0, 1, usuario,0);
           Modal.Close(this, "OK");
       }
       else if (estadoReserva == 8)
       {
           funcamas.terminaReserva(id);
           Modal.Close(this, "OK");
       }
       
    }

    private bool bloqueacontroles()
    {
        txtape.ReadOnly = true;
        txtnom.ReadOnly = true;
        cboCamas.Enabled = false;
        cboestado.Enabled = false;
        ButtonOK.Enabled = false;
        TextBoxStart.ReadOnly = true;
        TextBoxEnd.ReadOnly = true;
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
}
