using CMS.Core;

namespace Xperience.Zapier.Common
{
    public class RegisterWebhookWebFarmTask : WebFarmTaskBase
    {
        public int WebhookID
        {
            get;
            set;
        }


        public override void ExecuteTask()
        {
            Service.Resolve<IWebhookHandlerRegister>().RegisterWebhook(WebhookID);
        }
    }
}