using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroceryShoppingCart.Startup))]
namespace GroceryShoppingCart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
