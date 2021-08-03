using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public static class Routing
    {
        public static void Include(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                ////About
                //endpoints.MapControllerRoute(
                //    name: "About",
                //    pattern: "{culture}/About/{id?}", new
                //    {
                //        controller = "About",
                //        action = "Index",
                //    });

                ////Cart
                //endpoints.MapControllerRoute(
                //    name: "Cart",
                //    pattern: "{culture}/{id?}", new
                //    {
                //        controller = "Cart",
                //        action = "Index",
                //    });

                ////Contact
                //endpoints.MapControllerRoute(
                //    name: "Contact",
                //    pattern: "{culture}/Contact/{id?}", new
                //    {
                //        controller = "Contact",
                //        action = "Index",
                //    });

                ////News
                //endpoints.MapControllerRoute(
                //    name: "News",
                //    pattern: "{culture}/News/{id?}", new
                //    {
                //        controller = "News",
                //        action = "Index",
                //    });

                ////Product
                //endpoints.MapControllerRoute(
                //    name: "Product",
                //    pattern: "{culture}/Product/{id?}", new
                //    {
                //        controller = "Product",
                //        action = "Index",
                //    });

                //Product Detail
                endpoints.MapControllerRoute(
                    name: "Product Detail Vn",
                    pattern: "{culture}/san-pham/{id}", new
                    {
                        controller = "Product",
                        action = "Detail"
                    });

                endpoints.MapControllerRoute(
                    name: "Product Detail En",
                    pattern: "{culture}/product/{id}", new
                    {
                        controller = "Product",
                        action = "Detail"
                    });

                //Default
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
