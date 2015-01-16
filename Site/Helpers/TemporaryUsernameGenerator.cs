using Infrastructure.Common;

namespace WebBase.Mvc.Helpers
{
    public class TemporaryUsernameGenerator
    {
        public string Generate()
        {
            return DateTimeHelper.Now.ToString("yyyyMMddHHmmss");
        }
    }
}