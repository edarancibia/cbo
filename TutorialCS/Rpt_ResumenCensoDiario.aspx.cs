using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Configuration;

public partial class Rpt_ResumenCensoDiario : System.Web.UI.Page
{
    Camas func = new Camas();
    int area;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboarea.DataSource = func.llenaNivel();
            cboarea.DataTextField = "DESCRIPCION";
            cboarea.DataValueField = "ID_AREA";
            cboarea.DataBind();
            cboarea.Items.Insert(0, new ListItem("Seleccione", ""));
            //llenaCbo();
            ReportViewer1.Visible = false;
        }
    }

    private void llenaCbo()
    {
        cboarea.DataSource = func.llenaNivel();
        cboarea.DataTextField = "DESCRIPCION";
        cboarea.DataValueField = "ID_AREA";
        cboarea.DataBind();
        cboarea.Items.Insert(0, new ListItem("Seleccione", ""));
    }

    private DataTable getReport()
    {
        area = Convert.ToInt32(cboarea.SelectedValue);
        DataTable tabla = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_VW_RESUMENCENSO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("FECHA", Convert.ToDateTime(txtfecha.Text));
            cmd.Parameters.AddWithValue("AREA", area);

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
        DataTable dt = getReport();
        ReportViewer1.LocalReport.ReportPath = "Reportes/ResumenCensoDiario.rdlc";
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        ReportViewer1.LocalReport.Refresh();
        ReportViewer1.Visible = true;
    }
    protected void cboarea_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = getReport();
        ReportViewer1.LocalReport.ReportPath = "Reportes/ResumenCensoDiario.rdlc";
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        ReportViewer1.LocalReport.Refresh();
        ReportViewer1.Visible = true;
    }
}