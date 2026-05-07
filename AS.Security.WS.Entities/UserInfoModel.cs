using System;

namespace AS.Security.WS.Entities
{
	public class UserInfoModel
	{
		public int ClientId { get; set; }
		public string UserName { get; set; }
		public string UserNameFirst { get; set; }
		public string UserNameLast { get; set; }
		public string UserNameFull { get; set; }
		public string UserPassword { get; set; }
		public int UserPasswordType { get; set; }
		public string Email { get; set; }
		public int LoginQuestionIndex { get; set; }
		public string LoginQuestionAnswer { get; set; }
		public string Status { get; set; }
		public string[] HierarchyIds { get; set; }
		public string UserSecRole { get; set; }
		public Guid CreatedBy { get; set; }
		public int SiteId { get; set; }
		public string EntityId { get; set; }
		public string UserTypeMode { get; set; }
		public int UserType { get; set; }
		public string SalesRepCode { get; set; }
		public string Organizations { get; set; }
	}

	public class UpdateUserInfoModel: UserInfoModel
	{
		public int SystemId { get; set; }
		public string OriginalUserId { get; set; }
		public Guid RecId { get; set; }
		public Guid UpdatedBy { get; set; }
		public string LoginQuestionAnswerOriginalValue { get; set; }
		public string LoginQuestionAnswerNewValue { get; set; }
		public string PhoneForSMS { get; set; }
		public string ContactEmail { get; set; }
		public string HasSyncDataUw { get; set; }
		public bool IsUpdateUserName { get; set; }
	}
}
