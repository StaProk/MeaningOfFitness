USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spMeals_Get 
    @UserId INT = NULL,
    @SearchValue NVARCHAR(MAX) = NULL,
    @MealId INT = NULL
AS
BEGIN
    SELECT [Meals].[MealId],
        [Meals].[UserId],
        [Meals].[MealTitle],
        [Meals].[MealContent],
        [Meals].[GramsOfCarbohydrates],
        [Meals].[GramsOfProtein],
        [Meals].[GramsOfFats],
        [Meals].[GramsOfFibers],
        [Meals].[TotalCalories],
        [Meals].[Shared], 
        [Meals].[MealCreated],
        [Meals].[MealUpdated]
    FROM MOFAppSchema.Meals AS Meals
        WHERE   Meals.UserId = ISNULL(@UserId, Meals.UserId)
            AND Meals.MealId = ISNULL(@MealId, Meals.MealId)
            AND (@SearchValue IS NULL
                OR Meals.MealContent LIKE '%' + @SearchValue + '%'
                OR Meals.MealTitle LIKE '%' + @SearchValue + '%')
END