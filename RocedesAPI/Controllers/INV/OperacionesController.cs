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



        #region "CODIGO GSD"

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
                            if (_Conexion.CodigoGSD.FirstOrDefault(f => f.CodigoGSD1.ToLower().Equals(Datos.CodigoGSD.ToLower()) && f.IdCodGSD != Datos.IdCodGSD) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new CodigoGSD
                            {
                                CodigoGSD1 = Datos.CodigoGSD.ToUpper(),
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

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.CodigoGSD.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.CodigoGSD1 = Datos.CodigoGSD.ToUpper();
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
        #endregion



        #region "PARTES"

        [Route("api/Inventario/Operaciones/GetPartes")]
        [HttpGet]
        public string GetPartes()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<PartesCustom> lst = (from q in _Conexion.Partes
                                                 select new PartesCustom()
                                                 {
                                                     IdParte = q.IdParte,
                                                     Nombre = q.Nombre
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


        [Route("api/Inventario/Operaciones/GuardarPartes")]
        [HttpPost]
        public IHttpActionResult GuardarPartes(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarPartes(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarPartes(string d)
        {
            string json = string.Empty;


            try
            {
                PartesCustom Datos = JsonConvert.DeserializeObject<PartesCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Partes Registro = null;

                        if (Datos.IdParte == -1)
                        {
                            if (_Conexion.Partes.FirstOrDefault(f => f.Nombre.ToLower().Equals(Datos.Nombre.ToLower()) && f.IdParte != Datos.IdParte) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre de la parte ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new Partes
                            {
                                Nombre = Datos.Nombre.ToUpper()
                            };
                            _Conexion.Partes.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdParte = Registro.IdParte;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.Partes.Find(Datos.IdParte);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Partes.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Nombre = Datos.Nombre.ToUpper();

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
        #endregion



        #region "TIPOS TELA"

        [Route("api/Inventario/Operaciones/GetTela")]
        [HttpGet]
        public string GetTela()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<TiposTelaCustom> lst = (from q in _Conexion.TipoTela
                                              select new TiposTelaCustom()
                                              {
                                                  IdTela = q.IdTela,
                                                  Nombre = q.Nombre
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


        [Route("api/Inventario/Operaciones/GuardarTela")]
        [HttpPost]
        public IHttpActionResult GuardarTela(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarTela(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarTela(string d)
        {
            string json = string.Empty;


            try
            {
                TiposTelaCustom Datos = JsonConvert.DeserializeObject<TiposTelaCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        TipoTela Registro = null;

                        if (Datos.IdTela == -1)
                        {
                            if (_Conexion.TipoTela.FirstOrDefault(f => f.Nombre.ToLower().Equals(Datos.Nombre.ToLower()) && f.IdTela != Datos.IdTela) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre de la tela ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new TipoTela
                            {
                                Nombre = Datos.Nombre.ToUpper()
                            };
                            _Conexion.TipoTela.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdTela = Registro.IdTela;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.TipoTela.Find(Datos.IdTela);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.TipoTela.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Nombre = Datos.Nombre.ToUpper();

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
        #endregion


        #region "SEWING"

        [Route("api/Inventario/Operaciones/GetSewing")]
        [HttpGet]
        public string GetSewing()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<SewingCustom> lst = (from q in _Conexion.Sewing
                                                 select new SewingCustom()
                                                 {
                                                     IdSewing = q.IdSewing,
                                                     Level = q.Level,
                                                     Code = q.Code,
                                                     Factor = q.Factor
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


        [Route("api/Inventario/Operaciones/GuardarSewing")]
        [HttpPost]
        public IHttpActionResult GuardarSewing(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarSewing(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarSewing(string d)
        {
            string json = string.Empty;


            try
            {
                SewingCustom Datos = JsonConvert.DeserializeObject<SewingCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Sewing Registro = null;

                        if (Datos.IdSewing == -1)
                        {
                            if (_Conexion.Sewing.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdSewing != Datos.IdSewing) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo de Sewing ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new Sewing
                            {
                                Level = Datos.Level,
                                Code = Datos.Code.ToUpper(),
                                Factor = Datos.Factor
                            };
                            _Conexion.Sewing.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdSewing = Registro.IdSewing;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.Sewing.Find(Datos.IdSewing);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Sewing.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Code = Datos.Code.ToUpper();
                                Registro.Level = Datos.Level;
                                Registro.Factor = Datos.Factor;

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
        #endregion
    }
}