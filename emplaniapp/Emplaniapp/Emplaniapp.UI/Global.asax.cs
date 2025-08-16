using Emplaniapp.AccesoADatos;
using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Emplaniapp.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // La inicialización de la base de datos, incluyendo usuarios, roles,
            // y datos maestros, ahora se gestiona 100% a través del script SQL.
            // No se requiere ninguna acción en el arranque de la aplicación.
        }
    }
}
