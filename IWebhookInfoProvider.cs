using CMS.DataEngine;

namespace Xperience.Zapier
{
    /// <summary>
    /// Declares members for <see cref="WebhookInfo"/> management.
    /// </summary>
    public partial interface IWebhookInfoProvider : IInfoProvider<WebhookInfo>, IInfoByIdProvider<WebhookInfo>, IInfoBySiteAndNameProvider<WebhookInfo>
    {
    }
}