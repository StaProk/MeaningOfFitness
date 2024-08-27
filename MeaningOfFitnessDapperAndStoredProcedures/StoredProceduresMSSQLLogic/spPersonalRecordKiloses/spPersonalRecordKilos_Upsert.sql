USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spPersonalRecordKiloses_Upsert
    @UserId INT, 
    @ExerciseTitle NVARCHAR(50),
    @WeightInKilos INT,
    @NumberOfRepetitions INT,
    @Shared BIT = 0, 
    @PersonalRecordKilosId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM MOFAppSchema.PersonalRecordKiloses WHERE PersonalRecordKilosId = @PersonalRecordKilosId)
        BEGIN
            INSERT INTO MOFAppSchema.PersonalRecordKiloses(
                [UserId],
                [ExerciseTitle],
                [WeightInKilos],
                [NumberOfRepetitions],
                [Shared],
                [PersonalRecordKilosCreated],
                [PersonalRecordKilosUpdated]
            ) VALUES (
                @UserId,
                @ExerciseTitle,
                @WeightInKilos,
                @NumberOfRepetitions,
                @Shared,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE MOFAppSchema.PersonalRecordKiloses 
                SET ExerciseTitle = @ExerciseTitle,
                    WeightInKilos = @WeightInKilos,
                    NumberOfRepetitions = @NumberOfRepetitions,
                    Shared = @Shared,
                    PersonalRecordKilosUpdated = GETDATE()
                WHERE PersonalRecordKilosId = @PersonalRecordKilosId
        END
END

