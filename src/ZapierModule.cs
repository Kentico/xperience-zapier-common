using System;

using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;

using Xperience.Zapier.Common;

[assembly: AssemblyDiscoverable]
[assembly: RegisterModule(typeof(ZapierModule))]
namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Custom module which initializes the Zapier integration.
    /// </summary>
    public class ZapierModule : Module
    {
        private IWebhookHandlerRegister webhookHandlerRegister;


        /// <summary>
        /// Constructor.
        /// </summary>
        public ZapierModule() : base(nameof(ZapierModule))
        {
        }


        /// <inheritdoc/>
        protected override void OnInit()
        {
            base.OnInit();

            webhookHandlerRegister = Service.Resolve<IWebhookHandlerRegister>();

            WebFarmHelper.RegisterTask<RegisterWebhookWebFarmTask>();
            WebFarmHelper.RegisterTask<UnregisterWebhookWebFarmTask>();

            ApplicationEvents.PostStart.Execute += RegisterExistingWebhooks;

            WebhookInfo.TYPEINFO.Events.Insert.After += RegisterNewWebhook;
            WebhookInfo.TYPEINFO.Events.Update.Before += CheckEnabledChange;
            WebhookInfo.TYPEINFO.Events.Delete.After += RemoveWebhook;
        }


        private void RegisterExistingWebhooks(object sender, EventArgs e)
        {
            var webhooks = WebhookInfoProvider.ProviderObject.Get().TypedResult;
            foreach (var webhook in webhooks)
            {
                webhookHandlerRegister.RegisterWebhook(webhook, true);
            }
        }


        private void CheckEnabledChange(object sender, ObjectEventArgs e)
        {
            var webhook = e.Object as WebhookInfo;
            if (webhook.ChangedColumns().Contains(nameof(webhook.WebhookEnabled)))
            {
                if (webhook.WebhookEnabled)
                {
                    // Webhook changed from disabled to enabled
                    webhookHandlerRegister.RegisterWebhook(webhook, true);
                }
                else
                {
                    // Webhook changed from enabled to disabled
                    webhookHandlerRegister.UnregisterWebhook(webhook, true);
                }
            }
        }


        private void RemoveWebhook(object sender, ObjectEventArgs e)
        {
            var webhook = e.Object as WebhookInfo;
            webhookHandlerRegister.UnregisterWebhook(webhook, true);
        }


        private void RegisterNewWebhook(object sender, ObjectEventArgs e)
        {
            var webhook = e.Object as WebhookInfo;
            webhookHandlerRegister.RegisterWebhook(webhook, true);
        }
    }
}