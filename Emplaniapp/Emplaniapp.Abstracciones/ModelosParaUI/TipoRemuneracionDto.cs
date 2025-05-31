using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class TipoRemuneracionDto
    {
        public int Id { get; set; }
        public string nombreTipoRemuneracion { get; set; }
        public double porcentajeRemuneracion { get; set; }
        public int idEstado { get; set; }
    }
}
