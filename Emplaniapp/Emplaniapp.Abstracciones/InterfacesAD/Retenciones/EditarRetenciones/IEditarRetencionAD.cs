using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.Abstracciones.InterfacesAD.Retenciones
{
    public interface IEditarRetencionAD
    {
        void Editar(Retencion retencion);
    }
}