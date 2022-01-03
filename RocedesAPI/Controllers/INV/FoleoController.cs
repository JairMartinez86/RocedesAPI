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
    public class FoleoController : ApiController
    {
        [Route("api/Inventario/Foleo/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<FoleoFactorCustom> lst = (from p in _Cnx.FoleoProceso
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

                    lst.ForEach(
                        e => {
                            e.Factor1 = (e.Factor1 == 0) ? (decimal?)null : e.Factor1;
                            e.Factor2 = (e.Factor2 == 0) ? (decimal?)null : e.Factor2;
                            e.Factor3 = (e.Factor3 == 0) ? (decimal?)null : e.Factor3;
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




        [Route("api/Inventario/Foleo/Guardar")]
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
                FoleoFactorCustom Datos = JsonConvert.DeserializeObject<FoleoFactorCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        List<FoleoFactor> Registros = _Conexion.FoleoFactor.Where(w => w.IdProcesoFoleo == Datos.IdProcesoFoleo).ToList();

                        Registros.ForEach(f =>
                        {
                            switch (f.NoFactor)
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