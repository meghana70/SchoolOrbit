using System.Web;
using System.Web.Optimization;

namespace SchoolOrbit
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/js/app.js",
                        "~/Content/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/js/jquery.js"));
            bundles.Add(new ScriptBundle("~/bundles/IE").Include(
                        "~/Scripts/js/plugins/misc/html5shiv.js"));

            bundles.Add(new ScriptBundle("~/bundles/maxlength").Include(
                       "~/Scripts/js/jquery.plugin.min.js",
                       "~/Scripts/js/jquery.maxlength.js"));

            bundles.Add(new ScriptBundle("~/bundles/WYSWYG").Include(
                "~/Scripts/js/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/js/plugins/datatables/jquery.dataTables.min.js",
                        "~/Scripts/js/plugins/datatables/dataTables.bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/chartjs").Include(
                      "~/Scripts/js/plugins/chartjs/Chart.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/TagInput").Include(
                 "~/Scripts/js/plugins/taginput/jquery.tokeninput.js"));

            bundles.Add(new ScriptBundle("~/bundles/avatar").Include(
                     "~/Scripts/js/site.avatar.js"));

            bundles.Add(new ScriptBundle("~/bundles/jcrop").Include(
                      "~/Scripts/js/jquery.Jcrop.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                      "~/Scripts/js/jquery.form.min.js"));

            bundles.Add(new StyleBundle("~/Content/jcrop").Include(
                    "~/Content/jquery.Jcrop.css"));

            bundles.Add(new StyleBundle("~/Content/TagInput").Include(
                   "~/Scripts/js/plugins/taginput/token-input.css",
            "~/Scripts/js/plugins/taginput/token-input-facebook.css"));

            bundles.Add(new StyleBundle("~/Content/WYSWYG").Include(
                   "~/Scripts/js/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/stylev11.css",
                      "~/Scripts/js/plugins/datatables/jquery.dataTables.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/iCheck/flat/blue.css",
                       "~/Content/skins/skin-blue.css"                       
                     ));
        }
    }
}
