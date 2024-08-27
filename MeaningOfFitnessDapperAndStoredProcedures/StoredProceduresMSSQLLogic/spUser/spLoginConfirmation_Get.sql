USE MOFDatabase;
GO


CREATE OR ALTER PROCEDURE MOFAppSchema.spLoginConfirmation_Get
    @Email NVARCHAR(50)
AS
BEGIN
    SELECT [Auth].[PasswordHash],
        [Auth].[PasswordSalt] 
    FROM MOFAppSchema.Auth AS Auth 
        WHERE Auth.Email = @Email
END;
GO