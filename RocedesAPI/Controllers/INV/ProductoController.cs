using Newtonsoft.Json;
using RocedesAPI.Models;
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

namespace RocedesAPI.Controllers.INV
{
    public class ProductoController : ApiController
    {

        [Route("api/Inventario/Producto/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    List<ProductoCustom> lst = (from q in _Conexion.Product
                                                orderby q.Code
                                                 select new ProductoCustom()
                                                 {
                                                     IdProducto = q.IdProducto,
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

    

        [Route("api/Inventario/Producto/Guardar")]
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
                ProductoCustom Datos = JsonConvert.DeserializeObject<ProductoCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        Product Registro = null;

                        if (Datos.IdProducto == -1)
                        {
                            if (_Conexion.Product.FirstOrDefault(f => f.Nombre.ToLower().Equals(Datos.Nombre.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre del producto ya se ecnuentra registrado.", 1);
                                return json;
                            }

                            if (_Conexion.Product.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower())) != null)
                            {
                                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo ya se ecnuentra registrado.", 1);
                                return json;
                            }
                            Registro = new Product
                            {
                                Nombre = Datos.Nombre.ToUpper(),
                                Code = Datos.Code.ToUpper()
                            };
                            _Conexion.Product.Add(Registro);
                            _Conexion.SaveChanges();

                            Datos.IdProducto = Registro.IdProducto;

                            json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                        {

                           
                            Registro = _Conexion.Product.Find(Datos.IdProducto);

                            if (Datos.Evento == "Eliminar")
                            {
                                _Conexion.Product.Remove(Registro);
                                json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Eliminado.", 0);

                            }
                            else
                            {
                                if (_Conexion.Product.FirstOrDefault(f => f.Nombre.ToLower().Equals(Datos.Nombre.ToLower()) && f.IdProducto != Datos.IdProducto) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El nombre del producto ya se ecnuentra registrado.", 1);
                                    return json;
                                }

                                if (_Conexion.Product.FirstOrDefault(f => f.Code.ToLower().Equals(Datos.Code.ToLower()) && f.IdProducto != Datos.IdProducto) != null)
                                {
                                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", "El codigo ya se ecnuentra registrado.", 1);
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
    }
}