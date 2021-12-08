﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using Newtonsoft.Json;
using RocedesAPI.Models.Cls.INV;
using BoldReports.Writer;
using ProcessingMode = BoldReports.Web.ReportViewer.ProcessingMode;

using Syncfusion.Pdf.Parsing;
using System.Drawing.Printing;
using System.Windows.Forms;

public class SerialComponente
{
    public string Corte { get; set; }
    public string Estilo { get; set; }
    public string Pieza { get; set; }
    public int Capaje { get; set; }
    public int Cantidad { get; set; }
    public string Serial { get; set; }
    public byte[] Imagen { get; set; }

    // return "{'Corte':'MP350028 - 1','CorteCompleto':'MP350028','Estilo':'X1 VTXRDP','Pieza':'Prueba','IdPresentacionSerial':1,'IdMaterial':1,'Capaje':25,'Cantidad':1,'Serial':'35281700000','Login':'JMartinez'}";

    public static IList GetData(SerialBoxingCustom Datos)
    {
        List<SerialComponente> datas = new List<SerialComponente>();
        SerialComponente data = null;


        BarcodeLib.Barcode b = new BarcodeLib.Barcode();
        b.IncludeLabel = false;
        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
        b.ImageFormat = ImageFormat.Png;
        System.Drawing.Image img = b.Encode(BarcodeLib.TYPE.UPCA, "35281700000", Color.Black, Color.Transparent, 290, 120);
       


        MemoryStream _MS = new MemoryStream();
        img.Save(_MS, ImageFormat.Png);




        data = new SerialComponente()
        {
            Corte = Datos.Corte,
            Estilo = Datos.Estilo,
            Pieza = Datos.Pieza,
            Capaje = Datos.Cantidad,
            Cantidad = Datos.Capaje,
            Serial = Datos.Serial,
            Imagen = _MS.ToArray()
        };
        datas.Add(data);

        _MS.Dispose();


        return datas;
    }
}


namespace RocedesAPI.Controllers
{
    public class ReportViewerController : ApiController, IReportController
    {
        SerialBoxingCustom Datos = null;

        MemoryStream _stream = null;

       [Route("api/ReportViewer/PostReportAction")]
        // Post action for processing the RDL/RDLC report
        public object PostReportAction(Dictionary<string, object> jsonResult)
        {


            if (jsonResult.ContainsKey("parameters") && jsonResult["parameters"] != null)
            {
                var json = jsonResult.Where(x => x.Key == "parameters");

                var convertedDictionary = json.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()); //This converts your dictionary to have the Key and Value of type string.

                string str_Datos = convertedDictionary.ToList()[0].Value.ToString();
                str_Datos = str_Datos.Replace("[", string.Empty);
                str_Datos = str_Datos.Replace("]", string.Empty);
                str_Datos = str_Datos.TrimStart().TrimEnd();

                str_Datos = "{'Corte':'MP350028 - 1','CorteCompleto':'MP350028','Estilo':'X1 VTXRDP','Pieza':'Prueba','IdPresentacionSerial':1,'IdMaterial':1,'Capaje':25,'Cantidad':1,'Serial':'35281700000','Login':'JMartinez'}";
                Datos = JsonConvert.DeserializeObject<SerialBoxingCustom>(str_Datos);

            }




            return ReportHelper.ProcessReport(jsonResult, this);
        }

        // Get action for getting resources from the report
        [Route("api/ReportViewer/GetResource")]
        [System.Web.Http.ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(string key, string resourcetype, bool isPrint)
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }


        // Method that will be called when initialize the report options before start processing the report
        [Route("api/ReportViewer/OnInitReportOptions")]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            /* string filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Resources/SerialComponente.rdl"); ;
             // Opens the report from application Resources folder using FileStream
             FileStream reportStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
             reportOption.ReportModel.Stream = reportStream;
             reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "DataSet1", Value = ProductList.GetData() });*/

