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
    
    public partial class POrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POrder()
        {
            this.Bundle = new HashSet<Bundle>();
        }
    
        public int Id_Order { get; set; }
        public Nullable<int> Id_Cliente { get; set; }
        public Nullable<int> Id_Style { get; set; }
        public string POrder1 { get; set; }
        public string Description { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Bundles { get; set; }
        public Nullable<int> Id_Planta { get; set; }
        public Nullable<int> Id_Linea { get; set; }
        public Nullable<int> Semana { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Id_Linea2 { get; set; }
        public string Describir { get; set; }
        public Nullable<int> AfterIntex { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string POrderClient { get; set; }
        public Nullable<bool> washed { get; set; }
    
        public virtual Style Style { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bundle> Bundle { get; set; }
    }
}
