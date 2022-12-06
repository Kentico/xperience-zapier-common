namespace Xperience.Zapier.Common
{
    public interface IWebhookHandlerRegister
    {
        void RegisterWebhook(int id, bool logTask = false);


        void RegisterWebhook(WebhookInfo webhook, bool logTask = false);


        void UnregisterWebhook(int id, bool logTask = false);


        void UnregisterWebhook(WebhookInfo webhook, bool logTask = false);
    }
}
