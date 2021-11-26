using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class BundleBoxingCustom
    {

        public int Serial { get; set; }
        public string Nombre { get; set; }
        public int Seccion { get; set; }
        public int Bulto { get; set; }
        public Int16 Capaje { get; set; }
        public Nullable<int> Saco { get; set; }
        public string Corte { get; set; }
        public string Oper { get; set; }
        public bool Escaneado { get; set; }
  
    }
}