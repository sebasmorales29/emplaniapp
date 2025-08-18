using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.Entidades;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Emplaniapp.AccesoADatos
{
    public class Contexto : IdentityDbContext<ApplicationUser>
    {
        public Contexto() : base("Contexto", throwIfV1Schema: false)
        {

        }

        public static Contexto Create()
        {
            return new Contexto();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoRemuneracion>().ToTable("TipoRemuneracion");
            modelBuilder.Entity<TipoRetencion>().ToTable("TipoRetenciones");
            modelBuilder.Entity<Empleados>().ToTable("Empleado");
            modelBuilder.Entity<Remuneracion>().ToTable("Remuneracion");
            modelBuilder.Entity<Estado>().ToTable("Estado");
            modelBuilder.Entity<Retencion>().ToTable("Retenciones");
            modelBuilder.Entity<PeriodoPagos>().ToTable("PeriodoPago");
            modelBuilder.Entity<PagoQuincenal>().ToTable("PagoQuincenal");
            modelBuilder.Entity<Liquidacion>().ToTable("Liquidaciones");
            modelBuilder.Entity<Cargo>().ToTable("Cargos");
            modelBuilder.Entity<Empleados>()
                .Property(e => e.IdNetUser)
                .IsOptional();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Provincia>().ToTable("Provincia");
            modelBuilder.Entity<Canton>().ToTable("Canton");
            modelBuilder.Entity<Distrito>().ToTable("Distrito");
            modelBuilder.Entity<Calle>().ToTable("Calle");
            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Direccion>().ToTable("Direccion");
            modelBuilder.Entity<NumeroOcupacion>().ToTable("NumeroOcupacion");
            modelBuilder.Entity<TipoMoneda>().ToTable("TipoMoneda");
            modelBuilder.Entity<Banco>().ToTable("Bancos");
            modelBuilder.Entity<TiposEventoHistorial>().ToTable("TiposEventoHistorial");
            modelBuilder.Entity<HistorialEmpleado>().ToTable("HistorialEmpleado");

        }


        // Entidades ---------------------------------------
        public DbSet<TipoRemuneracion> TipoRemu {  get; set; }
        public DbSet<TipoRetencion> TipoReten {  get; set; }
        public DbSet<Empleados> Empleados { get; set; }
        public DbSet<Remuneracion> Remuneracion { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Retencion> Retenciones { get; set; }
        public DbSet<PeriodoPagos> PeriodoPago { get; set; }
        public DbSet<PagoQuincenal> PagoQuincenal { get; set; }
        public DbSet<Liquidacion> Liquidaciones { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Observacion> Observaciones { get; set; }
        public DbSet<Provincia> Provincia { get; set; }
        public DbSet<Canton> Canton { get; set; }
        public DbSet<Distrito> Distrito { get; set; }
        public DbSet<Calle> Calle { get; set; }
        public DbSet<Emplaniapp.Abstracciones.ModelosAD.Direccion> Direccion { get; set; }
        public DbSet<NumeroOcupacion> NumeroOcupacion { get; set; }
        public DbSet<TipoMoneda> TipoMoneda { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<TiposEventoHistorial> TiposEventoHistorial { get; set; }
        public DbSet<HistorialEmpleado> HistorialEmpleado { get; set; }

    }
}
