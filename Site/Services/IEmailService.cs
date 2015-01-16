using Domain.Entities;

namespace WebBase.Mvc.Services
{
    public interface IEmailService
    {
        bool Ping();

        void SendPasswordReset(User user, string resetUrl);       
    }
}