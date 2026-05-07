<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="AS.Security.WS.Entities" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="AS.Web.LogServices" %>
<%@ Import Namespace="AS.VW.Api.RestClient" %>
<%@ Import Namespace="VW.PCI.Api.Client" %>
<%@ Import Namespace="AS.VW.PCI.Api.Client.Common" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        Application["ActiveUsers"] = 0;
        Application["MaxUsers"] = 0;
        Application["WebSiteIPAddr"] = "Init";

        string applicationName = (ConfigurationManager.AppSettings["ApplicationName"] != null ? ConfigurationManager.AppSettings["ApplicationName"].ToString() : "VisionWeb");

        var trackedUrlsConfig = ConfigurationManager.AppSettings["PerformanceLog_TrackedUrls"] ?? "";
        string[] _trackedUrls;
        if (!string.IsNullOrEmpty(trackedUrlsConfig))
        {
            _trackedUrls = trackedUrlsConfig.Split(',').Select(u => u.Trim()).Where(u => !string.IsNullOrEmpty(u)).ToArray();
        }
        else
        {
            _trackedUrls = new string[0];
        }

        Application["TrackedUrls"] = _trackedUrls;

        var trackedClientIdsConfig = ConfigurationManager.AppSettings["PerformanceLog_TrackedClientIds"] ?? "";
        string[] _trackedClientIds;
        if (!string.IsNullOrEmpty(trackedClientIdsConfig))
        {
            _trackedClientIds = trackedClientIdsConfig.Split(',').Select(u => u.Trim()).Where(u => !string.IsNullOrEmpty(u)).ToArray();
        }
        else
        {
            _trackedClientIds = new string[0];
        }

        Application["TrackedClientIds"] = _trackedClientIds;

        // Change the Application Name in runtime
        FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
        HttpRuntime theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
        FieldInfo appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);
        appNameInfo.SetValue(theRuntime, applicationName);

        var rootPath = HttpContext.Current.Server.MapPath("~/");
        var appSettingsFolder = Path.Combine(rootPath, "App_Data", "ApiSettings");
        ApiSettingsManager.Setup(appSettingsFolder);
        //PCIServiceClient.Configure(logger: VWLogger.Instance, loggingService: ApiLoggingService.Instance);

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
    }
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception error = Server.GetLastError();
        if (error.InnerException != null)
            error = error.InnerException;
        if (error is ArgumentException)
        {
            AS.Common.Logger.LoggerManager.Error("Intruder:" + PostData());
        }
        else if (error is ViewStateException)
        {
            AS.Common.Logger.LoggerManager.Error(string.Format("ViewState Error - Message: {0} \n StackTrace: {1}", error.Message, error.StackTrace));
            LogAppErrorLog(error);
        }
        else if (error is HttpException)
        {
            //AS.Common.Logger.LoggerManager.Info(string.Format("Total=Timeout; SessionId={0}; Page={1}", Session != null ? Session.SessionID : string.Empty, Request != null ? Request.Url.ToString() : string.Empty));
            LogAppErrorLog(error);
        }
        else
        {
            LogAppErrorLog(error);
        }
    }

    private void LogAppErrorLog(Exception error)
    {
        string request = Request != null ? Request.Url.ToString() : string.Empty;
        string userInfo = string.Empty;
        if (Session != null)
        {
            string format = "Session={0}; UserId={1}; ClientId={2}";
            var currentUser = GetCurrentUserInfo();
            if (currentUser != null)
            {
                userInfo = string.Format(format, Session.SessionID, currentUser.UserID, currentUser.ASClient);
            }
            else
            {
                userInfo = string.Format(format, Session.SessionID, null, null);
            }
        }
        AS.Common.Logger.LoggerManager.Error(string.Format("Request={0}; {1}{3}{2}{3}", request, userInfo, error, Environment.NewLine));
    }

    /// <summary>
    /// Get current user in session
    /// </summary>
    /// <returns></returns>
    private AS.Security.WS.Entities.User GetCurrentUserInfo()
    {
        if (Session == null)
            return null;
        var currentUser = Session["ASLogUser"];
        if (currentUser == null)
        {
            currentUser = Session["ForgetPasswordUser"];
        }
        if (currentUser == null)
        {
            currentUser = Session["ResetPasswordUser"];
        }
        if (currentUser == null)
        {
            currentUser = Session["ASLandingLogUser"];
        }
        if (currentUser != null && currentUser is AS.Security.WS.Entities.User)
        {
            return currentUser as AS.Security.WS.Entities.User;
        }
        return null;
    }

    string PostData()
    {

        System.IO.StreamReader post_stream = new System.IO.StreamReader(Request.InputStream);
        string _postData = post_stream.ReadToEnd();
        return _postData;
    }

    void Session_Start(object sender, EventArgs e)
    {
        UpdateHostRequest();

        int iActiveUsers = 0;
        int iMaxUsers = 0;
        var enableTrackingWebServerSession = false;
        string trackingSetting = ConfigurationManager.AppSettings["EnableTrackingWebServerSession"];
        if (!string.IsNullOrEmpty(trackingSetting) && Boolean.TryParse(trackingSetting, out enableTrackingWebServerSession))
        {
            enableTrackingWebServerSession = true;
        }

        Application.Lock();
        iActiveUsers = Convert.ToInt32(Application["ActiveUsers"]) + 1;
        iMaxUsers = Convert.ToInt32(Application["Maxusers"]);
        if (iActiveUsers > iMaxUsers) { iMaxUsers = iActiveUsers; }
        Application["ActiveUsers"] = iActiveUsers;
        Application["MaxUsers"] = iMaxUsers;

        // AppVar "WebSiteIPAddr" is set to "Init" in db() to ensure other tasks are completed upon first run through.
        if (Application["WebSiteIPAddr"].ToString() == "Init")
        {
            Application["WebSiteIPAddr"] = Request.ServerVariables["LOCAL_ADDR"];
            Application["WebSiteDNS"] = Request.ServerVariables["SERVER_NAME"];

            if (enableTrackingWebServerSession)
            {
                // Remove any previous references in the "RefWebServerSessionStats" table for the web site's IP address
                WebServices.SecurityServices.DeleteLOG_WebServerSessionStats((string)Application["WebSiteIPAddr"]);

                // Insert a new reference in the "RefWebServerSessionStats" table for the web site's IP address
                WebServices.SecurityServices.InsertLOG_WebServerSessionStats((string)Application["WebSiteIPAddr"], WebSiteSettings.DefaultClient.ToString(), WebSiteSettings.WebSiteType, WebSiteSettings.WebSiteGroup, (string)Application["WebSiteDNS"]);
            }
        }

        if (enableTrackingWebServerSession)
        {
            //Update the user counts in the "RefWebServerSessionStats" table for the website's IP address
            WebServices.SecurityServices.UpdateLOG_WebServerSessionStats((string)Application["WebSiteIPAddr"], (int)Application["MaxUsers"], (int)Application["ActiveUsers"]);
        }

        string sApp = "";
        string sVal = "";

        foreach (string a in Application.Contents)
        {
            sApp = a;
            sVal = Application[a].ToString();
            Session[sApp] = sVal;
        }
        // Unlock the Application block now that the user count and web server session stats have been updated
        Application.UnLock();

        Session["SessionID"] = Session.SessionID.ToString();

        // We want to track the ClientIPAddress, which [normally] is Remote_Addr. But, if there exists HTTP_F5CLIENTIPADDR,
        // use that first, as it means the Load Balancer has overwritten Remote_Addr and placed Remote_Addr in HTTP_F5CLIENTIPADDR. *RKY*
        string clientIPAddr = Request.ServerVariables["HTTP_F5CLIENTIPADDR"];
        if (string.IsNullOrEmpty(clientIPAddr)) clientIPAddr = Request.ServerVariables["REMOTE_ADDR"];
        else
        {
            clientIPAddr = clientIPAddr.Replace(",", "");
        }

        if (enableTrackingWebServerSession)
        {
            //Remove any previous references in the "RefWebServerSessionStats" table for the current session id.
            WebServices.SecurityServices.DeleteLOG_WebServerSessionLog((string)Application["WebSiteIPAddr"], Session.SessionID);
        }

        //Add the web site and user IP addresses for this specific session id to the "RefWebServerSessionLog" table - SessionStartDTS is updated by GetDate() automatically
        string cookies = "";
        for (int i = 0; i < Request.Cookies.Count; i++)
        {
            cookies += Request.Cookies[i].Name + "=" + Request.Cookies[i].Value + "&";
        }

        if (enableTrackingWebServerSession)
        {
            WebServices.SecurityServices.InsertLOG_WebServerSessionLog((string)Application["WebSiteIPAddr"], Session.SessionID, Request.UserHostAddress, Request.UserAgent, cookies);
        }

        string userAgents = ConfigurationManager.AppSettings["USER_AGENTS"];
        AS.Web.SharedSession.SharedSessionManager.UserAgents = userAgents.IsNullData() ? string.Empty : userAgents;
    }

    void Session_End(object sender, EventArgs e)
    {
        SessionCacheManager.Cache.RemoveAll();
        GeneralFuncsLib.UpdateUserLogs(this.Session.SessionID, "Logout");


        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.


        // Setup the basic SQL query that is used through the Session_OnEnd function with the parameter @WebSiteIPAddress
        // just add the mode and additional parameters as needed when calling the database



        Application.Lock();

        // Decrease the number of users currently on the site
        Application["ActiveUsers"] = Convert.ToInt32(Application["ActiveUsers"]) - 1;
        // Update the user count (one less active user) in the "RefWebServerSessionStats" table for the web site's IP address


        WebServices.SecurityServices.UpdateLOG_WebServerSessionStats((string)Application["WebSiteIPAddr"], (int)Application["MaxUsers"], (int)Application["ActiveUsers"]);

        // Unlock the Application block now that the user count stat have been updated
        Application.UnLock();

        // Update the session end time for the specific session ID in the "RefWebServerSessionLog" table

        WebServices.SecurityServices.UpdateLOG_WebServerSessionLogEnd(Session.SessionID);


    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        UpdateHostRequest();

        //Set EnableSessionState = "ReadOnly" for config pages. 
        string filePath = Server.MapPath("~/App_Data/EnableSessionStateReadOnly.txt");
        if (System.IO.File.Exists(filePath))
        {
            string[] pageList = null;
            if (HttpRuntime.Cache[filePath] == null)
            {
                pageList = System.IO.File.ReadAllLines(filePath);
                HttpRuntime.Cache.Add(filePath, pageList, new CacheDependency(filePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            else
            {
                pageList = HttpRuntime.Cache[filePath] as string[];
            }
            HttpContext currrentContext = (sender as HttpApplication).Context;
            foreach (var page in pageList)
            {
                if (!string.IsNullOrEmpty(page.Trim()) && currrentContext.Request.Url.ToString().ToLower().Contains(page.Trim().ToLower()))
                {
                    // Only apply ReadOnly to regular page requests, not WebMethod calls
                    if (string.IsNullOrEmpty(currrentContext.Request.PathInfo))
                    {
                        currrentContext.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
                    }
                    break;
                }
            }
        }

        if (!ShouldTrackUrl(Request.RawUrl))
        {
            return;
        }

        if (IsJsonRequest())
        {
            try
            {
                var serverReceiveTime = GetCurrentUnixTimeMilliseconds();
                HttpContext.Current.Items["ServerReceiveTime"] = serverReceiveTime;
                var clientTimestamp = Request.Headers["X-Client-Timestamp"];
                if (!string.IsNullOrEmpty(clientTimestamp))
                {
                    HttpContext.Current.Items["ClientTimestamp"] = long.Parse(clientTimestamp);
                }
                var clientClickTimestamp = Request.Headers["X-Client-Click-Timestamp"];
                if (!string.IsNullOrEmpty(clientClickTimestamp))
                {
                    HttpContext.Current.Items["ClientClickTimestamp"] = long.Parse(clientClickTimestamp);
                }
                var clientTimeZone = Request.Headers["X-Client-TimeZone"];
                HttpContext.Current.Items["ClientTimeZone"] = clientTimeZone;
                var requestBody = ReadRequestBody();
                HttpContext.Current.Items["RequestBody"] = requestBody;
                HttpContext.Current.Items["ShouldTrack"] = true;
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("[BeginRequest] Error: " + ex.Message);
            }
        }

        //43724 - Auto-refresh Cache of JavasScripts and CSS files after deployment
        AS.Web.BundleWebForm.BundleRegister.Register((HttpApplication)sender);
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        if (HttpContext.Current.Items["ShouldTrack"] == null)
        {
            return;
        }

        var session = HttpContext.Current.Session;
        if (session != null)
        {
            var currentClient = session["CurrentClient"] != null ? session["CurrentClient"].ToString() : null;
            var currentSystem = session["CurrentSystem"] != null ? session["CurrentSystem"].ToString() : null;
            var currentUser = session["ASLogUser"] != null ? (User)HttpContext.Current.Session["ASLogUser"] : null;
            if (currentUser != null)
            {
                HttpContext.Current.Items["UserNameFull"] = currentUser.UserNameFull;
                HttpContext.Current.Items["UserID"] = currentUser.UserID;
                HttpContext.Current.Items["CurrentClient"] = currentClient;
                HttpContext.Current.Items["CurrentSystem"] = currentSystem;
            }
        }
    }

    void Application_EndRequest(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["ForceSecuredCookie"].ToLower() == "true" && Response.Cookies.Count > 0)
        {
            foreach (string s in Response.Cookies.AllKeys)
            {
                Response.Cookies[s].Secure = true;
            }
        }

        if (HttpContext.Current.Items["ShouldTrack"] == null)
        {
            return;
        }

        try
        {
            var currentClient = HttpContext.Current.Items["CurrentClient"] != null ? HttpContext.Current.Items["CurrentClient"].ToString() : "";
            if (!ShouldTrackClient(currentClient))
            {
                return;
            }

            var serverReceiveTime = HttpContext.Current.Items["ServerReceiveTime"];
            if (serverReceiveTime != null)
            {
                var startTime = (long)serverReceiveTime;
                var endTime = GetCurrentUnixTimeMilliseconds();
                var serverProcessTime = (int)(endTime - startTime);
                var clientTimestamp = HttpContext.Current.Items["ClientTimestamp"];
                var clientClickTimestamp = HttpContext.Current.Items["ClientClickTimestamp"];
                long? ajaxSendDelay = null, networkLatency = null;
                string logData2 = "";
                if (clientTimestamp != null)
                {
                    var cts = (long)clientTimestamp;
                    networkLatency = (startTime - cts);
                    logData2 = "Network Latency: " + networkLatency.ToString();
                    if (clientClickTimestamp != null)
                    {
                        ajaxSendDelay = Math.Abs((long)clientClickTimestamp - cts);
                    }
                }

                var clientTimeZone = HttpContext.Current.Items["ClientTimeZone"] != null ? HttpContext.Current.Items["ClientTimeZone"].ToString() : "";
                var userNameFull = HttpContext.Current.Items["UserNameFull"] != null ? HttpContext.Current.Items["UserNameFull"].ToString() : "";
                var userID = HttpContext.Current.Items["UserID"] != null ? HttpContext.Current.Items["UserID"].ToString() : "";
                var requestBody = HttpContext.Current.Items["RequestBody"] != null ? HttpContext.Current.Items["RequestBody"].ToString() : "";

                string logData6 = string.Format("ClientTimeZone={0}|ClientTimestamp={1}|ClientClickTimestamp={2}|AjaxSendDelay={3}|ServerReceiveTime={4}",
                    clientTimeZone, clientTimestamp, clientClickTimestamp, ajaxSendDelay, startTime);

                var trackingLog = new AspxTracking();
                trackingLog.LogWebServerDts = DateTime.Now;
                trackingLog.LogId1 = userID;
                trackingLog.LogFullName = userNameFull;
                trackingLog.LogElapsedTime = serverProcessTime;
                trackingLog.LogData1 = Request.Path;
                trackingLog.LogData2 = logData2;
                trackingLog.LogData6 = logData6;
                trackingLog.LogTxt1 = requestBody;
                trackingLog.LogClientId = Convert.ToInt32(currentClient);

                LogService logService = new LogService();
                logService.AddRequestHeader("ClientId", currentClient);
                logService.InsertASPXTrackingLog(trackingLog);
            }
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("InsertASPXTrackingLog error: " + ex.Message);
        }
    }

    private bool ShouldTrackUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        var trackedUrls = Application["TrackedUrls"] as string[];

        if (trackedUrls == null || trackedUrls.Length == 0)
        {
            return false;
        }

        foreach (var trackedUrl in trackedUrls)
        {
            if (url.IndexOf(trackedUrl, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool ShouldTrackClient(string clientId)
    {
        if (string.IsNullOrEmpty(clientId))
        {
            return false;
        }

        var trackedClientIds = Application["TrackedClientIds"] as string[];

        if (trackedClientIds == null || trackedClientIds.Length == 0)
        {
            return false;
        }

        foreach (var trackedClientId in trackedClientIds)
        {
            if (clientId.IndexOf(trackedClientId, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    private long GetCurrentUnixTimeMilliseconds()
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var now = DateTime.UtcNow;
        return (long)(now - epoch).TotalMilliseconds;
    }

    private bool IsJsonRequest()
    {
        return Request.HttpMethod == "POST" && Request.ContentType != null && Request.ContentType.Contains("application/json");
    }

    private string ReadRequestBody()
    {
        try
        {
            Request.InputStream.Position = 0;
            using (var reader = new StreamReader(Request.InputStream, Encoding.UTF8, true, 1024, true))
            {
                var body = reader.ReadToEnd();
                Request.InputStream.Position = 0;
                return body;
            }
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ReadRequestBody error: " + ex.Message);
            return null;
        }
    }

    private void UpdateHostRequest()
    {
        var request = HttpContext.Current.Request;
        if (request.Url.Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
        {
            AS.Common.Logger.LoggerManager.Info("UpdateHostRequest: " + HttpContext.Current.Request.Url.Host);

            var newUrl = request.Url.Scheme + "://" + request.Url.Host.Substring(4) + request.Url.PathAndQuery;
            HttpContext.Current.Response.RedirectPermanent(newUrl);
        }
    }

</script>
