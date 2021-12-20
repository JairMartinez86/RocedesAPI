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


        [Route("api/Inventario/BundleBoxing/GetBundleBoxing")]
        [HttpGet]
        public string GetBundleBoxing(string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<BundleBoxingCustom> lst = (from b in _Cnx.BundleBoxing
                                                    join p in _Cnx.POrder on b.Corte equals p.POrder1
                                                    join s in _Cnx.Bundle on p.Id_Order equals s.Id_Order
                                                    where p.POrderClient == corte && b.Activo
                                                    join u in _Cnx.Usuario on b.IdUsuario equals u.IdUsuario
                                                    join sc in _Cnx.BundleBoxing_Saco on b.IdSaco equals sc.IdSaco into LeftSaco
                                                    from lf in LeftSaco.DefaultIfEmpty()
                                                    orderby b.Seccion
                                                    select new BundleBoxingCustom()
                                                    {
                                                        Grupo = (!b.EnSaco) ? "Complementos" : string.Concat("Seccion# ㅤ", b.Seccion, "ㅤㅤㅤㅤㅤEstilo# ㅤ" + b.Estilo + "ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤMesa # ㅤ", b.NoMesa),
                                                        Mesa = b.NoMesa,
                                                        Serial = b.Serial,
                                                        Nombre = b.Nombre,
                                                        Talla = s.Size,
                                                        Bulto = b.Bulto,
                                                        Capaje = b.Capaje,
                                                        Seccion = b.Seccion,
                                                        Saco = (lf == null) ? (int?)null : lf.Saco,
                                                        Yarda = b.Yarda,
                                                        Corte = b.Corte,
                                                        Estilo = b.Estilo,
                                                        Login = u.Login,
                                                        Fecha =   b.FechaRegistro

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

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {


                        BundleBoxing_Saco RegisttroSaco = _Conexion.BundleBoxing_Saco.FirstOrDefault(f => f.Saco == Datos.Saco && f.NoMesa == Datos.Mesa && f.Corte.Equals(Datos.Corte));


                        if(Datos.EnSaco)
                        {
                            if (!RegisttroSaco.Abierto)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # <b>{ Datos.Saco}</b> se encuentra cerrado", 1);
                                return json;
                            }
                        }
                       




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
                                Yarda = Datos.Yarda,
                                EnSaco = Datos.EnSaco,
                                IdSaco = (!Datos.EnSaco)? (int?)null : _Conexion.BundleBoxing_Saco.FirstOrDefault(sc => sc.Saco == Datos.Saco && sc.CorteCompleto == Datos.CorteCompleto && sc.Seccion == Datos.Seccion && sc.Activo).IdSaco,
                                Corte = Datos.Corte,
                                CorteCompleto = Datos.CorteCompleto,
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
                            Saco = (!Datos.EnSaco)? 0 :  _Conexion.BundleBoxing_Saco.FirstOrDefault(sc => sc.IdSaco == Boxing.IdSaco && sc.Corte == Boxing.Corte && sc.Seccion == Boxing.Seccion).Saco,
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
                                                    where b.Corte == corte && b.Activo
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
                            Registro.CorteCompleto = _Saco.CorteCompleto;
                            Registro.Corte = _Saco.Corte;
                            Registro.IdSaco = _Saco.IdSaco;
                            Registro.Saco = _Saco.Saco;
                            Registro.Bulto = _Cnx.BundleBoxing.Where(w => w.IdSaco == _Saco.IdSaco).Sum(s => s.Bulto);
                        }
                        else
                        {
                            BundleBoxing _Boxin = _Cnx.BundleBoxing.ToList().Find(f => f.Serial.Equals(Datos.Serial) && f.CorteCompleto.Equals(Datos.CorteCompleto) && !f.EnSaco);

                            if(_Boxin == null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, $"Serial # <b>{Datos.Serial}</b> no encontrado.", 0);
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
