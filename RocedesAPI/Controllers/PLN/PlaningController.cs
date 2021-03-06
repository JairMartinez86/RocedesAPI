using Newtonsoft.Json;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.INV;
using RocedesAPI.Models.Cls.PLN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;



public class IUpload
{
    public string link = string.Empty;
    public object datos = null;

}


public class PlanningFormat
{
    public string Week = string.Empty;
    public string Linea = string.Empty;
    public string Cut = string.Empty;
    public string Style = string.Empty;
    public decimal Quant = 0;
}


public class AsignacionCorteFormat
{
    
    public string Cut = string.Empty;
    public string Style = string.Empty;
    public string Linea = string.Empty;
    public string Week = string.Empty;
    public string Location = string.Empty;
    public string Comment = string.Empty;
}

public class PlotterFormat
{
    public string Week = string.Empty;
    public string cut = string.Empty;
    public string Style = string.Empty;
    public decimal Largo = 0;
    public string Marker = string.Empty;
}

namespace RocedesAPI.Controllers.PLN
{
    public class PlaningController : ApiController
    {


        [Route("api/Pln/Planning/Get")]
        [HttpGet]
        public string Get()
        {

       
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    List<PlanningCustom> lst = (from q in _Cnx.PlanningSwing
                                               join cl in _Cnx.Cliente on q.IdCliente equals cl.Id_Cliente
                                            select new PlanningCustom
                                            {
                                                IdPlanningSwing = q.IdPlanningSwing,
                                                Week = q.Week,
                                                IdCliente = q.IdCliente,
                                                Cliente = cl.Cliente1,
                                                Modulo = q.Modulo,
                                                Linea = q.Linea,
                                                Cut_date_all_component = q.Cut_date_all_component,
                                                Ct = q.Ct,
                                                Marker = q.Marker,
                                                Largo = q.Largo,
                                                NotasEspeciales = q.NotasEspeciales,
                                                Origen_segun_wip = q.Origen_segun_wip,
                                                Cutting_plan = q.Cutting_plan,
                                                Cut = q.Cut,
                                                Style = q.Style,
                                                Cut_date_body = q.Cut_date_body,
                                                foleo_date_body = q.foleo_date_body,
                                                In_plant = q.In_plant,
                                                Quant = q.Quant,
                                                Status_cut = q.Status_cut,
                                                Status_comp = q.Status_comp,
                                                Status_cuerpo = q.Status_cuerpo,
                                                Foleo = q.Foleo,
                                                Status_envio = q.Status_envio,
                                                Fabric = q.Fabric,
                                                Pocketing = q.Pocketing,
                                                Fuse1 = q.Fuse1,
                                                Fuse2 = q.Fuse2,
                                                Cordura = q.Cordura,
                                                Quilt = q.Quilt,
                                                Dracon = q.Dracon,
                                                Linning = q.Linning,
                                                Binding1 = q.Binding1,
                                                Binding2 = q.Binding2,
                                                Sherpa = q.Sherpa,
                                                Rib = q.Rib,
                                                Price = q.Price,
                                                Total = q.Total
                                            }
                                  ).ToList();

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }


