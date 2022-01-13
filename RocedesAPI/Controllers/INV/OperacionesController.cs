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
        #region "SEWING ACCURACY"

        [Route("api/Inventario/Operaciones/GetSewingAccuracy")]
        [HttpGet]
        public string GetSewingAccuracy()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<SewingAccuracyCustom> lst = (from q in _Conexion.SewingAccuracy
                                              select new SewingAccuracyCustom()
                                              {
                                                  IdSewingAccuracy = q.IdSewingAccuracy,
                                                  Level = q.Level,
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


        [Route("api/Inventario/Operaciones/GuardarSewingAccuracy")]
        [HttpPost]
        public IHttpActionResult GuardarSewingAccuracy(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarSewingAccuracy(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarSewingAccuracy(string d)
        {
            string json = string.Empty;


            try
            {
                SewingAccuracyCustom Datos = JsonConvert.DeserializeObject<SewingAccuracyCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        SewingAccuracy Registro = null;

                        if (Datos.IdSewingAccuracy == -1)
                        {
                            if (_Conexion.SewingAccuracy.FirstOrDefault(f => f.Level.ToLower().Equals(Datos.Level.ToLower()) && f.IdSewingAccuracy != Datos.IdSewingAccuracy) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El Nivel ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new SewingAccuracy
                            {
                                Level = Datos.Level.ToUpper(),
                                Factor = Datos.Factor
                            };
                            _Conexion.SewingAccuracy.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdSewingAccuracy = Registro.IdSewingAccuracy;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.SewingAccuracy.Find(Datos.IdSewingAccuracy);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.SewingAccuracy.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Level = Datos.Level.ToUpper();
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

        #region "OUNCE"

        [Route("api/Inventario/Operaciones/GetOunce")]
        [HttpGet]
        public string GetOunce()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<ClassOunceCustom> lst = (from q in _Conexion.ClassOunce
                                              select new ClassOunceCustom()
                                              {
                                                  IdOunce = q.IdOunce,
                                                  Ounce = q.Ounce,
                                                  Category = q.Category,
                                                  FeedDog = q.FeedDog,
                                                  Caliber = q.Caliber
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


        [Route("api/Inventario/Operaciones/GuardarOunce")]
        [HttpPost]
        public IHttpActionResult GuardarOunce(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarOunce(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarOunce(string d)
        {
            string json = string.Empty;


            try
            {
                ClassOunceCustom Datos = JsonConvert.DeserializeObject<ClassOunceCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        ClassOunce Registro = null;

                        if (Datos.IdOunce == -1)
                        {
                            Registro = new ClassOunce
                            {
                                Ounce = Datos.Ounce,
                                Category = Datos.Category.ToUpper(),
                                FeedDog = Datos.FeedDog.ToUpper(),
                                Caliber = Datos.Caliber
                            };
                            _Conexion.ClassOunce.Add(Registro);
                            _Conexion.SaveChanges();


                            Datos.IdOunce = Registro.IdOunce;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.ClassOunce.Find(Datos.IdOunce);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.ClassOunce.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Ounce = Datos.Ounce;
                                Registro.Category = Datos.Category.ToUpper();
                                Registro.FeedDog = Datos.FeedDog.ToUpper();
                                Registro.Caliber = Datos.Caliber;
       

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


        #region "OUNCE"

        [Route("api/Inventario/Operaciones/GetDataMachine")]
        [HttpGet]
        public string GetDataMachine()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<MachineDataCustom> lst = (from q in _Conexion.MachineData
                                                  select new MachineDataCustom()
                                                  {
                                                      IdDataMachine = q.IdDataMachine,
                                                      Name = q.Name,
                                                      Stitch = q.Stitch,
                                                      Rpm = q.Rpm,
                                                      Delay = q.Delay,
                                                      Personal = q.Personal,
                                                      Fatigue = q.Fatigue,
                                                      Nomenclature = q.Nomenclature,
                                                      Machine = q.Machine,
                                                      Description = q.Description,
                                                      Needle = q.Needle
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


        [Route("api/Inventario/Operaciones/GuardarDataMachine")]
        [HttpPost]
        public IHttpActionResult GuardarDataMachine(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarDataMachine(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarDataMachine(string d)
        {
            string json = string.Empty;


            try
            {
                MachineDataCustom Datos = JsonConvert.DeserializeObject<MachineDataCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        MachineData Registro = null;

                        if (Datos.IdDataMachine == -1)
                        {
                            if (_Conexion.MachineData.FirstOrDefault(f => f.Stitch.ToLower().Equals(Datos.Stitch.ToLower()) && f.IdDataMachine != Datos.IdDataMachine) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El Stitch ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new MachineData
                            {
                                Name = Datos.Name.ToUpper(),
                                Stitch = Datos.Stitch.ToUpper(),
                                Rpm = Datos.Rpm,
                                Delay = Datos.Delay,
                                Personal = Datos.Personal,
                                Fatigue = Datos.Fatigue,
                                Nomenclature = Datos.Nomenclature.ToUpper(),
                                Machine = Datos.Machine.ToUpper(),
                                Description = Datos.Description.ToUpper(),
                                Needle = Datos.Needle.ToUpper()
                            };
                            _Conexion.MachineData.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdDataMachine = Registro.IdDataMachine;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.MachineData.Find(Datos.IdDataMachine);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.MachineData.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Name = Datos.Name.ToUpper();
                                Registro.Stitch = Datos.Stitch.ToUpper();
                                Registro.Rpm = Datos.Rpm;
                                Registro.Delay = Datos.Delay;
                                Registro.Personal = Datos.Personal;
                                Registro.Fatigue = Datos.Fatigue;
                                Registro.Nomenclature = Datos.Nomenclature.ToUpper();
                                Registro.Machine = Datos.Machine.ToUpper();
                                Registro.Description = Datos.Description.ToUpper();
                                Registro.Needle = Datos.Needle.ToUpper();


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