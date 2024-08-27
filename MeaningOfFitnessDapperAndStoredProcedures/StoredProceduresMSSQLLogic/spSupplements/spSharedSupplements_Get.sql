USE MOFDatabase
GO

CREATE OR ALTER PROCEDURE MOFAppSchema.spSharedSupplements_Get 
    @UserId INT = NULL,
    @SearchValue NVARCHAR(MAX) = NULL,
    @SupplementId INT = NULL
AS
BEGIN
    SELECT [Supplements].[SupplementId],
        [Supplements].[UserId],
        [Supplements].[SupplementTitle],
        [Supplements].[Shared],
        [Supplements].[SupplementCreated],
        [Supplements].[SupplementUpdated]
    FROM MOFAppSchema.Supplements AS Supplements
        WHERE Supplements.Shared = 1
            AND Supplements.UserId = ISNULL(@UserId, Supplements.UserId)
            AND Supplements.SupplementId = ISNULL(@SupplementId, Supplements.SupplementId)  
            AND (@SearchValue IS NULL
                OR Supplements.SupplementTitle LIKE '%' + @SearchValue + '%')
END