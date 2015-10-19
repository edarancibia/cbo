using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

/// <summary>
/// Descripción breve de Conexion
/// </summary>
public class Conexion
{
    public static SqlConnection con;

	public Conexion()
	{
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["cnsicbo"].ToString();
            con = new SqlConnection(connectionString);        
        }
        catch (SqlException e)
        {
            MessageBox.Show(e.Message);
        }
	}

    public void desconectar()
    {
        con.Close();
    }
}