﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocedesAPI.Conex_Pervasive;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.INV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace RocedesAPI.Controllers.INV
{
    public class BundleBoxingController : ApiController
    {
        [Route("api/BundleBoxing/GetSerialesEscaneado")]
        [HttpGet]
        public string GetSerialesEscaneado(string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
       
                    var lst = (from b in _Cnx.BundleBoxing
                               join s in _Cnx.BundleBoxing_Saco on b.IdSaco equals s.IdSaco
                                           where b.Corte == corte && b.Activo
                                           select new
                                           {
                                               Serial = b.Serial,
                                               Bulto = b.Bulto,
                                               Saco = s.Saco
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


        [Route("api/BundleBoxing/GetBundleBoxing")]
        [HttpGet]
        public string GetBundleBoxing(string corte, string estilo)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    var lst = (from b in _Cnx.BundleBoxing
                               where b.Corte == corte && b.Estilo == estilo && b.Activo
                               select new
                               {
                                   Serial = b.Serial,
                                   Bulto = b.Bulto,
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




        [Route("api/BundleBoxing/Saco")]
        [HttpPost]
        public IHttpActionResult Saco(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarSaco(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarSaco(string d)
        {
            string json = string.Empty;


            SacoEstado Datos = JsonConvert.DeserializeObject<SacoEstado>(d);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        int IdUsuario = _Conexion.Usuario.ToList().Find(u => u.Login == Datos.Login).IdUsuario;

                        BundleBoxing_Saco Registro = null;


                        if (Datos.Saco == 0 && Datos.Estado != "Cerrar")
                        {
                            var _Saco = (from sc in _Conexion.BundleBoxing_Saco
                                         where sc.Corte == Datos.Corte && sc.NoMesa == Datos.Mesa && sc.IdUsuarioAbre == IdUsuario && sc.Abierto
                                         select sc.Saco
                                     ).ToList();



                            if (_Saco.Count == 0)
                            {

                                _Saco = (from sc in _Conexion.BundleBoxing_Saco
                                         where !_Conexion.BundleBoxing.Any(b => b.IdSaco == sc.IdSaco) && sc.NoMesa == Datos.Mesa
                                         select sc.Saco).ToList();


                                if (_Saco.Count > 0)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"<p>El saco # <b>{_Saco[0].ToString()}</b> se encuentra vacio.<br>Utilize el saco vacio.</p>", 1);
                                    return json;
                                }



                                Registro = new BundleBoxing_Saco()
                                {
                                    Saco = -1,
                                    NoMesa = Datos.Mesa,
                                    Corte = Datos.Corte,
                                    Seccion = Datos.Seccion,
                                    IdUsuarioCrea = IdUsuario,
                                    FechaRegistro = DateTime.Now,
                                    IdUsuarioAbre = IdUsuario,
                                    Abierto = true,
                                    Activo = true
                                };

                                _Conexion.BundleBoxing_Saco.Add(Registro);

                                _Conexion.SaveChanges();




                                var _Query = (from b in _Conexion.BundleBoxing_Saco
                                              where b.Corte == Datos.Corte && b.Activo
                                              group b by b.Corte into SacoGroup
                                              select new
                                              {
                                                  Saco = SacoGroup.Max(m => m.Saco) + 1
                                              }).ToList();

                                Registro.Saco = 0;
                                if (_Query.Count > 0) Registro.Saco = _Query[0].Saco;
                                if (Registro.Saco <= 0) Registro.Saco = 1;



                            }
                            else
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # {_Saco[0].ToString()} se encuentra abierto, Primero cierre el saco para crear uno nuevo.", 1);
                                return json;

                            }


                        }
                        else
                        {
                            Registro = _Conexion.BundleBoxing_Saco.FirstOrDefault(b => b.Corte == Datos.Corte && b.Saco == Datos.Saco && b.Activo);

                            if (Registro != null)
                            {
                                if(Registro.NoMesa == Datos.Mesa)
                                {
                                    Registro.IdUsuarioAbre = ((Datos.Estado == "Abrir") ? IdUsuario : (int?)null);
                                    Registro.Abierto = ((Datos.Estado == "Abrir") ? true : false);
                                }
                                else
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{Datos.Saco}</b> pertenece a la mesa # <b>{ Registro.NoMesa}</b>", 1);
                                    return json;
                                }

                                
                            }
                            else
                            {
                                if (Datos.Estado != "Cerrar")
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # {Datos.Saco} no se encuentra registrado", 1);
                                }
                                else
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 1, string.Empty, $"Saco # {Datos.Saco} cerado.", 0);

                                }
                                return json;
                            }


                        }



                        BundleBoxing_SacoCustom RegistoCustom = new BundleBoxing_SacoCustom();
                        RegistoCustom.Saco = Registro.Saco;



                        json = Cls.Cls_Mensaje.Tojson(RegistoCustom, 1, string.Empty, "Registro Guardado.", 0);

                        _Conexion.SaveChanges();
                        scope.Complete();
                        scope.Dispose();




                    }
                }
                
            }
            catch(Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }


        [Route("api/BundleBoxing/Pieza")]
        [HttpPost]
        public IHttpActionResult Pieza(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarPieza(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarPieza(string d)
        {
            string json = string.Empty;

            try
            {
                BundleBoxingCustom Datos = JsonConvert.DeserializeObject<BundleBoxingCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {
                        BundleBoxing Boxing = _Conexion.BundleBoxing.FirstOrDefault(b => b.Corte == Datos.Corte && b.Serial == Datos.Serial && b.Oper == Datos.Oper && b.Activo);

                        if (Boxing != null)
                        {
                           
                            json = Cls.Cls_Mensaje.Tojson(Boxing, 1, string.Empty, $"Serial # <b>{Datos.Serial}</b> escaneado.", 0);
                        }
                        else
                        {

                            Boxing = new BundleBoxing
                            {
                                NoMesa = Datos.Mesa,
                                Serial = Datos.Serial,
                                Nombre = Datos.Nombre,
                                Seccion = Datos.Seccion,
                                Bulto = Datos.Bulto,
                                Capaje = Datos.Capaje,
                                IdSaco = _Conexion.BundleBoxing_Saco.FirstOrDefault(sc => sc.Saco == Datos.Saco && sc.Corte == Datos.Corte && sc.Seccion == Datos.Seccion && sc.Activo).IdSaco,
                                Corte = Datos.Corte,
                                Estilo = Datos.Estilo,
                                Oper = Datos.Oper,
                                IdUsuario = _Conexion.Usuario.FirstOrDefault(u => u.Login == Datos.Login).IdUsuario,
                                FechaRegistro = DateTime.Now,
                                Activo = true
                            };

                            _Conexion.BundleBoxing.Add(Boxing);
                        }

                        BundleBoxingCustom BoxingCustom = new BundleBoxingCustom
                        {
                            Escaneado = Boxing.Activo,
                            Saco = _Conexion.BundleBoxing_Saco.FirstOrDefault(sc => sc.IdSaco == Boxing.IdSaco && sc.Corte == Boxing.Corte && sc.Seccion == Boxing.Seccion).Saco,
                            Mesa = Boxing.NoMesa
                        };





                        _Conexion.SaveChanges();
                        scope.Complete();

                        json = Cls.Cls_Mensaje.Tojson(BoxingCustom, 1, string.Empty, $"Serial # <b>{Datos.Serial}</b> escaneado.", 0);

                    }
                }
            }
            catch(Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

           

            return json;
        }
    }
}
