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
                using (RocedesEntities _Cnx = new RocedesEntities())
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
                    using (var _Conexion = new RocedesEntities())
                    {

                        int IdUsuario = _Conexion.Usuario.ToList().Find(u => u.Login == Datos.Login).IdUsuario;

                        BundleBoxing_Saco Registro = null;


                        if (Datos.Saco == 0)
                        {
                            var _Saco = (from sc in _Conexion.BundleBoxing_Saco
                                         where sc.Corte == Datos.Corte && sc.IdUsuarioAbre == IdUsuario && sc.Abierto
                                         select sc.Saco
                                     ).ToList();

                      

                            if (_Saco.Count == 0)
                            {

                                _Saco = (from sc in _Conexion.BundleBoxing_Saco
                                       where !_Conexion.BundleBoxing.Any( b => b.IdSaco == sc.IdSaco)
                                       select sc.Saco).ToList();


                                if(_Saco.Count > 0)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"<p>El saco # <b>{_Saco[0].ToString()}</b> se encuentra vacio.<br>Utilize el saco vacio.</p>", 1);
                                    return json;
                                }



                                Registro = new BundleBoxing_Saco()
                                {
                                    Saco = -1,
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
                                              where b.Seccion == Datos.Seccion && b.Corte == Datos.Corte && b.Activo
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

                            if(Registro != null)
                            {
                                Registro.IdUsuarioAbre =  ((Datos.Estado == "Abrir")?  IdUsuario :  (int?)null);
                                Registro.Abierto = ((Datos.Estado == "Abrir") ? true : false);
                            }
                            else
                            {
                                if(Datos.Estado != "Cerrar")
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



                        List<BundleBoxing_Saco> lst = new List<BundleBoxing_Saco>();
                        lst.Add(Registro);


                        json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, "Registro Guardado.", 0);

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

    }
}
