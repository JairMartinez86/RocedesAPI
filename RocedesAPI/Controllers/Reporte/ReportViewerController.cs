using System;
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
using System.Web;
using RocedesAPI.Controllers.INV;
using System.Data;
using RocedesAPI.Models.Dset;

public class ReporteRDLC
{
    public string Rdlc = string.Empty;
    public object json = null;
}



namespace RocedesAPI.Controllers.Reporte
{
    public class ReportViewerController : ApiController, IReportController
    {
        BarcodeLib.Barcode BarCode;
        ReporteRDLC Datos = null;
        //MemoryStream _stream = null;

        [Route("api/Reporte/ReportViewer/PostReportAction")]
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


                Datos = JsonConvert.DeserializeObject<ReporteRDLC>(str_Datos);

            }




            return ReportHelper.ProcessReport(jsonResult, this);
        }

        // Get action for getting resources from the report
        [Route("api/Reporte/ReportViewer/GetResource")]
        [System.Web.Http.ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(string key, string resourcetype, bool isPrint)
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }


        // Method that will be called when initialize the report options before start processing the report
        [Route("api/Reporte/ReportViewer/OnInitReportOptions")]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {


            if(Datos != null)
            {
                reportOption.ReportModel.ProcessingMode = ProcessingMode.Local;
                reportOption.ReportModel.ReportPath = HttpContext.Current.Server.MapPath("~/Resources/" + Datos.Rdlc);

                reportOption.ReportModel.DataSources.Add(new ReportDataSource { Name = "DataSet1", Value = GetData(Datos) });


            }
           

        }




        // Method that will be called when reported is loaded


        public  DataTable GetData(ReporteRDLC d)
        {

            DataSet1 Dset = new DataSet1();
            DataTable tbl = null;
            DataRow _Row;

            MemoryStream _MS = new MemoryStream();


            switch (d.Rdlc)
            {
                case "SerialComponente.rdlc":

                    SerialBoxingCustom SerialBoxing = JsonConvert.DeserializeObject<SerialBoxingCustom>(d.json.ToString());



                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                    b.IncludeLabel = true;
                    b.BarWidth = 4;
                    Image img = b.Encode(BarcodeLib.TYPE.UPCA, SerialBoxing.Serial, Color.Black, Color.White, 300, 120);



                    img.Save(_MS, ImageFormat.Png);


                    tbl = Dset.Tables["CodbarComponente"].Clone();
                    _Row = tbl.NewRow();

                    _Row["Corte"] = SerialBoxing.Corte;
                    _Row["Estilo"] = SerialBoxing.Estilo;
                    _Row["Pieza"] = SerialBoxing.Pieza;
                    _Row["Cantidad"] = SerialBoxing.Cantidad;
                    _Row["Capaje"] = SerialBoxing.Capaje;
                    _Row["Serial"] = SerialBoxing.Serial;
                    _Row["Imagen"] = _MS.ToArray();
                    tbl.Rows.Add(_Row);


                    _MS.Dispose();

                    break;





                case "SerialSaco.rdlc":

                    SacoSerial Saco = JsonConvert.DeserializeObject<SacoSerial>(d.json.ToString());
                    tbl = Dset.Tables["SacoSerial"].Clone();
                    _Row = tbl.NewRow();



                    BarCode = new BarcodeLib.Barcode();
                    BarCode.IncludeLabel = true;
                    BarCode.BarWidth = 4;


                    Image img2 = BarCode.Encode(BarcodeLib.TYPE.CODE128, Saco.Serial, Color.Black, Color.White, 300, 120);

                    img2.Save(_MS, ImageFormat.Png);

                    _Row["Saco"] = Saco.Saco;
                    _Row["NoMesa"] = Saco.NoMesa;
                    _Row["Corte"] = Saco.Corte;
                    _Row["Serial"] = Saco.Serial;
                    _Row["Imagen"] = _MS.ToArray();
                    tbl.Rows.Add(_Row);


                    _MS.Dispose();

                    break;
            }



          


            return tbl;

            
        }

        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
          
        }
    }
}