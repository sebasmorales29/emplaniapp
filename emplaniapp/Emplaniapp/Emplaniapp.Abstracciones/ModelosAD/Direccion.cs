using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Direccion")]
    public class Direccion
    {
        [Key]
        public int idDireccion { get; set; }
        public int idProvincia { get; set; }
        public int idCanton { get; set; }
        public int idDistrito { get; set; }
        public int idCalle { get; set; }
    }
} 