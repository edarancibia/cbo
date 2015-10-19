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
public partial class Rpt_cd_uce : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = getReport();
            DataTable dt2 = getReport2();
            DataTable dt3 = getReport3();
            DataTable dt4 = getReport4();
            DataTable dt5 = getReport5();
            DataTable dt6 = getReport6();
            DataTable dt7 = getReport7();
            DataTable dt8 = getReport8();
            DataTable dt9 = getReport9();

            ReportViewer1.LocalReport.ReportPath = "Reportes/RCD_UCE.rdlc";
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dt2));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", dt3));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet4", dt4));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet5", dt5));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet6", dt6));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet7", dt7));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet8", dt8));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet9", dt9));
            //ReportViewer1.DataBind();
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Visible = true;
        }
    }

    private DataTable getReport()
    {
        DataTable tabla = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_EXISTENCIA", con);
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

    private DataTable getReport2()
    {
        DataTable tabla2 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_URGENCIA", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla2);

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
        return tabla2;
    }

    private DataTable getReport3()
    {
        DataTable tabla3 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_FUERA", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla3);

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
        return tabla3;
    }

    private DataTable getReport4()
    {
        DataTable tabla4 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_TRASLADODE", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla4);

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
        return tabla4;
    }

    private DataTable getReport5()
    {
        DataTable tabla5 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_TRASLADOA", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla5);

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
        return tabla5;
    }

    private DataTable getReport6()
    {
        DataTable tabla6 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_VIVOS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla6);

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
        return tabla6;
    }

    private DataTable getReport7()
    {
        DataTable tabla7 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_FALLECIDOS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla7);

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
        return tabla7;
    }

    private DataTable getReport8()
    {
        DataTable tabla8 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_MISMODIA", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla8);

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
        return tabla8;
    }

    private DataTable getReport9()
    {
        DataTable tabla9 = new DataTable();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cnsicbo"].ConnectionString);

        try
        {
            SqlCommand cmd = new SqlCommand("HOS_RPT_CD_UCE_DISPONIBLES", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tabla9);

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
        return tabla9;
    }
}