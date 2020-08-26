using CMS.Core;

namespace Xperience.Zapier
{
    public class RegisterWebhookWebFarmTask : WebFarmTaskBase
    {
        public int WebhookID { get; set; }

        public override void ExecuteTask()
        {
            ZapierHelper.RegisterWebhook(WebhookID);
        }
    }
}