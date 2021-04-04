using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchoolOrbit.Startup))]
namespace SchoolOrbit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
