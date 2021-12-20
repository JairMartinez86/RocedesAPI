using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class BundleBoxingEnvioCustom
    {
        public int IdEnvio = 0;
        public string CorteCompleto = string.Empty;
        public string Corte = string.Empty;
        public string Serial = string.Empty;
        public Nullable<int> IdSaco = null;
        public Nullable<int> Saco = null;
        public Nullable<int> Bulto = null;
        public Nullable<int> Yarda = null;
        public int Polin = 0;
        public DateTime Fecha = DateTime.Now;
        public string Login = string.Empty;
        public bool Activo = false;
    }
}