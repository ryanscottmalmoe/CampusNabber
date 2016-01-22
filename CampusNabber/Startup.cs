using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CampusNabber.Startup))]
namespace CampusNabber
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
