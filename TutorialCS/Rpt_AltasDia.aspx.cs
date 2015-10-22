using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Rpt_AltasDia : System.Web.UI.Page
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
            SqlCommand cmd = new SqlCommand("HOS_ALTASDIA", con);
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
        ReportViewer1.LocalReport.ReportPath = "Reportes/AltasDia.rdlc";
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        ReportViewer1.DataBind();
        ReportViewer1.Visible = true;
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string extension;

        byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType,
                       out encoding, out extension, out streamids, out warnings);

        FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("output.pdf"), FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();

        //Open exsisting pdf
        Document document = new Document(PageSize.LETTER);
        PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("output.pdf"));
        //Getting a instance of new pdf wrtiter
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
           HttpContext.Current.Server.MapPath("Print.pdf"), FileMode.Create));
        document.Open();
        PdfContentByte cb = writer.DirectContent;

        int i = 0;
        int p = 0;
        int n = reader.NumberOfPages;
        Rectangle psize = reader.GetPageSize(1);

        float width = psize.Width;
        float height = psize.Height;

        //Add Page to new document
        while (i < n)
        {
            document.NewPage();
            p++;
            i++;

            PdfImportedPage page1 = writer.GetImportedPage(reader, i);
            cb.AddTemplate(page1, 0, 0);
        }

        //Attach javascript to the document
        PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
        writer.AddJavaScript(jAction);
        document.Close();

        //Attach pdf to the iframe
        frmPrint.Attributes["src"] = "Print.pdf";
    }
}