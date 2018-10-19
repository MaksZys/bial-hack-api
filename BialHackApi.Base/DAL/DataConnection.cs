using BialHackApi.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BialHackApi.Base.DAL
{
    public class DataConnection : IDataConnection
    {
        private readonly IConfiguration configuration;

        public DataConnection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IDbConnection Connect() => new SqlConnection(configuration.GetConnectionString("DataContext"));
    }
}
