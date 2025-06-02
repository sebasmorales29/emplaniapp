using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Empleado")]
    public class Empleado
    {
        [Key]
        [Column("idEmpleado")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string nombre { get; set; }
        
        [MaxLength(100)]
        public string segundoNombre { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string primerApellido { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string segundoApellido { get; set; }
        
        [Required]
        public DateTime fechaNacimiento { get; set; }
        
        [Required]
        public int cedula { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string numeroTelefonico { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string correoInstitucional { get; set; }
        
        [Required]
        public int idDireccion { get; set; }
        
        [Required]
        public int idCargo { get; set; }
        
        [Required]
        public DateTime fechaContratacion { get; set; }
        
        public DateTime? fechaSalida { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string periocidadPago { get; set; }
        
        [Required]
        public decimal salarioDiario { get; set; }
        
        [Required]
        public decimal salarioAprobado { get; set; }
        
        [Required]
        public decimal salarioPorMinuto { get; set; }
        
        [Required]
        public decimal salarioPoHora { get; set; }
        
        [Required]
        public decimal salarioPorHoraExtra { get; set; }
        
        [Required]
        public int idTipoMoneda { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string cuentaIBAN { get; set; }
        
        [Required]
        public int idBanco { get; set; }
        
        [Required]
        public int idEstado { get; set; }
    }
} 