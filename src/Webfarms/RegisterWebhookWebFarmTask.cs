using CMS.Core;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// A web farm task which registers a <see cref="WebhookHandler"/>.
    /// </summary>
    internal class RegisterWebhookWebFarmTask : WebFarmTaskBase
    {
        /// <summary>
        /// The <see cref="WebhookInfo.WebhookID"/> to register.
        /// </summary>
        public int WebhookID
        {
            get;
            set;
        }


        /// <summary>
        /// Registers a <see cref="WebhookHandler"/> with the specified <see cref="WebhookID"/>.
        /// </summary>
        public override void ExecuteTask()
        {
            Service.Resolve<IWebhookHandlerRegister>().RegisterWebhook(WebhookID);
        }
    }
}