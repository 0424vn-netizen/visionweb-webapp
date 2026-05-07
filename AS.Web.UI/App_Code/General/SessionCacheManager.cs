using System.Web;

public class SessionCacheManager
{
    public static SessionCache Cache { get { return new SessionCache(); } }

    public class SessionCache
    {
        const string ALL_KEYS = "AllKeys";
        public static string CacheId { get { return "SessionCache_" + HttpContext.Current.Session.SessionID + "_"; } }
        public static string SessionCacheAllKeysId = CacheId + ALL_KEYS;

        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Add(key, value);
            }
        }

        public static string[] Keys { get { return HttpRuntime.Cache[SessionCacheAllKeysId].ToString().TrimEnd(';').Split(';'); } }

        private object Get(string key)
        {
            return HttpRuntime.Cache[CacheId + key];
        }

        private void Add(string key, object value)
        {
            string sessionCacheId = CacheId + key;
            HttpRuntime.Cache[sessionCacheId] = value;
            //Add new key to list keys
            if (HttpRuntime.Cache[SessionCacheAllKeysId] == null)
                HttpRuntime.Cache[SessionCacheAllKeysId] = string.Empty;
            if (!HttpRuntime.Cache[SessionCacheAllKeysId].ToString().Contains(sessionCacheId))
                HttpRuntime.Cache[SessionCacheAllKeysId] = HttpRuntime.Cache[SessionCacheAllKeysId].ToString() + sessionCacheId + ";";
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(CacheId + key);
        }

        public void RemoveAll()
        {
            foreach (var key in Keys)
            {
                Remove(key);
            }
            Remove(ALL_KEYS);
        }
    }
}