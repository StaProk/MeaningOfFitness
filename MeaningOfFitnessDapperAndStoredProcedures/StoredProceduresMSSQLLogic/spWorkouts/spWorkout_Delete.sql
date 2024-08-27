USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spWorkout_Delete
    @WorkoutId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM MOFAppSchema.Workouts 
        WHERE WorkoutId = @WorkoutId
            AND UserId = @UserId
END