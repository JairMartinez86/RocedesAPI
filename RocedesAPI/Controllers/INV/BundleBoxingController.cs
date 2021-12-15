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



        [Route("api/BundleBoxing/GenerarSerial")]
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
                            Pieza = Datos.Pieza.TrimStart().TrimEnd(),
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


  


    }
      
}
