using System;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Registers and unregisters an Xperience event handler using the configuration of the provided <see cref="Webhook"/>.
    /// Sends a request to Zapier to trigger the webhook when the Xperience event is executed.
    /// </summary>
    internal class WebhookHandler
    {
        private readonly IEventLogService eventLogService;
        private readonly IZapierClient zapierClient;


        /// <summary>
        /// The configuration to use when registering, unregistering, and executing the webhook.
        /// </summary>
        public WebhookInfo Webhook
        {
            get;
            private set;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
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


        /// <summary>
        /// Enables the Xperience event handler which will trigger the webhook.
        /// </summary>
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
                    eventLogService.LogInformation(nameof(WebhookHandler), nameof(EnableHandler),
                        $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} create event.");
                    break;
                case WebhookEventType.Update:
                    typeInfo.Events.Update.After += Run;
                    eventLogService.LogInformation(nameof(WebhookHandler), nameof(EnableHandler),
                        $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} update event.");
                    break;
                case WebhookEventType.Delete:
                    typeInfo.Events.Delete.Before += Run;
                    eventLogService.LogInformation(nameof(WebhookHandler), nameof(EnableHandler),
                        $"Registered handler '{Webhook.WebhookName}' to {Webhook.WebhookObjectType} delete event.");
                    break;
                case WebhookEventType.None:
                    eventLogService.LogError(nameof(WebhookHandler), nameof(EnableHandler),
                        $"Handler '{Webhook.WebhookName}' could not be registered: Invalid event type.");
                    break;
            }
        }


        /// <summary>
        /// Disables the Xperience event handler.
        /// </summary>
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
                    eventLogService.LogInformation(nameof(WebhookHandler), nameof(DisableHandler),
                        $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} create event.");
                    break;
                case WebhookEventType.Update:
                    typeInfo.Events.Update.After -= Run;
                    eventLogService.LogInformation(nameof(WebhookHandler), nameof(DisableHandler),
                        $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} update event.");
                    break;
                case WebhookEventType.Delete:
                    typeInfo.Events.Delete.Before -= Run;
                    eventLogService.LogInformation(nameof(WebhookHandler), nameof(DisableHandler),
                        $"Unregistered handler '{Webhook.WebhookName}' from {Webhook.WebhookObjectType} delete event.");
                    break;
                case WebhookEventType.None:
                    eventLogService.LogError(nameof(WebhookHandler), nameof(DisableHandler),
                        $"Handler '{Webhook.WebhookName}' could not be unregistered: Invalid event type.");
                    break;
            }
        }


        private void Run(object sender, ObjectEventArgs e)
        {
            if(Webhook.WebhookEnabled && e.Object != null)
            {
                try
                {
                    var thread = new CMSThread(() => {
                        zapierClient.TriggerWebhook(Webhook.WebhookURL, e.Object);
                    });
                    thread.Start(false);
                }
                catch (Exception ex)
                {
                    eventLogService.LogException(nameof(WebhookHandler), nameof(Run), ex);
                }
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