using Newtonsoft.Json;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.INV;
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

namespace RocedesAPI.Controllers.INV
{
    public class FlujoController : ApiController
    {
        [Route("api/Premium/Flujo/Get")]
        [HttpGet]
        public string Get(int IdFactorDetalleCorte, string Estilo)
        {
            string json = string.Empty;
            int IdFactorCorte = 0;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {



                    List<FactorTendidoCustom> lstTendido = (from p in _Cnx.ProcesosTendido
                                                     join f in _Cnx.FactorTendido on p.IdProcesoTendido equals f.IdProcesoTendido
                                                     group f by new { f.IdProcesoTendido, p.Descripcion, p.Orden, p.NoFactor } into grupo

                                                     orderby grupo.Key.Orden ascending
                                                     select new FactorTendidoCustom()
                                                     {
                                                         IdProcesoTendido = grupo.Key.IdProcesoTendido,
                                                         Orden = grupo.Key.Orden,
                                                         Descripcion = grupo.Key.Descripcion,
                                                         NoFactor = grupo.Key.NoFactor,
                                                         Factor1 = grupo.Where(c => c.NoFactor == 1).Sum(c => c.ValorFactor),
                                                         Factor2 = grupo.Where(c => c.NoFactor == 2).Sum(c => c.ValorFactor),
                                                         Factor3 = grupo.Where(c => c.NoFactor == 3).Sum(c => c.ValorFactor),
                                                         Factor4 = grupo.Where(c => c.NoFactor == 4).Sum(c => c.ValorFactor),
                                                         Factor5 = grupo.Where(c => c.NoFactor == 5).Sum(c => c.ValorFactor),
                                                         Factor6 = grupo.Where(c => c.NoFactor == 6).Sum(c => c.ValorFactor),
                                                         Factor7 = grupo.Where(c => c.NoFactor == 7).Sum(c => c.ValorFactor),
                                                         Factor8 = grupo.Where(c => c.NoFactor == 8).Sum(c => c.ValorFactor),
                                                         Factor9 = grupo.Where(c => c.NoFactor == 9).Sum(c => c.ValorFactor),
                                                         Factor10 = grupo.Where(c => c.NoFactor == 10).Sum(c => c.ValorFactor),
                                                         Factor11 = grupo.Where(c => c.NoFactor == 11).Sum(c => c.ValorFactor),
                                                         Factor12 = grupo.Where(c => c.NoFactor == 12).Sum(c => c.ValorFactor),
                                                         TotalFactor = grupo.Sum(c => c.ValorFactor),
                                                         Minutos = 0

                                                     }).ToList();

                    lstTendido.ForEach(
                        e => {
                            e.Factor1 = (e.Factor1 == 0) ? (decimal?)null : e.Factor1;
                            e.Factor2 = (e.Factor2 == 0) ? (decimal?)null : e.Factor2;
                            e.Factor3 = (e.Factor3 == 0) ? (decimal?)null : e.Factor3;
                            e.Factor4 = (e.Factor4 == 0) ? (decimal?)null : e.Factor4;
                            e.Factor5 = (e.Factor5 == 0) ? (decimal?)null : e.Factor5;
                            e.Factor6 = (e.Factor6 == 0) ? (decimal?)null : e.Factor6;
                            e.Factor7 = (e.Factor7 == 0) ? (decimal?)null : e.Factor7;
                            e.Factor8 = (e.Factor8 == 0) ? (decimal?)null : e.Factor8;
                            e.Factor9 = (e.Factor9 == 0) ? (decimal?)null : e.Factor9;
                            e.Factor10 = (e.Factor10 == 0) ? (decimal?)null : e.Factor10;
                            e.Factor11 = (e.Factor11 == 0) ? (decimal?)null : e.Factor11;
                            e.Factor12 = (e.Factor12 == 0) ? (decimal?)null : e.Factor12;
                            e.TotalFactor = (e.TotalFactor == 0) ? (decimal?)null : e.TotalFactor;
                            e.Minutos = (e.Minutos == 0) ? (decimal?)null : e.Minutos;
                        }

                        );


                    

                    List<FactorDetalleCorteCustom> lstDetalleFactor = (from d in _Cnx.FactorDetalleCorte
                                                                       where d.IdFactorDetalleCorte == IdFactorDetalleCorte
                                                                       select new FactorDetalleCorteCustom()
                                                                       {
                                                                           IdFactorDetalleCorte = d.IdFactorDetalleCorte,
                                                                           IdFactorCorte = d.IdFactorCorte,
                                                                           Item = d.Item,
                                                                           Componente = d.Componente,
                                                                           Estilo = d.Estilo,
                                                                           LayLimits = d.LayLimits,
                                                                           TotalPieces = d.TotalPieces,
                                                                           StraightPerimeter = d.StraightPerimeter,
                                                                           CurvedPerimeter = d.CurvedPerimeter,
                                                                           TotalPerimeter = d.TotalPerimeter,
                                                                           TotalNotches = d.TotalNotches,
                                                                           TotalCorners = d.TotalCorners,
                                                                           Segundos = 0,
                                                                           Minutos_Pza = 0

                                                                       }).ToList();


                    IdFactorCorte = lstDetalleFactor[0].IdFactorCorte;

                    var FactorCorte = (from f in _Cnx.FactorCorte
                                       select new
                                       {
                                           IdFactorCorte = f.IdFactorCorte,
                                           Linearecta = f.Linearecta,
                                           Curva = f.Curva,
                                           Esquinas = f.Esquinas,
                                           Piquetes = f.Piquetes,
                                           HacerOrificio = f.HacerOrificio,
                                           PonerTape = f.PonerTape
                                       }).ToList();






                    List<FoleoFactorCustom> lstFoleo = (from p in _Cnx.FoleoProceso
                                                   join f in _Cnx.FoleoFactor on p.IdProcesoFoleo equals f.IdProcesoFoleo
                                                   group f by new { f.IdProcesoFoleo, p.Proceso, p.Orden, p.NoFactor } into grupo

                                                   orderby grupo.Key.Orden ascending
                                                   select new FoleoFactorCustom()
                                                   {
                                                       IdProcesoFoleo = grupo.Key.IdProcesoFoleo,
                                                       Orden = grupo.Key.Orden,
                                                       Proceso = grupo.Key.Proceso,
                                                       NoFactor = grupo.Key.NoFactor,
                                                       Factor1 = grupo.Where(c => c.NoFactor == 1).Sum(c => c.ValorFactor),
                                                       Factor2 = grupo.Where(c => c.NoFactor == 2).Sum(c => c.ValorFactor),
                                                       Factor3 = grupo.Where(c => c.NoFactor == 3).Sum(c => c.ValorFactor),
                                                       TotalFactor = grupo.Sum(c => c.ValorFactor),
                                                       Minutos = 0

                                                   }).ToList();

                    lstFoleo.ForEach(
                        e => {
                            e.Factor1 = (e.Factor1 == 0) ? (decimal?)null : e.Factor1;
                            e.Factor2 = (e.Factor2 == 0) ? (decimal?)null : e.Factor2;
                            e.Factor3 = (e.Factor3 == 0) ? (decimal?)null : e.Factor3;
                            e.TotalFactor = (e.TotalFactor == 0) ? (decimal?)null : e.TotalFactor;
                            e.Minutos = (e.Minutos == 0) ? (decimal?)null : e.Minutos;
                        }

                        );


                    List<FoleoDatos> lstEstilos = (from q in _Cnx.FoleoDatos
                                                   where q.Estilo.ToLower().Equals(Estilo.TrimStart().TrimEnd().ToLower())
                                                   orderby q.Estilo, q.Estilo.Length
                                                   select q
                                 ).Take(1).ToList();



                    List<object> registros = new List<object>();
                    registros.Add(lstTendido);
                    registros.Add(FactorCorte.First(w => w.IdFactorCorte == IdFactorCorte));
                    registros.Add(lstDetalleFactor[0]);
                    registros.Add(lstFoleo);
                    registros.Add(lstEstilos);


                    json = Cls.Cls_Mensaje.Tojson(registros, registros.Count, string.Empty, string.Empty, 0);
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