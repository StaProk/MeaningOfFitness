USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spPersonalRecordKilos_Delete
    @PersonalRecordKilosId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM MOFAppSchema.PersonalRecordKiloses 
        WHERE PersonalRecordKilosId = @PersonalRecordKilosId
            AND UserId = @UserId
END