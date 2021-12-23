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

                List<BundleBoxing> lstGuardar = new List<BundleBoxing>();

                if (!esComplemento)
                {
                    sql = $"SELECT S.serialno, OP.Descr, S.bundleno, BD.qty, S.prodno, S.operno, OB.seqno  \n" +
                    $"FROM SERIAL2 AS S \n" +
                    $"INNER JOIN OPER AS OP ON OP.operno = S.operno \n" +
                    $"INNER JOIN BUNDLE AS BD ON BD.prodno = S.prodno AND BD.bundleno = S.bundleno \n" +
                    $"INNER JOIN OPBULL AS OB ON OB.operno = OP.operno AND OB.bulletin = '{estilo}'  \n" +
                    $"WHERE S.prodno = '{corte}' " +
                    $"GROUP BY  S.serialno,  OP.Descr, S.bundleno, BD.qty, S.prodno, S.operno, OB.bulletin, OB.seqno";

                    Cls_ConexionPervasive _Cnx = new Cls_ConexionPervasive();
                    DataTable tbl = _Cnx.GetDatos(sql, out json);

                    sql = $"SELECT OB.operno, OP.Descr, OB.seqno  \n" +
                   $"FROM  OPBULL AS OB " +
                   $"INNER JOIN OPER AS OP ON OP.operno = OB.operno \n" +
                   $"WHERE OB.bulletin = '{estilo}' AND OB.varid = '' \n";

                    DataTable tbl2 = _Cnx.GetDatos(sql, out json);

          

                    if (tbl != null && tbl2 != null)
                    {

                        DataTableExt.ConvertColumnType(tbl, "serialno", typeof(string));


                        if (tbl.Rows.Count > 0 && tbl.Rows.Count > 0)
                        {
                            var OperMaster = (from OB in tbl2.AsEnumerable()
                                              select new
                                              {
                                                  operno = OB.Field<string>("operno"),
                                                  Descr = OB.Field<string>("Descr"),
                                                  seqno = OB.Field<Int16>("seqno")
                                              }).ToList();





                           


                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                            {
                                using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                                {
                                    List<BundleBoxing> lstBundleBoxing = _Conexion.BundleBoxing.ToList().FindAll(b => b.Corte.Equals(corte) && b.FechaInactivo == null).ToList();
                                    List<BundleBoxingCustom> ltsBoxingCustom = null;

                                    if (lstBundleBoxing.FindAll(w => w.Oper != string.Empty &&  w.Corte == corte && OperMaster.Any(f => f.operno != w.Oper) ).Count == 0)
                                    {

                                        ltsBoxingCustom = (from s in tbl.AsEnumerable()
                                                           join p in _Conexion.POrder on s.Field<string>("prodno").TrimStart().TrimEnd() equals p.POrder1.TrimEnd().TrimStart()
                                                           join b in lstBundleBoxing on new { _Corte = s.Field<string>("prodno"), _Serial = s.Field<string>("serialno")} equals new { _Corte = b.Corte, _Serial = b.Serial} into SerialesUnion
                                                           from sb in SerialesUnion.DefaultIfEmpty()
                                                           join bun in _Conexion.Bundle on new { ID = p.Id_Order, Bld = s.Field<int>("bundleno") } equals new { ID = (bun.Id_Order == null) ? 0 : (int)bun.Id_Order, Bld = (bun.Bld == null) ? 0 : (int)bun.Bld } into BundleUnion
                                                           from bu in BundleUnion.DefaultIfEmpty()
                                                           join sc in _Conexion.BundleBoxing_Saco on (sb == null ? 0 : sb.IdSaco) equals sc.IdSaco into SacoUnion
                                                           from sc in SacoUnion.DefaultIfEmpty()
                                                           select new BundleBoxingCustom()
                                                           {
                                                               Serial = s.Field<string>("serialno"),
                                                               Nombre = OperMaster.FindLast(f => f.seqno < s.Field<Int16>("seqno")).Descr,//s.Field<string>("Descr").TrimStart().TrimEnd(),
                                                               Talla = (bu == null) ? string.Empty : bu.Size,
                                                               Bulto = s.Field<int>("bundleno"),
                                                               Capaje = Convert.ToInt32(s.Field<Int16>("qty")),
                                                               Saco = (sb != null && sc != null) ? sc.Saco : 0,
                                                               Yarda = (sc != null) ? sb.Yarda : 0,
                                                               Mesa = (sc != null) ? sb.NoMesa : 0,
                                                               EnSaco = (sc != null) ? sb.EnSaco : true,
                                                               Corte = s.Field<string>("prodno").TrimStart().TrimEnd(),
                                                               CorteCompleto = p.POrderClient.TrimStart().TrimEnd(),
                                                               Estilo = (sb != null) ? SerialesUnion.First(x => x.Corte == s.Field<string>("prodno").TrimStart().TrimEnd()).Estilo : estilo,
                                                               Oper = OperMaster.FindLast(f => f.seqno < s.Field<Int16>("seqno")).operno,//s.Field<string>("operno").TrimStart().TrimEnd(),
                                                               Escaneado = (sb == null) ? false : sb.Activo
                                                           }).Union(from com in _Conexion.SerialComplemento
                                                                    join presen in _Conexion.PresentacionSerial on com.IdPresentacionSerial equals presen.IdPresentacionSerial
                                                                    where com.Corte.Equals(corte) &&  com.Activo
                                                                    join p in _Conexion.POrder on com.CorteCompleto equals p.POrder1.TrimEnd().TrimStart()
                                                                    join bun in _Conexion.Bundle on new { ID = p.Id_Order, Bld = com.Cantidad } equals new { ID = (bun.Id_Order == null) ? 0 : (int)bun.Id_Order, Bld = (bun.Bld == null) ? 0 : (int)bun.Bld } into BundleUnion
                                                                    from bu in BundleUnion.DefaultIfEmpty()
                                                                    select new BundleBoxingCustom()
                                                                    {
                                                                        Serial = com.Serial,
                                                                        Nombre = com.Pieza,
                                                                        Talla = (bu == null) ? string.Empty : bu.Size,
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


  

                                        ltsBoxingCustom.Where(w =>  w.Corte.Equals(corte) && (lstBundleBoxing.Any(item2 => item2.Serial.Equals(w.Serial)))).ToList().ForEach(i => i.Escaneado = true);




                                        foreach (BundleBoxingCustom b in ltsBoxingCustom.Where(w => w.Oper != string.Empty &&  !lstBundleBoxing.Any(f => f.Serial == w.Serial)))
                                        {

                                            lstGuardar.Add(new BundleBoxing
                                            {
                                                NoMesa = b.Mesa,
                                                Serial = b.Serial,
                                                Nombre = b.Nombre,
                                                Talla = b.Talla,
                                                Seccion = b.Seccion,
                                                Bulto = b.Bulto,
                                                Capaje = b.Capaje,
                                                Yarda = b.Yarda,
                                                EnSaco = b.EnSaco,
                                                IdSaco = null,
                                                Corte = b.Corte,
                                                CorteCompleto = b.CorteCompleto,
                                                Estilo = b.Estilo,
                                                Oper = b.Oper,
                                                IdUsuario = null,
                                                FechaRegistro = null,
                                                Activo = false
                                            });



                                        }

                                        if (lstGuardar.Count > 0) _Conexion.BundleBoxing.AddRange(lstGuardar);


                                    }
                                    else
                                    {
                                        ltsBoxingCustom = (from s in lstBundleBoxing
                                                           join sc in _Conexion.BundleBoxing_Saco on  s.IdSaco equals sc.IdSaco into SacoUnion
                                                           from sc in SacoUnion.DefaultIfEmpty()
                                                           select new BundleBoxingCustom()
                                                           {
                                                               Serial = s.Serial,
                                                               Nombre = s.Nombre,
                                                               Talla = s.Talla,
                                                               Bulto = s.Bulto,
                                                               Capaje = s.Capaje,
                                                               Saco = (sc != null) ? sc.Saco : 0,
                                                               Yarda = s.Yarda,
                                                               Mesa = s.NoMesa,
                                                               EnSaco = s.EnSaco,
                                                               Corte = s.Corte,
                                                               CorteCompleto = s.CorteCompleto,
                                                               Estilo = s.Estilo,
                                                               Oper = s.Oper,
                                                               Escaneado = s.Activo
                                                           }).ToList();

                                    }


                                    json = Cls.Cls_Mensaje.Tojson(ltsBoxingCustom, ltsBoxingCustom.Count, string.Empty, string.Empty, 0);


                                    _Conexion.SaveChanges();
                                    scope.Complete();
                                    scope.Dispose();
                                }

                            }



                        }

                        else
                            json = Cls.Cls_Mensaje.Tojson(null, 0, string.Empty, "Registro no encontrado.", 0);
                    }
                }
                else
                {


                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                    {

                        using (AuditoriaEntities _Conexion = new AuditoriaEntities())
                        {
                            List<BundleBoxing> lstBoxing = _Conexion.BundleBoxing.ToList().FindAll(b => b.Corte.Equals(corte) && !b.EnSaco && b.Activo).ToList();





                            List<BundleBoxingCustom> ltsBoxingCustom = (from com in _Conexion.SerialComplemento
                                                                        join presen in _Conexion.PresentacionSerial on com.IdPresentacionSerial equals presen.IdPresentacionSerial
                                                                        where com.Corte.Equals(corte) && !com.EnSaco && com.Activo
                                                                        join p in _Conexion.POrder on com.CorteCompleto equals p.POrder1.TrimEnd().TrimStart()
                                                                        join bun in _Conexion.Bundle on new { ID = p.Id_Order, Bld = com.Cantidad } equals new { ID = (bun.Id_Order == null) ? 0 : (int)bun.Id_Order, Bld = (bun.Bld == null) ? 0 : (int)bun.Bld } into BundleUnion
                                                                        from bu in BundleUnion.DefaultIfEmpty()
                                                                        select new BundleBoxingCustom()
                                                                        {
                                                                            Serial = com.Serial,
                                                                            Nombre = com.Pieza,
                                                                            Talla = (bu == null) ? string.Empty : bu.Size,
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



                            foreach (BundleBoxingCustom b in ltsBoxingCustom.Where(w => !_Conexion.BundleBoxing.Any(f => f.Serial == w.Serial)))
                            {

                                lstGuardar.Add( new BundleBoxing
                                {
                                    NoMesa = b.Mesa,
                                    Serial = b.Serial,
                                    Nombre = b.Nombre,
                                    Talla = b.Talla,
                                    Seccion = b.Seccion,
                                    Bulto = b.Bulto,
                                    Capaje = b.Capaje,
                                    Yarda = b.Yarda,
                                    EnSaco = b.EnSaco,
                                    IdSaco = null,
                                    Corte = b.Corte,
                                    CorteCompleto = b.CorteCompleto,
                                    Estilo = b.Estilo,
                                    Oper = b.Oper,
                                    IdUsuario = null,
                                    FechaRegistro = null,
                                    Activo = true
                                });

                            }


                            if (lstGuardar.Count > 0) _Conexion.BundleBoxing.AddRange(lstGuardar);

                            json = Cls.Cls_Mensaje.Tojson(ltsBoxingCustom, ltsBoxingCustom.Count, string.Empty, string.Empty, 0);



                            _Conexion.SaveChanges();
                            scope.Complete();
                            scope.Dispose();
                        }



                        //ski
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