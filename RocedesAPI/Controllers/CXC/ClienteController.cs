using Newtonsoft.Json;
using RocedesAPI.Models;
using RocedesAPI.Models.Cls.CXC;
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

namespace RocedesAPI.Controllers.CXC
{
    public class ClienteController : ApiController
    {
        [Route("api/CXC/Cliente/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<ClienteCustom> lst = (from q in _Conexion.Cliente
                                                 select new ClienteCustom()
                                                 {
                                                     IdCliente = q.Id_Cliente,
                                                     Cliente = q.Cliente1,
                                                     Estado = (bool)q.Estado
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


        [Route("api/CXC/Cliente/Guardar")]
        [HttpPost]
        public IHttpActionResult Guardar(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(_Guardar(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string _Guardar(string d)
        {
            string json = string.Empty;


            try
            {
                ClienteCustom Datos = JsonConvert.DeserializeObject<ClienteCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Cliente Registro = null;

                        if (Datos.IdCliente == -1)
                        {
                            if (_Conexion.Cliente.FirstOrDefault(f => f.Cliente1.ToLower().Equals(Datos.Cliente.ToLower()) && f.Id_Cliente != Datos.IdCliente) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre del cliente ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new Cliente
                            {
                                Cliente1 = Datos.Cliente.ToUpper(),
                                Estado = Datos.Estado
                            };
                            _Conexion.Cliente.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdCliente = Registro.Id_Cliente;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {
                            Registro = _Conexion.Cliente.Find(Datos.IdCliente);

                            Registro.Cliente1 = Datos.Cliente.ToUpper();
                            Registro.Estado = Datos.Estado;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);

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
    }
}