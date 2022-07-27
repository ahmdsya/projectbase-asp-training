using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectBase.Startup))]
namespace ProjectBase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