        [Route("api/Pln/Planning/SubirArchivo")]
        [HttpPost]
        public IHttpActionResult SubirArchivo(IUpload _IUload)
        {
            if (ModelState.IsValid)
            {

                return Ok(_SubirArchivo(_IUload));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _SubirArchivo(IUpload _IUload)
        {
            string json = string.Empty;


            try
            {

                PlanningSwing Planing = null;
                Cliente _Cliente = null;
                POrder _Po = null;
                Linea _Ln = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        switch(_IUload.link)
                        {
                            case "datos-planning":

                                List<PlanningFormat> _FomatoPlaning  = new List<PlanningFormat>();

                                foreach (object item in (JsonConvert.DeserializeObject<List<object>>(_IUload.datos.ToString())).ToList())
                                {
                                    string[] Datos = JsonConvert.DeserializeObject<List<string>>(item.ToString()).ToArray();

                                    _FomatoPlaning.Add(new PlanningFormat {
                                        Week = Datos[0],
                                        Linea = Datos[1],
                                        Cut = Datos[2],
                                        Style = Datos[3],
                                        Quant =  Convert.ToInt32(Datos[4]),
                                    });


                                }


                                foreach (PlanningFormat Registro in _FomatoPlaning)
                                {

                                    _Po = _Conexion.POrder.FirstOrDefault(f => f.POrder1.TrimStart().TrimEnd().Equals(Registro.Cut.TrimStart().TrimEnd()));

                                    if (_Po == null)
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"No se encuentra registrada el Corte No.  <b>{Registro.Cut}</b>.", 1);
                                        return json;
                                    }

                                    _Cliente = _Conexion.Cliente.FirstOrDefault(f => f.Id_Cliente == _Po.Id_Cliente);

                                    if(_Cliente == null)
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"No se encuentra el Cliente para el Corte No.  <b>{Registro.Cut}</b>.", 1);
                                        return json;
                                    }


                                    _Ln = _Conexion.Linea.FirstOrDefault(f => f.Linea1 == Registro.Linea);

                                    if (_Ln == null)
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"No se encuentra el Modulo para la linea  <b>{Registro.Linea}</b>.", 1);
                                        return json;
                                    }


                                    Planing = _Conexion.PlanningSwing.FirstOrDefault(f => f.Week == Registro.Week && f.IdCliente == _Cliente.Id_Cliente && f.Linea == Registro.Linea && f.Cut == Registro.Cut && f.Style == Registro.Style);

                                    if(Planing == null)
                                    {
                                        Planing = new PlanningSwing
                                        {
                                            Week = Registro.Week,
                                            IdCliente = _Cliente.Id_Cliente,
                                            Modulo = _Ln.Modulo,
                                            Linea = Registro.Linea,
                                            Cut_date_all_component = null,
                                            Ct = string.Empty,
                                            Marker = string.Empty,
                                            Largo = 0,
                                            NotasEspeciales = string.Empty,
                                            Origen_segun_wip = string.Empty,
                                            Cutting_plan = string.Empty,
                                            Cut = Registro.Cut,
                                            Style = Registro.Style,
                                            Cut_date_body = null,
                                            foleo_date_body = null,
                                            In_plant = null,
                                            Quant = Registro.Quant,
                                            Status_cut = string.Empty,
                                            Status_comp = string.Empty,
                                            Status_cuerpo = string.Empty,
                                            Foleo = string.Empty,
                                            Status_envio = string.Empty,
                                            Fabric = string.Empty,
                                            Pocketing = string.Empty,
                                            Fuse1 = string.Empty,
                                            Fuse2 = string.Empty,
                                            Cordura = string.Empty,
                                            Quilt = string.Empty,
                                            Dracon = string.Empty,
                                            Linning = string.Empty,
                                            Binding1 = string.Empty,
                                            Binding2 = string.Empty,
                                            Sherpa = string.Empty,
                                            Rib = string.Empty,
                                            Price = 0,
                                            Total = 0
                                        };

                                        _Conexion.PlanningSwing.Add(Planing);
                                    }
                                    else
                                    {
                                        Planing.Week = Registro.Week.TrimStart().TrimEnd();
                                        Planing.IdCliente = _Cliente.Id_Cliente;
                                        Planing.Linea = Registro.Linea.TrimStart().TrimEnd();
                                        Planing.Cut = Registro.Cut.TrimStart().TrimEnd();
                                        Planing.Style = Registro.Style.TrimStart().TrimEnd();
                                        Planing.Quant = Registro.Quant;

                                    }

                                }

         
                                break;



                            case "datos-asignacion-corte":


                                 List<AsignacionCorteFormat> _FomatoAsignacionCorte = new List<AsignacionCorteFormat>();

                                foreach (object item in (JsonConvert.DeserializeObject<List<object>>(_IUload.datos.ToString())).ToList())
                                {
                                    string[] Datos = JsonConvert.DeserializeObject<List<string>>(item.ToString()).ToArray();

                                    _FomatoAsignacionCorte.Add(new AsignacionCorteFormat
                                    {
                                        Cut = Datos[0],
                                        Style = Datos[1],
                                        Linea = Datos[2],
                                        Week = Datos[3],
                                        Location = Datos[4],
                                        Comment = Datos[5]
                                    });


                                }


