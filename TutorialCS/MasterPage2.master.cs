using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class MasterPage2 : System.Web.UI.MasterPage
{

    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString());
    SqlDataReader reader;
    SqlDataReader reader2;
    public string nomusuario;
    string rut;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            con.Open();
            string sql3 = "SELECT RUT_NUM FROM PER_FIL WHERE CLAVE=@usuario";
            SqlCommand cmd2 = new SqlCommand(sql3,con);
            cmd2.Parameters.AddWithValue("usuario",Session["usuario"]);
            reader2 = cmd2.ExecuteReader();
            //con.Close();

            if (reader2.Read())
            {
                rut = reader2["RUT_NUM"].ToString();
                con.Close();
            }
            
            string sql2 = "SELECT nombres,a_pat,a_mat FROM personal WHERE rut_num=@rut";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql2, con);
            cmd.Parameters.AddWithValue("@rut", rut);
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string nomusuario = reader["NOMBRES"].ToString() + " " + reader["A_PAT"].ToString() + " " + reader["A_MAT"].ToString();
                Label1.Text = nomusuario;
                Session["nomusuario"] = Label1.Text;
                con.Close();
            }

            LinkButton1.Text = "Cerrar sesión";
        }
        else
        {
            Label1.Visible = false;
            LinkButton1.Text = "Iniciar sesion";
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (LinkButton1.Text == "Iniciar sesion")
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }
    }
    
}
