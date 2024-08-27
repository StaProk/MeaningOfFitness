using System.Data;
using Dapper;
using MOFAPI.Data;
using MOFAPI.Models;

namespace MOFAPI.Helpers
{
    public class ReusableSql
    {
        private readonly DataContextDapper _dapper;
        public ReusableSql(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        public bool UpsertUser(User user)
        {
            string sql = @"EXEC MOFAppSchema.spUser_Upsert
                @FirstName = @FirstNameParameter, 
                @LastName = @LastNameParameter, 
                @Email = @EmailParameter, 
                @Gender = @GenderParameter, 
                @SportTitle = @SportTitleParameter,  
                @HeightInCentimeters = @HeightInCentimetersParameter,
                @WeightOfPersonInKilos = @WeightOfPersonInKilosParameter, 
                @Active = @ActiveParameter, 
                @UserId = @UserIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();

            sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
            sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
            sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
            sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
            sqlParameters.Add("@SportTitleParameter", user.SportTitle, DbType.String);
            sqlParameters.Add("@HeightInCentimetersParameter", user.HeightInCentimeters, DbType.Int32);
            sqlParameters.Add("@WeightOfPersonInKilosParameter", user.WeightOfPersonInKilos, DbType.Int32);
            sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
            sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

            return _dapper.ExecuteSqlWithParameters(sql, sqlParameters);
        }
    }
}