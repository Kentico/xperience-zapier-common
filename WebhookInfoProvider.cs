using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace Xperience.Zapier
{
    /// <summary>
    /// Class providing <see cref="WebhookInfo"/> management.
    /// </summary>
    public partial class WebhookInfoProvider : AbstractInfoProvider<WebhookInfo, WebhookInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="WebhookInfoProvider"/>.
        /// </summary>
        public WebhookInfoProvider()
            : base(WebhookInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="WebhookInfo"/> objects.
        /// </summary>
        public static ObjectQuery<WebhookInfo> GetWebhooks()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="WebhookInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="WebhookInfo"/> ID.</param>
        public static WebhookInfo GetWebhookInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="WebhookInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="WebhookInfo"/> name.</param>
        public static WebhookInfo GetWebhookInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="WebhookInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="WebhookInfo"/> to be set.</param>
        public static void SetWebhookInfo(WebhookInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="WebhookInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="WebhookInfo"/> to be deleted.</param>
        public static void DeleteWebhookInfo(WebhookInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="WebhookInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="WebhookInfo"/> ID.</param>
        public static void DeleteWebhookInfo(int id)
        {
            WebhookInfo infoObj = GetWebhookInfo(id);
            DeleteWebhookInfo(infoObj);
        }
    }
}