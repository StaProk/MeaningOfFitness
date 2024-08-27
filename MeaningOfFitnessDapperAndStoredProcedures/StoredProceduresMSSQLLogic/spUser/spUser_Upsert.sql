USE MOFDatabase;
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spUser_Upsert
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@Email NVARCHAR(50),
	@Gender NVARCHAR(50),
    @SportTitle NVARCHAR(50),
    @HeightInCentimeters INT,
    @WeightOfPersonInKilos INT,
	@Active BIT = 1,
	@UserId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM MOFAppSchema.Users WHERE UserId = @UserId)
        BEGIN
        IF NOT EXISTS (SELECT * FROM MOFAppSchema.Users WHERE Email = @Email)
            BEGIN
                INSERT INTO MOFAppSchema.Users(
                    [FirstName],
                    [LastName],
                    [Email],
                    [Gender],
                    [SportTitle],
                    [HeightInCentimeters],
                    [WeightOfPersonInKilos],
                    [Active]
                ) VALUES (
                    @FirstName,
                    @LastName,
                    @Email,
                    @Gender,
                    @SportTitle,
                    @HeightInCentimeters,
                    @WeightOfPersonInKilos,
                    @Active
                )
            END
        END
    ELSE 
        BEGIN
            UPDATE MOFAppSchema.Users 
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    Gender = @Gender,
                    SportTitle = @SportTitle,
                    HeightInCentimeters = @HeightInCentimeters,
                    WeightOfPersonInKilos = @WeightOfPersonInKilos,
                    Active = @Active
                WHERE UserId = @UserId
        END
END;
GO
