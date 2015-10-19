using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Descripción breve de LoginClass
/// </summary>
public class LoginClass
{
    public SqlDataReader lector;
    public string rut_num;

    public static bool autenticar(string password)
    {
        string sql = "SELECT COUNT(*) FROM per_fil WHERE clave=@password";

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@password", password);

            int count = Convert.ToInt32(command.ExecuteScalar());
            con.Close();

            if (count == 0)
                return false;
            else
                return true;
                
        }
    }
    /*
    public bool obtieneRut(string password)
    {
        string sql2 = "SELECT RUT_NUM FROM PER_FIL WHERE CLAVE=@password";

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ToString()))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(sql2, con);
            cmd.Parameters.AddWithValue("@password",password);
            lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                rut_num = lector["RUT_NUM"].ToString();
            }
            return rut_num;
        }
    }*/
}