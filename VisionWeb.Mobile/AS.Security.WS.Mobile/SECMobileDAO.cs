using AS.Common.DBManager;
using AS.Security.WS.Entities;
using System.Data;
using System.Linq;

namespace AS.Security.WS.Mobile
{
    public partial class SecMobileDao : BaseDAO
    {
        private readonly ASSqlDatabase _dbmanager = null;

        public SecMobileDao(string connString)
        {
            _dbmanager = DatabaseManager.Create(connString);
        }

        public MobileUser GetMobileUser(int clientId, string userId)
        {
            var pr = GetCommonParameters(clientId, userId);

            DataSet ds = base.GetDataSet(_dbmanager, "spa_SEC_GetMobileUsers", pr);
            if (ds == null || ds.Tables.Count < 3 || ds.Tables[2].Rows.Count == 0)
            {
                return null;
            }

            //Base out columns
            MobileUser user = ds.Tables[2].Rows[0].To<MobileUser>();

            //Permission
            var permissions = ds.Tables[0].To<Permission>();
            user.Permissions = permissions.Select(p => p.PermissionCode).ToArray();
             
            //Additional output columns
            var obj = ds.Tables[1].Rows[0].To<MobileUser>();
            user.HasMobileAccess = obj.HasMobileAccess;
            user.UserMode = obj.UserMode;
            user.IsChainHQ = obj.IsChainHQ;

            return user;
        }

        private FilterParameterCollection GetCommonParameters(int clientId, string userId)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@ASClientID",
                ParameterType = DbType.Int32,
                ParameterValue = clientId
            });
            parameters.Add(new FilterParameter()
            {
                ParameterName = "@UserID",
                ParameterType = DbType.String,
                ParameterValue = userId
            });
            return parameters;
        }

    }
}
