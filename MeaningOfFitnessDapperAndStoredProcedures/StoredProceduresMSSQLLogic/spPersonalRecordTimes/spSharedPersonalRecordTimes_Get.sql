USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spSharedPersonalRecordTimes_Get 
    @UserId INT = NULL,
    @SearchValue NVARCHAR(MAX) = NULL,
    @PersonalRecordTimeId INT = NULL
AS
BEGIN
    SELECT [PersonalRecordTimes].[PersonalRecordTimeId],
        [PersonalRecordTimes].[UserId],
        [PersonalRecordTimes].[ExerciseTitle],
        [PersonalRecordTimes].[TimeInSeconds],
        [PersonalRecordTimes].[Shared],
        [PersonalRecordTimes].[PersonalRecordTimeCreated],
        [PersonalRecordTimes].[PersonalRecordTimeUpdated]
    FROM MOFAppSchema.PersonalRecordTimes AS PersonalRecordTimes
        WHERE PersonalRecordTimes.Shared = 1
            AND PersonalRecordTimes.UserId = ISNULL(@UserId, PersonalRecordTimes.UserId)
            AND PersonalRecordTimes.PersonalRecordTimeId = ISNULL(@PersonalRecordTimeId, PersonalRecordTimes.PersonalRecordTimeId)  
            AND (@SearchValue IS NULL
                OR PersonalRecordTimes.ExerciseTitle LIKE '%' + @SearchValue + '%')
END