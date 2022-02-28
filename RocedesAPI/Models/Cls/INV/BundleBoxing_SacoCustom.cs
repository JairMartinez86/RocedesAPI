using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class BundleBoxing_SacoCustom
    {
        public int IdSaco = 0;
        public string Serial = string.Empty;
        public int Saco = 0;
        public int NoMesa = 0;
        public string Corte = string.Empty;
        public int Seccion = 0;
        public int IdUsuarioCrea = 0;
        public System.DateTime FechaRegistro = DateTime.Now;
        public bool Abierto = false;
        public Nullable<int> IdUsuarioAbre = null;
        public bool Activo = false;
    }
}