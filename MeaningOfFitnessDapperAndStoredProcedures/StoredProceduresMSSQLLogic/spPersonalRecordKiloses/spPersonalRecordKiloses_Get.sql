USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spPersonalRecordKiloses_Get 
    @UserId INT = NULL,
    @SearchValue NVARCHAR(MAX) = NULL,
    @PersonalRecordKilosId INT = NULL
AS
BEGIN
    SELECT [PersonalRecordKiloses].[PersonalRecordKilosId],
        [PersonalRecordKiloses].[UserId],
        [PersonalRecordKiloses].[ExerciseTitle],
        [PersonalRecordKiloses].[WeightInKilos],
        [PersonalRecordKiloses].[NumberOfRepetitions],
        [PersonalRecordKiloses].[Shared],
        [PersonalRecordKiloses].[PersonalRecordKilosCreated],
        [PersonalRecordKiloses].[PersonalRecordKilosUpdated]
    FROM MOFAppSchema.PersonalRecordKiloses AS PersonalRecordKiloses
        WHERE PersonalRecordKiloses.UserId = ISNULL(@UserId, PersonalRecordKiloses.UserId)
            AND PersonalRecordKiloses.PersonalRecordKilosId = ISNULL(@PersonalRecordKilosId, PersonalRecordKiloses.PersonalRecordKilosId)
            AND (@SearchValue IS NULL
                OR PersonalRecordKiloses.ExerciseTitle LIKE '%' + @SearchValue + '%')
END