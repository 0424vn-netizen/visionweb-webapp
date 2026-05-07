using AS.VW.Api.Model.Security;
using System;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace AS.VW.Api.Utility.Utils
{
    public static class UserHelper
    {
        private static bool IsCheckUserInWhiteList = CommonHelper.GetConfig("IsCheckUserInWhiteList", false);
        private static string WhiteListUsers = CommonHelper.GetConfig("WhiteListUsers", string.Empty);

        public static UserModel GetUser(string authHeader)
        {
            if (!string.IsNullOrEmpty(authHeader))
            {
                var svcCredentials = Encoding.ASCII
                        .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                        .Split(':');

                return new UserModel { Name = svcCredentials[0], Password = svcCredentials[1] };
            }

            return null;
        }

        public static UserModel GetUser(HttpRequest request)
        {
            if (request == null || request.Headers["Authorization"] == null)
                return null;

            var authHeader = request.Headers["Authorization"];
            return GetUser(authHeader);
        }

        public static UserModel GetUser(IncomingWebRequestContext request)
        {
            if (request == null || request.Headers["Authorization"] == null)
                return null;

            var authHeader = request.Headers["Authorization"];
            return GetUser(authHeader);
        }

        public static bool CanAcess(HttpRequest request)
        {
            var user = GetUser(request);

            return CanAcess(user);
        }

        public static bool CanAcess(IncomingWebRequestContext request)
        {
            var user = GetUser(request);

            return CanAcess(user);
        }

        public static bool CanAcess(UserModel user)
        {
            if (user == null || string.IsNullOrEmpty(user.Name)) return true;

            if (IsCheckUserInWhiteList && !string.IsNullOrEmpty(WhiteListUsers))
            {
                var users = WhiteListUsers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (users.Any(x => x.Trim().Equals(user.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                    return true;

                return false;
            }

            return true;
        }
    }
}
