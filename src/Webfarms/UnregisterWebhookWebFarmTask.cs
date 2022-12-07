using CMS.Core;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// A web farm task which unregisters a <see cref="WebhookHandler"/>.
    /// </summary>
    internal class UnregisterWebhookWebFarmTask : WebFarmTaskBase
    {
        /// <summary>
        /// The <see cref="WebhookInfo.WebhookID"/> to unregister.
        /// </summary>
        public int WebhookID
        {
            get;
            set;
        }


        /// <summary>
        /// Unregisters the <see cref="WebhookHandler"/> with the specified <see cref="WebhookID"/>.
        /// </summary>
        public override void ExecuteTask()
        {
            Service.Resolve<IWebhookHandlerRegister>().UnregisterWebhook(WebhookID);
        }
    }
}