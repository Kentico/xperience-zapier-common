using System;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    internal class WebhookHandler
    {
        private readonly IEventLogService eventLogService;
        private readonly IZapierClient zapierClient;


        public WebhookInfo Webhook
        {
            get;
            private set;
        }


        public WebhookHandler(WebhookInfo webhook)
        {
            if (webhook == null)
            {
                throw new ArgumentNullException(nameof(webhook));
            }

            Webhook = webhook;
            eventLogService = Service.Resolve<IEventLogService>();
            zapierClient = Service.Resolve<IZapierClient>();
        }


        public void EnableHandler()
        {
            var typeInfo = ObjectTypeManager.GetTypeInfo(Webhook.WebhookObjectType);
            if (typeInfo == null)
            {
                throw new InvalidOperationException($"The webhook '{Webhook.WebhookName}' has an invalid object type.");
            }

            var eventType = GetWebhookEventTypeEnum(Webhook.WebhookEventType);
            switch (eventType)
            {
                case WebhookEventType.Create:
                    typeInfo.Events.Insert.After += Run;
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(EnableHandler),
                        $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} create event.");
                    break;
                case WebhookEventType.Update:
                    typeInfo.Events.Update.After += Run;
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(EnableHandler),
                        $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} update event.");
                    break;
                case WebhookEventType.Delete:
                    typeInfo.Events.Delete.Before += Run;
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(EnableHandler),
                        $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} delete event.");
                    break;
                case WebhookEventType.None:
                    eventLogService.LogEvent(EventTypeEnum.Error, nameof(WebhookHandler), nameof(EnableHandler),
                        $"Handler '{Webhook.WebhookName}' could not be registered: Invalid event type.");
                    break;
            }
        }


        public void DisableHandler()
        {
            var typeInfo = ObjectTypeManager.GetTypeInfo(Webhook.WebhookObjectType);
            if (typeInfo == null)
            {
                throw new InvalidOperationException($"The webhook '{Webhook.WebhookName}' has an invalid object type.");
            }

            var eventType = GetWebhookEventTypeEnum(Webhook.WebhookEventType);
            switch (eventType)
            {
                case WebhookEventType.Create:
                    typeInfo.Events.Insert.After -= Run;
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(DisableHandler),
                        $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} create event.");
                    break;
                case WebhookEventType.Update:
                    typeInfo.Events.Update.After -= Run;
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(DisableHandler),
                        $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} update event.");
                    break;
                case WebhookEventType.Delete:
                    typeInfo.Events.Delete.Before -= Run;
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(DisableHandler),
                        $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} delete event.");
                    break;
                case WebhookEventType.None:
                    eventLogService.LogEvent(EventTypeEnum.Information, nameof(WebhookHandler), nameof(DisableHandler),
                        $"Handler '{Webhook.WebhookName}' could not be unregistered: Invalid event type.");
                    break;
            }
        }


        public void Run(object sender, ObjectEventArgs e)
        {
            if(Webhook.WebhookEnabled && e.Object != null)
            {
                var thread = new CMSThread(() => {
                    zapierClient.TriggerWebhook(Webhook.WebhookURL, e.Object);
                });
                thread.Start(false);
            }
        }


        private WebhookEventType GetWebhookEventTypeEnum(int value)
        {
            switch (value)
            {
                case 0:
                    return WebhookEventType.Create;
                case 1:
                    return WebhookEventType.Update;
                case 2:
                    return WebhookEventType.Delete;
                default:
                    return WebhookEventType.None;
            }
        }
    }
}