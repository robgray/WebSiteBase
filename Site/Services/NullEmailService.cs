using Castle.Core.Logging;
using Domain.Entities;

namespace WebBase.Mvc.Services
{
    public class NullEmailService : IEmailService
    {
        private ILogger Log { get; set; }
        
        public NullEmailService(ILogger logger)
        {
            Log = logger.CreateChildLogger("Email");
        }

        public bool Ping()
        {
            Log.Debug("Dummy PING email service");
            return true;
        }

        public void SendPasswordReset(User user, string resetUrl)
        {
            Log.Debug("Dummy send password reset email to user");
        }   
    }
}