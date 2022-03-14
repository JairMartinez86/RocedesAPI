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
    
    public partial class MethodAnalysis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MethodAnalysis()
        {
            this.MethodAnalysisDetalle = new HashSet<MethodAnalysisDetalle>();
        }
    
        public int IdMethodAnalysis { get; set; }
        public string Codigo { get; set; }
        public string Operacion { get; set; }
        public decimal Rate { get; set; }
        public decimal JornadaLaboral { get; set; }
        public int IdManufacturing { get; set; }
        public string Manufacturing { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public int IdFamily { get; set; }
        public string Family { get; set; }
        public int IdSecuence { get; set; }
        public int Secuence { get; set; }
        public int IdDataMachine { get; set; }
        public string DataMachine { get; set; }
        public decimal Delay { get; set; }
        public decimal Personal { get; set; }
        public decimal Fatigue { get; set; }
        public int IdStitchType { get; set; }
        public string TypeStitch { get; set; }
        public int IdNeedle { get; set; }
        public string NeedleType { get; set; }
        public int IdRpm { get; set; }
        public decimal Rpm { get; set; }
        public int IdStitchInch { get; set; }
        public int StitchInch { get; set; }
        public int IdTela { get; set; }
        public string Tela { get; set; }
        public int IdOunce { get; set; }
        public int Ounce { get; set; }
        public int IdCaliber { get; set; }
        public string Caliber { get; set; }
        public int IdFeedDog { get; set; }
        public string FeedDog { get; set; }
        public int IdPresserFoot { get; set; }
        public string PresserFoot { get; set; }
        public int IdFolder { get; set; }
        public string Folder { get; set; }
        public string MateriaPrima_1 { get; set; }
        public string MateriaPrima_2 { get; set; }
        public string MateriaPrima_3 { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int IdUsuario { get; set; }
        public decimal FactorSewing { get; set; }
        public decimal FactorSewingAccuracy { get; set; }
        public decimal Sewing { get; set; }
        public decimal Tmus_Mac { get; set; }
        public decimal Tmus_MinL { get; set; }
        public decimal Min_Mac { get; set; }
        public decimal Min_NML { get; set; }
        public decimal Min_Mac_CC { get; set; }
        public decimal Min_NML_CC { get; set; }
        public decimal Sam { get; set; }
        public decimal ProducJL { get; set; }
        public decimal Precio { get; set; }
        public Nullable<int> IdUsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
    
        public virtual MachineData MachineData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodAnalysisDetalle> MethodAnalysisDetalle { get; set; }
    }
}
