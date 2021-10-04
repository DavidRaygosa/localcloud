using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistence.DapperConnection.Pagination
{
    public class PaginationRepo : IPagination
    {
        private readonly IFactoryConnection _factoryConnection;
        public PaginationRepo(IFactoryConnection factoryConnection){
            _factoryConnection = factoryConnection;
        }
        public async Task<PaginationModel> returnPagination(string storeProcedure, int PageNumber, int Elements, IDictionary<string, object> FiltherParams, string OrderBy)
        {
            PaginationModel paginationModel = new PaginationModel();
            List<IDictionary<string,object>> ReportList = null;
            int totalRecords = 0;
            int totalPages = 0;
            try{
                // CONNECTION
                var connection = _factoryConnection.GetConnection();

                // PARAMS
                DynamicParameters Params = new DynamicParameters();
                    // PARAM FILTHER
                foreach(var param in FiltherParams){
                    Params.Add("@"+param.Key, param.Value);
                }
                    // IN PARAMS
                Params.Add("@PageNumber", PageNumber);
                Params.Add("@Elements", Elements);
                Params.Add("@OrderBy", OrderBy);
                    // OUT PARAMS
                Params.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                Params.Add("@TotalPages", totalPages, DbType.Int32, ParameterDirection.Output);
                
                // STORED PROCEDURE
                var result = await connection.QueryAsync(storeProcedure, Params, commandType:CommandType.StoredProcedure);
                ReportList = result.Select(x=>(IDictionary<string,object>)x).ToList();
                paginationModel.ReportList = ReportList;
                paginationModel.PageNumber = Params.Get<int>("@TotalPages");
                paginationModel.TotalRecords = Params.Get<int>("@TotalRecords");
            }catch(Exception e){
                throw new Exception("Not Run Stored Procedure",e);
            }finally{
                _factoryConnection.CloseConnection();
            }
            return paginationModel;
        }
    }
}