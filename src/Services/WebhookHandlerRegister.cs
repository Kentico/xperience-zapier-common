using System;
using System.Collections.Generic;
using System.Linq;

using CMS;
using CMS.Core;

using Xperience.Zapier.Common;

[assembly: RegisterImplementation(typeof(IWebhookHandlerRegister), typeof(WebhookHandlerRegister), Priority = RegistrationPriority.SystemDefault)]
namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Default implementation of <see cref="IWebhookHandlerRegister"/>.
    /// </summary>
    internal class WebhookHandlerRegister : IWebhookHandlerRegister
    {
        private readonly IWebFarmService webFarmService;
        private readonly IWebhookInfoProvider webhookInfoProvider;
        private readonly List<WebhookHandler> registeredHandlers = new();


        /// <summary>
        /// Constructor.
        /// </summary>
        public WebhookHandlerRegister(IWebFarmService webFarmService, IWebhookInfoProvider webhookInfoProvider)
        {
            this.webFarmService = webFarmService;
            this.webhookInfoProvider = webhookInfoProvider;
        }


        /// <inheritdoc/>
        public void RegisterWebhook(int id, bool logTask = false)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Webhook ID must be greater than zero.");
            }

            var webhook = webhookInfoProvider.Get(id);
            if (webhook == null)
            {
                throw new InvalidOperationException($"Webhook with ID {id} not found.");
            }

            RegisterWebhook(webhook, logTask);
        }


        /// <inheritdoc/>
        public void RegisterWebhook(WebhookInfo webhook, bool logTask = false)
        {
            if (webhook == null)
            {
                throw new ArgumentNullException(nameof(webhook));
            }

            var handler = new WebhookHandler(webhook);
            registeredHandlers.Add(handler);

            if (webhook.WebhookEnabled)
            {
                handler.EnableHandler();
            }

            if (logTask)
            {
                webFarmService.CreateTask(new RegisterWebhookWebFarmTask()
                {
                    WebhookID = webhook.WebhookID
                });
            }
        }


        /// <inheritdoc/>
        public void UnregisterWebhook(int id, bool logTask = false)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Webhook ID must be greater than zero.");
            }

            var webhook = webhookInfoProvider.Get(id);
            if (webhook == null)
            {
                throw new InvalidOperationException($"Webhook with ID {id} not found.");
            }

            UnregisterWebhook(webhook, logTask);
        }


        /// <inheritdoc/>
        public void UnregisterWebhook(WebhookInfo webhook, bool logTask = false)
        {
            if (webhook == null)
            {
                throw new ArgumentNullException(nameof(webhook));
            }

            var handler = registeredHandlers.SingleOrDefault(h => h.Webhook.WebhookID == webhook.WebhookID);
            if (handler == null)
            {
                throw new InvalidOperationException($"Handler with webhook ID {webhook.WebhookID} not registered.");
            }

            handler.DisableHandler();
            registeredHandlers.Remove(handler);

            if (logTask)
            {
                webFarmService.CreateTask(new UnregisterWebhookWebFarmTask()
                {
                    WebhookID = webhook.WebhookID
                });
            }
        }
    }
}
