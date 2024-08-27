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
    public class WorkoutController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public WorkoutController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [AllowAnonymous]
        [HttpGet("SharedWorkouts/{workoutId}/{userId}/{searchParam}")]
        public IEnumerable<Workout> GetSharedWorkouts(int workoutId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spSharedWorkouts_Get";
            string stringParameters = "";

            DynamicParameters sqlParameters = new DynamicParameters();
            if (workoutId != 0)
            {
                stringParameters += ", @WorkoutId=@WorkoutIdParameter";
                sqlParameters.Add("@WorkoutIdParameter", workoutId, DbType.Int32);
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

            return _dapper.LoadDataWithParameters<Workout>(sql, sqlParameters);
        }

        [HttpGet("MyWorkouts/{workoutId}/{searchParam}")]
        public IEnumerable<Workout> GetMyWorkouts(int workoutId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spWorkouts_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            if (workoutId != 0)
            {
                sql += ", @WorkoutId=@WorkoutIdParameter";
                sqlParameters.Add("@WorkoutIdParameter", workoutId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                sql += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            return _dapper.LoadDataWithParameters<Workout>(sql, sqlParameters);
        }

        [HttpPut("UpsertWorkout")]
        public IActionResult UpsertWorkout(Workout workoutToUpsert)
        {
            string sql = @"EXEC MOFAppSchema.spWorkout_Upsert
                @UserId=@UserIdParameter, 
                @WorkoutTitle=@WorkoutTitleParameter, 
                @WorkoutContent=@WorkoutContentParameter,
                @Shared=@SharedParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@WorkoutTitleParameter", workoutToUpsert.WorkoutTitle, DbType.String);
            sqlParameters.Add("@WorkoutContentParameter", workoutToUpsert.WorkoutContent, DbType.String);
            sqlParameters.Add("@SharedParameter", workoutToUpsert.Shared, DbType.Boolean);

            if (workoutToUpsert.WorkoutId > 0)
            {
                sql += ", @WorkoutId=@WorkoutIdParameter";
                sqlParameters.Add("@WorkoutIdParameter", workoutToUpsert.WorkoutId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert workout!");
        }


        [HttpDelete("Workout/{workoutId}")]
        public IActionResult DeleteWorkout(int workoutId)
        {
            string sql = @"EXEC MOFAppSchema.spWorkout_Delete 
                @UserId=@UserIdParameter, 
                @WorkoutId=@WorkoutIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@WorkoutIdParameter", workoutId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to delete workout!");
        }
    }
}