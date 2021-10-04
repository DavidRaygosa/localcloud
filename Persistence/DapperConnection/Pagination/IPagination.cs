using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.DapperConnection.Pagination
{
    public interface IPagination
    {
         Task<PaginationModel> returnPagination(string storeProcedure, int PageNumber, int Elements, IDictionary<string,object> FiltherParams, string OrderBy);
    }
}