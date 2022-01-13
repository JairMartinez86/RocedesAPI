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
    public class FoleoController : ApiController
    {
        [Route("api/Premium/Foleo/Get")]
        [HttpGet]
        public string Get()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<FoleoFactorCustom> lst = (from p in _Cnx.FoleoProceso
                                                     join f in _Cnx.FoleoFactor on p.IdProcesoFoleo equals f.IdProcesoFoleo
                                                     group f by new { f.IdProcesoFoleo, p.Proceso, p.Orden, p.NoFactor } into grupo

                                                     orderby grupo.Key.Orden ascending
                                                     select new FoleoFactorCustom()
                                                     {
                                                         IdProcesoFoleo = grupo.Key.IdProcesoFoleo,
                                                         Orden = grupo.Key.Orden,
                                                         Proceso = grupo.Key.Proceso,
                                                         NoFactor = grupo.Key.NoFactor,
                                                         Factor1 = grupo.Where(c => c.NoFactor == 1).Sum(c => c.ValorFactor),
                                                         Factor2 = grupo.Where(c => c.NoFactor == 2).Sum(c => c.ValorFactor),
                                                         Factor3 = grupo.Where(c => c.NoFactor == 3).Sum(c => c.ValorFactor),
                                                         TotalFactor = grupo.Sum(c => c.ValorFactor),
                                                         Minutos = 0

                                                     }).ToList();

                    lst.ForEach(
                        e => {
                            e.Factor1 = (e.Factor1 == 0) ? (decimal?)null : e.Factor1;
                            e.Factor2 = (e.Factor2 == 0) ? (decimal?)null : e.Factor2;
                            e.Factor3 = (e.Factor3 == 0) ? (decimal?)null : e.Factor3;
                            e.TotalFactor = (e.TotalFactor == 0) ? (decimal?)null : e.TotalFactor;
                            e.Minutos = (e.Minutos == 0) ? (decimal?)null : e.Minutos;
                        }

                        );

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }




        [Route("api/Premium/Foleo/Guardar")]
        [HttpPost]
        public IHttpActionResult Guardar(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarFactor(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarFactor(string d)
        {
            string json = string.Empty;





            try
            {
                FoleoFactorCustom Datos = JsonConvert.DeserializeObject<FoleoFactorCustom>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        List<FoleoFactor> Registros = _Conexion.FoleoFactor.Where(w => w.IdProcesoFoleo == Datos.IdProcesoFoleo).ToList();

                        Registros.ForEach(f =>
                        {
                            switch (f.NoFactor)
                            {
                                case 1:
                                    f.ValorFactor = (decimal)((Datos.Factor1 == null) ? 0 : Datos.Factor1);
                                    break;
                                case 2:
                                    f.ValorFactor = (decimal)((Datos.Factor2 == null) ? 0 : Datos.Factor2);
                                    break;
                                case 3:
                                    f.ValorFactor = (decimal)((Datos.Factor3 == null) ? 0 : Datos.Factor3);
                                    break;

                            }

                        }
                        );





                        json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);

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





        [Route("api/Premium/Foleo/GetEstilo")]
        [HttpGet]
        public string GetEstilo(string filtro)
        {

            if (filtro == null) filtro = string.Empty;

            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    List<FoleoDatos> lst  = (from q in _Cnx.FoleoDatos
                                  where q.Estilo.ToLower().Contains(filtro.TrimStart().TrimEnd().ToLower())
                                  orderby q.Estilo, q.Estilo.Length
                                  select q
                                  ).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }




        [Route("api/Premium/Foleo/GetDato")]
        [HttpGet]
        public string GetDato()
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<FoleoDatos> lst =  _Cnx.FoleoDatos.ToList();
                                        
 
                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }




        [Route("api/Premium/Foleo/GuardarDato")]
        [HttpPost]
        public IHttpActionResult GuardarDato(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(GuardarDatos(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string GuardarDatos(string d)
        {
            string json = string.Empty;





            try
            {
                FoleoDatos Datos = JsonConvert.DeserializeObject<FoleoDatos>(d);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        FoleoDatos Registro;
                        if (Datos.IdFoleoDato == -1)
                        {
                            Registro = new FoleoDatos();
                            Registro.Estilo = Datos.Estilo.TrimStart().TrimEnd().ToUpper();
                            Registro.Pieza_grande = Datos.Pieza_grande;
                            Registro.Pieza_pequena = Datos.Pieza_pequena;
                            Registro.Pieza_doble = Datos.Pieza_doble;
                            _Conexion.FoleoDatos.Add(Registro);
                            Datos.IdFoleoDato = Registro.IdFoleoDato;
                        }
                        else
                        {

                            Registro = _Conexion.FoleoDatos.FirstOrDefault(f => f.IdFoleoDato == Datos.IdFoleoDato);
                            Registro.Estilo = Datos.Estilo;
                            Registro.Pieza_grande = Datos.Pieza_grande;
                            Registro.Pieza_pequena = Datos.Pieza_pequena;
                            Registro.Pieza_doble = Datos.Pieza_doble;
                        }



                        json = Cls.Cls_Mensaje.Tojson(Datos, 1, string.Empty, "Registro Guardado.", 0);

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






        [Route("api/Premium/Foleo/EliminarDato")]
        [HttpPost]
        public IHttpActionResult EliminarDato(int id)
        {
            if (ModelState.IsValid)
            {

                return Ok(EliminarDatos(id));

            }
            else
            {
                return BadRequest();
            }

        }

        private string EliminarDatos(int id)
        {
            string json = string.Empty;


            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {

                        FoleoDatos Registro = _Conexion.FoleoDatos.FirstOrDefault(f => f.IdFoleoDato == id);

                        if (Registro != null) _Conexion.FoleoDatos.Remove(Registro);


                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro Eliminado.", 0);

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