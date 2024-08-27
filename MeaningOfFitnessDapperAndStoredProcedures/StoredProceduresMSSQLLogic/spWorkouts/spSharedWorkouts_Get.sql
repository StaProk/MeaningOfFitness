USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spSharedWorkouts_Get 
    @UserId INT = NULL,
    @SearchValue NVARCHAR(MAX) = NULL,
    @WorkoutId INT = NULL
AS
BEGIN
    SELECT [Workouts].[WorkoutId],
        [Workouts].[UserId],
        [Workouts].[WorkoutTitle],
        [Workouts].[WorkoutContent],
        [Workouts].[Shared],
        [Workouts].[WorkoutCreated],
        [Workouts].[WorkoutUpdated]
    FROM MOFAppSchema.Workouts AS Workouts
        WHERE Workouts.Shared = 1
            AND Workouts.UserId = ISNULL(@UserId, Workouts.UserId)
            AND Workouts.WorkoutId = ISNULL(@WorkoutId, Workouts.WorkoutId)  
            AND (@SearchValue IS NULL
                OR Workouts.WorkoutTitle LIKE '%' + @SearchValue + '%'
                OR Workouts.WorkoutContent LIKE '%' + @SearchValue + '%')
END