using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Ubicacion
{
    public class ListarCantonesAD
    {
        public List<CantonDto> ObtenerCantones(int? idProvincia = null)
        {
            using (var contexto = new Contexto())
            {
                var query = contexto.Canton.AsQueryable();

                if (idProvincia.HasValue)
                {
                    query = query.Where(c => c.idProvincia == idProvincia.Value);
                }

                return query
                    .Select(c => new CantonDto
                    {
                        idCanton = c.idCanton,
                        nombreCanton = c.nombreCanton,
                        idProvincia = c.idProvincia
                    })
                    .OrderBy(c => c.nombreCanton)
                    .ToList();
            }
        }
    }
} 