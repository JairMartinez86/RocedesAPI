using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class FoleoFactorCustom
    {
        public int IdProcesoFoleo = 0;
        public int Orden = 0;
        public string Proceso = string.Empty;
        public int NoFactor = 0;
        public Nullable<decimal> Factor1 = null;
        public Nullable<decimal> Factor2 = null;
        public Nullable<decimal> Factor3 = null;
        public Nullable<decimal> TotalFactor = null;
        public Nullable<decimal> Minutos = null;
    }
}