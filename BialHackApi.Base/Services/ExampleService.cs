using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BialHackApi.Base.Services
{
    public class ExampleService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ExampleService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> ExampleMethod()
        {
            var setting = configuration["Example:Setting"];
            var httpContext = httpContextAccessor.HttpContext;

            return new object();
        }
    }
}
