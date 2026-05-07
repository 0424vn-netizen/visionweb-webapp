namespace AS.NetCore.Api.Client.Models
{
    public class ApiHeader : DataModel
    {
        /// <summary>
        /// for each agent/leg of an interaction a GUID, source app responsible for generating GUID
        /// </summary>
        /// <example>254b03bf-7008-4f4d-9802-ca9d0edf80b3</example>
        public string SessionId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Current logged in user
        /// </summary>
        /// <example> 131</example>
        public int AsClientId { get; set; }

        /// <summary>
        /// Trace Id
        /// </summary>
        /// <example> 07209c83-6893-46fa-81ac-92dc4d7d09bc</example>
        public string TraceId { get; set; }

        /// <summary>
        /// Current display lanaguage: en, jp, fr, vi ...
        /// </summary>
        /// <example>en</example>
        public string LanguageId { get; set; } = "EN";

        public string FullName { get; set; }

        public int UserTimezoneValue { get; set; }
        public int UserDaylightSavingTime { get; set; }
    }
}
