using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocedesAPI.Conex_Pervasive;
using RocedesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using RocedesAPI.Models.Cls.INV;

namespace RocedesAPI.Controllers.INV
{
    public class SerialComponenteController : ApiController
    {
        [Route("api/Inventario/SerialComponente/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<SerialComponenteCustom> lst = (from q in _Cnx.SerialComplemento
                                                        join m in _Cnx.Material on q.IdMaterial equals m.IdMaterial
                                                        join p in _Cnx.PresentacionSerial on q.IdPresentacionSerial equals p.IdPresentacionSerial
                                                        join u in _Cnx.Usuario on  (q.Activo? q.IdUsuarioCrea : q.IdUsuarioInactiva) equals u.IdUsuario
                                                          select new SerialComponenteCustom()
                                                          {
                                                              IdSerialComplemento = q.IdSerialComplemento,
                                                              Serial = q.Serial,
                                                              Pieza = q.Pieza,
                                                              IdPresentacionSerial = q.IdPresentacionSerial,
                                                              PresentacionSerial = p.Presentacion,
                                                              IdMaterial = q.IdMaterial,
                                                              Material = m.Material1,
                                                              Cantidad = q.Cantidad,
                                                              Capaje = q.Capaje,
                                                              EnSaco = q.EnSaco,
                                                              IdUsuarioCrea = (q.Activo) ? q.IdUsuarioCrea : q.IdUsuarioInactiva,
                                                              Usuario = u.Login,
                                                              FechaRegistro = (q.Activo) ? q.FechaRegistro : q.FechaInactivo,
                                                              Corte = q.Corte,
                                                              CorteCompleto = q.CorteCompleto,
                                                              Estilo = q.Estilo,
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


        [Route("api/Inventario/SerialComponente/Eliminar")]
        [HttpPost]
        public IHttpActionResult Eliminar(string serial, string login)
        {
            if (ModelState.IsValid)
            {

                return Ok(EliminarSerial(serial, login));

            }
            else
            {
                return BadRequest();
            }

        }

        private string EliminarSerial(string serial, string login)
        {
            string json = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {



                        SerialComplemento Registro = (SerialComplemento)_Conexion.SerialComplemento.FirstOrDefault( w => w.Serial.Equals(serial));

                        Registro.Activo = false;
                        Registro.IdUsuarioInactiva = _Conexion.Usuario.Where(w => w.Login.Equals(login)).First().IdUsuario;
                        Registro.FechaInactivo = DateTime.Now;


                        json = Cls.Cls_Mensaje.Tojson(Registro, 1, string.Empty, $"Serial # <b>{serial}</b> eliminado.", 0);
      

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