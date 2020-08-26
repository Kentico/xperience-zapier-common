using CMS.Core;

namespace Xperience.Zapier
{
    public class UnregisterWebhookWebFarmTask : WebFarmTaskBase
    {
        public int WebhookID { get; set; }

        public override void ExecuteTask()
        {
            ZapierHelper.UnregisterWebhook(WebhookID);
        }
    }
}