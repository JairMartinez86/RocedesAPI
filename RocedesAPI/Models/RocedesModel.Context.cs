﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AuditoriaEntities : DbContext
    {
        public AuditoriaEntities()
            : base("name=AuditoriaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<POrder> POrder { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Style> Style { get; set; }
        public virtual DbSet<Bundle> Bundle { get; set; }
        public virtual DbSet<BundleBoxing_Saco> BundleBoxing_Saco { get; set; }
        public virtual DbSet<FactorTendido> FactorTendido { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<PresentacionSerial> PresentacionSerial { get; set; }
        public virtual DbSet<ProcesosTendido> ProcesosTendido { get; set; }
        public virtual DbSet<BundleBoxingEnvio> BundleBoxingEnvio { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<BundleBoxing> BundleBoxing { get; set; }
        public virtual DbSet<FactorCorte> FactorCorte { get; set; }
        public virtual DbSet<FactorDetalleCorte> FactorDetalleCorte { get; set; }
        public virtual DbSet<FoleoDatos> FoleoDatos { get; set; }
        public virtual DbSet<FoleoFactor> FoleoFactor { get; set; }
        public virtual DbSet<FoleoProceso> FoleoProceso { get; set; }
        public virtual DbSet<SerialComplemento> SerialComplemento { get; set; }
        public virtual DbSet<CodigoGSD> CodigoGSD { get; set; }
    }
}
