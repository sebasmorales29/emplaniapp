using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class RetencionEditarDto
    {
        public int idRetencion { get; set; }
        public int idTipoRetencio { get; set; }
        public decimal rebajo { get; set; }
        public DateTime fechaRetencio { get; set; }
    }
}