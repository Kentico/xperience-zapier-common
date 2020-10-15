using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using System;

[assembly: RegisterModule(typeof(Xperience.Zapier.ZapierModule))]
namespace Xperience.Zapier
{
    public class ZapierModule : Module
    {
        public ZapierModule() : base(nameof(ZapierModule))
        {

        }

        protected override void OnInit()
        {
            base.OnInit();

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
                ZapierHelper.RegisterWebhook(webhook, true);
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
                    ZapierHelper.RegisterWebhook(webhook, true);
                }
                else
                {
                    // Webhook changed from enabled to disabled
                    ZapierHelper.UnregisterWebhook(webhook, true);
                }
            }
        }

        private void RemoveWebhook(object sender, ObjectEventArgs e)
        {
            var webhook = e.Object as WebhookInfo;
            ZapierHelper.UnregisterWebhook(webhook, true);
        }

        private void RegisterNewWebhook(object sender, ObjectEventArgs e)
        {
            var webhook = e.Object as WebhookInfo;
            ZapierHelper.RegisterWebhook(webhook, true);
        }
    }
}