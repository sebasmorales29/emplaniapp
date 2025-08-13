using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Ubicacion
{
    public class ListarCallesAD
    {
        public List<CalleDto> ObtenerCalles(int? idDistrito = null)
        {
            using (var contexto = new Contexto())
            {
                var query = contexto.Calle.AsQueryable();

                if (idDistrito.HasValue)
                {
                    query = query.Where(c => c.idDistrito == idDistrito.Value);
                }

                return query
                    .Select(c => new CalleDto
                    {
                        idCalle = c.idCalle,
                        nombreCalle = c.nombreCalle,
                        idDistrito = c.idDistrito
                    })
                    .OrderBy(c => c.nombreCalle)
                    .ToList();
            }
        }
    }
} 