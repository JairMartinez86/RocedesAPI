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
    
    public partial class SerialComplemento
    {
        public int IdSerialComplemento { get; set; }
        public string Serial { get; set; }
        public string Pieza { get; set; }
        public int IdPresentacionSerial { get; set; }
        public int IdMaterial { get; set; }
        public int Cantidad { get; set; }
        public int Capaje { get; set; }
        public int IdUsuarioCrea { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string Corte { get; set; }
        public string CorteCompleto { get; set; }
        public string Estilo { get; set; }
        public bool Activo { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual PresentacionSerial PresentacionSerial { get; set; }
    }
}
