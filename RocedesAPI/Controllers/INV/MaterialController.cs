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

namespace RocedesAPI.Controllers
{
    public class MaterialController : ApiController
    {

        [Route("api/Inventario/Material/Get")]
        [HttpGet]
        public string Get(int IdPresentacionSerial)
        {
            string json = string.Empty;

            try
            {
                using (AuditoriaEntities _Cnx = new AuditoriaEntities())
                {

                    List<MaterialCustom> lst = (from q in _Cnx.Material
                                                          where q.Activo && q.IdPresentacionSerial == IdPresentacionSerial
                                                select new MaterialCustom()
                                                          {
                                                              IdMaterial = q.IdMaterial,
                                                              Material = q.Material1,
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
    }
}