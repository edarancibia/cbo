using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Rpt_pctesSector : System.Web.UI.Page
{
    int sector;
    Camas func = new Camas();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cbosector.DataSource = func.llenaSector();
            cbosector.DataTextField = "DESCRIP";
            cbosector.DataValueField = "COD_SEC";
            cbosector.DataBind();
        }
    }
    
    private DataTable getReport()
    {
        DataTable tabla = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_PCTES_SECTOR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("COD_SEC", sector);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla);

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
        return tabla;
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        //sector = Convert.ToInt32(Session["sector"]);
        DataTable dt = getReport();
        ReportViewer1.LocalReport.ReportPath = "Reportes/Pctes_sector.rdlc";
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        ReportViewer1.DataBind();
        ReportViewer1.Visible = true;
        
    }

    protected void cbosector_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["sector"] = Convert.ToInt32(cbosector.SelectedValue);
        sector = Convert.ToInt32(Session["sector"]);

    }
}