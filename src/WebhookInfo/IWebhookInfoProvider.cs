using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Declares members for <see cref="WebhookInfo"/> management.
    /// </summary>
    public partial interface IWebhookInfoProvider : IInfoProvider<WebhookInfo>, IInfoByIdProvider<WebhookInfo>, IInfoBySiteAndNameProvider<WebhookInfo>
    {
    }
}