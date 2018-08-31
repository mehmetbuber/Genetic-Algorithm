using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeneticAlgorithm.Startup))]
namespace GeneticAlgorithm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
