using BialHackApi.Base.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BialHackApi.Base.Services
{
    public class ExampleService
    {
        private readonly IDataConnection dataConnection;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ExampleService(IDataConnection dataConnection, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.dataConnection = dataConnection;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> ExampleMethod()
        {
            using (var connect = dataConnection.Connect())
            {
                string userInfoQuery =
                    $@"SELECT anu.Id, anu.Email, anu.[Name], anu.LastSeen, us.Rating
                        FROM [dbo].[AspNetUsers] anu
                        INNER JOIN [dbo].[UserStatistic] us
                        ON anu.Id = us.[UserId]
                        WHERE anu.Id = @UserId";

                var result = await connect.QueryFirstOrDefaultAsync<object>(userInfoQuery, new
                {

                });

                return result;
            }
        }
    }
}
