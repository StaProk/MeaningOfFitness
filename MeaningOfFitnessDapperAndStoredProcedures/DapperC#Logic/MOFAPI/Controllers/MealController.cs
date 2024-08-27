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
    public class MealController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public MealController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [AllowAnonymous]
        [HttpGet("SharedMeals/{mealId}/{userId}/{searchParam}")]
        public IEnumerable<Meal> GetSharedMeals(int mealId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spSharedMeals_Get";
            string stringParameters = "";

            DynamicParameters sqlParameters = new DynamicParameters();
            if (mealId != 0)
            {
                stringParameters += ", @MealId=@MealIdParameter";
                sqlParameters.Add("@MealIdParameter", mealId, DbType.Int32);
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

            return _dapper.LoadDataWithParameters<Meal>(sql, sqlParameters);
        }

        [HttpGet("MyMeals/{mealId}/{searchParam}")]
        public IEnumerable<Meal> GetMyMeals(int mealId = 0, string searchParam = "None")
        {
            string sql = @"EXEC MOFAppSchema.spMeals_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            if (mealId != 0)
            {
                sql += ", @MealId=@MealIdParameter";
                sqlParameters.Add("@MealIdParameter", mealId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                sql += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            return _dapper.LoadDataWithParameters<Meal>(sql, sqlParameters);
        }

        [HttpPut("UpsertMeal")]
        public IActionResult UpsertMeal(Meal mealToUpsert)
        {
            string sql = @"EXEC MOFAppSchema.spMeal_Upsert
                @UserId=@UserIdParameter, 
                @MealTitle=@MealTitleParameter,
                @MealContent=@MealContentParameter,
                @GramsOfCarbohydrates=@GramsOfCarbohydratesParameter,
                @GramsOfProtein=@GramsOfProteinParameter,
                @GramsOfFats=@GramsOfFatsParameter,
                @GramsOfFibers=@GramsOfFibersParameter, 
                @Shared=@SharedParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@MealTitleParameter", mealToUpsert.MealTitle, DbType.String);
            sqlParameters.Add("@MealContentParameter", mealToUpsert.MealContent, DbType.String);
            sqlParameters.Add("@GramsOfCarbohydratesParameter", mealToUpsert.GramsOfCarbohydrates, DbType.Int32);
            sqlParameters.Add("@GramsOfProteinParameter", mealToUpsert.GramsOfProtein, DbType.Int32);
            sqlParameters.Add("@GramsOfFatsParameter", mealToUpsert.GramsOfFats, DbType.Int32);
            sqlParameters.Add("@GramsOfFibersParameter", mealToUpsert.GramsOfFibers, DbType.Int32);
            sqlParameters.Add("@SharedParameter", mealToUpsert.Shared, DbType.Boolean);

            if (mealToUpsert.MealId > 0)
            {
                sql += ", @MealId=@MealIdParameter";
                sqlParameters.Add("@MealIdParameter", mealToUpsert.MealId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert meal!");
        }


        [HttpDelete("Meal/{mealId}")]
        public IActionResult DeleteMeal(int mealId)
        {
            string sql = @"EXEC MOFAppSchema.spMeal_Delete 
                @UserId=@UserIdParameter, 
                @MealId=@MealIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@MealIdParameter", mealId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }

            throw new Exception("Failed to delete meal!");
        }
    }
}