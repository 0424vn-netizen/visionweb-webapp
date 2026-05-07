using AS.Common.Logger;
using AS.Core.WCF;
using AS.EventHandler.Common.Models;
using AS.EventHandler.MT.ServiceContract.Interfaces;
using AS.EventHandler.MT.ServiceContract.Models;
using System;

/// <summary>
/// The Event Listener Service Client
/// </summary>
public class EventListenerClient : WcfClient<IEventListenerService>
{

    /// <summary>
    /// Creates the event data.
    /// </summary>
    /// <param name="siteId">The site Id.</param>
    /// <param name="eventCategoryId">The event category Id.</param>
    /// <param name="eventTypeId">The event type Id.</param>
    /// <param name="description">The description.</param>
    /// <param name="eventData">The event data.</param>
    /// <param name="loginUserId">The login user identifier.</param>
    /// <exception cref="Exception">An error occurred while creating event data</exception>
    public void CreateEvent(int? siteId, int? eventCategoryId, int? eventTypeId, string description, string data, int sourceId,
        Guid? loginUserId = null)
    {
        try
        {
            var eventDataItem = new EventDataItem
            {
                ASClientId = SessionManager.CurrentClient,
                SourceId = sourceId,
                SiteId = siteId,
                EventTypeId = eventTypeId,
                EventCategoryId = eventCategoryId,
                Data = data,
                Description = description,
                LoginUserId = loginUserId ?? SessionManager.CurrentUser.RecId
            };

            var result = Proxy.SendEvent(eventDataItem);
            if (result.ResponseCode == (int)ExecuteResultCode.Failed)
            {
                throw new Exception("ResponseCode: " + ((ExecuteResultCode)result.ResponseCode).ToString() + Environment.NewLine + result.Message);
            }
        }
        catch (Exception ex)
        {
            var message = string.Format("An error occurred while creating event [AS Client: {0}. Site: {1}. Event Category: {2}. Event Type: {3}. Description: {4}].",
                                        SessionManager.CurrentClient, siteId, eventCategoryId, eventTypeId, description);
            LoggerManager.Error(message, ex);
        }
    }
}