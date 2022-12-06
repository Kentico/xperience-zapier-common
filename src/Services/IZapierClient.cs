using System.Threading.Tasks;

using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    public interface IZapierClient
    {
        Task TriggerWebhook(string url, params BaseInfo[] data);
    }
}
