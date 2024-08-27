USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spMeal_Delete
    @MealId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM MOFAppSchema.Meals 
        WHERE MealId = @MealId
            AND UserId = @UserId
END