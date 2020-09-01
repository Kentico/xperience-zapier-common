using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;

namespace Xperience.Zapier
{
    public class WebhookHandler
    {
        public WebhookInfo Webhook { get; set; }

        public WebhookHandler(WebhookInfo webhook)
        {
            Webhook = webhook;
        }

        public bool Register()
        {
            if (Webhook != null)
            {
                var typeInfo = ObjectTypeManager.GetTypeInfo(Webhook.WebhookObjectType);
                var eventType = ZapierHelper.GetWebhookEventTypeEnum(Webhook.WebhookEventType);
                if(typeInfo != null)
                {
                    switch (eventType)
                    {
                        case WebhookEventTypeEnum.Create:
                            typeInfo.Events.Insert.After += Run;
                            EventLogProvider.LogEvent("I", nameof(WebhookHandler), "REGISTER", $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} create event.");
                            break;
                        case WebhookEventTypeEnum.Update:
                            typeInfo.Events.Update.After += Run;
                            EventLogProvider.LogEvent("I", nameof(WebhookHandler), "REGISTER", $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} update event.");
                            break;
                        case WebhookEventTypeEnum.Delete:
                            typeInfo.Events.Delete.Before += Run;
                            EventLogProvider.LogEvent("I", nameof(WebhookHandler), "REGISTER", $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} delete event.");
                            break;
                        case WebhookEventTypeEnum.None:
                            EventLogProvider.LogEvent("E", nameof(WebhookHandler), "REGISTER", $"Handler '{Webhook.WebhookName}' could not be registered: event type not found.");
                            return false;
                    }

                    return true;
                }
                else
                {
                    EventLogProvider.LogEvent("E", nameof(WebhookHandler), "REGISTER", $"Unable to register handler '{Webhook.WebhookName}': TypeInfo for {Webhook.WebhookObjectType} not found.");
                    return false;
                }
                
            }

            EventLogProvider.LogEvent("E", nameof(WebhookHandler), "REGISTER", $"Unable to register handler: not found.");
            return false;
        }

        public bool Unregister()
        {
            var typeInfo = ObjectTypeManager.GetTypeInfo(Webhook.WebhookObjectType);
            var eventType = ZapierHelper.GetWebhookEventTypeEnum(Webhook.WebhookEventType);
            if (typeInfo != null)
            {
                switch (eventType)
                {
                    case WebhookEventTypeEnum.Create:
                        typeInfo.Events.Insert.After -= Run;
                        EventLogProvider.LogEvent("I", nameof(WebhookHandler), "UNREGISTER", $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} create event.");
                        break;
                    case WebhookEventTypeEnum.Update:
                        typeInfo.Events.Update.After -= Run;
                        EventLogProvider.LogEvent("I", nameof(WebhookHandler), "UNREGISTER", $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} update event.");
                        break;
                    case WebhookEventTypeEnum.Delete:
                        typeInfo.Events.Delete.Before -= Run;
                        EventLogProvider.LogEvent("I", nameof(WebhookHandler), "UNREGISTER", $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} delete event.");
                        break;
                    case WebhookEventTypeEnum.None:
                        EventLogProvider.LogEvent("E", nameof(WebhookHandler), "UNREGISTER", $"Handler '{Webhook.WebhookName}' could not be unregistered: event type not found.");
                        return false;
                }

                return true;
            }

            return false;
        }


        public void Run(object sender, ObjectEventArgs e)
        {
            if(Webhook != null && Webhook.WebhookEnabled)
            {
                if (e.Object != null)
                {
                    var thread = new CMSThread(() => {
                        var site = SiteInfoProvider.GetSiteInfo(Webhook.WebhookSiteID);
                        ZapierHelper.SendPostToWebhook(Webhook.WebhookURL, site.DomainName, e.Object);
                    });
                    thread.Start(false);
                }
            }
        }
    }
}