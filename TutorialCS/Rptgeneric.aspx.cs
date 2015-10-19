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

public partial class Rptgeneric : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void bntok_Click(object sender, EventArgs e)
    {
        DataTable dt = getReport();
        ReportViewer2.LocalReport.ReportPath = "Reportes/Reporte_generico.rdlc";
        ReportViewer2.LocalReport.DataSources.Clear();
        ReportViewer2.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        ReportViewer2.DataBind();
        ReportViewer2.Visible = true;
    }

    private DataTable getReport()
    {
        DataTable tabla = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_GENERICO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("tipo", DropDownList1.SelectedValue);
            cmd.Parameters.AddWithValue("fechaini", Convert.ToDateTime(txtfechaini.Text));
            cmd.Parameters.AddWithValue("fechafin", Convert.ToDateTime(txtfechafin.Text));

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
}