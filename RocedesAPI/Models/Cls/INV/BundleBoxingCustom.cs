using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class BundleBoxingCustom
    {
        public string Grupo = string.Empty;
        public string Serial = string.Empty;
        public string Nombre = string.Empty;
        public string Talla = string.Empty;
        public int Seccion = 0;
        public int Bulto = 0;
        public int Capaje = 0;
        public int Yarda = 0;
        public Nullable<int> Saco = 0;
        public int Mesa = 0;
        public bool EnSaco = false;
        public string Corte = string.Empty;
        public string CorteCompleto = string.Empty;
        public string Estilo = string.Empty;
        public string Oper = string.Empty;
        public bool Escaneado = false;
        public string Login = string.Empty;
        public Nullable<DateTime> Fecha = null;
    }
}