using System.Web.Optimization;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace WebBase.Mvc.App_Start
{
    public class ScriptBundleBuilder
    {
        private readonly JsTransformer jsTransformer;
        private readonly NullOrderer nullOrderer;

        public ScriptBundleBuilder()
        {
            jsTransformer = new JsTransformer();
            nullOrderer = new NullOrderer();
        }

        public Bundle Build(string bundleName, string cdn = null)
        {
            var bundle = !string.IsNullOrEmpty(cdn) ? new ScriptBundle(bundleName, cdn) : new ScriptBundle(bundleName);

            bundle.Transforms.Add(jsTransformer);
            bundle.Orderer = nullOrderer;

            return bundle;
        }        
    }
}