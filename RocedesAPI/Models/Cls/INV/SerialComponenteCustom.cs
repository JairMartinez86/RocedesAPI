using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class SerialComponenteCustom
    {
        public int IdSerialComplemento = 0;
        public string Serial = string.Empty;
        public string Pieza = string.Empty;
        public int IdPresentacionSerial = 0;
        public string PresentacionSerial = string.Empty;
        public int IdMaterial = 0;
        public string Material = string.Empty;
        public int Cantidad = 0;
        public int Capaje = 0;
        public bool EnSaco = false;
        public Nullable<int> IdUsuarioCrea = 0;
        public string Usuario = string.Empty;
        public Nullable<DateTime> FechaRegistro = DateTime.Now;
        public string Corte = string.Empty;
        public string CorteCompleto = string.Empty;
        public string Estilo = string.Empty;
        public bool Activo = false;
    }
}