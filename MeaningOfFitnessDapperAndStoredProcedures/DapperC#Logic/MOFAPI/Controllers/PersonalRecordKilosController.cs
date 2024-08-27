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
    public class PersonalRecordKilosController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PersonalRecordKilosController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [AllowAnonymous]
        [HttpGet("SharedPersonalRecordKiloses/{personalRecordKilosId}/{userId}/{searchParam}")]
        public IEnumerable<PersonalRecordKilos> GetSharedPersonalRecordKiloses(int personalRecordKilosId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spSharedPersonalRecordKiloses_Get";
            string stringParameters = "";

            DynamicParameters sqlParameters = new DynamicParameters();
            if (personalRecordKilosId != 0)
            {
                stringParameters += ", @PersonalRecordKilosId=@PersonalRecordKilosIdParameter";
                sqlParameters.Add("@PersonalRecordKilosIdParameter", personalRecordKilosId, DbType.Int32);
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

            return _dapper.LoadDataWithParameters<PersonalRecordKilos>(sql, sqlParameters);
        }

        [HttpGet("MyPersonalRecordKiloses/{personalRecordKilosId}/{searchParam}")]
        public IEnumerable<PersonalRecordKilos> GetMyPersonalRecordKiloses(int personalRecordKilosId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spPersonalRecordKiloses_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            if (personalRecordKilosId != 0)
            {
                sql += ", @PersonalRecordKilosId=@PersonalRecordKilosIdParameter";
                sqlParameters.Add("@PersonalRecordKilosIdParameter", personalRecordKilosId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                sql += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            return _dapper.LoadDataWithParameters<PersonalRecordKilos>(sql, sqlParameters);
        }

        [HttpPut("UpsertPersonalRecordKilos")]
        public IActionResult UpsertPersonalRecordKilos(PersonalRecordKilos personalRecordKilosToUpsert)
        {
            string sql = @"EXEC MOFAppSchema.spPersonalRecordKilos_Upsert
                @UserId=@UserIdParameter, 
                @ExerciseTitle=@ExerciseTitleParameter, 
                @WeightInKilos=@WeightInKilosParameter,
                @NumberOfRepetitions=@NumberOfRepetitionsParameter,
                @Shared=@SharedParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@ExerciseTitleParameter", personalRecordKilosToUpsert.ExerciseTitle, DbType.String);
            sqlParameters.Add("@WeightInKilosParameter", personalRecordKilosToUpsert.WeightInKilos, DbType.Int32);
            sqlParameters.Add("@NumberOfRepetitionsParameter", personalRecordKilosToUpsert.NumberOfRepetitions, DbType.Int32);
            sqlParameters.Add("@SharedParameter", personalRecordKilosToUpsert.Shared, DbType.Boolean);

            if (personalRecordKilosToUpsert.PersonalRecordKilosId > 0)
            {
                sql += ", @PersonalRecordKilosId=@PersonalRecordKilosIdParameter";
                sqlParameters.Add("@PersonalRecordKilosIdParameter", personalRecordKilosToUpsert.PersonalRecordKilosId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert personalRecordKilos!");
        }


        [HttpDelete("PersonalRecordKilos/{personalRecordKilosId}")]
        public IActionResult DeletePersonalRecordKilos(int personalRecordKilosId)
        {
            string sql = @"EXEC MOFAppSchema.spPersonalRecordKilos_Delete 
                @UserId=@UserIdParameter, 
                @PersonalRecordKilosId=@PersonalRecordKilosIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@PersonalRecordKilosIdParameter", personalRecordKilosId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to delete personalRecordKilos!");
        }
    }
}