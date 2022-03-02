using Newtonsoft.Json;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.INV;
using RocedesAPI.Models.Cls.PRM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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

        [Route("api/Premium/Operaciones/GetCodigoGSD")]
        [HttpGet]
        public string GetCodigoGSD(string codigo)
        {
            string json = string.Empty;
            if (codigo == null) codigo = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<CodigoGSDCustom> lst = (from q in _Conexion.CodigoGSD
                                                 where q.CodigoGSD1 == ((codigo == string.Empty) ? q.CodigoGSD1 : codigo)
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


        [Route("api/Premium/Operaciones/GuardarCodigoGSD")]
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

        #region "FAMILY"

        [Route("api/Premium/Operaciones/GetFamily")]
        [HttpGet]
        public string GetFamily(string Components)
        {
            string json = string.Empty;
            if (Components == null) Components = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<FamilyCustom> lst = (from q in _Conexion.Family
                                                 where q.Components == ((Components == string.Empty) ? q.Components : Components)
                                                 select new FamilyCustom()
                                                 {
                                                     IdFamily = q.IdFamily,
                                                     Components = q.Components,
                                                     Product = q.Product,
                                                     Code = q.Code
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


        [Route("api/Premium/Operaciones/GuardarFamily")]
        [HttpPost]
        public IHttpActionResult GuardarFamily(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarFamily(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarFamily(string d)
        {
            string json = string.Empty;


            try
            {
                FamilyCustom Datos = JsonConvert.DeserializeObject<FamilyCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Family Registro = null;

                        if (Datos.IdFamily == -1)
                        {
                            if (_Conexion.Family.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new Family
                            {
                                Components = Datos.Components.ToUpper(),
                                Product = Datos.Product.ToUpper(),
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.Family.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdFamily = Registro.IdFamily;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {

                           
                            Registro = _Conexion.Family.Find(Datos.IdFamily);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Family.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.Family.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdFamily != Datos.IdFamily) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                    return json;
                                }
                                Registro.Components = Datos.Components.ToUpper();
                                Registro.Product = Datos.Product.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();

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

        #region "SECUENCE"

        [Route("api/Premium/Operaciones/GetSecuence")]
        [HttpGet]
        public string GetSecuence(int Secuence)
        {
            string json = string.Empty;
    

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<SecuenceCustom> lst = (from q in _Conexion.Secuence
                                              where q.Secuence1 == ((Secuence <= -1) ? q.Secuence1 : Secuence)
                                              select new SecuenceCustom()
                                              {
                                                   IdSecuence = q.IdSecuence,
                                                  Secuence = q.Secuence1,
                                                  Code = q.Code
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


        [Route("api/Premium/Operaciones/GuardarSecuence")]
        [HttpPost]
        public IHttpActionResult GuardarSecuence(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarSecuence(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarSecuence(string d)
        {
            string json = string.Empty;


            try
            {
                SecuenceCustom Datos = JsonConvert.DeserializeObject<SecuenceCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Secuence Registro = null;

                        if (Datos.IdSecuence == -1)
                        {
                            if (_Conexion.Secuence.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new Secuence
                            {
                                Secuence1 = Datos.Secuence,
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.Secuence.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdSecuence = Registro.IdSecuence;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {

                           
                            Registro = _Conexion.Secuence.Find(Datos.IdSecuence);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Secuence.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.Secuence.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdSecuence != Datos.IdSecuence) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                    return json;
                                }
                                Registro.Secuence1 = Datos.Secuence;
                                Registro.Code = Datos.Code.ToUpper();

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


        #region "DATA MACHINE"

        [Route("api/Premium/Operaciones/GetDataMachine")]
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
                                                       Delay = q.Delay,
                                                       Personal = q.Personal,
                                                       Fatigue = q.Fatigue,
                                                       Nomenclature = q.Nomenclature,
                                                       Machine = q.Machine,
                                                       Description = q.Description,
                                                       Code = q.Code
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


        [Route("api/Premium/Operaciones/GetDataMachineAuto")]
        [HttpGet]
        public string GetDataMachineAuto(string nombre)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<MachineDataCustom> lst = (from q in _Conexion.MachineData
                                                   where q.Name.ToLower().StartsWith(nombre.TrimEnd().ToLower())
                                                   orderby q.Name, q.Name.Length
                                                   select new MachineDataCustom()
                                                   {
                                                       IdDataMachine = q.IdDataMachine,
                                                       Name = string.Concat(q.Name, " ", q.Machine, q.Description),
                                                       Delay = q.Delay,
                                                       Personal = q.Personal,
                                                       Fatigue = q.Fatigue,
                                                       Nomenclature = q.Nomenclature,
                                                       Machine = q.Machine,
                                                       Description = q.Description,
                                                       Code = q.Code
                                                   }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }


        [Route("api/Premium/Operaciones/GuardarDataMachine")]
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
                            if (_Conexion.MachineData.FirstOrDefault(f => f.Name.ToLower().Equals(Datos.Name.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new MachineData
                            {
                                Name = Datos.Name.ToUpper(),
                                Delay = Datos.Delay,
                                Personal = Datos.Personal,
                                Fatigue = Datos.Fatigue,
                                Nomenclature = Datos.Nomenclature.ToUpper(),
                                Machine = Datos.Machine.ToUpper(),
                                Description = Datos.Description.ToUpper(),
                                Code = Datos.Code.ToUpper()
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
                                if (_Conexion.MachineData.FirstOrDefault(f => f.Name.ToLower().Equals(Datos.Name.ToLower()) && f.IdDataMachine != Datos.IdDataMachine) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                if (_Conexion.MachineData.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdDataMachine != Datos.IdDataMachine) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.Name = Datos.Name.ToUpper();
                                Registro.Delay = Datos.Delay;
                                Registro.Personal = Datos.Personal;
                                Registro.Fatigue = Datos.Fatigue;
                                Registro.Nomenclature = Datos.Nomenclature.ToUpper();
                                Registro.Machine = Datos.Machine.ToUpper();
                                Registro.Description = Datos.Description.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();


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

        #region "STITCH TYPE"

        [Route("api/Premium/Operaciones/GetStitchType")]
        [HttpGet]
        public string GetStitchType(string TypeStitch)
        {
            string json = string.Empty;
            if (TypeStitch == null) TypeStitch = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<StitchTypeCustom> lst = (from q in _Conexion.StichTypeCatalogue
                                                where q.TypeStitch == ((TypeStitch == string.Empty) ? q.TypeStitch : TypeStitch)
                                                select new StitchTypeCustom()
                                                {
                                                    IdStitchType = q.IdStitchType,
                                                    TypeStitch = q.TypeStitch,
                                                    Code = q.Code
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


        [Route("api/Premium/Operaciones/GuardarStitchType")]
        [HttpPost]
        public IHttpActionResult GuardarStitchType(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarStitchType(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarStitchType(string d)
        {
            string json = string.Empty;


            try
            {
                StitchTypeCustom Datos = JsonConvert.DeserializeObject<StitchTypeCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        StichTypeCatalogue Registro = null;

                        if (Datos.IdStitchType == -1)
                        {
                            if (_Conexion.StichTypeCatalogue.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new StichTypeCatalogue
                            {
                                TypeStitch = Datos.TypeStitch.ToUpper(),
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.StichTypeCatalogue.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdStitchType = Registro.IdStitchType;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                           

                            Registro = _Conexion.StichTypeCatalogue.Find(Datos.IdStitchType);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.StichTypeCatalogue.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.StichTypeCatalogue.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdStitchType != Datos.IdStitchType) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.TypeStitch = Datos.TypeStitch.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();

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

        #region "NEEDLE TYPE"

        [Route("api/Premium/Operaciones/GetNeedleType")]
        [HttpGet]
        public string GetNeedleType(string NeedleType)
        {
            string json = string.Empty;
            if (NeedleType == null) NeedleType = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<NeedleTypeCustom> lst = (from q in _Conexion.NeedleType
                                                  where q.NeedleType1 == ((NeedleType == string.Empty) ? q.NeedleType1 : NeedleType)
                                                  select new NeedleTypeCustom()
                                                  {
                                                      IdNeedle = q.IdNeedle,
                                                      NeedleType = q.NeedleType1,
                                                      Code = q.Code
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


        [Route("api/Premium/Operaciones/GuardarNeedleType")]
        [HttpPost]
        public IHttpActionResult GuardarNeedleType(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarNeedleType(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarNeedleType(string d)
        {
            string json = string.Empty;


            try
            {
                NeedleTypeCustom Datos = JsonConvert.DeserializeObject<NeedleTypeCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        NeedleType Registro = null;

                        if (Datos.IdNeedle == -1)
                        {
                            if (_Conexion.NeedleType.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new NeedleType
                            {
                                NeedleType1 = Datos.NeedleType.ToUpper(),
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.NeedleType.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdNeedle = Registro.IdNeedle;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                          

                            Registro = _Conexion.NeedleType.Find(Datos.IdNeedle);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.NeedleType.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.NeedleType.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdNeedle != Datos.IdNeedle) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.NeedleType1 = Datos.NeedleType.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();

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

        #region "RPM CATALOGUE"

        [Route("api/Premium/Operaciones/GetRpm")]
        [HttpGet]
        public string GetRpm(decimal Rpm)
        {
            string json = string.Empty;
      
            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<RpmCatalogueCustom> lst = (from q in _Conexion.RpmCatalogue
                                                  where q.Rpm == ((Rpm <= -1) ? q.Rpm : Rpm)
                                                  select new RpmCatalogueCustom()
                                                  {
                                                      IdRpm = q.IdRpm,
                                                      Rpm = q.Rpm,
                                                      Code = q.Code
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


        [Route("api/Premium/Operaciones/GuardarRpm")]
        [HttpPost]
        public IHttpActionResult GuardarRpm(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarRpm(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarRpm(string d)
        {
            string json = string.Empty;


            try
            {
                RpmCatalogueCustom Datos = JsonConvert.DeserializeObject<RpmCatalogueCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        RpmCatalogue Registro = null;

                        if (Datos.IdRpm == -1)
                        {
                            if (_Conexion.RpmCatalogue.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new RpmCatalogue
                            {
                                Rpm = Datos.Rpm,
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.RpmCatalogue.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdRpm = Registro.IdRpm;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {


                            Registro = _Conexion.RpmCatalogue.Find(Datos.IdRpm);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.RpmCatalogue.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.RpmCatalogue.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdRpm != Datos.IdRpm) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.Rpm = Datos.Rpm;
                                Registro.Code = Datos.Code.ToUpper();

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

        #region "STITCH INCH"

        [Route("api/Premium/Operaciones/GetStitchInch")]
        [HttpGet]
        public string GetStitchInch(int StitchInch)
        {
            string json = string.Empty;
  

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<StitchInchCustom> lst = (from q in _Conexion.StichIncCatalogue
                                                  where q.StitchInch == ((StitchInch == -1) ? q.StitchInch : StitchInch)
                                                  select new StitchInchCustom()
                                                  {
                                                      IdStitchInch = q.IdStitchInch,
                                                      StitchInch = q.StitchInch,
                                                      Categorie = q.Categorie,
                                                      Code = q.Code
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


        [Route("api/Premium/Operaciones/GuardarStitchInch")]
        [HttpPost]
        public IHttpActionResult GuardarStitchInch(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarStitchInch(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarStitchInch(string d)
        {
            string json = string.Empty;


            try
            {
                StitchInchCustom Datos = JsonConvert.DeserializeObject<StitchInchCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        StichIncCatalogue Registro = null;

                        if (Datos.IdStitchInch == -1)
                        {
                            if (_Conexion.StichIncCatalogue.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new StichIncCatalogue
                            {
                                StitchInch = Datos.StitchInch,
                                Categorie = Datos.Categorie,
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.StichIncCatalogue.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdStitchInch = Registro.IdStitchInch;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {


                            Registro = _Conexion.StichIncCatalogue.Find(Datos.IdStitchInch);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.StichIncCatalogue.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.StichIncCatalogue.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdStitchInch != Datos.IdStitchInch) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.StitchInch = Datos.StitchInch;
                                Registro.Categorie = Datos.Categorie;
                                Registro.Code = Datos.Code.ToUpper();

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

        [Route("api/Premium/Operaciones/GetTela")]
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
                                                     Nombre = q.Nombre,
                                                     Code = q.Code
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


        [Route("api/Premium/Operaciones/GetTelaAuto")]
        [HttpGet]
        public string GetTelaAuto(string nombre)
        {
            string json = string.Empty;


            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<TiposTelaCustom> lst = (from q in _Conexion.TipoTela
                                                 where q.Nombre.ToLower().StartsWith(nombre.TrimEnd().ToLower())
                                                 orderby q.Nombre, q.Nombre.Length
                                                 select new TiposTelaCustom()
                                                 {
                                                     IdTela = q.IdTela,
                                                     Nombre = q.Nombre,
                                                     Code = q.Code
                                                 }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }

        [Route("api/Premium/Operaciones/GuardarTela")]
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
                            if (_Conexion.TipoTela.FirstOrDefault(f => f.Nombre.ToLower().Equals(Datos.Nombre.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre de la tela ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            if (_Conexion.TipoTela.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo de la tela ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new TipoTela
                            {
                                Nombre = Datos.Nombre.ToUpper(),
                                Code = Datos.Code.ToUpper()
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
                                if (_Conexion.TipoTela.FirstOrDefault(f => f.Nombre.ToLower().Equals(Datos.Nombre.ToLower()) && f.IdTela != Datos.IdTela) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre de la tela ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                if (_Conexion.TipoTela.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdTela != Datos.IdTela) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo de la tela ya se ecnuentra registrado.", 1);
                                    return json;
                                }
          

                                Registro.Nombre = Datos.Nombre.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();

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

        [Route("api/Premium/Operaciones/GetOunce")]
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
                                                      Code = q.Code,
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


        [Route("api/Premium/Operaciones/GuardarOunce")]
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
                                Code = Datos.Code.ToUpper(),
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
                                Registro.Code = Datos.Code.ToUpper();


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

        #region "CALIBER"

        [Route("api/Premium/Operaciones/GetCaliber")]
        [HttpGet]
        public string GetCaliber()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<CaliberCustom> lst = (from q in _Conexion.Caliber
                                                  select new CaliberCustom()
                                                  {
                                                      IdCaliber = q.IdCaliber,
                                                      Caliber = q.Caliber1,
                                                      Category = q.Category,
                                                      Code = q.Code,
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


        [Route("api/Premium/Operaciones/GuardarCaliber")]
        [HttpPost]
        public IHttpActionResult GuardarCaliber(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarCaliber(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarCaliber(string d)
        {
            string json = string.Empty;


            try
            {
                CaliberCustom Datos = JsonConvert.DeserializeObject<CaliberCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Caliber Registro = null;

                        if (Datos.IdCaliber == -1)
                        {
                            Registro = new Caliber
                            {
                                Caliber1 = Datos.Caliber.ToUpper(),
                                Category = Datos.Category.ToUpper(),
                                Code = Datos.Code.ToUpper(),
                            };
                            _Conexion.Caliber.Add(Registro);
                            _Conexion.SaveChanges();


                            Datos.IdCaliber = Registro.IdCaliber;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.Caliber.Find(Datos.IdCaliber);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Caliber.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                Registro.Caliber1 = Datos.Caliber;
                                Registro.Category = Datos.Category.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();


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

        #region "FEEDOG"

        [Route("api/Premium/Operaciones/GetFeedDog")]
        [HttpGet]
        public string GetFeedDog()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<FeedDogCustom> lst = (from q in _Conexion.FeedDog
                                               select new FeedDogCustom()
                                               {
                                                   IdFeedDog = q.IdFeedDog,
                                                   Part = q.Part,
                                                   MachineType = q.MachineType,
                                                   ReferenceBrand = q.ReferenceBrand,
                                                   ReferenceModel = q.ReferenceModel,
                                                   Position = q.Position,
                                                   Category = q.Category,
                                                   Code = q.Code,
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


        [Route("api/Premium/Operaciones/GuardarFeedDog")]
        [HttpPost]
        public IHttpActionResult GuardarFeedDog(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarFeedDog(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarFeedDog(string d)
        {
            string json = string.Empty;


            try
            {
                FeedDogCustom Datos = JsonConvert.DeserializeObject<FeedDogCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        FeedDog Registro = null;

                        if (Datos.IdFeedDog == -1)
                        {
                            if (_Conexion.FeedDog.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new FeedDog
                            {
                                Part = Datos.Part.ToUpper(),
                                MachineType = Datos.MachineType.ToUpper(),
                                ReferenceBrand = Datos.ReferenceBrand.ToUpper(),
                                ReferenceModel = Datos.ReferenceModel.ToUpper(),
                                Position = Datos.Position.ToUpper(),
                                Category = Datos.Category.ToUpper(),
                                Code = Datos.Code.ToUpper(),
                            };
                            _Conexion.FeedDog.Add(Registro);
                            _Conexion.SaveChanges();


                            Datos.IdFeedDog = Registro.IdFeedDog;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.FeedDog.Find(Datos.IdFeedDog);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.FeedDog.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {

                                if (_Conexion.FeedDog.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdFeedDog != Datos.IdFeedDog) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo  ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.Part = Datos.Part;
                                Registro.MachineType = Datos.MachineType.ToUpper();
                                Registro.ReferenceBrand = Datos.ReferenceBrand.ToUpper();
                                Registro.ReferenceModel = Datos.ReferenceModel.ToUpper();
                                Registro.Position = Datos.Position.ToUpper();
                                Registro.Category = Datos.Category.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();


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

        #region "PRESSER FOOT"

        [Route("api/Premium/Operaciones/GetPresserFoot")]
        [HttpGet]
        public string GetPresserFoot()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<PresserFootCustom> lst = (from q in _Conexion.PresserFoot
                                                   select new PresserFootCustom()
                                               {
                                                   IdPresserFoot = q.IdPresserFoot,
                                                   Part = q.Part,
                                                   MachineType = q.MachineType,
                                                   ReferenceBrand = q.ReferenceBrand,
                                                   ReferenceModel = q.ReferenceModel,
                                                   Code = q.Code,
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


        [Route("api/Premium/Operaciones/GuardarPresserFoot")]
        [HttpPost]
        public IHttpActionResult GuardarPresserFoot(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarPresserFoot(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarPresserFoot(string d)
        {
            string json = string.Empty;


            try
            {
                PresserFootCustom Datos = JsonConvert.DeserializeObject<PresserFootCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        PresserFoot Registro = null;

                        if (Datos.IdPresserFoot == -1)
                        {
                            if (_Conexion.PresserFoot.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new PresserFoot
                            {
                                Part = Datos.Part.ToUpper(),
                                MachineType = Datos.MachineType.ToUpper(),
                                ReferenceBrand = Datos.ReferenceBrand.ToUpper(),
                                ReferenceModel = Datos.ReferenceModel.ToUpper(),
                                Code = Datos.Code.ToUpper(),
                            };
                            _Conexion.PresserFoot.Add(Registro);
                            _Conexion.SaveChanges();


                            Datos.IdPresserFoot = Registro.IdPresserFoot;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.PresserFoot.Find(Datos.IdPresserFoot);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.PresserFoot.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {

                                if (_Conexion.PresserFoot.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdPresserFoot != Datos.IdPresserFoot) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.Part = Datos.Part;
                                Registro.MachineType = Datos.MachineType.ToUpper();
                                Registro.ReferenceBrand = Datos.ReferenceBrand.ToUpper();
                                Registro.ReferenceModel = Datos.ReferenceModel.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();


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

        #region "FOLDER"

        [Route("api/Premium/Operaciones/GetFolder")]
        [HttpGet]
        public string GetFolder()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<FolderCustom> lst = (from q in _Conexion.Folder
                                                   select new FolderCustom()
                                                   {
                                                       IdFolder = q.IdFolder,
                                                       Part = q.Part,
                                                       Operation = q.Operation,
                                                       Code = q.Code,
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


        [Route("api/Premium/Operaciones/GuardarFolder")]
        [HttpPost]
        public IHttpActionResult GuardarFolder(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarFolder(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarFolder(string d)
        {
            string json = string.Empty;


            try
            {
                FolderCustom Datos = JsonConvert.DeserializeObject<FolderCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Folder Registro = null;

                        if (Datos.IdFolder == -1)
                        {
                            if (_Conexion.Folder.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El código ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            Registro = new Folder
                            {
                                Part = Datos.Part.ToUpper(),
                                Operation = Datos.Operation.ToUpper(),
                                Code = Datos.Code.ToUpper(),
                            };
                            _Conexion.Folder.Add(Registro);
                            _Conexion.SaveChanges();


                            Datos.IdFolder = Registro.IdFolder;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.Folder.Find(Datos.IdFolder);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Folder.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {

                                if (_Conexion.Folder.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdFolder != Datos.IdFolder) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                Registro.Part = Datos.Part;
                                Registro.Operation = Datos.Operation.ToUpper();
                                Registro.Code = Datos.Code.ToUpper();


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

        [Route("api/Premium/Operaciones/GetPartes")]
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


        [Route("api/Premium/Operaciones/GuardarPartes")]
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



   


        #region "SEWING"

        [Route("api/Premium/Operaciones/GetSewing")]
        [HttpGet]
        public string GetSewing(string codigo)
        {
            string json = string.Empty;
            if (codigo == null) codigo = string.Empty;
            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<SewingCustom> lst = (from q in _Conexion.Sewing
                                              where q.Code == ((codigo == string.Empty)? q.Code : codigo)
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


        [Route("api/Premium/Operaciones/GuardarSewing")]
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

        [Route("api/Premium/Operaciones/GetSewingAccuracy")]
        [HttpGet]
        public string GetSewingAccuracy(string level)
        {
            string json = string.Empty;
            if (level == null) level = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<SewingAccuracyCustom> lst = (from q in _Conexion.SewingAccuracy
                                                      where q.Level == ((level == string.Empty) ? q.Level : level)
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


        [Route("api/Premium/Operaciones/GuardarSewingAccuracy")]
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






        #region "METOD ANALYSIS"

        [Route("api/Premium/Operaciones/GetMethodAnalysis")]
        [HttpGet]
        public string GetMethodAnalysis(string FechaInicio, string FechaFin)
        {
            string json = string.Empty;

            DateTime Inicio = new DateTime(1900, 1, 1);
            DateTime Fin =  DateTime.Now;

            if (FechaInicio != null) Inicio = Convert.ToDateTime(FechaInicio);
            if (FechaFin != null) Fin = Convert.ToDateTime(FechaFin);


            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {
                    List<MethodAnalysis> lstMethod = _Conexion.MethodAnalysis.Where(w => DbFunctions.TruncateTime(w.FechaRegistro) >= Inicio.Date && DbFunctions.TruncateTime(w.FechaRegistro) <= Fin.Date).ToList();
               

                    var lst = (from q in lstMethod
                               join t in _Conexion.TipoTela on q.IdTela equals t.IdTela
                               join m in _Conexion.MachineData on q.IdDataMachine equals m.IdDataMachine
                               join o in _Conexion.ClassOunce on q.Onza equals o.Ounce into unionO
                               from u_o in unionO.DefaultIfEmpty()

                               select new
                               {
                                   IdMethodAnalysis = q.IdMethodAnalysis,
                                   Codigo = q.Codigo,
                                   ProcesoManufact = q.ProcesoManufact,
                                   IdProducto = q.IdProducto,
                                   TipoProducto = q.TipoProducto,
                                   Operacion = q.Operacion,
                                   IdDataMachine = q.IdDataMachine,
                                   DataMachine = q.DataMachine,
                                   Stitch = q.Stitch,
                                   Delay = q.Delay,
                                   Personal = q.Personal,
                                   Fatigue = q.Fatigue,
                                   Rpm = q.Rpm,
                                   Sewing = q.Sewing,
                                   Puntadas = q.Puntadas,
                                   ManejoPaquete = q.ManejoPaquete,
                                   Rate = q.Rate,
                                   JornadaLaboral = q.JornadaLaboral,
                                   IdTela = q.IdTela,
                                   Tela = t.Nombre,
                                   Onza = q.Onza,
                                   MateriaPrima_1 = q.MateriaPrima_1,
                                   MateriaPrima_2 = q.MateriaPrima_2,
                                   MateriaPrima_3 = q.MateriaPrima_3,
                                   MateriaPrima_4 = q.MateriaPrima_4,
                                   MateriaPrima_5 = q.MateriaPrima_5,
                                   MateriaPrima_6 = q.MateriaPrima_6,
                                   MateriaPrima_7 = q.MateriaPrima_7,
                                   Familia = q.Familia,
                                   TipoConstruccion = q.TipoConstruccion,
                                   FechaRegistro = q.FechaRegistro,
                                   IdUsuario = q.IdUsuario,
                                   Usuario = _Conexion.Usuario.First(u => u.IdUsuario == q.IdUsuario).Login,
                                   Tmus_Mac = q.Tmus_Mac,
                                   Tmus_MinL = q.Tmus_MinL,
                                   Min_Mac = q.Min_Mac,
                                   Min_NML = q.Min_NML,
                                   Min_Mac_CC = q.Min_Mac_CC,
                                   Min_NML_CC = q.Min_NML_CC,
                                   Sam = q.Sam,
                                   ProducJL = q.ProducJL,
                                   Precio = q.Precio,
                                   IdUsuarioModifica = q.IdUsuarioModifica,
                                   UsuarioModifica = (q.IdUsuarioModifica == null ? string.Empty : _Conexion.Usuario.First(u => u.IdUsuario == (int)q.IdUsuarioModifica).Login),
                                   FechaModifica = q.FechaModifica,
                                   UbicacionSecuencia = string.Empty,
                                   Machine = m.Machine,
                                   Needle = "",//m.Needle,
                                   Caliber = "",//(u_o == null) ? string.Empty : u_o.Caliber,
                                   FeedDog = "",//(u_o == null) ? string.Empty : u_o.FeedDog,
                                   CodPrensatela = string.Empty,
                                   TipoFolder = string.Empty

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

        [Route("api/Premium/Operaciones/GetDetMethodAnalysis")]
        [HttpGet]
        public string GetDetMethodAnalysis(int IdMethodAnalysis)
        {
            string json = string.Empty;

      
            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<MethosAnalisysDetCustom> lst = (from q in _Conexion.MethodAnalysisDetalle
                                                         where q.IdMethodAnalysis == IdMethodAnalysis
                                                         select new MethosAnalisysDetCustom
                                                         {
                                                             IdDetMethodAnalysis = q.IdDetMethodAnalysis,
                                                             IdMethodAnalysis = q.IdMethodAnalysis,
                                                             Codigo1 = q.Codigo1.ToUpper().TrimEnd(),
                                                             Codigo2 = q.Codigo2.ToUpper().TrimEnd(),
                                                             Codigo3 = q.Codigo3.ToUpper().TrimEnd(),
                                                             Codigo4 = q.Codigo4.ToUpper().TrimEnd(),
                                                             Descripcion = q.Descripcion.ToUpper().TrimEnd(),
                                                             Freq = q.Freq,
                                                             Tmus = q.Tmus,
                                                             Sec = q.Sec,
                                                             Sam = q.Sam

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


        [Route("api/Premium/Operaciones/GuardarMethodAnalysis")]
        [HttpPost]
        public IHttpActionResult GuardarMethodAnalysis(MethodAnalysisData d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_GuardarMethodAnalysis(d.d, d.d2));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _GuardarMethodAnalysis(MethodAnalysisCustom Datos1, MethosAnalisysDetCustom[] Datos2)
        {
            string json = string.Empty;


            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {
                        MethodAnalysis Registro = _Conexion.MethodAnalysis.Find(Datos1.IdMethodAnalysis);
                        
                        DateTime Fecha = DateTime.Now;

                        if (Registro == null)
                        {
                            string Codigo = string.Empty;
                            

                            Registro = new MethodAnalysis
                            {
                                Codigo = string.Empty,
                                ProcesoManufact = Datos1.ProcesoManufact.ToUpper().TrimEnd(),
                                IdProducto = Datos1.IdProducto,
                                TipoProducto = Datos1.TipoProducto.ToUpper().TrimEnd(),
                                Operacion = Datos1.Operacion.ToUpper().TrimEnd(),
                                IdDataMachine = Datos1.IdDataMachine,
                                DataMachine = Datos1.DataMachine.ToUpper().TrimEnd(),
                                Stitch = Datos1.Stitch,
                                Delay = Datos1.Delay,
                                Personal = Datos1.Personal,
                                Fatigue = Datos1.Fatigue,
                                Rpm = Datos1.Rpm,
                                Sewing = Datos1.Sewing,
                                Puntadas = Datos1.Puntadas,
                                ManejoPaquete = Datos1.ManejoPaquete.ToUpper().TrimEnd(),
                                Rate = Datos1.Rate,
                                JornadaLaboral = Datos1.JornadaLaboral,
                                IdTela = Datos1.IdTela,
                                Onza = Datos1.Onza,
                                MateriaPrima_1 = Datos1.MateriaPrima_1.ToUpper().TrimEnd(),
                                MateriaPrima_2 = Datos1.MateriaPrima_2.ToUpper().TrimEnd(),
                                MateriaPrima_3 = Datos1.MateriaPrima_3.ToUpper().TrimEnd(),
                                MateriaPrima_4 = Datos1.MateriaPrima_4.ToUpper().TrimEnd(),
                                MateriaPrima_5 = Datos1.MateriaPrima_5.ToUpper().TrimEnd(),
                                MateriaPrima_6 = Datos1.MateriaPrima_6.ToUpper().TrimEnd(),
                                MateriaPrima_7 = Datos1.MateriaPrima_7.ToUpper().TrimEnd(),
                                Familia = Datos1.Familia.ToUpper(),
                                TipoConstruccion = Datos1.TipoConstruccion.ToUpper().TrimEnd(),
                                FechaRegistro = Fecha,
                                IdUsuario = _Conexion.Usuario.First(u => u.Login == Datos1.Usuario).IdUsuario,
                                Tmus_Mac = Datos1.Tmus_Mac,
                                Tmus_MinL = Datos1.Tmus_MinL,
                                Min_Mac = Datos1.Min_Mac,
                                Min_NML = Datos1.Min_NML,
                                Min_Mac_CC = Datos1.Min_Mac_CC,
                                Min_NML_CC = Datos1.Min_NML_CC,
                                Sam = Datos1.Sam,
                                ProducJL = Datos1.ProducJL,
                                Precio = Datos1.Precio
                        };
                            _Conexion.MethodAnalysis.Add(Registro);

                            _Conexion.SaveChanges();

                            Codigo = _Conexion.MethodAnalysis.Where(w => w.Codigo != string.Empty).Max(m => m.Codigo);
                            if (Codigo != null)
                                Codigo = (Convert.ToInt32(Codigo) + 1).ToString();
                            else
                                Codigo = "1";

                            Codigo = Codigo.PadLeft(10, '0');
                            Registro.Codigo = Codigo;

                            Datos1.IdMethodAnalysis = Registro.IdMethodAnalysis;
                            Datos1.Codigo = Codigo;


                            _Conexion.SaveChanges();
                        }
                        else
                        {
                            Registro.Codigo = Datos1.Codigo;
                            Registro.ProcesoManufact = Datos1.ProcesoManufact.ToUpper().TrimEnd();
                            Registro.IdProducto = Datos1.IdProducto;
                            Registro.TipoProducto = Datos1.TipoProducto.ToUpper().TrimEnd();
                            Registro.Operacion = Datos1.Operacion.ToUpper().TrimEnd();
                            Registro.IdDataMachine = Datos1.IdDataMachine;
                            Registro.DataMachine = Datos1.DataMachine.ToUpper().TrimEnd();
                            Registro.Stitch = Datos1.Stitch;
                            Registro.Delay = Datos1.Delay;
                            Registro.Personal = Datos1.Personal;  
                            Registro.Fatigue = Datos1.Fatigue;
                            Registro.Rpm = Datos1.Rpm;
                            Registro.Sewing = Datos1.Sewing;
                            Registro.Puntadas = Datos1.Puntadas;
                            Registro.ManejoPaquete = Datos1.ManejoPaquete.ToUpper().TrimEnd();
                            Registro.Rate = Datos1.Rate;
                            Registro.JornadaLaboral = Datos1.JornadaLaboral;
                            Registro.IdTela = Datos1.IdTela;
                            Registro.Onza = Datos1.Onza;
                            Registro.MateriaPrima_1 = Datos1.MateriaPrima_1.ToUpper().TrimEnd();
                            Registro.MateriaPrima_2 = Datos1.MateriaPrima_2.ToUpper().TrimEnd();
                            Registro.MateriaPrima_3 = Datos1.MateriaPrima_3.ToUpper().TrimEnd();
                            Registro.MateriaPrima_4 = Datos1.MateriaPrima_4.ToUpper().TrimEnd();
                            Registro.MateriaPrima_5 = Datos1.MateriaPrima_5.ToUpper().TrimEnd();
                            Registro.MateriaPrima_6 = Datos1.MateriaPrima_6.ToUpper().TrimEnd();
                            Registro.MateriaPrima_7 = Datos1.MateriaPrima_7.ToUpper().TrimEnd();
                            Registro.Familia = Datos1.Familia.ToUpper();
                            Registro.TipoConstruccion = Datos1.TipoConstruccion.ToUpper().TrimEnd();
                            Registro.Tmus_Mac = Datos1.Tmus_Mac;
                            Registro.Tmus_MinL = Datos1.Tmus_MinL;
                            Registro.Min_Mac = Datos1.Min_Mac;
                            Registro.Min_NML = Datos1.Min_NML;
                            Registro.Min_Mac_CC = Datos1.Min_Mac_CC;
                            Registro.Min_NML_CC = Datos1.Min_NML_CC;
                            Registro.Sam = Datos1.Sam;
                            Registro.ProducJL = Datos1.ProducJL;
                            Registro.Precio = Datos1.Precio;
                            Registro.IdUsuarioModifica = _Conexion.Usuario.First(u => u.Login == Datos1.Usuario).IdUsuario;
                            Registro.FechaModifica = Fecha;
                            

                            _Conexion.SaveChanges();
                        }


                        foreach(MethosAnalisysDetCustom DetalleCustom in Datos2)
                        {
                            MethodAnalysisDetalle Detalle = _Conexion.MethodAnalysisDetalle.Find(DetalleCustom.IdDetMethodAnalysis);

                            if(Detalle == null)
                            {
                                Detalle = new MethodAnalysisDetalle
                                {
                                    IdMethodAnalysis = Registro.IdMethodAnalysis,
                                    Codigo1 = DetalleCustom.Codigo1.ToUpper().TrimEnd(),
                                    Codigo2 = DetalleCustom.Codigo2.ToUpper().TrimEnd(),
                                    Codigo3 = DetalleCustom.Codigo3.ToUpper().TrimEnd(),
                                    Codigo4 = DetalleCustom.Codigo4.ToUpper().TrimEnd(),
                                    Descripcion = DetalleCustom.Descripcion.ToUpper().TrimEnd(),
                                    Freq = DetalleCustom.Freq,
                                    Tmus = DetalleCustom.Tmus,
                                    Sec = DetalleCustom.Sec,
                                    Sam = DetalleCustom.Sam
                                };
                                _Conexion.MethodAnalysisDetalle.Add(Detalle);
                            }
                            else
                            {
                                Detalle.IdMethodAnalysis = Registro.IdMethodAnalysis;
                                Detalle.Codigo1 = DetalleCustom.Codigo1.ToUpper().TrimEnd();
                                Detalle.Codigo2 = DetalleCustom.Codigo2.ToUpper().TrimEnd();
                                Detalle.Codigo3 = DetalleCustom.Codigo3.ToUpper().TrimEnd();
                                Detalle.Codigo4 = DetalleCustom.Codigo4.ToUpper().TrimEnd();
                                Detalle.Descripcion = DetalleCustom.Descripcion.ToUpper().TrimEnd();
                                Detalle.Freq = DetalleCustom.Freq;
                                Detalle.Tmus = DetalleCustom.Tmus;
                                Detalle.Sec = DetalleCustom.Sec;
                                Detalle.Sam = DetalleCustom.Sam;
                            }

                            _Conexion.SaveChanges();
                            DetalleCustom.IdDetMethodAnalysis = Detalle.IdDetMethodAnalysis;


                        }


                        List<object> lst = new List<object>();
                        lst.Add(Datos1);
                        lst.Add(Datos2);



                        json = Cls.Cls_Mensaje.Tojson(lst, 1, string.Empty, "Registro Guardado.", 0);


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


        [Route("api/Premium/Operaciones/EliminarMethodAnalysis")]
        [HttpPost]
        public IHttpActionResult EliminarMethodAnalysis(int IdDetMethodAnalysis, string user)
        {
            if (ModelState.IsValid)
            {

                return Ok(_EliminarMethodAnalysis(IdDetMethodAnalysis, user));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _EliminarMethodAnalysis(int IdDetMethodAnalysis, string user)
        {
            string json = string.Empty;


            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    { 

                        MethodAnalysisDetalle Detalle = _Conexion.MethodAnalysisDetalle.Find(IdDetMethodAnalysis);
                        _Conexion.MethodAnalysisDetalle.Remove(Detalle);

                        MethodAnalysis Registro = _Conexion.MethodAnalysis.Find(Detalle.IdMethodAnalysis);
                        Registro.IdUsuarioModifica = _Conexion.Usuario.First(u => u.Login == user).IdUsuario;
                        Registro.FechaModifica = DateTime.Now;


                        json = Cls.Cls_Mensaje.Tojson(Detalle, 1, string.Empty, "Registro Eliminado.", 0);
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


        [Route("api/Premium/Operaciones/EliminarMatrixOperacion")]
        [HttpPost]
        public IHttpActionResult EliminarMatrixOperacion(int IdMethodAnalysis, string user)
        {
            if (ModelState.IsValid)
            {

                return Ok(_EliminarMatrixOperacion(IdMethodAnalysis, user));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _EliminarMatrixOperacion(int IdMethodAnalysis, string user)
        {
            string json = string.Empty;


            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {


                        MethodAnalysis Registro = _Conexion.MethodAnalysis.Find(IdMethodAnalysis);

                        foreach(MethodAnalysisDetalle Detalle in _Conexion.MethodAnalysisDetalle.Where(w => w.IdMethodAnalysis == Registro.IdMethodAnalysis))
                        {
                            _Conexion.MethodAnalysisDetalle.Remove(Detalle);
                        }


                        _Conexion.MethodAnalysis.Remove(Registro);

       

                        json = Cls.Cls_Mensaje.Tojson(Registro, 1, string.Empty, "Registro Eliminado.", 0);
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
