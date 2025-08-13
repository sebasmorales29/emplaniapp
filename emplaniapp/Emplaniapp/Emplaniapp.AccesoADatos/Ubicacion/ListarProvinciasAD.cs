using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Ubicacion
{
    public class ListarProvinciasAD
    {
        public List<ProvinciaDto> ObtenerProvincias()
        {
            using (var contexto = new Contexto())
            {
                return contexto.Provincia
                    .Select(p => new ProvinciaDto
                    {
                        idProvincia = p.idProvincia,
                        nombreProvincia = p.nombreProvincia
                    })
                    .OrderBy(p => p.nombreProvincia)
                    .ToList();
            }
        }
    }
} 