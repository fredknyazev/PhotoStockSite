using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCSite.Startup))]
namespace MVCSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
