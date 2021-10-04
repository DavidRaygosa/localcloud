using System.Data;
namespace Persistence.DapperConnection
{
    public interface IFactoryConnection
    {
         void CloseConnection();
         IDbConnection GetConnection();
    }
}