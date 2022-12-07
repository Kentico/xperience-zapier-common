namespace Xperience.Zapier.Common
{
    /// <summary>
    /// The type of Xperience event handler which triggers the webhook.
    /// </summary>
    public enum WebhookEventType
    {
        /// <summary>
        /// Invalid event type.
        /// </summary>
        None = -1,


        /// <summary>
        /// Triggers when a new object is created.
        /// </summary>
        Create = 0,


        /// <summary>
        /// Triggers when an existing object is updated.
        /// </summary>
        Update = 1,


        /// <summary>
        /// Triggers when an existing object is deleted.
        /// </summary>
        Delete = 2
    }
}