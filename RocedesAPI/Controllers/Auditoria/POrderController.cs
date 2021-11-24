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


namespace RocedesAPI.Controllers
{
    public class POrderController : ApiController
    {
       
        [Route("api/Auditoria/GetAutoPOrder")]
        [HttpGet]
        public string GetAutoPOrder(string corte)
        {
            string json = string.Empty;

            try
            {
                using (RocedesEntities _Cnx = new RocedesEntities())
                {
                    var _Query = (from q in _Cnx.POrder
                                 where q.POrder1.TrimEnd().TrimStart().ToLower().StartsWith(corte.TrimStart().TrimEnd().ToLower())
                                 orderby q.POrder1, q.POrder1.TrimStart().TrimEnd().Length 
                                  select new
                                 {
                                     Corte = q.POrder1
                                 }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(_Query, _Query.Count, string.Empty, string.Empty, 0);


                }
            }
            catch(Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.InnerException.Message, 1);
            }

           


                return json;
        }


        [Route("api/Auditoria/GetSerial2")]
        [HttpGet]
        public string GetSerial2(string corte)
        {
            string json = string.Empty;

            try
            {
                string sql = $"SELECT S.prodno, S.operno FROM SERIAL2  AS S WHERE RTRIM(LTRIM(S.prodno)) LIKE '%{corte}' AND LENGTH(S.prodno) = {corte.Length}";

                Cls_ConexionPervasive _Cnx = new Cls_ConexionPervasive();
                DataTable tbl = _Cnx.GetDatos(sql);

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
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.InnerException.Message, 1);
            }


            return json;

        }


    }
}