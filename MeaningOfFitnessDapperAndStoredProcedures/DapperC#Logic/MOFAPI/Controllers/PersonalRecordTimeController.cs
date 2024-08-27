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
    public class PersonalRecordTimeController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PersonalRecordTimeController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [AllowAnonymous]
        [HttpGet("SharedPersonalRecordTimes/{personalRecordTimeId}/{userId}/{searchParam}")]
        public IEnumerable<PersonalRecordTime> GetPersonalRecordTimes(int personalRecordTimeId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spSharedPersonalRecordTimes_Get";
            string stringParameters = "";

            DynamicParameters sqlParameters = new DynamicParameters();
            if (personalRecordTimeId != 0)
            {
                stringParameters += ", @PersonalRecordTimeId=@PersonalRecordTimeIdParameter";
                sqlParameters.Add("@PersonalRecordTimeIdParameter", personalRecordTimeId, DbType.Int32);
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

            return _dapper.LoadDataWithParameters<PersonalRecordTime>(sql, sqlParameters);
        }

        [HttpGet("MyPersonalRecordTimes/{personalRecordId}/{searchParam}")]
        public IEnumerable<PersonalRecordTime> GetMyPersonalRecordTimes(int personalRecordId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spPersonalRecordTimes_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            if (personalRecordId != 0)
            {
                sql += ", @PersonalRecordId=@PersonalRecordIdParameter";
                sqlParameters.Add("@PersonalRecordIdParameter", personalRecordId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                sql += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            return _dapper.LoadDataWithParameters<PersonalRecordTime>(sql, sqlParameters);
        }

        [HttpPut("UpsertPersonalRecordTime")]
        public IActionResult UpsertPersonalRecordTime(PersonalRecordTime personalRecordTimeToUpsert)
        {
            string sql = @"EXEC MOFAppSchema.spPersonalRecordTime_Upsert
                @UserId=@UserIdParameter, 
                @ExerciseTitle=@ExerciseTitleParameter, 
                @TimeInSeconds=@TimeInSecondsParameter,
                @Shared=@SharedParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@ExerciseTitleParameter", personalRecordTimeToUpsert.ExerciseTitle, DbType.String);
            sqlParameters.Add("@TimeInSecondsParameter", personalRecordTimeToUpsert.TimeInSeconds, DbType.Int32);
            sqlParameters.Add("@SharedParameter", personalRecordTimeToUpsert.Shared, DbType.Boolean);

            if (personalRecordTimeToUpsert.PersonalRecordTimeId > 0)
            {
                sql += ", @PersonalRecordTimeId=@PersonalRecordTimeIdParameter";
                sqlParameters.Add("@PersonalRecordTimeIdParameter", personalRecordTimeToUpsert.PersonalRecordTimeId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert personalRecordTime!");
        }


        [HttpDelete("PersonalRecordTime/{personalRecordTimeId}")]
        public IActionResult DeletePersonalRecordTime(int personalRecordTimeId)
        {
            string sql = @"EXEC MOFAppSchema.spPersonalRecordTime_Delete 
                @UserId=@UserIdParameter, 
                @PersonalRecordTimeId=@PersonalRecordTimeIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@PersonalRecordTimeIdParameter", personalRecordTimeId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to delete personalRecordTime!");
        }
    }
}