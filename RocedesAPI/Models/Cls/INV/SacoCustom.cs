using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.INV
{
    public class SacoCustom
    {
        public int IdSaco = 0;
        public string Serial = string.Empty;
        public int Saco = 0;
        public int NoMesa = 0;
        public string Corte = string.Empty;
        public string CorteCompleto = string.Empty;
        public int Seccion = 0;
        public Nullable<int>IdUsuario = 0;
        public string Usuario = string.Empty;
        public bool Abierto = false;
        public Nullable<int> IdUsuarioAbre = 0;
        public string UsuarioAbre = string.Empty;
        public Nullable<DateTime> FechaRegistro = DateTime.Now;
        public bool Activo = false;
    }
}