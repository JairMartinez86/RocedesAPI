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
    public class ProcesoCorteController : ApiController
    {

        [Route("api/Inventario/ProcesoCorte/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    var FactorCorte = (from f in  _Cnx.FactorCorte
                                       select new 
                                       {
                                           IdFactorCorte = f.IdFactorCorte,
                                           Linearecta = f.Linearecta,
                                           Curva = f.Curva,
                                           Esquinas = f.Esquinas,
                                           Piquetes = f.Piquetes,
                                           HacerOrificio = f.HacerOrificio,
                                           PonerTape = f.PonerTape
                                       }).Take(1);



                    List<FactorDetalleCorteCustom> lstDetalleFactor = (from d in _Cnx.FactorDetalleCorte
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
                                                                    Segundos = d.Segundos,
                                                                    Minutos_Pza = d.Minutos_Pza

                                                                }).ToList();

                    List<object> registros = new List<object>();
                    registros.Add(FactorCorte);
                    registros.Add(lstDetalleFactor);



                    json = Cls.Cls_Mensaje.Tojson(registros, registros.Count, string.Empty, string.Empty, 0);
                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }



        [Route("api/Inventario/ProcesoCorte/Guardar")]
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