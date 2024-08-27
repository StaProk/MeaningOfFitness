USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spMeals_Upsert
    @UserId INT, 
    @MealTitle NVARCHAR(50),
    @MealContent NVARCHAR(50),
    @GramsOfCarbohydrates INT,
    @GramsOfProtein INT,
    @GramsOfFats INT,
    @GramsOfFibers INT,
    @Shared BIT = 0, 
    @MealId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM MOFAppSchema.Meals WHERE MealId = @MealId)
        BEGIN
            INSERT INTO MOFAppSchema.Meals(
                [UserId],
                [MealTitle],
                [MealContent],
                [GramsOfCarbohydrates],
                [GramsOfProtein],
                [GramsOfFats],
                [GramsOfFibers],
                [TotalCalories],
                [Shared],
                [MealCreated],
                [MealUpdated]
            ) VALUES (
                @UserId,
                @MealTitle,
                @MealContent,
                @GramsOfCarbohydrates,
                @GramsOfProtein,
                @GramsOfFats,
                @GramsOfFibers,
                (@GramsOfCarbohydrates * 4 + @GramsOfProtein * 4 + @GramsOfFats * 9),
                @Shared,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE MOFAppSchema.Meals 
                SET MealTitle = @MealTitle,
                    MealContent = @MealContent,
                    GramsOfCarbohydrates = @GramsOfCarbohydrates,
                    GramsOfProtein = @GramsOfProtein,
                    GramsOfFats = @GramsOfFats,
                    GramsOfFibers = @GramsOfFibers,
                    TotalCalories = (@GramsOfCarbohydrates * 4 + @GramsOfProtein * 4 + @GramsOfFats * 9),
                    Shared = @Shared,
                    MealUpdated = GETDATE()
                WHERE MealId = @MealId
        END
END

