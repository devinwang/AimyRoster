using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AimyRoster.Startup))]
namespace AimyRoster
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
