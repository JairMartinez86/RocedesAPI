using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Cls
{

    public class Cls_Mensaje
    {
        public static string Tojson(object o, int Length, string CodError, string Mensaje, int esError)
        {
            string json = string.Empty;


            if (o != null)
            {
                json = JsonConvert.SerializeObject(o);
                json = string.Concat("{ \"d\": ", json, ",  \"msj\": ", "{\"Codigo\":\"", CodError, "\",\"Mensaje\":\"<p>", Mensaje, "</p>\"}", ", \"count\":" , Length, ", \"esError\":", 0, "}");
            }
            else
                json = string.Concat("{ \"d\":  [{ }],  \"msj\": ", "{\"Codigo\":\"", CodError, "\",\"Mensaje\":\"<p align='center'>", Mensaje, "</p>\"}", ", \"count\":", Length, ", \"esError\":", esError,  "}");





            return json;
        }


    }
}