using CMS.Core;

namespace Xperience.Zapier.Common
{
    public class UnregisterWebhookWebFarmTask : WebFarmTaskBase
    {
        public int WebhookID
        {
            get;
            set;
        }


        public override void ExecuteTask()
        {
            Service.Resolve<IWebhookHandlerRegister>().UnregisterWebhook(WebhookID);
        }
    }
}