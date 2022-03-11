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
    
    public partial class MachineData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MachineData()
        {
            this.MethodAnalysis = new HashSet<MethodAnalysis>();
        }
    
        public int IdDataMachine { get; set; }
        public string Name { get; set; }
        public decimal Delay { get; set; }
        public decimal Personal { get; set; }
        public decimal Fatigue { get; set; }
        public string Nomenclature { get; set; }
        public string Machine { get; set; }
        public string Description { get; set; }
        public string Ref { get; set; }
        public string Code { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodAnalysis> MethodAnalysis { get; set; }
    }
}
