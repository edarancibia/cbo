using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class Historial : System.Web.UI.Page
{
    int ficha;
    Pinicio inicio = new Pinicio();

    protected void Page_Load(object sender, EventArgs e)
    {
        ficha = Convert.ToInt32(Session["fichah"]);
        inicio.buscaNomPac(ficha);

        if (inicio.nomPac != null)
        {
            lblpac.Text = inicio.nomPac.ToString();
            historial(ficha);
        }
        else
        {
            lblpac.Text = "No se encontró la ficha";
        }
        

    }

    private DataTable historial(int ficha)
    {
        DataTable Thistorial = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand(@"SELECT G.DESCRIPCION AS EVENTO,A.FECHA,SUBSTRING(D.DESCRIP,1,4) +' '+ LTRIM(Str(E.NRO_CA, 25))CAMA,
                                            F.NOMBRES +' '+ F.A_PAT AS USUARIO 
                                            FROM HOS_GESTIONCAMA A, SECTOR C,PIEZA D, CAMA E, PERSONAL F,HOS_ESTADOCAMAS G 
                                            WHERE A.NRO_FI=@ficha AND A.ID_CAMA=E.ID_CAMA AND E.COD_PIE=D.COD_PIE AND D.COD_SEC=C.COD_SEC AND
                                            A.ID_ESTADOCAMA=G.ID_ESTADOCAMA AND A.USUARIO=F.RUT_NUM ORDER BY A.FECHA,A.HORA", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("ficha", ficha);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Thistorial);
            GridView1.DataSource = Thistorial;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
        return Thistorial;
    }
   
}