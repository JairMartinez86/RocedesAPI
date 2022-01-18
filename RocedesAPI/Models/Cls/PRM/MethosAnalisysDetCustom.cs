using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.PRM
{
    public class MethosAnalisysDetCustom
    {
        public int IdDetMethodAnalysis = 0;
        public int IdMethodAnalysis = 0;
        public string Codigo1 = string.Empty;
        public string Codigo2 = string.Empty;
        public string Codigo3 = string.Empty;
        public string Codigo4 = string.Empty;
        public string Descripcion = string.Empty;
        public decimal Freq = 0;
        public decimal Tmus = 0;
        public decimal Sec = 0;
        public decimal Sam = 0;
        public decimal Rpm = 0;
        public decimal FactorSewing = 0;
        public decimal FactorSewingAccuracy = 0;
        public bool EsTotal = false;
    }
}