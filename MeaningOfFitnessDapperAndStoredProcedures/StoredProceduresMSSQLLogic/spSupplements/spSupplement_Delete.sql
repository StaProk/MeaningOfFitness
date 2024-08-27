USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spSupplement_Delete
    @SupplementId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM MOFAppSchema.Supplements 
        WHERE SupplementId = @SupplementId
            AND UserId = @UserId
END