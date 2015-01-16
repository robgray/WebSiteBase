using System.Web.Optimization;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace WebBase.Mvc.App_Start
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
		    var builder = new ScriptBundleBuilder();

		    bundles.UseCdn = true;

            bundles.Add(builder.Build("~/bundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js").Include(
                    "~/Scripts/libs/jquery-{version}.js",
                    "~/Scripts/App/ajaxFileUpload.js"));

            bundles.Add(builder.Build("~/bundles/jqueryui", 
                                "//ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js")
                                .Include("~/Scripts/libs/jquery-ui-{version}.js"));

            bundles.Add(builder.Build("~/bundles/toastr").Include(
                "~/Scripts/libs/toastr.js",
                "~/Scripts/App/toastr.config.js"));

            bundles.Add(builder.Build("~/bundles/jqueryval").Include(
                                "~/Scripts/libs/jquery.unobtrusive*",
                                "~/Scripts/libs/jquery.validate*",
                                "~/Scripts/App/custom.jquery.validate.js"));

            bundles.Add(builder.Build("~/bundles/bootstrap").Include(
                        "~/Scripts/libs/bootstrap.js",
                        "~/Scripts/libs/bootstrap-datepicker.js"));

            bundles.Add(builder.Build("~/bundles/customval").Include(
                "~/Scripts/App/RequiredValidationAdorner.js"));

            bundles.Add(builder.Build("~/bundles/site").Include(
		        "~/Scripts/libs/moment.js",
		        "~/Scripts/libs/knockout-{version}.js",
		        "~/Scripts/App/knockout.bindings.js",
		        "~/Scripts/libs/knockout.dirtyFlag.js",
                "~/Scripts/libs/knockout.validation.debug.js",		        
		        "~/Scripts/App/knockout.validation.custom.js",
		        "~/Scripts/libs/koExternalTemplateEngine_all.js",
		        "~/Scripts/libs/underscore.js",
		        "~/Scripts/libs/underscore-ko-{version}.js",
		        "~/Scripts/App/site.js"));                
            
			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(builder.Build("~/bundles/modernizr").Include(
                        "~/Scripts/libs/modernizr-*"));

		    RegisterViewModelBundles(bundles, builder);
            
		    RegisterStyleBundles(bundles);

            // If you'd like to test the optimization locally
            // you can use this line to force it
		    //BundleTable.EnableOptimizations = true;
		}
       
        private static void RegisterViewModelBundles(BundleCollection bundles, ScriptBundleBuilder builder)
        {
            // Directory include taken out because we need to specify order to load.
            // Potential use of RequireJS in the future.
            //.IncludeDirectory("~/Scripts/App/Views/", "*.js", searchSubdirectories: true));

            // Implement a bundle per view and place the bundle code on that view.
            
        }

        public static void RegisterStyleBundles(BundleCollection bundles)
        {
            var cssTransformer = new CssTransformer();
            var nullOrderder = new NullOrderer();
            
            var stylesBundle= new StyleBundle("~/bundles/css").Include(
//                "~/Content/bootstrap.css",
//                "~/Content/bootstrap-responsive.css",                
                //"~/Content/bootstrap-datepicker.less",
                //"~/Content/bootstrap-override.less",
                //"~/Content/toastr.less",
                "~/Content/site.less");
            stylesBundle.Transforms.Add(cssTransformer);
            stylesBundle.Transforms.Add(new CssMinify());
            stylesBundle.Orderer = nullOrderder;

            bundles.Add(stylesBundle);

        }
	}	
}