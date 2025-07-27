using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Empleado")]
    public class Empleados
    {
        
        [Column("idEmpleado")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idEmpleado { get; set; }
        public string nombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int cedula { get; set; }
        public string numeroTelefonico { get; set; }
        public string correoInstitucional { get; set; }
        public int idDireccion { get; set; }
        public int? idProvincia { get; set; }
        public int? idCanton { get; set; }
        public int? idDistrito { get; set; }
        public String direccionDetallada { get; set; }
        public int? idCargo { get; set; }
        public DateTime fechaContratacion { get; set; }
        public DateTime? fechaSalida { get; set; }
        public string periocidadPago { get; set; }
        public decimal salarioDiario { get; set; }
        public decimal salarioAprobado { get; set; }
        public decimal salarioPorMinuto { get; set; }
        public decimal salarioPoHora { get; set; }
        public decimal salarioPorHoraExtra { get; set; }
        public int? idTipoMoneda { get; set; }
        public string cuentaIBAN { get; set; }
        public int? idBanco { get; set; }
        [Column("idEstado")]
        public int idEstado { get; set; }
        [Column("IdNetUser")]
        public string IdNetUser { get; set; }

    }
}
