using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


public partial class Resultado : System.Web.UI.Page
{
    string pac, cama;

    protected void Page_Load(object sender, EventArgs e)
    {
        pac = Convert.ToString(Session["buscapac"]);
        cama = Convert.ToString(Session["buscacama"]);

        if (pac != "")
        {
            lblpac.Text = pac.ToString();
            lblcama.Text = cama.ToString();
        }
        else
        {
            lblpac.Text = "No se encontraron resultados";
            lblcama.Visible = false;
        }
    }
}