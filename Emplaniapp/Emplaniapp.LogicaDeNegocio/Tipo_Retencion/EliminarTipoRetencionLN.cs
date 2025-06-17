using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Retenciones;
using Emplaniapp.AccesoADatos.Tipo_Retencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class EliminarTipoRetencionLN : IEliminarTipoRetencionLN
    {
        IEliminarTipoRetencionAD _eliminarTR;

        public EliminarTipoRetencionLN()
        {
            _eliminarTR = new EliminarTipoRetencionAD();
        }

        public int Eliminar(int id)
        {
            int seEliminoTR = _eliminarTR.Eliminar(id);
            return seEliminoTR;
        }

    }
}
