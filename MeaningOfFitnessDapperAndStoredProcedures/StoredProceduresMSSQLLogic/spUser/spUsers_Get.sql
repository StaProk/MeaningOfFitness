USE MOFDatabase;
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spUsers_Get
    @UserId INT = NULL
AS
BEGIN
    SELECT [Users].[UserId],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Gender],
        [Users].[SportTitle],
        [Users].[HeightInCentimeters],
        [Users].[WeightOfPersonInKilos],
        [Users].[Active]
    FROM MOFAppSchema.Users AS Users 
        WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
END;
GO