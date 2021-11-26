using Newtonsoft.Json;
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






        [Route("api/BundleBoxing/CrearSaco")]
        public IHttpActionResult CrearSaco(string usuario, string corte, string seccion)
        {
            if (ModelState.IsValid)
            {

                return Ok(NuevoSaco(usuario,  corte, seccion));

            }
            else
            {
                return BadRequest();
            }

        }

        private string NuevoSaco(string usuario, string corte, string seccion)
        {
            string json = string.Empty;

            if (seccion == string.Empty) seccion = "0";



            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (var _Conexion = new RocedesEntities())
                    {

                        int IdUsuario = _Conexion.Usuario.ToList().Find(u => u.Login == usuario).IdUsuario;

                        var _Saco = (from sc in _Conexion.BundleBoxing_Saco
                                      where sc.Corte == corte && sc.IdUsuarioAbre == IdUsuario && sc.Abierto
                                      select sc.Saco 
                                      ).ToList();


                        if(_Saco.Count == 0)
                        {
                            _Conexion.BundleBoxing_Saco.Add(new BundleBoxing_Saco
                            {
                                Saco = -1,
                                Corte = corte,
                                Seccion = int.Parse(seccion),
                                IdUsuarioCrea = IdUsuario,
                                FechaRegistro = DateTime.Now,
                                IdUsuarioAbre = IdUsuario,
                                Abierto = true,
                                Activo = true
                            });

                            BundleBoxing_Saco Registro = (BundleBoxing_Saco)_Conexion.BundleBoxing_Saco.Where(u => u.IdUsuarioCrea == IdUsuario && u.Saco == -1 && u.Seccion == int.Parse(seccion) && u.Corte == corte && u.Abierto);

                            Registro.Saco = _Conexion.BundleBoxing_Saco.Where(u => u.IdUsuarioCrea == IdUsuario && u.Saco == -1 && u.Seccion == int.Parse(seccion) && u.Corte == corte && u.Abierto).Max(m => m.Saco) + 1;


                            _Conexion.SaveChanges();


                            List<BundleBoxing_Saco> lst = new List<BundleBoxing_Saco>();
                            lst.Add(Registro);


                            json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, "Registro Guardado.", 0);

                        }
                        else
                        {
                            json = Cls.Cls_Mensaje.Tojson(null, 0, "1", $"El saco # {_Saco[0].ToString()} se encuentra abierto, Primero cierre el saco para crear uno nuevo.", 1);
                        }

                                     
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
