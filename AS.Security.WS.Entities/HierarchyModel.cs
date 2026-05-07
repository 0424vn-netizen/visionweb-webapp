using System;

namespace AS.Security.WS.Entities
{
	public class HierarchyModel
	{
		public int SystemId { get; set; }
		public int HierarchyId { get; set; }
		public string HierarchyName { get; set; }
		public int ParentHierarchy { get; set; }
		public string HierarchyDesc { get; set; }
		public string Status { get; set; }
		public Guid CreatedBy { get; set; }
		public Guid UpdatedBy { get; set; }
		public int ClientId { get; set; }
		public string HierarchyCode { get; set; }
		public string HierarchyLevel { get; set; }
	}
}
