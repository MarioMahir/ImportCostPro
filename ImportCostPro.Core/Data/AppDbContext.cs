using ImportCostPro.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base (options)
        {
        }

        public DbSet<Pais> Paises {  get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Importador> Importadores { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<CategoriaArancelaria> CategoriasArancelarias { get; set; }
        public DbSet<TasaCambio> TasasCambio { get; set; }

        public DbSet<OrdenImportacion> OrdenesImportacion { get; set; }

        public DbSet<DetalleOrdenImportacion> DetallesOrdenImportacion { get; set; }

        public DbSet<GastoImportacion> GastosImportacion { get; set; }

        public DbSet<ConfiguracionImpuestos> ConfiguracionesImpuestos { get; set; }

        public DbSet<ResultadoLandedCost> ResultadosLandedCost { get; set; }

        public DbSet<DetalleResultadoLandedCost> DetallesResultadoLandedCost { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .Property(p => p.PesoUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Largo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Ancho)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Alto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Pais)
                .WithMany()
                .HasForeignKey(p => p.PaisId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.CategoriaArancelaria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaArancelariaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CategoriaArancelaria>()
                .Property(c => c.PorcentajeArancel)
                .HasPrecision(5, 2);

            modelBuilder.Entity<CategoriaArancelaria>()
                .Property(c => c.PorcentajeImpuestoSelectivo)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TasaCambio>()
                .Property(t => t.ValorTasa)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TasaCambio>()
                .HasOne(t => t.MonedaOrigen)
                .WithMany()
                .HasForeignKey(t => t.MonedaOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TasaCambio>()
                .HasOne(t => t.MonedaDestino)
                .WithMany()
                .HasForeignKey(t => t.MonedaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleOrdenImportacion>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GastoImportacion>()
                .Property(g => g.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrdenImportacion>()
                .HasOne(o => o.Moneda)
                .WithMany()
                .HasForeignKey(o => o.MonedaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ConfiguracionImpuestos>()
                .Property(c => c.PorcentajeITBIS)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ConfiguracionImpuestos>()
                .Property(c => c.PorcentajeTasaServicioAduanal)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Proveedor>()
                .HasOne(p => p.Moneda)
                .WithMany()
                .HasForeignKey(p => p.MonedaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Importador>()
                .HasOne(i => i.Pais)
                .WithMany()
                .HasForeignKey(i => i.PaisId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DetalleOrdenImportacion>()
                .Property(d => d.MargenDeseado)
                .HasPrecision(5, 2);

            modelBuilder.Entity<OrdenImportacion>()
                .HasOne(o => o.ResultadoOficial)
                .WithOne(r => r.OrdenImportacion)
                .HasForeignKey<ResultadoLandedCost>(r => r.OrdenImportacionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResultadoLandedCost>(r =>
            {
                r.Property(x => x.TasaCambioOrden).HasPrecision(18, 4);
                r.Property(x => x.PorcentajeITBIS).HasPrecision(5, 2);
                r.Property(x => x.PorcentajeTasaServicioAduanal).HasPrecision(5, 2);
                r.Property(x => x.FleteTotal).HasPrecision(18, 4);
                r.Property(x => x.SeguroTotal).HasPrecision(18, 4);
                foreach (var nombre in new[] { "FOBTotalOriginal", "FOBTotalLocal", "CIFTotal", "TotalArancel", "TotalImpuestoSelectivo", "TotalTasaServicioAduanal", "TotalITBIS", "TotalGastosLocales", "CostoTotalImportacion" })
                {
                    r.Property(nombre).HasPrecision(18, 4);
                }
            });

            modelBuilder.Entity<DetalleResultadoLandedCost>(d =>
            {
                d.HasOne(x => x.Producto)
                    .WithMany()
                    .HasForeignKey(x => x.ProductoId)
                    .OnDelete(DeleteBehavior.NoAction);
                d.Property(x => x.PorcentajeArancel).HasPrecision(5, 2);
                d.Property(x => x.PorcentajeImpuestoSelectivo).HasPrecision(5, 2);
                d.Property(x => x.MargenDeseado).HasPrecision(5, 2);
                foreach (var nombre in new[] { "FOBOriginal", "FOBLocal", "FleteAsignado", "SeguroAsignado", "CIF", "Arancel", "ImpuestoSelectivo", "TasaServicioAduanal", "ITBIS", "GastosLocalesAsignados", "CostoTotalImportado", "CostoUnitarioImportado", "PrecioVentaSugerido" })
                {
                    d.Property(nombre).HasPrecision(18, 4);
                }
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
