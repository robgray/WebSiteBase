using System.Web.Http;
using Castle.Core.Logging;
using Domain.Interfaces;
using Infrastructure.Common;

namespace WebBase.Mvc.Api
{
    public abstract class ApiControllerBase : ApiController
    {        
        public ILogger Logger { get; set; }
        
        public IUserRepository UserRepository { get; set; }

        public ICryptographyProvider CryptographyProvider { get; set; }

        public ITransactioner Transactioner { get; set; }
    }
}