USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spPersonalRecordTimes_Upsert
    @UserId INT, 
    @ExerciseTitle NVARCHAR(50),
    @TimeInSeconds INT,
    @Shared BIT = 0, 
    @PersonalRecordTimeId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM MOFAppSchema.PersonalRecordTimes WHERE PersonalRecordTimeId = @PersonalRecordTimeId)
        BEGIN
            INSERT INTO MOFAppSchema.PersonalRecordTimes(
                [UserId],
                [ExerciseTitle],
                [TimeInSeconds],
                [Shared],
                [PersonalRecordTimeCreated],
                [PersonalRecordTimeUpdated]
            ) VALUES (
                @UserId,
                @ExerciseTitle,
                @TimeInSeconds,
                @Shared,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE MOFAppSchema.PersonalRecordTimes 
                SET ExerciseTitle = @ExerciseTitle,
                    TimeInSeconds = @TimeInSeconds,
                    Shared = @Shared,
                    PersonalRecordTimeUpdated = GETDATE()
                WHERE PersonalRecordTimeId = @PersonalRecordTimeId
        END
END

