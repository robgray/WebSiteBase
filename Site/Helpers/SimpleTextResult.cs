using System.Web.Mvc;

namespace WebBase.Mvc.Helpers
{
    public class SimpleTextResult : ContentResult
    {
        public SimpleTextResult(string message)
        {
            this.Content = message;
        }
    }
}