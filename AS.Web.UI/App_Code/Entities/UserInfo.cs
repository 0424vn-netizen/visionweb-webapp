using System;

namespace As.VisionWeb.Web.Entity
{
    [Serializable]
    public class UserInfo
    {
        public int ASClient { get; set; }
        public int SiteID { get; set; }
        public Guid RecId { get; set; }
        public string UserID { get; set; }
        public string UserNameFull { get; set; }
    }
}
