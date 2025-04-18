using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Emplaniapp.UI.Startup))]
namespace Emplaniapp.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
