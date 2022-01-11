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
    public class OperacionesController : ApiController
    {



        [Route("api/Inventario/Operaciones/GetCodigoGSD")]
        [HttpGet]
        public string GetCodigoGSD()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<CodigoGSDCustom> lst = (from q in _Conexion.CodigoGSD
                                                 select new CodigoGSDCustom()
                                                {
                                                    IdCodGSD = q.IdCodGSD,
                                                    CodigoGSD = q.CodigoGSD1,
                                                    Tmus = q.Tmus
                                                 }).ToList();

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }


        [Route("api/Inventario/Operaciones/GuardarCodigoGSD")]
        [HttpPost]
        public IHttpActionResult GuardarCodigoGSD(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarCodigoGSD(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarCodigoGSD(string d)
        {
            string json = string.Empty;


            try
            {
                CodigoGSDCustom Datos = JsonConvert.DeserializeObject<CodigoGSDCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        CodigoGSD Registro = null;

                        if (Datos.IdCodGSD == -1)
                        {
                            if(_Conexion.CodigoGSD.FirstOrDefault( f => f.CodigoGSD1.ToLower().Equals(Datos.CodigoGSD.ToLower()) && f.IdCodGSD != Datos.IdCodGSD) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new CodigoGSD
                            {
                                CodigoGSD1 = Datos.CodigoGSD,
                                Tmus = Datos.Tmus
                            };
                            _Conexion.CodigoGSD.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdCodGSD = Registro.IdCodGSD;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.CodigoGSD.Find(Datos.IdCodGSD);

                            if(Datos.Evento == "Eliminar")
                            {
                                _Conexion.CodigoGSD.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);
                              
                            }
                            else
                            {
                                Registro.CodigoGSD1 = Datos.CodigoGSD;
                                Registro.Tmus = Datos.Tmus;
                               
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                            }
                            
                        }


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