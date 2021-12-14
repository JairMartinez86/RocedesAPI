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

namespace RocedesAPI.Controllers
{
    public class UsuarioController : ApiController
    {
        //GET api/Get
        [HttpGet]
        public string Get(string usr, string pwd)
        {
            return Login(usr, pwd);
        }

        private string Login(string usr, string pwd)
        {

            string json = string.Empty;
            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {

                    var query = (from _q in _Conexion.Usuario.AsEnumerable()
                                 where _q.Activo == true && _q.Login == usr && _q.Pass == pwd
                                 select new
                                 {
                                     Nombre = string.Concat(_q.Nombres, " ", _q.Apellidos),
                                     Fecha = DateTime.Now
                                 }).ToList();


                    if(query.Count == 0)
                    {
                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Usuario y/o contraseña invalida.", 1);
                    }
                    else
                    {
                        json = Cls.Cls_Mensaje.Tojson(query, query.Count, string.Empty, string.Empty, 0);
                    }


                    
                }
             
            }
            catch(Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }
            
            return json;
        }




        
        [HttpPost]
        public IHttpActionResult Inicio(string usr, string pwd)
        {
            if (ModelState.IsValid)
            {
              
                return Ok(Login(usr, pwd));

            }
            else
            {
                return BadRequest();
            }



        }




        //GET api/GetCodBar
        [HttpGet]
        public string GetCodBar(string codbar)
        {

            string json = string.Empty;

            try
            {
                string sql = "select top 1 empno,estatus,module,lastname,firstname,badgeno,loc1,loc2 from employee where badgeno = '" + codbar + "'";

                Cls_ConexionPervasive _Cnx = new Cls_ConexionPervasive();
                DataTable tbl = _Cnx.GetDatos(sql, out json);

                if(tbl != null)
                {
                    List<Usuario> lst = (from q in tbl.AsEnumerable()
                                         select new Usuario()
                                         {
                                             Login = q.Field<string>("EMPNO"),
                                             Nombres = q.Field<string>("firstname"),
                                             Apellidos = q.Field<string>("lastname")
                                         }).ToList();

                    if (lst.Count > 0)
                        json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);
                    else
                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro no encontrado.", 0);
                }
               

            }
            catch(Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }


            return json;

        }



        //GET api/Registros
        [Route("api/Usuario/Registros")]
        [HttpGet]
        public string Registros()
        {

            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                {
                    List<Usuario> lst = _Conexion.Usuario.ToList();

                    json = Cls.Cls_Mensaje.Tojson(lst, lst.Count, string.Empty, string.Empty, 0);
                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }


            return json;

        }




        [Route("api/Usuario/NuevoUsuario")]
        public IHttpActionResult NuevoUsuario(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(Nuevo(d));

            }
            else
            {
                return BadRequest();
            }



        }

        private string Nuevo(string d)
        {
            string json = string.Empty;

            Usuario Registro = JsonConvert.DeserializeObject<Usuario>(d);


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {

                try
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {
                        int x = _Conexion.Usuario.ToList().FindAll(s => s.Login == Registro.Login.TrimStart().TrimEnd() ||  (s.Login != Registro.Login && s.CodBar != string.Empty && s.CodBar == Registro.CodBar.TrimStart().TrimEnd())).Count();

                        if(x == 0)
                        {
                            _Conexion.Usuario.Add(new Usuario
                            {
                                Login = Registro.Login.TrimStart().TrimEnd(),
                                Pass = Registro.Pass.TrimStart().TrimEnd(),
                                Nombres = Registro.Nombres.TrimStart().TrimEnd(),
                                Apellidos = Registro.Apellidos.TrimStart().TrimEnd(),
                                CodBar = Registro.CodBar.TrimStart().TrimEnd(),
                                Activo = true

                            });

                            _Conexion.SaveChanges();
                            scope.Complete();
                            scope.Dispose();
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro Guardado.", 0);
                        }
                        else
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "El Usuario ya se encuentra registrado.", 1);


                    }
                }
                catch(Exception ex)
                {
                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
                }
                
            }


            return json;

        }

        [Route("api/Usuario/EditarUsuario")]
        public IHttpActionResult EditarUsuario(string d)
        {
            if (ModelState.IsValid)
            {

                return Ok(Editar(d));

            }
            else
            {
                return BadRequest();
            }



        }
        private string Editar(string d)
        {
            string json = string.Empty;

            Usuario Registro = JsonConvert.DeserializeObject<Usuario>(d);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {

                try
                {
                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {
                        Usuario u = _Conexion.Usuario.ToList().Find(s => s.IdUsuario == Registro.IdUsuario);

                        if(u.Login == "JMartinez" && u.Activo)
                        {
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "No se permite modificar este usuario.", 1);
                            return json;
                        }

                        if (u != null)
                        {
                            u.Pass = Registro.Pass.TrimStart().TrimEnd();
                            u.Activo = Registro.Activo;
                            _Conexion.SaveChanges();
                            scope.Complete();
                            scope.Dispose();
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, Registro.Activo ? "Cambios guardados." : "Usuario inactivo.", 0);
                        }
                        else
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "El Usuario ya se encuentra registrado.", 1); 


                    }
                }
                catch (Exception ex)
                {
                    json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
                }

            }


            return json;

        }
    }
}
