//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RocedesAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BundleBoxing
    {
        public int IdBundleBoxing { get; set; }
        public int NoMesa { get; set; }
        public string Serial { get; set; }
        public string Nombre { get; set; }
        public int Seccion { get; set; }
        public int Bulto { get; set; }
        public int Capaje { get; set; }
        public int Yarda { get; set; }
        public Nullable<int> IdSaco { get; set; }
        public bool EnSaco { get; set; }
        public string Corte { get; set; }
        public string CorteCompleto { get; set; }
        public string Estilo { get; set; }
        public string Oper { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public Nullable<int> IdUsuarioTermina { get; set; }
        public Nullable<System.DateTime> FechaTermina { get; set; }
        public bool Activo { get; set; }
        public Nullable<int> IdUsuarioInactiva { get; set; }
        public Nullable<System.DateTime> FechaInactivo { get; set; }
        public System.Guid ID { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
