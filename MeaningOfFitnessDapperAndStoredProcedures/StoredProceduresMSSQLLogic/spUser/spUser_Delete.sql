USE MOFDatabase
GO

CREATE PROCEDURE MOFAppSchema.spUser_Delete
    @UserId INT
AS
BEGIN
    DECLARE @Email NVARCHAR(50);

    SELECT  @Email = Users.Email
      FROM  MOFAppSchema.Users
     WHERE  Users.UserId = @UserId;

    DELETE  FROM MOFAppSchema.Users
     WHERE  Users.UserId = @UserId;

    DELETE  FROM MOFAppSchema.Auth
     WHERE  Auth.Email = @Email;
END;
GO