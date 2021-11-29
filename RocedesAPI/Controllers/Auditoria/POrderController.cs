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
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    var _Query = (from q in _Cnx.POrder
                                  join  s in _Cnx.Style on q.Id_Style equals s.Id_Style
                                 where q.POrder1.TrimEnd().TrimStart().ToLower().StartsWith(corte.TrimStart().TrimEnd().ToLower())
                                 orderby q.POrder1, q.POrder1.TrimStart().TrimEnd().Length 
                                  select new
                                 {
                                     Corte = q.POrder1,
                                     Style = s.Style1
                                 }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(_Query, _Query.Count, string.Empty, string.Empty, 0);


                }
            }
            catch(Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

           


                return json;
        }


        [Route("api/Auditoria/GetSerial2")]
        [HttpGet]
        public string GetSerial2(string corte, string estilo)
        {
            string json = string.Empty;
            //MP350028-1
            try
            {
                string sql = $"SELECT S.serialno, OP.Descr, S.bundleno, BD.qty, S.prodno, S.operno  \n" +
                    $"FROM SERIAL2 AS S \n" +
                    $"INNER JOIN OPER AS OP ON OP.operno = S.operno \n" +
                     $"INNER JOIN BUNDLE AS BD ON BD.prodno = S.prodno AND BD.bundleno = S.bundleno \n" +
                    $"WHERE S.prodno = '{corte}' " +
                    $"GROUP BY  S.serialno,  OP.Descr, S.bundleno, BD.qty, S.prodno, S.operno";

                Cls_ConexionPervasive _Cnx = new Cls_ConexionPervasive();
                DataTable tbl = _Cnx.GetDatos(sql, out json);

               
                if(tbl != null)
                {
                    if (tbl.Rows.Count > 0)
                    {



                        using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                        {
                            List<BundleBoxing> lstBundleBoxing = _Conexion.BundleBoxing.ToList().FindAll(b => b.Corte == corte && b.Activo).ToList();


                            var ltsBoxingCustom = (from s in tbl.AsEnumerable()
                                                   join b in lstBundleBoxing on new { _Corte = s.Field<string>("prodno"), Serial = s.Field<int>("serialno")} equals new { _Corte = b.Corte, Serial = b.Serial} into SerialesUnion
                                                   from sb in SerialesUnion.DefaultIfEmpty()
                                                   join sc in _Conexion.BundleBoxing_Saco on (sb == null ? 0 : sb.IdSaco) equals sc.IdSaco into SacoUnion
                                                   from sc in SacoUnion.DefaultIfEmpty()
                                                   select new
                                                   {
                                                       Serial = s.Field<int>("serialno"),
                                                       Nombre = s.Field<string>("Descr").TrimStart().TrimEnd(),
                                                       Bulto = s.Field<int>("bundleno"),
                                                       Capaje = s.Field<Int16>("qty"),
                                                       Saco = (sb != null) ? sc.Saco : 0,
                                                       Mesa = (sc != null) ? sb.NoMesa : 0,
                                                       Corte = s.Field<string>("prodno").TrimStart().TrimEnd(),
                                                       Estilo = (sb != null) ? SerialesUnion.First( x => x.Corte == s.Field<string>("prodno").TrimStart().TrimEnd()).Estilo : estilo,
                                                       Oper = s.Field<string>("operno").TrimStart().TrimEnd(),
                                                       Escaneado = (sb != null) ? true : false
                                                   }).ToList();

           



                            json = Cls.Cls_Mensaje.Tojson(ltsBoxingCustom, ltsBoxingCustom.Count, string.Empty, string.Empty, 0);
                        }




                    }

                    else
                        json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro no encontrado.", 0);
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