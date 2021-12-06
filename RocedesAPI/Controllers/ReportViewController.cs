using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BoldReports.Web.ReportViewer;
using Microsoft.Extensions.Caching.Memory;


using BoldReports.Web.ReportViewer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using System.Web.Hosting;
using Microsoft.Reporting.WebForms;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace RocedesAPI.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    public class ReportViewerController : Controller, IReportController
    {
        // Report viewer requires a memory cache to store the information of consecutive client request and
        // have the rendered Report Viewer information in server.
        private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        // IHostingEnvironment used with sample to get the application data from wwwroot.
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        // Post action to process the report from server based json parameters and send the result back to the client.
        public ReportViewerController(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }

        // Post action to process the report from server based json parameters and send the result back to the client.
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public object PostReportAction([Microsoft.AspNetCore.Mvc.FromBody] Dictionary<string, object> jsonArray)
        {
            //Contains helper methods that help to process a Post or Get request from the Report Viewer control and return the response to the Report Viewer control
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }

        // Method will be called to initialize the report information to load the report with ReportHelper for processing.
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string basePath = _hostingEnvironment.WebRootPath;
            // Here, we have loaded the sales-order-detail.rdl report from application the folder wwwroot\Resources. sales-order-detail.rdl should be there in wwwroot\Resources application folder.
            FileStream inputStream = new FileStream(basePath + @"\Resources\" + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
        }

        // Method will be called when reported is loaded with internally to start to layout process with ReportHelper.
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        //Get action for getting resources from the report
        [Microsoft.AspNetCore.Mvc.ActionName("GetResource")]
        [Microsoft.AspNetCore.Mvc.AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }
    }


}