            reportOption.ReportModel.ProcessingMode = ProcessingMode.Local;
            reportOption.ReportModel.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Resources/SerialComponente.rdlc");
            reportOption.ReportModel.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = SerialComponente.GetData(Datos) });
            

            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 0;
            pg.Landscape = true;
            pg.Margins.Bottom = 0;
            pg.Margins.Left = 50;
            pg.Margins.Right = 0;

            System.Drawing.Printing.PaperSize size = new PaperSize();
            size.RawKind = (int)PaperKind.A4;
            pg.PaperSize = size;
            pg.Landscape = true;

       


            string reportPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Resources/SerialComponente.rdlc");
            ReportWriter reportWriter = new ReportWriter();
            reportWriter.ReportPath = reportPath;
            reportWriter.ReportProcessingMode = BoldReports.Writer.ProcessingMode.Local;
            reportWriter.DataSources.Clear();
            reportWriter.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = SerialComponente.GetData(Datos) });
            _stream = new MemoryStream();
            reportWriter.Save(_stream, WriterFormat.PDF);
          
        }


  

        // Method that will be called when reported is loaded
        [Route("api/ReportViewer/OnReportLoaded")]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
 
            string reportPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Resources/SerialComponente.pdf");


            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(_stream);

            //load the document in pdf document view
            Syncfusion.Windows.Forms.PdfViewer.PdfDocumentView view = new Syncfusion.Windows.Forms.PdfViewer.PdfDocumentView();
            view.Load(loadedDocument);

            //print the document using print dialog
            System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();
            dialog.Document = view.PrintDocument;
            

            PaperSize paperSize = new PaperSize("Custom", 70, 105);
            paperSize.RawKind = (int)PaperKind.Custom;
            dialog.Document.DefaultPageSettings.PaperSize = paperSize;

            dialog.Document.DefaultPageSettings.Landscape = false;
            dialog.Document.DefaultPageSettings.Margins.Top = 0;
            dialog.Document.DefaultPageSettings.Margins.Bottom = 0;
            dialog.Document.DefaultPageSettings.Margins.Left = 0;
            dialog.Document.DefaultPageSettings.Margins.Right = 0;



            /* foreach (System.Drawing.Printing.PaperSize ps in dialog.PrinterSettings.PaperSizes)
             {
                 if (ps.Kind == System.Drawing.Printing.PaperKind.A4)
                 {   //<-- replace with papersize of your choice
                     dialog.Document.DefaultPageSettings.PaperSize = paperSize;
                     dialog.Document.DefaultPageSettings.Landscape = false;
                     dialog.Document.DefaultPageSettings.Margins.Top = 0;
                     dialog.Document.DefaultPageSettings.Margins.Bottom = 0;
                     dialog.Document.DefaultPageSettings.Margins.Left = 0;
                     dialog.Document.DefaultPageSettings.Margins.Right = 0;
                     break;
                 }
             }*/
            dialog.Document.PrintPage += new PrintPageEventHandler(Document_PrintPage);
            dialog.Document.Print();
            
        }

        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            int X = (int)e.PageSettings.PrintableArea.X;
            int Y = (int)e.PageSettings.PrintableArea.Y;
            int width = (int)e.PageSettings.PrintableArea.Width - X;
            int height = (int)e.PageSettings.PrintableArea.Height - Y;

            int centerX = 0;
            int centerY = 0;

            width = 70;
            height = 105;

            PaperSize paperSize = new PaperSize("Custom", 70, 105);
            paperSize.RawKind = (int)PaperKind.Custom;
            e.PageSettings.PaperSize =  paperSize;
            e.PageSettings.Landscape = false;
            e.PageSettings.Margins.Top = 0;
            e.PageSettings.Margins.Bottom = 0;
            e.PageSettings.Margins.Left = 0;
            e.PageSettings.Margins.Right = 0;



            e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(X, Y, width, height));
            e.Graphics.DrawLine(Pens.Black, 0, 0, 0, 0);
            e.Graphics.DrawLine(Pens.Black, 0, 0, 0, 0);
        }
    }
}