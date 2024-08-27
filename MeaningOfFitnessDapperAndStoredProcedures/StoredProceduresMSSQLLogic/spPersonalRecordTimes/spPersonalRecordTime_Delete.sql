USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spPersonalRecordTime_Delete
    @PersonalRecordTimeId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM MOFAppSchema.PersonalRecordTimes 
        WHERE PersonalRecordTimeId = @PersonalRecordTimeId
            AND UserId = @UserId
END