                                foreach (AsignacionCorteFormat Registro in _FomatoAsignacionCorte)
                                {

                                    Planing = _Conexion.PlanningSwing.FirstOrDefault(f => f.Week == Registro.Week && f.Linea == Registro.Linea && f.Cut == Registro.Cut && f.Style == Registro.Style);



                                    if (Planing == null)
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"No se ha encontado el planning para el corte  <b>{string.Concat(Registro.Cut , " " , Registro.Style, " ", Registro.Linea)}</b>.", 1);
                                        return json;
                                    }

                                    switch(Registro.Location.TrimStart().TrimEnd().ToUpper())
                                    {
                                        case "ROCEDES 5":
                                            Planing.Ct = "READY";
                                            break;
                                        case "ROC5":
                                            Planing.Ct = "READY";
                                            break;
                                        case "ROC 5":
                                            Planing.Ct = "READY";
                                            break;
                                        case "ROC 05":
                                            Planing.Ct = "READY";
                                            break;
                                        case "SHORT FABRIC":
                                            Planing.Ct = "SHORT FABRIC";
                                            break;
                                        default:
                                            Planing.Ct = "ON HOLD";
                                            break;

                                    }

                                    Planing.NotasEspeciales = Registro.Comment;

                                }


                                break;

                            case "datos-plotter":

                                List<PlotterFormat> _FomatoPlotter = new List<PlotterFormat>();

                                foreach (object item in (JsonConvert.DeserializeObject<List<object>>(_IUload.datos.ToString())).ToList())
                                {
                                    string[] Datos = JsonConvert.DeserializeObject<List<string>>(item.ToString()).ToArray();

                                    _FomatoPlotter.Add(new PlotterFormat
                                    {
                                        Week = Datos[0],
                                        cut = Datos[1],
                                        Style = Datos[2],
                                        Largo = Convert.ToDecimal(Datos[3]),
                                        Marker = Datos[4]
                                    });


                                }


                                foreach (PlotterFormat Registro in _FomatoPlotter)
                                {

                                    Planing = _Conexion.PlanningSwing.FirstOrDefault(f => f.Week == Registro.Week && f.Cut == Registro.cut  && f.Style == Registro.Style);


                                    if (Planing == null)
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"No se ha encontado el planning para el corte  <b>{string.Concat(Registro.cut, " ", Registro.Style)}</b>.", 1);
                                        return json;
                                    }

                                    if (Planing.Ct != "READY" && Planing.Ct != "ON HOLD")
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"El corte <b>{string.Concat(Registro.cut, " ", Registro.Style)}</b> debe de tener un estado de <b>READY</b> Y/O <b>ON HOLD</b>.", 1);
                                        return json;
                                    }

                                    Planing.Largo = Registro.Largo;
                                    Planing.Marker = Registro.Marker;

                                }


                                break;


                        }



                        json = Cls.Cls_Mensaje.Tojson(null, 1, string.Empty, "Registro Guardado.", 0);

                        _Conexion.SaveChanges();
                        scope.Complete();
                        scope.Dispose();




                    }
                }

            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }



        [Route("api/Pln/Planning/GuardarEstadoCorte")]
        [HttpPost]
        public IHttpActionResult GuardarEstadoCorte(int IdPlanningSwing, string estado)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarEstadoCorte(IdPlanningSwing, estado));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarEstadoCorte(int IdPlanningSwing, string estado)
        {
            string json = string.Empty;
            if (estado == "NONE") estado = string.Empty;

            try
            {

               
      

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        PlanningSwing Planing  = _Conexion.PlanningSwing.FirstOrDefault(f => f.IdPlanningSwing == IdPlanningSwing);

                        if (Planing == null)
                        {
                            json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"Registro no encontrado.", 1);
                            return json;
                        }

                        Planing.Status_cut = estado;


                        json = Cls.Cls_Mensaje.Tojson(null, 1, string.Empty, "Registro Guardado.", 0);

                        _Conexion.SaveChanges();
                        scope.Complete();
                        scope.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }



    }
}