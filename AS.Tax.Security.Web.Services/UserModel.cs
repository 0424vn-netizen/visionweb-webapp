using System;

namespace AS.Tax.Security.Web.Services.Model
{
    public class UserModel
    {
        public int ClientID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ActiveStatus { get; set; }
        public int UserType { get; set; }
        public string HierarchyIDs { get; set; }
        public int ThemeID { get; set; }
        public Guid CreatedBy { get; set; }
        public int? Question { get; set; }
        public string Answer { get; set; }
    }
}
