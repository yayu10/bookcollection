using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookCollectionWebsite.Startup))]
namespace BookCollectionWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
