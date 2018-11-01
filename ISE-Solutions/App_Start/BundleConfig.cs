using System.Web;
using System.Web.Optimization;

namespace ISE_Solutions
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/app.min.js",
                       "~/JScript/Utitly.js",
                      "~/Scripts/bootstrap-datetimepicker.js"));



            /* CSS Library File */
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/font-awesome.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/skin-blue.min.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/jquery-ui.css"));



            /* Custom CSS  File */
            bundles.Add(new StyleBundle("~/Css/css").Include(
                     "~/Css/Admin.min.css",
                     "~/Css/custom.css"));

            /* Jquery Grid Css And Js File */
            bundles.Add(new ScriptBundle("~/jscript/gridJs").Include(
                     "~/Scripts/grid-0.4.3.min.js"));

            bundles.Add(new StyleBundle("~/Content/GridCss").Include(
                     "~/Content/grid-0.4.3.min.css"));

           

        }
    }
}
