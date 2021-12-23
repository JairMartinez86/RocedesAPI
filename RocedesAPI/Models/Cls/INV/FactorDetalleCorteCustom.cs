using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class FactorDetalleCorteCustom
    {
        public int IdFactorDetalleCorte = 0;
        public int IdFactorCorte = 0;
        public string Item = string.Empty;
        public string Componente = string.Empty;
        public string Estilo = string.Empty;
        public string LayLimits = string.Empty;
        public int TotalPieces = 0;
        public decimal StraightPerimeter = 0;
        public decimal CurvedPerimeter = 0;
        public decimal TotalPerimeter = 0;
        public int TotalNotches = 0;
        public int TotalCorners = 0;
        public decimal Segundos = 0;
        public decimal Minutos_Pza = 0;

    }
}