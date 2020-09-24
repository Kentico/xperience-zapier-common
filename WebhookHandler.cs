using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;

namespace Xperience.Zapier
{
    public class WebhookHandler
    {
        private IEventLogService mLogService;

        public WebhookInfo Webhook { get; set; }

        public IEventLogService LogService
        {
            get
            {
                if(mLogService == null)
                {
                    mLogService = Service.Resolve<IEventLogService>();
                }

                return mLogService;
            }
        }

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
                            LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "REGISTER", $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} create event.");
                            break;
                        case WebhookEventTypeEnum.Update:
                            typeInfo.Events.Update.After += Run;
                            LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "REGISTER", $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} update event.");
                            break;
                        case WebhookEventTypeEnum.Delete:
                            typeInfo.Events.Delete.Before += Run;
                            LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "REGISTER", $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} delete event.");
                            break;
                        case WebhookEventTypeEnum.None:
                            LogService.LogEvent(EventTypeEnum.Error, nameof(WebhookHandler), "REGISTER", $"Handler '{Webhook.WebhookName}' could not be registered: event type not found.");
                            return false;
                    }

                    return true;
                }
                else
                {
                    LogService.LogEvent(EventTypeEnum.Error, nameof(WebhookHandler), "REGISTER", $"Unable to register handler '{Webhook.WebhookName}': TypeInfo for {Webhook.WebhookObjectType} not found.");
                    return false;
                }
                
            }

            LogService.LogEvent(EventTypeEnum.Error, nameof(WebhookHandler), "REGISTER", $"Unable to register handler: not found.");
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
                        LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "UNREGISTER", $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} create event.");
                        break;
                    case WebhookEventTypeEnum.Update:
                        typeInfo.Events.Update.After -= Run;
                        LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "UNREGISTER", $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} update event.");
                        break;
                    case WebhookEventTypeEnum.Delete:
                        typeInfo.Events.Delete.Before -= Run;
                        LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "UNREGISTER", $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} delete event.");
                        break;
                    case WebhookEventTypeEnum.None:
                        LogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), "UNREGISTER", $"Handler '{Webhook.WebhookName}' could not be unregistered: event type not found.");
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
                        ZapierHelper.SendPostToWebhook(Webhook.WebhookURL, e.Object);
                    });
                    thread.Start(false);
                }
            }
        }
    }
}