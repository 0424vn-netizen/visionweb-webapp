using AS.Common.DBManager;
using AS.VW.PCI.Api.Client.Common;
using AS.VW.PCI.Api.Client.Settings;
using System;
using System.Data;

namespace AS.VW.PCI.Api.Client.Providers
{
    /*
     * Table required (create once):
     *
     *   CREATE TABLE PCI_TokenCache (
     *       ApplicationId  INT           NOT NULL PRIMARY KEY,
     *       AccessToken    NVARCHAR(MAX) NOT NULL,
     *       TokenType      NVARCHAR(50)  NOT NULL,
     *       ExpireAt       DATETIME      NOT NULL,
     *       UpdatedAt      DATETIME      NOT NULL DEFAULT GETUTCDATE()
     *   )
     *
     * Stored procedures required:
     *
     *   spa_PCI_GetToken     @ApplicationId INT
     *       → SELECT AccessToken, TokenType, ExpireAt WHERE ApplicationId = @ApplicationId
     *
     *   spa_PCI_UpsertToken  @ApplicationId INT, @AccessToken NVARCHAR(MAX),
     *                        @TokenType NVARCHAR(50), @ExpireAt DATETIME
     *       → MERGE / INSERT OR UPDATE into PCI_TokenCache
     */
    public class DbTokenRepository : ITokenRepository
    {
        public TokenCacheEntry GetToken(int applicationId)
        {
            var parameters = new FilterParameterCollection
            {
                new FilterParameter("@ApplicationId", applicationId, DbType.Int32)
            };

            var dt = PCIClientSettings.Instance.PciReportServices.GetReports("spa_PCI_GetToken", parameters);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new TokenCacheEntry
            {
                AccessToken = row["AccessToken"].ToString(),
                TokenType   = row["TokenType"].ToString(),
                ExpireAt    = Convert.ToDateTime(row["ExpireAt"])
            };
        }

        public void SaveToken(int applicationId, string accessToken, string tokenType, DateTime expireAt)
        {
            var parameters = new FilterParameterCollection
            {
                new FilterParameter("@ApplicationId", applicationId, DbType.Int32),
                new FilterParameter("@AccessToken",   accessToken,   DbType.String),
                new FilterParameter("@TokenType",     tokenType,     DbType.AnsiString),
                new FilterParameter("@ExpireAt",      expireAt,      DbType.DateTime)
            };

            PCIClientSettings.Instance.PciReportServices.ExecuteNonQueryCommand("spa_PCI_UpsertToken", parameters, out _);
        }
    }
}
