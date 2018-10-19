using System.Data;

namespace BialHackApi.Base.Interfaces
{
    public interface IDataConnection
    {
        IDbConnection Connect();
    }
}
