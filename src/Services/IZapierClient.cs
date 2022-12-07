using System.Threading.Tasks;

using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Contains methods for sending requests to Zapier.
    /// </summary>
    public interface IZapierClient
    {
        /// <summary>
        /// Sends a POST to the Zapier <paramref name="webhookUrl"/> with the provided data.
        /// </summary>
        /// <param name="webhookUrl">The Zapier webhook URL.</param>
        /// <param name="data">Xperience objects to serialize and send to Zapier.</param>
        Task TriggerWebhook(string webhookUrl, params BaseInfo[] data);
    }
}
