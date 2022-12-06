using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Class providing <see cref="WebhookInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IWebhookInfoProvider))]
    public partial class WebhookInfoProvider : AbstractInfoProvider<WebhookInfo, WebhookInfoProvider>, IWebhookInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookInfoProvider"/> class.
        /// </summary>
        public WebhookInfoProvider()
            : base(WebhookInfo.TYPEINFO)
        {
        }
    }
}