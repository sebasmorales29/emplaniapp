using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name=Contexto")//Nombre de instancia a la que se esta llamadno
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoRemuneracion>().ToTable("TipoRemuneracion");
            modelBuilder.Entity<TipoRetencion>().ToTable("TipoRetenciones");
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Remuneracion>().ToTable("Remuneracion");
            modelBuilder.Entity<Estado>().ToTable("Estado");
            modelBuilder.Entity<Retencion>().ToTable("Retenciones");
            modelBuilder.Entity<PeriodoPago>().ToTable("PeriodoPago");
            modelBuilder.Entity<PagoQuincenal>().ToTable("PagoQuincenal");
            modelBuilder.Entity<Liquidaciones>().ToTable("Liquidaciones");
            modelBuilder.Entity<Cargos>().ToTable("Cargos");
        }


        // Entidades ---------------------------------------
        public DbSet<TipoRemuneracion> TipoRemu {  get; set; }
        public DbSet<TipoRetencion> TipoReten {  get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Remuneracion> Remuneracion { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Retencion> Retenciones { get; set; }
        public DbSet<PeriodoPago> PeriodoPago { get; set; }
        public DbSet<PagoQuincenal> PagoQuincenal { get; set; }
        public DbSet<Liquidaciones> Liquidaciones { get; set; }
        public DbSet<Cargos> Cargos { get; set; }

    }
}
