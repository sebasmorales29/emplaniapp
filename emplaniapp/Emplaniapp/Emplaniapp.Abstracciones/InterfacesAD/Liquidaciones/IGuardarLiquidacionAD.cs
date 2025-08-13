using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesAD.Liquidaciones
{
    public interface IGuardarLiquidacionAD
    {
        Task<int> Guardar(Liquidacion Liquid);
    }
}
