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
                                                                    Segundos = 0,
                                                                    Minutos_Pza = 0

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


        [Route("api/Inventario/ProcesoCorte/GetAuto")]
        [HttpGet]
        public string GetAuto(string filtro)
        {

            if (filtro == null) filtro = string.Empty;

            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    var _Query = (from q in _Cnx.FactorDetalleCorte
                                  join m in _Cnx.FactorCorte on q.IdFactorCorte equals m.IdFactorCorte
                                  where string.Concat(q.Componente, " ", q.Estilo, " ", q.LayLimits).ToLower().Contains(filtro.TrimStart().TrimEnd().ToLower())
                                  group q by new { Componente = string.Concat(q.Componente, " ", q.Estilo, " ", q.LayLimits), q.IdFactorDetalleCorte }  into grupo
                                  orderby grupo.Key.Componente, grupo.Key.Componente.Length
                                  select new
                                  {
                                      Componente = grupo.Key.Componente,
                                      IdFactorDetalleCorte = grupo.Key.IdFactorDetalleCorte
                                  }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(_Query, _Query.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }




        [Route("api/Inventario/ProcesoCorte/GetDetalle")]
        [HttpGet]
        public string GetDetalle(int IdFactorDetalleCorte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    int IdFactorCorte = 0;

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




                    List<object> registros = new List<object>();
                    registros.Add(FactorCorte.First(w => w.IdFactorCorte == IdFactorCorte));
                   registros.Add(lstDetalleFactor[0]);



                    json = Cls.Cls_Mensaje.Tojson(registros, registros.Count, string.Empty, string.Empty, 0);
                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }



        [Route("api/Inventario/ProcesoCorte/GuardarFactor")]
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
                FactorCorteCustom Datos = JsonConvert.DeserializeObject<FactorCorteCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        FactorCorte Registro = _Conexion.FactorCorte.FirstOrDefault(f => f.IdFactorCorte == Datos.IdFactorCorte);
                        Registro.Linearecta = Datos.Linearecta;
                        Registro.Curva = Datos.Curva;
                        Registro.Esquinas = Datos.Esquinas;
                        Registro.Piquetes = Datos.Piquetes;
                        Registro.HacerOrificio = Datos.HacerOrificio;
                        Registro.PonerTape = Datos.PonerTape;


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



        [Route("api/Inventario/ProcesoCorte/GuardarDetalle")]
        [HttpPost]
        public IHttpActionResult GuardarDetalle(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarFactorDetalle(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarFactorDetalle(string d)
        {
            string json = string.Empty;





            try
            {
                FactorDetalleCorteCustom Datos = JsonConvert.DeserializeObject<FactorDetalleCorteCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        FactorDetalleCorte Registro;
                        if(Datos.IdFactorDetalleCorte == -1)
                        {
                            Registro = new FactorDetalleCorte();
                            Registro.IdFactorCorte = Datos.IdFactorCorte;
                            Registro.Item = Datos.Item.TrimStart().TrimEnd().ToUpper();
                            Registro.Componente = Datos.Componente.TrimStart().TrimEnd().ToUpper(); ;
                            Registro.Estilo = Datos.Estilo.TrimStart().TrimEnd().ToUpper(); ;
                            Registro.LayLimits = Datos.LayLimits.TrimStart().TrimEnd().ToUpper(); ;
                            Registro.TotalPieces = Datos.TotalPieces;
                            Registro.StraightPerimeter = Datos.StraightPerimeter;
                            Registro.CurvedPerimeter = Datos.CurvedPerimeter;
                            Registro.TotalPerimeter = Datos.TotalPerimeter;
                            Registro.TotalNotches = Datos.TotalNotches;
                            Registro.TotalCorners = Datos.TotalCorners;
                            _Conexion.FactorDetalleCorte.Add(Registro);
                            Datos.IdFactorDetalleCorte = Registro.IdFactorDetalleCorte;
                        }
                        else
                        {

                            Registro = _Conexion.FactorDetalleCorte.FirstOrDefault(f => f.IdFactorDetalleCorte == Datos.IdFactorDetalleCorte);
                            Registro.Item = Datos.Item;
                            Registro.Componente = Datos.Componente;
                            Registro.Estilo = Datos.Estilo;
                            Registro.LayLimits = Datos.LayLimits;
                            Registro.TotalPieces = Datos.TotalPieces;
                            Registro.StraightPerimeter = Datos.StraightPerimeter;
                            Registro.CurvedPerimeter = Datos.CurvedPerimeter;
                            Registro.TotalPerimeter = Datos.TotalPerimeter;
                            Registro.TotalNotches = Datos.TotalNotches;
                            Registro.TotalCorners = Datos.TotalCorners;
                        }



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






        [Route("api/Inventario/ProcesoCorte/EliminarDetalle")]
        [HttpPost]
        public IHttpActionResult EliminarDetalle(int id)
        {
            if (ModelState.IsValid)
            {

                return Ok(EliminarFactorDetalle(id));

            }
            else
            {
                return BadRequest();
            }

        }

        private string EliminarFactorDetalle(int id)
        {
            string json = string.Empty;


            try
            { 

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        FactorDetalleCorte Registro = _Conexion.FactorDetalleCorte.FirstOrDefault(f => f.IdFactorDetalleCorte == id);

                    if (Registro != null) _Conexion.FactorDetalleCorte.Remove(Registro);


                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro Guardado.", 0);

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