namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Contains methods for storing <see cref="WebhookHandler"/> objects in memory.
    /// </summary>
    public interface IWebhookHandlerRegister
    {
        /// <summary>
        /// Registers a handler for the <see cref="WebhookInfo"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The <see cref="WebhookInfo.WebhookID"/> to register a handler for.</param>
        /// <param name="logTask">If <c>true</c>, a web farm task is created to register the handler on
        /// web farm servers.</param>
        void RegisterWebhook(int id, bool logTask = false);


        /// <summary>
        /// Registers a handler with the specified <paramref name="webhook"/>.
        /// </summary>
        /// <param name="webhook">The webhook to register a handler for.</param>
        /// <param name="logTask">If <c>true</c>, a web farm task is created to register the handler on
        /// web farm servers.</param>
        void RegisterWebhook(WebhookInfo webhook, bool logTask = false);


        /// <summary>
        /// Unregisters the handler with the specified <see cref="WebhookInfo.WebhookID"/>.
        /// </summary>
        /// <param name="id">The <see cref="WebhookInfo.WebhookID"/> to unregister the handler for.</param>
        /// <param name="logTask">If <c>true</c>, a web farm task is created to unregister the handler on
        /// web farm servers.</param>
        void UnregisterWebhook(int id, bool logTask = false);


        /// <summary>
        /// Unregisters the handler with the specified <paramref name="webhook"/>.
        /// </summary>
        /// <param name="webhook">The webhook to unregister a handler for.</param>
        /// <param name="logTask">If <c>true</c>, a web farm task is created to unregister the handler on
        /// web farm servers.</param>
        void UnregisterWebhook(WebhookInfo webhook, bool logTask = false);
    }
}
