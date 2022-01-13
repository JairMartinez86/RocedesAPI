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
    public class ProcesoTendidoController : ApiController
    {
        [Route("api/Premium/ProcesoTendido/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<FactorTendidoCustom> lst = (from p in _Cnx.ProcesosTendido
                                                     join f in _Cnx.FactorTendido on p.IdProcesoTendido equals f.IdProcesoTendido
                                                     group f  by  new { f.IdProcesoTendido, p.Descripcion, p.Orden, p.NoFactor } into grupo
          
                                                     orderby grupo.Key.Orden  ascending
                                                     select new FactorTendidoCustom()
                                                     {
                                                         IdProcesoTendido = grupo.Key.IdProcesoTendido,
                                                         Orden = grupo.Key.Orden,
                                                         Descripcion =   grupo.Key.Descripcion,
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

                    lst.ForEach(
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

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }



        [Route("api/Premium/ProcesoTendido/Guardar")]
        [HttpPost]
        public IHttpActionResult Guardar(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarFactor(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarFactor(string d)
        {
            string json = string.Empty;


  
            

            try
            {
                FactorTendidoCustom Datos = JsonConvert.DeserializeObject<FactorTendidoCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        List<FactorTendido> Registros = _Conexion.FactorTendido.Where(w => w.IdProcesoTendido == Datos.IdProcesoTendido).ToList();

                        Registros.ForEach(f =>
                        {
                            switch(f.NoFactor)
                            {
                                case 1:
                                    f.ValorFactor = (decimal)((Datos.Factor1 == null) ? 0 : Datos.Factor1);
                                    break;
                                case 2:
                                    f.ValorFactor = (decimal)((Datos.Factor2 == null) ? 0 : Datos.Factor2);
                                    break;
                                case 3:
                                    f.ValorFactor = (decimal)((Datos.Factor3 == null) ? 0 : Datos.Factor3);
                                    break;
                                case 4:
                                    f.ValorFactor = (decimal)((Datos.Factor4 == null) ? 0 : Datos.Factor4);
                                    break;
                                case 5:
                                    f.ValorFactor = (decimal)((Datos.Factor5 == null) ? 0 : Datos.Factor5);
                                    break;
                                case 6:
                                    f.ValorFactor = (decimal)((Datos.Factor6 == null) ? 0 : Datos.Factor6);
                                    break;
                                case 7:
                                    f.ValorFactor = (decimal)((Datos.Factor7 == null) ? 0 : Datos.Factor7);
                                    break;
                                case 8:
                                    f.ValorFactor = (decimal)((Datos.Factor8 == null) ? 0 : Datos.Factor8);
                                    break;
                                case 9:
                                    f.ValorFactor = (decimal)((Datos.Factor9 == null) ? 0 : Datos.Factor9);
                                    break;
                                case 10:
                                    f.ValorFactor = (decimal)((Datos.Factor10 == null) ? 0 : Datos.Factor10);
                                    break;
                                case 11:
                                    f.ValorFactor = (decimal)((Datos.Factor11 == null) ? 0 : Datos.Factor11);
                                    break;
                                case 12:
                                    f.ValorFactor = (decimal)((Datos.Factor12 == null) ? 0 : Datos.Factor12);
                                    break;
                            }
                            
                        }
                        );





                        json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);

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