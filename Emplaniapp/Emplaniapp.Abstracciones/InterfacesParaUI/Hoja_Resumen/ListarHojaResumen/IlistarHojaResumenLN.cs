﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen
{
    public interface IlistarHojaResumenLN
    {
        List<HojaResumenDto> ObtenerHojasResumen();
        List<HojaResumenDto> ObtenerFiltrado(string filtro, int? idCargo);
        int ObtenerTotalEmpleados(string filtro, int? idCargo);
    }
}
