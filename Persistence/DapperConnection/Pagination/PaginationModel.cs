using System.Collections.Generic;
namespace Persistence.DapperConnection.Pagination
{
    public class PaginationModel
    {
        public List<IDictionary<string,object>> ReportList{get;set;}
        public int TotalRecords{get;set;}
        public int PageNumber{get;set;}   
    }
}