USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spWorkout_Upsert
    @UserId INT, 
    @WorkoutTitle NVARCHAR(50),
    @WorkoutContent NVARCHAR(MAX),
    @Shared BIT = 0, 
    @WorkoutId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM MOFAppSchema.Workouts WHERE WorkoutId = @WorkoutId)
        BEGIN
            INSERT INTO MOFAppSchema.Workouts(
                [UserId],
                [WorkoutTitle],
                [WorkoutContent],
                [Shared],
                [WorkoutCreated],
                [WorkoutUpdated]
            ) VALUES (
                @UserId,
                @WorkoutTitle,
                @WorkoutContent,
                @Shared,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE MOFAppSchema.Workouts 
                SET WorkoutTitle = @WorkoutTitle,
                    Shared = @Shared,
                    WorkoutUpdated = GETDATE()
                WHERE WorkoutId = @WorkoutId
        END
END

