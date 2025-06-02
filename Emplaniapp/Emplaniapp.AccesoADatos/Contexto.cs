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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoRemuneracion>().ToTable("TipoRemuneracion");
            modelBuilder.Entity<TipoRetencion>().ToTable("TipoRetenciones");
            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Cargo>().ToTable("Cargos");
            modelBuilder.Entity<TipoMoneda>().ToTable("TipoMoneda");
            modelBuilder.Entity<Banco>().ToTable("Bancos");
            modelBuilder.Entity<Estado>().ToTable("Estado");

            // Configuración específica para la clave primaria de Empleado
            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // Configuración específica para campos decimales de Empleado
            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>()
                .Property(e => e.salarioDiario)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>()
                .Property(e => e.salarioAprobado)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>()
                .Property(e => e.salarioPorMinuto)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>()
                .Property(e => e.salarioPoHora)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Emplaniapp.Abstracciones.ModelosAD.Empleado>()
                .Property(e => e.salarioPorHoraExtra)
                .HasPrecision(12, 2);
        }


        // Entidades ---------------------------------------
        public DbSet<TipoRemuneracion> TipoRemu {  get; set; }
        public DbSet<TipoRetencion> TipoReten {  get; set; }
        public DbSet<Emplaniapp.Abstracciones.ModelosAD.Empleado> Empleados { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<TipoMoneda> TiposMoneda { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Estado> Estados { get; set; }

    }
}
