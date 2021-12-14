﻿using Newtonsoft.Json;
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
       
        [Route("api/Auditoria/GetAutoCorte")]
        [HttpGet]
        public string GetAutoCorte(string corte, bool esSeccion)
        {
            if (esSeccion)
                return GetAutoPOrderSeccion(corte);

            return GetAutoPOrder(corte);
        }

  
        public string GetAutoPOrderSeccion(string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    var _Query = (from q in _Cnx.POrder
                                  join s in _Cnx.Style on q.Id_Style equals s.Id_Style
                                  where q.POrder1.TrimEnd().TrimStart().ToLower().StartsWith(corte.TrimStart().TrimEnd().ToLower())
                                  orderby q.POrder1, q.POrder1.TrimStart().TrimEnd().Length
                                  select new
                                  {
                                      Corte = q.POrder1,
                                      CorteCompleto = q.POrderClient,
                                      Style = s.Style1
                                  }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(_Query, _Query.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }

        public string GetAutoPOrder(string corte)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {
                    var _Query = (from q in _Cnx.POrder
                                  join s in _Cnx.Style on q.Id_Style equals s.Id_Style
                                  where q.POrderClient.TrimEnd().TrimStart().ToLower().StartsWith(corte.TrimStart().TrimEnd().ToLower())
                                  group q by q.POrderClient into grupo
                                  orderby grupo.Key.TrimStart().TrimEnd().Length
                                  select new
                                  {
                                      Corte = grupo.Key
                                  }).Take(20).ToList();

                    json = Cls.Cls_Mensaje.Tojson(_Query, _Query.Count, string.Empty, string.Empty, 0);


                }
            }
            catch (Exception ex)
            {
                json = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }




            return json;
        }




        [Route("api/Auditoria/GetSerial2")]
        [HttpGet]
        public string GetSerial2(string corte, string estilo, bool esComplemento)
        {
            string json = string.Empty;
 
            try
            {
                string sql = string.Empty;

                if (!esComplemento)
                {
                    sql = $"SELECT S.serialno, OP.Descr, S.bundleno, BD.qty, S.prodno, S.operno  \n" +
                    $"FROM SERIAL2 AS S \n" +
                    $"INNER JOIN OPER AS OP ON OP.operno = S.operno \n" +
                     $"INNER JOIN BUNDLE AS BD ON BD.prodno = S.prodno AND BD.bundleno = S.bundleno \n" +
                    $"WHERE S.prodno = '{corte}' " +
                    $"GROUP BY  S.serialno,  OP.Descr, S.bundleno, BD.qty, S.prodno, S.operno";

                    Cls_ConexionPervasive _Cnx = new Cls_ConexionPervasive();
                    DataTable tbl = _Cnx.GetDatos(sql, out json);

                    if (tbl != null)
                    {

                        DataTableExt.ConvertColumnType(tbl, "serialno", typeof(string));


                        if (tbl.Rows.Count > 0)
                        {

                            using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                            {
                                List<BundleBoxing> lstBundleBoxing = _Conexion.BundleBoxing.ToList().FindAll(b => b.Corte.Equals(corte) && b.EnSaco && b.Activo).ToList();



                                List<BundleBoxingCustom> ltsBoxingCustom = (from s in tbl.AsEnumerable()
                                                                            join p in _Conexion.POrder on s.Field<string>("prodno").TrimStart().TrimEnd() equals p.POrder1.TrimEnd().TrimStart()
                                                                            join b in lstBundleBoxing on new { _Corte = s.Field<string>("prodno"), _Serial = s.Field<string>("serialno") } equals new { _Corte = b.Corte, _Serial = b.Serial } into SerialesUnion
                                                                            from sb in SerialesUnion.DefaultIfEmpty()
                                                                            join sc in _Conexion.BundleBoxing_Saco on (sb == null ? 0 : sb.IdSaco) equals sc.IdSaco into SacoUnion
                                                                            from sc in SacoUnion.DefaultIfEmpty()
                                                                            select new BundleBoxingCustom()
                                                                            {
                                                                                Serial = s.Field<string>("serialno"),
                                                                                Nombre = s.Field<string>("Descr").TrimStart().TrimEnd(),
                                                                                Bulto = s.Field<int>("bundleno"),
                                                                                Capaje = Convert.ToInt32(s.Field<Int16>("qty")),
                                                                                Saco = (sb != null) ? sc.Saco : 0,
                                                                                Yarda = (sc != null) ? sb.Yarda : 0,
                                                                                Mesa = (sc != null) ? sb.NoMesa : 0,
                                                                                EnSaco = (sc != null) ? sb.EnSaco : true,
                                                                                Corte = s.Field<string>("prodno").TrimStart().TrimEnd(),
                                                                                CorteCompleto = p.POrderClient.TrimStart().TrimEnd(),
                                                                                Estilo = (sb != null) ? SerialesUnion.First(x => x.Corte == s.Field<string>("prodno").TrimStart().TrimEnd()).Estilo : estilo,
                                                                                Oper = s.Field<string>("operno").TrimStart().TrimEnd(),
                                                                                Escaneado = (sb != null) ? true : false
                                                                            }).Union(from com in _Conexion.SerialComplemento
                                                                                     join presen in _Conexion.PresentacionSerial on com.IdPresentacionSerial equals presen.IdPresentacionSerial
                                                                                     where com.Corte.Equals(corte) && com.EnSaco && com.Activo
                                                                                     select new BundleBoxingCustom()
                                                                                     {
                                                                                         Serial = com.Serial,
                                                                                         Nombre = com.Pieza,
                                                                                         Bulto = com.Cantidad,
                                                                                         Capaje = (presen.EsUnidad) ? com.Capaje : 0,
                                                                                         Saco = 0,
                                                                                         Yarda = (presen.EsUnidad) ? 0 : com.Capaje,
                                                                                         Mesa = 0,
                                                                                         EnSaco = com.EnSaco,
                                                                                         Corte = com.Corte,
                                                                                         CorteCompleto = com.CorteCompleto,
                                                                                         Estilo = com.Estilo,
                                                                                         Oper = string.Empty,
                                                                                         Escaneado = false
                                                                                     }
                                          ).ToList();



                                ltsBoxingCustom.Where(w => w.Corte.Equals(corte) && (lstBundleBoxing.Any(item2 => item2.Serial.Equals(w.Serial)))).ToList().ForEach(i => i.Escaneado = true);



                                json = Cls.Cls_Mensaje.Tojson(ltsBoxingCustom, ltsBoxingCustom.Count, string.Empty, string.Empty, 0);
                            }




                        }

                        else
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro no encontrado.", 0);
                    }
                }
                else
                {

                    using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                    {
                        List<BundleBoxing> lstBoxing = _Conexion.BundleBoxing.ToList().FindAll(b => b.Corte.Equals(corte) && !b.EnSaco && b.Activo).ToList();





                        List<BundleBoxingCustom> ltsBoxingCustom = (from com in _Conexion.SerialComplemento
                                                                   join presen in _Conexion.PresentacionSerial on com.IdPresentacionSerial equals presen.IdPresentacionSerial
                                                                   where com.Corte.Equals(corte) && !com.EnSaco && com.Activo
                                                                   select new BundleBoxingCustom()
                                                                   {
                                                                       Serial = com.Serial,
                                                                       Nombre = com.Pieza,
                                                                       Bulto = com.Cantidad,
                                                                       Capaje = (presen.EsUnidad) ? com.Capaje : 0,
                                                                       Saco = 0,
                                                                       Yarda = (presen.EsUnidad) ? 0 : com.Capaje,
                                                                       Mesa = 0,
                                                                       EnSaco = com.EnSaco,
                                                                       Corte = com.Corte,
                                                                       CorteCompleto = com.CorteCompleto,
                                                                       Estilo = com.Estilo,
                                                                       Oper = string.Empty,
                                                                       Escaneado = false
                                                                   }).ToList();



                        ltsBoxingCustom.Where(w => w.Corte.Equals(corte) && (lstBoxing.Any(item2 => item2.Serial.Equals(w.Serial)))).ToList().ForEach(i => i.Escaneado = true);










                        json = Cls.Cls_Mensaje.Tojson(ltsBoxingCustom, ltsBoxingCustom.Count, string.Empty, string.Empty, 0);
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

    public static class DataTableExt
    {
        public static void ConvertColumnType(this DataTable dt, string columnName, Type newType)
        {
            using (DataColumn dc = new DataColumn(columnName + "_new", newType))
            {
                // Add the new column which has the new type, and move it to the ordinal of the old column
                int ordinal = dt.Columns[columnName].Ordinal;
                dt.Columns.Add(dc);
                dc.SetOrdinal(ordinal);

                // Get and convert the values of the old column, and insert them into the new
                foreach (DataRow dr in dt.Rows)
                    dr[dc.ColumnName] = Convert.ChangeType(dr[columnName], newType);

                // Remove the old column
                dt.Columns.Remove(columnName);

                // Give the new column the old column's name
                dc.ColumnName = columnName;
            }
        }
    }
}