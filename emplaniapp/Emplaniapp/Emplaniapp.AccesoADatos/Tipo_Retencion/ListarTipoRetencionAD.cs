using System.Collections.Generic;
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;
using System.Linq;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class ListarTipoRetencionAD : IListarTipoRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public List<TipoRetencion> Listar() => _ctx.TipoReten.ToList();
    }
}