using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

public partial class Login : System.Web.UI.Page
{
    public string ruthos;
    public int perfil;

    protected void Page_Load(object sender, EventArgs e)
    {
        txtPassword.Focus();
        Session["usuario"] = null;
        Session["perfil"] = null;
    }
    /*
    protected void Logear(object sender, EventArgs e)
    {
        if (LoginClass.autenticar(txtPassword.Text))
        {
            Session["usuario"] = txtPassword.Text;
            //Response.Redirect("~/Default.aspx");
            
        }
        else
            ErrorMessage.InnerHtml = "<b>Contraseña incorrecta...</b>";
    }*/

    protected void sesion(string password)
    {
        //comprueba que la clave exista en la tabla PER_FIL
        string sql = "select count(*) from per_fil where clave=@password";

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("password", password);

            int cant = (int)cmd.ExecuteScalar();
            con.Close();
            
            if (cant > 0)
            {
                
                //obtiene el rut del usuario desde la tabla PER_FIL
                string sql2 = "SELECT RUT_NUM FROM PER_FIL WHERE CLAVE=@password";
                con.Open();

                SqlCommand cmd2= new SqlCommand(sql2,con);
                cmd2.Parameters.AddWithValue("password",password);
                SqlDataReader dr = cmd2.ExecuteReader();

                while (dr.Read())
                {
                    ruthos = dr["RUT_NUM"].ToString();
                }

                con.Close();

                //obtiene el perfil de privilegios de usuario 
                string sql3 = "SELECT ID_PERFIL FROM HOS_USUARIO WHERE RUT_NUM=@ruthos";
                con.Open();

                SqlCommand cmd3 = new SqlCommand(sql3, con);
                cmd3.Parameters.AddWithValue("ruthos",ruthos);
                SqlDataReader dr2 = cmd3.ExecuteReader();

                while (dr2.Read())
                {
                    perfil = Convert.ToInt32(dr2["ID_PERFIL"]);
                }

                con.Close();

                Session["usuario"] = txtPassword.Text;
                Session["perfil"] = Convert.ToString(perfil);
                Session["rutusu"] = Convert.ToString(ruthos);
                Response.Redirect("~/Inicio.aspx");

                ErrorMessage.InnerHtml = perfil.ToString();
            }
            else
            {
                ErrorMessage.InnerHtml = "<br><b>Contraseña incorrecta...</b></b>";
            }
        }
    }

    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        sesion(txtPassword.Text);
    }
}