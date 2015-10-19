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

public partial class Rpt_PacientesActuales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Button1.Visible = false;
    }

    private DataTable getReport()
    {
        DataTable tabla = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_PACIENTES_ACTUALES", con);
            cmd.CommandType = CommandType.StoredProcedure;

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
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = getReport();
        ReportViewer1.LocalReport.ReportPath = "Reportes/PacientesActuales.rdlc";
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        ReportViewer1.DataBind();
        ReportViewer1.Visible = true;
    }
}