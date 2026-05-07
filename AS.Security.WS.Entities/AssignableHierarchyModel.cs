using System;

namespace AS.Security.WS.Entities
{
	public class AssignableHierarchyModel
	{
		public int HierarchyId { get; set; }
		public int AssignHierarchyId { get; set; }
		public bool IsRemove { get; set; }
		public int AsClientId { get; set; }
		public int SiteId { get; set; }
		public string UserId { get; set; }
		public Guid ChangedByRecId { get; set; }
		public bool IsUpdate { get; set; }
	}
}
