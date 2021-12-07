using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Printing;
using System.Reflection;
using System.Web.Http;
using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using Newtonsoft.Json;
using RocedesAPI.Models.Cls.INV;
using System.Windows.Documents;
using BoldReports.Writer;
using ProcessingMode = BoldReports.Web.ReportViewer.ProcessingMode;
using PrintDialog = System.Windows.Controls.PrintDialog;



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
            reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "DataSet1", Value = SerialComponente.GetData(Datos) });


            string reportPath = @"~/Resources/SerialComponente.rdlc";
            ReportWriter reportWriter = new ReportWriter(reportPath);
            reportWriter.ReportProcessingMode = (BoldReports.Writer.ProcessingMode)ProcessingMode.Local;
            reportWriter.DataSources.Clear();
            reportWriter.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = SerialComponente.GetData(Datos) });
            MemoryStream stream = new MemoryStream();
            reportWriter.Save(stream, WriterFormat.PDF);

            PdfDocumentView pdfdoc = new PdfDocumentView();
            pdfdoc.Load(stream);
            var doc = pdfdoc.PrintDocument as IDocumentPaginatorSource;
            PrintDialog printDialog = new PrintDialog();
            List<string> printersList = new List<string>();
            List<string> serversList = new List<string>();
            var server = new PrintServer();
            var queues = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local });
            foreach (var queue in queues)
            {
                if (!serversList.Contains(queue.HostingPrintServer.Name))
                {
                    serversList.Add(queue.HostingPrintServer.Name);
                }
                printersList.Add(queue.FullName);
            }
            server = new PrintServer(serversList[0].ToString());
            PrintQueue printer1 = server.GetPrintQueue(printersList[2].ToString());
            printDialog.PrintQueue = printer1;
            printDialog.PrintDocument(doc.DocumentPaginator, "PDF PRINTER");



        }


  

        // Method that will be called when reported is loaded
        [Route("api/ReportViewer/OnReportLoaded")]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {

        }
    }
}