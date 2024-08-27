USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spSupplements_Upsert
    @UserId INT, 
    @SupplementTitle NVARCHAR(50),
    @Shared BIT = 0, 
    @SupplementId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM MOFAppSchema.Supplements WHERE SupplementId = @SupplementId)
        BEGIN
            INSERT INTO MOFAppSchema.Supplementes(
                [UserId],
                [SupplementTitle],
                [Shared],
                [SupplementCreated],
                [SupplementUpdated]
            ) VALUES (
                @UserId,
                @SupplementTitle,
                @Shared,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE MOFAppSchema.Supplements 
                SET SupplementTitle = @SupplementTitle,
                    Shared = @Shared,
                    SupplementUpdated = GETDATE()
                WHERE SupplementId = @SupplementId
        END
END

