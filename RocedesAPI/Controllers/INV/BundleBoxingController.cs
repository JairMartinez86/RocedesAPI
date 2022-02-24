using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocedesAPI.Conex_Pervasive;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.INV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Cors;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace RocedesAPI.Controllers.INV
{
 
    public class BundleBoxingController : ApiController
    {
        [Route("api/Inventario/BundleBoxing/GetSerialesEscaneado")]
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
                                           where b.Corte == corte && b.Escaneado
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


        [Route("api/Inventario/BundleBoxing/GetBundleBoxing")]
        [HttpGet]
        public string GetBundleBoxing(string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<BundleBoxing> lstBundleBoxing = _Cnx.BundleBoxing.Where(w => w.CorteCompleto == corte && w.Escaneado).ToList();


                    List<BundleBoxingCustom> lst = (from b in lstBundleBoxing
                                                    where b.CorteCompleto == corte && b.Escaneado
                                                    join u in _Cnx.Usuario on b.IdUsuario equals u.IdUsuario
                                                    join sc in _Cnx.BundleBoxing_Saco on b.IdSaco equals sc.IdSaco into LeftSaco
                                                    from lf in LeftSaco.DefaultIfEmpty()
                                                    orderby b.Seccion
                                                    select new BundleBoxingCustom()
                                                    {
                                                        Grupo = (!b.EnSaco) ? "Complementos" : string.Concat("Seccion# ㅤ", b.Seccion, "ㅤㅤㅤㅤㅤEstilo# ㅤ" + b.Estilo),
                                                        Mesa = b.NoMesa,
                                                        Serial = b.Serial,
                                                        Nombre = b.Nombre,
                                                        Talla = b.Talla,
                                                        Bulto = b.Bulto,
                                                        Capaje = b.Capaje,
                                                        Seccion = b.Seccion,
                                                        Saco = (lf == null) ? (int?)null : lf.Saco,
                                                        Yarda = b.Yarda,
                                                        Corte = b.Corte,
                                                        Estilo = b.Estilo,
                                                        Login = u.Login,
                                                        Fecha = b.FechaRegistro

                                                    }).ToList();

                    //List<BundleBoxingCustom> lst = (from b in lstBundleBoxing
                    //                                join p in _Cnx.POrder on b.Corte equals p.POrder1
                    //                                join s in _Cnx.Bundle on new { ID = p.Id_Order, Bld = b.Bulto } equals new { ID = (s.Id_Order == null) ? 0 : (int)s.Id_Order , Bld = (s.Bld == null) ? 0 : (int)s.Bld } into BundleUnion
                    //                                from bu in BundleUnion.DefaultIfEmpty()
                    //                                where p.POrderClient == corte && b.Activo
                    //                                join u in _Cnx.Usuario on b.IdUsuario equals u.IdUsuario
                    //                                join sc in _Cnx.BundleBoxing_Saco on b.IdSaco equals sc.IdSaco into LeftSaco
                    //                                from lf in LeftSaco.DefaultIfEmpty()
                    //                                orderby b.Seccion
                    //                                select new BundleBoxingCustom()
                    //                                {
                    //                                    Grupo = (!b.EnSaco) ? "Complementos" : string.Concat("Seccion# ㅤ", b.Seccion, "ㅤㅤㅤㅤㅤEstilo# ㅤ" + b.Estilo + "ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤMesa # ㅤ", b.NoMesa),
                    //                                    Mesa = b.NoMesa,
                    //                                    Serial = b.Serial,
                    //                                    Nombre = b.Nombre,
                    //                                    Talla = (bu == null) ? string.Empty : bu.Size,
                    //                                    Bulto = (b.EnSaco)? b.Bulto : 0,
                    //                                    Capaje = b.Capaje,
                    //                                    Seccion = b.Seccion,
                    //                                    Saco = (lf == null) ? (int?)null : lf.Saco,
                    //                                    Yarda = b.Yarda,
                    //                                    Corte = b.Corte,
                    //                                    Estilo = b.Estilo,
                    //                                    Login = u.Login,
                    //                                    Fecha =   b.FechaRegistro

                    //                                }).ToList();


                    List<BundleBoxingCustom> lstGrupo = (List<BundleBoxingCustom>)(from datos in lst
                                                        group datos by new  { datos.Grupo, datos.Mesa, datos.Nombre, datos.Talla, datos.Seccion, datos.Saco, datos.Corte, datos.Estilo, datos.Capaje, datos.Bulto, datos.Yarda, datos.Login, datos.Fecha} into grupo
                                                        orderby grupo.Key.Grupo, grupo.Key.Mesa
                                                                                   select new BundleBoxingCustom()
                                                                                    {
                                                                                        Grupo = string.Concat(grupo.Key.Grupo, "ㅤㅤㅤBultos/Rollos : ㅤ", lstBundleBoxing.Where(w => w.Corte == grupo.Key.Corte && w.NoMesa == grupo.Key.Mesa && w.Seccion == grupo.Key.Seccion && w.Escaneado).GroupBy(g => new { g.Bulto, g.Talla, g.Capaje, g.Nombre }).Count()),
                                                                                       //Grupo = string.Concat(grupo.Key.Grupo, "ㅤㅤㅤBultos/Rollos : ㅤ", lstBundleBoxing.Where(w => w.Corte == grupo.Key.Corte && w.NoMesa == grupo.Key.Mesa && w.Seccion == grupo.Key.Seccion && w.Activo).GroupBy(g => new { g.Bulto, g.Talla, g.Capaje }).Count(), "ㅤㅤㅤCapaje : ㅤ", lstBundleBoxing.Where(w => w.Corte == grupo.Key.Corte && w.NoMesa == grupo.Key.Mesa && w.Seccion == grupo.Key.Seccion && w.Activo).GroupBy(g => new { g.Bulto, g.Talla, g.Capaje }).Sum(s => s.Key.Capaje), "ㅤㅤㅤYarda : ㅤ", lstBundleBoxing.Where(w => w.Corte == grupo.Key.Corte && w.NoMesa == grupo.Key.Mesa && w.Seccion == grupo.Key.Seccion && w.Activo).Sum(s => s.Yarda)),
                                                                                       //Grupo = grupo.Key.Grupo,
                                                                                       Mesa = grupo.Key.Mesa,
                                                                                        Nombre = grupo.Key.Nombre,
                                                                                        Talla = grupo.Key.Talla,
                                                                                        Bulto = grupo.Key.Bulto,
                                                                                        Capaje = grupo.Key.Capaje,
                                                                                        Seccion = grupo.Key.Seccion,
                                                                                        Saco = grupo.Key.Saco,
                                                                                        Yarda = grupo.Key.Yarda,
                                                                                        Corte = grupo.Key.Corte,
                                                                                        Estilo = grupo.Key.Estilo,
                                                                                        Login = grupo.Key.Login,
                                                                                        Fecha = grupo.Key.Fecha
                                                                                    }).ToList();


                    json = Cls.Cls_Mensaje.Tojson(lstGrupo, lstGrupo.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }






        [Route("api/Inventario/BundleBoxing/Pieza")]
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

                List<BundleBoxingCustom> BoxingCustom = new List<BundleBoxingCustom>();

                DateTime Fecha = DateTime.Now;
                Nullable<int> IdSaco = null;


                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {


                        BundleBoxing_Saco RegisttroSaco = _Conexion.BundleBoxing_Saco.FirstOrDefault(f => f.Saco == Datos.Saco && f.NoMesa == Datos.Mesa && f.Corte.Equals(Datos.Corte));
                        int Idusuario = _Conexion.Usuario.First(f => f.Login.Equals(Datos.Login)).IdUsuario;


                        if(Datos.EnSaco)
                        {
                            if (!RegisttroSaco.Abierto)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{ Datos.Saco}</b> se encuentra cerrado.", 1);
                                return json;
                            }

                            if(_Conexion.BundleBoxingEnvio.FirstOrDefault(f => f.Serial == RegisttroSaco.Serial) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{ Datos.Saco}</b> se encuentra en proceso de envio.", 1);
                                return json;
                            }
                        }
                       




                        BundleBoxing Boxing = _Conexion.BundleBoxing.FirstOrDefault(b => b.Corte == Datos.Corte && b.Serial == Datos.Serial && b.Oper == Datos.Oper);

                        if (Boxing == null)
                        {

                            json = Cls.Cls_Mensaje.Tojson(null, -1, string.Empty, $"Serial # <b>{Datos.Serial}</b> no encontrado.", 0);
                            return json;
                        }

                        if (Boxing.Escaneado)
                        {

                            json = Cls.Cls_Mensaje.Tojson(Boxing, 0, string.Empty, $"Serial # <b>{Datos.Serial}</b> ya escaneado.", 0);
                            return json;
                        }

                        IdSaco = (!Datos.EnSaco) ? 0 : _Conexion.BundleBoxing_Saco.FirstOrDefault(sc => sc.Corte == Datos.Corte && sc.Seccion == Datos.Seccion && sc.NoMesa == Datos.Mesa && sc.Saco == Datos.Saco).IdSaco;

                        if(Datos.Oper != string.Empty)
                        {
                            foreach (BundleBoxing b in _Conexion.BundleBoxing.Where(w => w.Oper.Equals(Datos.Oper) && w.Bulto == Datos.Bulto && w.Serial != Datos.Serial && !w.Escaneado))
                            {
                                b.Escaneado = true;
                                b.Seccion = Datos.Seccion;
                                b.IdSaco = IdSaco;
                                b.NoMesa = Datos.Mesa;
                                b.FechaRegistro = Fecha;
                                b.IdUsuario = Idusuario;

                                BoxingCustom.Add(new BundleBoxingCustom
                                {
                                    Serial = b.Serial,
                                    Escaneado = true,
                                    Saco = Datos.Saco,
                                    Mesa = Datos.Mesa
                                });
                            }

                        }
                        


                        Boxing.Escaneado = true;
                        Boxing.Seccion = Datos.Seccion;
                        Boxing.IdSaco = IdSaco;
                        Boxing.NoMesa = (Boxing.NoMesa == 0) ? Datos.Mesa : Boxing.NoMesa;
                        Boxing.FechaRegistro = Fecha;
                        Boxing.IdUsuario = Idusuario;

                        BoxingCustom.Add(new BundleBoxingCustom
                        {
                            Serial = Boxing.Serial,
                            Escaneado = true,
                            Saco = Datos.Saco,
                            Mesa = Datos.Mesa
                        });


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



        [Route("api/Inventario/BundleBoxing/GenerarSerial")]
        [HttpPost]
        public IHttpActionResult GenerarSerial(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarSerial(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarSerial(string d)
        {
            string json = string.Empty;

            try
            {
                SerialBoxingCustom Datos = JsonConvert.DeserializeObject<SerialBoxingCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {
                        SerialComplemento Registro = new SerialComplemento
                        {
                            Serial = string.Empty,
                            Pieza = Datos.Pieza.TrimStart().TrimEnd().ToUpper(),
                            IdPresentacionSerial = Datos.IdPresentacionSerial,
                            NoMesa = Datos.NoMesa,
                            IdMaterial = Datos.IdMaterial,
                            Cantidad = Datos.Cantidad,
                            Capaje = Datos.Capaje,
                            EnSaco = Datos.Ensaco,
                            IdUsuarioCrea = _Conexion.Usuario.FirstOrDefault(u => u.Login == Datos.Login).IdUsuario,
                            FechaRegistro = DateTime.Now,
                            Corte = Datos.Corte,
                            CorteCompleto = Datos.CorteCompleto,
                            Estilo = Datos.Estilo,
                            Activo = true
                        };

                        _Conexion.SerialComplemento.Add(Registro);



                        _Conexion.SaveChanges();

                        Registro.Serial = Datos.Serial.Replace("0", string.Empty);
                        Registro.Serial = string.Concat(Registro.Serial, Registro.IdSerialComplemento);
                        Registro.Serial = Registro.Serial.PadRight(11, '0');

                        BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                        Image img = b.Encode(BarcodeLib.TYPE.UPCA, Registro.Serial, Color.Black, Color.White, 290, 120);
                        Registro.Serial = b.RawData.ToString();
                        Datos.Serial = Registro.Serial;
                        _Conexion.SaveChanges();





                        BundleBoxing Boxing = new BundleBoxing
                        {
                            NoMesa = Registro.NoMesa,
                            Serial = Registro.Serial,
                            Nombre = Registro.Pieza,
                            Seccion = 0,
                            Bulto = Registro.Cantidad,
                            Capaje = (_Conexion.PresentacionSerial.First(f => f.IdPresentacionSerial == Datos.IdPresentacionSerial).EsUnidad) ? Registro.Capaje : 0,
                            Yarda = (_Conexion.PresentacionSerial.First( f => f.IdPresentacionSerial == Datos.IdPresentacionSerial).EsUnidad) ? 0 : Registro.Capaje,
                            EnSaco = Registro.EnSaco,
                            IdSaco = 0,
                            Talla = string.Empty,
                            Corte = Registro.Corte,
                            CorteCompleto = Registro.CorteCompleto,
                            Estilo = Registro.Estilo,
                            Oper = string.Empty,
                            IdUsuario = _Conexion.Usuario.FirstOrDefault(u => u.Login == Datos.Login).IdUsuario,
                            FechaRegistro = DateTime.Now,
                            Escaneado = true
                        };

                        _Conexion.BundleBoxing.Add(Boxing);
                        _Conexion.SaveChanges();


                        scope.Complete();

                        json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, $"Serial # <b>{Datos.Serial}</b> generado.", 0);

                    }
                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }



            return json;
        }







        [Route("api/Inventario/BundleBoxing/GetEnvio")]
        [HttpGet]
        public string GetEnvio(string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<BundleBoxingEnvioCustom> lst = (from b in _Cnx.BundleBoxingEnvio
                                                    where b.CorteCompleto == corte && b.Activo
                                                    join u in _Cnx.Usuario on b.IdUsuario equals u.IdUsuario
                                                    select new BundleBoxingEnvioCustom()
                                                    {
                                                        IdEnvio = b.IdEnvio,
                                                        CorteCompleto = b.CorteCompleto,
                                                        Corte = b.Corte,
                                                        Serial = b.Serial,
                                                        IdSaco = b.IdSaco,
                                                        Saco = (b.Saco == 0)? (int?) null: b.Saco,
                                                        Bulto = (b.Bulto == 0) ? (int?)null : b.Bulto,
                                                        Yarda = (b.Yarda == 0) ? (int?)null : b.Yarda,
                                                        Polin = b.Polin,
                                                        Fecha = b.Fecha,
                                                        Login = u.Login,
                                                        Activo = b.Activo

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




        [Route("api/Inventario/BundleBoxing/GuardarEnvio")]
        [HttpPost]
        public IHttpActionResult GuardarEnvio(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarSerialEnvio(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarSerialEnvio(string d)
        {
            string json = string.Empty;

            try
            {
                BundleBoxingEnvioCustom Datos = JsonConvert.DeserializeObject<BundleBoxingEnvioCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                    {

                        if(_Cnx.BundleBoxingEnvio.FirstOrDefault( f => f.Serial.Equals(Datos.Serial) && f.CorteCompleto.Equals(Datos.CorteCompleto)) != null)
                        {

                            json = Cls.Cls_Mensaje.Tojson(null, 1, string.Empty, $"Serial # <b>{Datos.Serial}</b> escaneado.", 0);
                            return json;
                        }

                        BundleBoxingEnvio Registro = new BundleBoxingEnvio();

                        BundleBoxing_Saco _Saco = _Cnx.BundleBoxing_Saco.ToList().Find(f => f.Serial.Equals(Datos.Serial) && f.CorteCompleto.Equals(Datos.CorteCompleto));


                        Registro.Serial = Datos.Serial;
                       
                        Registro.Polin = Datos.Polin;
                        Registro.Fecha = Datos.Fecha;
                        Registro.IdUsuario = _Cnx.Usuario.FirstOrDefault(f => f.Login.Equals(Datos.Login)).IdUsuario;
                        Registro.Activo = true;

                        if (_Saco != null)
                        {
                            List<BundleBoxing> lst = _Cnx.BundleBoxing.Where(w => w.IdSaco == _Saco.IdSaco).ToList();


                            if(lst.Count == 0)
                            {
                                Registro.Activo = false;
                                json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, $"El Saco # <b>{_Saco.Saco}</b> se encuentra vacio.", 1);
                                return json;
                            }


                            Registro.CorteCompleto = _Saco.CorteCompleto;
                            Registro.Corte = _Saco.Corte;
                            Registro.IdSaco = _Saco.IdSaco;
                            Registro.Saco = _Saco.Saco;
                            Registro.Bulto = 0;
                            Registro.Bulto = lst.Where(w => w.Corte == Registro.Corte  && w.Escaneado).GroupBy(g =>  g.Bulto).Count();
                        }
                        else
                        {
                            BundleBoxing _Boxin = _Cnx.BundleBoxing.ToList().Find(f => f.Serial.Equals(Datos.Serial) && f.CorteCompleto.Equals(Datos.CorteCompleto) && !f.EnSaco);

                            if(_Boxin == null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, $"Serial # <b>{Datos.Serial}</b> no encontrado.", 0);
                                return json;
                            }

                            if(!_Boxin.Escaneado)
                            {
                                Registro.Activo = false;
                                json = Cls.Cls_Mensaje.Tojson(Registro, 1, string.Empty, $"El Serial # <b>{Datos.Serial}</b> debe de pasar por un proceso de escaneo.", 0);
                                return json;
                            }

                            Registro.CorteCompleto = _Boxin.CorteCompleto;
                            Registro.Corte = _Boxin.Corte;
                            Registro.Yarda = _Boxin.Yarda;
                        }

                        _Cnx.BundleBoxingEnvio.Add(Registro);

                        _Cnx.SaveChanges();
                       

                        BundleBoxingEnvioCustom Envio = new BundleBoxingEnvioCustom
                        {
                            IdEnvio = Registro.IdEnvio,
                            CorteCompleto = Registro.CorteCompleto,
                            Corte = Registro.Corte,
                            Serial = Registro.Serial,
                            IdSaco = Registro.IdSaco,
                            Saco = (Registro.Saco == 0) ? (int?)null : Registro.Saco,
                            Bulto = (Registro.Bulto == 0) ? (int?)null : Registro.Bulto,
                            Yarda = (Registro.Yarda == 0) ? (int?)null : Registro.Yarda,
                            Polin = Registro.Polin,
                            Fecha = Registro.Fecha,
                            Login = Datos.Login,
                            Activo = Registro.Activo
                        };



                     

                        json = Cls.Cls_Mensaje.Tojson(Envio, 1, string.Empty, $"Serial # <b>{Datos.Serial}</b> escaneado.", 0);

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
