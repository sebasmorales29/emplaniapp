using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Ubicacion
{
    public class ListarDistritosAD
    {
        public List<DistristoDto> ObtenerDistritos(int? idCanton = null)
        {
            using (var contexto = new Contexto())
            {
                var query = contexto.Distrito.AsQueryable();

                if (idCanton.HasValue)
                {
                    query = query.Where(d => d.idCanton == idCanton.Value);
                }

                return query
                    .Select(d => new DistristoDto
                    {
                        idDistrito = d.idDistrito,
                        nombreDistrito = d.nombreDistrito,
                        idCanton = d.idCanton
                    })
                    .OrderBy(d => d.nombreDistrito)
                    .ToList();
            }
        }
    }
} 