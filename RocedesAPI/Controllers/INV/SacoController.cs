using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocedesAPI.Conex_Pervasive;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.INV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Cors;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace RocedesAPI.Controllers.INV
{
    public class SacoController : ApiController
    {


        [Route("api/Inventario/Saco/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<SacoCustom> lst = (from q in _Cnx.BundleBoxing_Saco
                                                        join u in _Cnx.Usuario on (q.Activo ? q.IdUsuarioCrea : q.IdUsuarioInactiva) equals u.IdUsuario
                                                        select new SacoCustom()
                                                        {
                                                            IdSaco = q.IdSaco,
                                                            Serial = q.Serial,
                                                            Saco = q.Saco,
                                                            NoMesa = q.NoMesa,
                                                            Corte = q.Corte,
                                                            CorteCompleto = q.CorteCompleto,
                                                            Seccion = q.Seccion,
                                                            IdUsuario = (q.Activo) ? q.IdUsuarioCrea : q.IdUsuarioInactiva,
                                                            Usuario = u.Login,
                                                            Abierto = q.Abierto,
                                                            IdUsuarioAbre = (q.Abierto) ? (int?)null : q.IdUsuarioCrea,
                                                            UsuarioAbre = (q.Abierto) ? (_Cnx.Usuario.FirstOrDefault( f  => f.IdUsuario == q.IdUsuarioAbre).Login) : string.Empty,
                                                            FechaRegistro = (q.Activo) ? q.FechaRegistro : q.FechaInactivo,
                                                            Activo = q.Activo
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


        [Route("api/Inventario/Saco/VerificarSacoAbierto")]
        [HttpGet]
        public string VerificarSacoAbierto(string user, string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    Usuario _Usuario = _Cnx.Usuario.FirstOrDefault(f => f.Login.Equals(user));

                    if (_Usuario == null)
                    {
                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, string.Empty, 0);
                        return json;
                    }


                    var _Saco = (from sc in _Cnx.BundleBoxing_Saco
                                 where sc.Corte == corte && sc.IdUsuarioAbre == _Usuario.IdUsuario && sc.Abierto
                                 select sc.Saco
                                     ).ToList();

                    if (_Saco.Count == 0)
                    {
                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, string.Empty, 0);
                        return json;
                    }

                    BundleBoxing_SacoCustom RegistoCustom = new BundleBoxing_SacoCustom();
                    RegistoCustom.Saco = _Saco[0];

                    json = Cls.Cls_Mensaje.Tojson(RegistoCustom, 1, string.Empty, string.Empty, 0);
                    return json;
                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }

        [Route("api/Inventario/Saco")]
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
                                         where sc.Corte == Datos.Corte && sc.NoMesa == Datos.Mesa && sc.Seccion == Datos.Seccion && sc.IdUsuarioAbre == IdUsuario && sc.Abierto
                                         select new { saco = sc.Saco, corte = sc.Corte}
                                     ).ToList();



                            if (_Saco.Count == 0)
                            {

                                _Saco = (from sc in _Conexion.BundleBoxing_Saco
                                         where !_Conexion.BundleBoxing.Any(b => b.IdSaco == sc.IdSaco) && sc.NoMesa == Datos.Mesa && sc.Seccion == Datos.Seccion && sc.Corte == Datos.Corte
                                         select new { saco = sc.Saco, corte = sc.Corte }).ToList();


                                if (_Saco.Count > 0)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"<p>El saco # <b>{_Saco[0].ToString()}</b> se encuentra vacio.<br>Utilize el saco vacio.</p>", 1);
                                    return json;
                                }



                                Registro = new BundleBoxing_Saco()
                                {
                                    Saco = -1,
                                    Serial = "0",
                                    NoMesa = Datos.Mesa,
                                    Corte = Datos.Corte,
                                    CorteCompleto = Datos.CorteCompleto,
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
                                              where b.CorteCompleto == Datos.CorteCompleto && b.Activo
                                              group b by b.CorteCompleto into SacoGroup
                                              select new
                                              {
                                                  Saco = SacoGroup.Max(m => m.Saco) + 1
                                              }).ToList();

                                Registro.Saco = 0;
                                if (_Query.Count > 0) Registro.Saco = _Query[0].Saco;
                                if (Registro.Saco <= 0) Registro.Saco = 1;

                                var _Query2 = (from b in _Conexion.BundleBoxing_Saco
                                               group b by b.CorteCompleto into SacoGroup
                                               select new
                                               {
                                                   Serial = SacoGroup.Max(m => m.Serial)
                                               }).ToList();

                                if (_Query2.Count > 0) Registro.Serial = string.Concat(Datos.Seccion, Datos.Mesa, Convert.ToInt32(_Query2[0].Serial) + 1);
                                if (_Query2.Count == 0) Registro.Serial = string.Concat(Datos.Seccion, Datos.Mesa, 1);



                            }
                            else
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{_Saco[0].saco}</b> en el corte <b>{_Saco[0].corte}</b> se encuentra abierto, Primero cierre el saco para crear uno nuevo.", 1);
                                return json;

                            }


                        }
                        else
                        {
                            Registro = _Conexion.BundleBoxing_Saco.FirstOrDefault(b => b.Corte == Datos.Corte && b.Saco == Datos.Saco);

                            if (Registro != null)
                            {
                                if(Registro.Abierto || Datos.Estado == "Abrir")
                                {

                                    if (Registro.NoMesa == Datos.Mesa && Registro.Seccion == Datos.Seccion)
                                    {

                                        if(Registro.IdUsuarioAbre != IdUsuario && Registro.Abierto && Datos.Estado == "Abrir")
                                        {
                                            json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{Datos.Saco}</b> se encuentra utilizado por otro usuario.</b>", 1);
                                            return json;
                                        }

                                        if (_Conexion.BundleBoxingEnvio.FirstOrDefault(f => f.Serial == Registro.Serial) != null)
                                        {
                                            json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{ Datos.Saco}</b> se encuentra en proceso de envio.", 1);
                                            return json;
                                        }



                                        Registro.IdUsuarioAbre = ((Datos.Estado == "Abrir") ? IdUsuario : (int?)null);
                                        Registro.Abierto = ((Datos.Estado == "Abrir") ? true : false);
                                    }
                                    else
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{Datos.Saco}</b> pertenece a la mesa # <b>{ Registro.NoMesa}</b>  Seccion # <b>{Registro.Seccion}</b>", 1);
                                        return json;
                                    }
                                }
                                else
                                {

                                    if(Datos.Estado == "Cerrar")
                                    {
                                        Registro.IdUsuarioAbre = (int?)null;
                                        Registro.Abierto = false;
                                    }
                                    else
                                    {
                                        json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{Datos.Saco}</b> se encuentra cerrado.", 1);
                                        return json;
                                    }
                                    
                                   
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
                                    json = Cls.Cls_Mensaje.Tojson(null, 1, string.Empty, $"Saco # {Datos.Saco} cerrado.", 0);

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
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }






        [Route("api/Inventario/Saco/Eliminar")]
        [HttpPost]
        public IHttpActionResult Eliminar(string serial, string login)
        {
            if (ModelState.IsValid)
            {

                return Ok(Eliminarsaco(serial, login));

            }
            else
            {
                return BadRequest();
            }

        }

        private string Eliminarsaco(string serial, string login)
        {
            string json = string.Empty;


            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        int IdUsuario = _Conexion.Usuario.ToList().Find(u => u.Login == login).IdUsuario;

                        BundleBoxing_Saco Registro = _Conexion.BundleBoxing_Saco.FirstOrDefault(f => f.Serial.Equals(serial));


                        if(_Conexion.BundleBoxingEnvio.FirstOrDefault(f => f.IdSaco == Registro.IdSaco) != null)
                        {
                            json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{Registro.Saco}</b> se encuentra en estado de envio<br>Elimine el envio para poder contunuar con la operación.", 1);
                            return json;
                        }

                        foreach( BundleBoxing b in _Conexion.BundleBoxing.Where(w => w.IdSaco == Registro.IdSaco))
                        {
                            b.IdUsuario = null;
                            b.FechaRegistro = null;
                            b.IdSaco = null;
                            b.Escaneado = false;
                        }

                        Registro.IdUsuarioAbre = null;
                        Registro.Abierto = false;
                        Registro.IdUsuarioInactiva = IdUsuario;
                        Registro.FechaInactivo = DateTime.Now;
                        Registro.Activo = false;



                        json = Cls.Cls_Mensaje.Tojson(null, 1, string.Empty, "Registro Guardado.", 0);

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