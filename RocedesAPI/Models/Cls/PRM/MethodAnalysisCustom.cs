using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.PRM
{
    public class MethodAnalysisCustom
    {
        public int IdMethodAnalysis = 0;
        public string Codigo = string.Empty;
        public string Operacion = string.Empty;
        public int IdDataMachine = 0;
        public string DataMachine = string.Empty;
        public decimal Puntadas = 0;
        public string ManejoPaquete = string.Empty;
        public decimal Rate = 0;
        public decimal JornadaLaboral = 0;
        public int IdTela = 0;
        public decimal Onza = 0;
        public string MateriaPrima_1 = string.Empty;
        public string MateriaPrima_2 = string.Empty;
        public string MateriaPrima_3 = string.Empty;
        public string MateriaPrima_4 = string.Empty;
        public string MateriaPrima_5 = string.Empty;
        public string MateriaPrima_6 = string.Empty;
        public string MateriaPrima_7 = string.Empty;
        public string ParteSeccion = string.Empty;
        public string TipoConstruccion = string.Empty;
        public DateTime FechaRegistro = DateTime.Now;
        public int IdUsuario = 0;
        public string Usuario = string.Empty;
    }
}