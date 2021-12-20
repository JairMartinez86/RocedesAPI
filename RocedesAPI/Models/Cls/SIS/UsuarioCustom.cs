using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.SIS
{
    public class UsuarioCustom
    {
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CodBar { get; set; }
        public bool Activo { get; set; }
    }
}