using System.Data;
using Dapper;
using MOFAPI.Data;
using MOFAPI.Dtos;
using MOFAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MOFAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SupplementController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public SupplementController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [AllowAnonymous]
        [HttpGet("SharedSupplements/{supplementId}/{userId}/{searchParam}")]
        public IEnumerable<Supplement> GetSupplements(int supplementId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spSharedSupplements_Get";
            string stringParameters = "";

            DynamicParameters sqlParameters = new DynamicParameters();
            if (supplementId != 0)
            {
                stringParameters += ", @SupplementId=@SupplementIdParameter";
                sqlParameters.Add("@SupplementIdParameter", supplementId, DbType.Int32);
            }
            if (userId != 0)
            {
                stringParameters += ", @UserId=@UserIdParameter";
                sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                stringParameters += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            if (stringParameters.Length > 0)
            {
                sql += stringParameters.Substring(1);
            }

            return _dapper.LoadDataWithParameters<Supplement>(sql, sqlParameters);
        }

        [HttpGet("MySupplements/{supplementId}/{searchParam}")]
        public IEnumerable<Supplement> GetMySupplements(int supplementId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spSupplements_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            if (supplementId != 0)
            {
                sql += ", @SupplementId=@SupplementIdParameter";
                sqlParameters.Add("@SupplementIdParameter", supplementId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                sql += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            return _dapper.LoadDataWithParameters<Supplement>(sql, sqlParameters);
        }

        [HttpPut("UpsertSupplement")]
        public IActionResult UpsertSupplement(Supplement supplementToUpsert)
        {
            string sql = @"EXEC MOFAppSchema.spSupplement_Upsert
                @UserId=@UserIdParameter, 
                @SupplementTitle=@SupplementTitleParameter, 
                @Shared=@SharedParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@SupplementTitleParameter", supplementToUpsert.SupplementTitle, DbType.String);
            sqlParameters.Add("@SharedParameter", supplementToUpsert.Shared, DbType.Boolean);

            if (supplementToUpsert.SupplementId > 0)
            {
                sql += ", @SupplementId=@SupplementIdParameter";
                sqlParameters.Add("@SupplementIdParameter", supplementToUpsert.SupplementId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert supplement!");
        }


        [HttpDelete("Supplement/{supplementId}")]
        public IActionResult DeleteSupplement(int supplementId)
        {
            string sql = @"EXEC MOFAppSchema.spSupplement_Delete 
                @UserId=@UserIdParameter, 
                @SupplementId=@SupplementIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@SupplementIdParameter", supplementId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to delete supplement!");
        }
    }
}