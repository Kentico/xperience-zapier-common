using CMS;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.UIControls;

using Xperience.Zapier.Common;

[assembly: RegisterCustomClass(nameof(WebhookListingExtender), typeof(WebhookListingExtender))]
namespace Xperience.Zapier.Common
{
    /// <summary>
    /// UniGrid extender for the Zapier webhook listing page.
    /// </summary>
    public class WebhookListingExtender : ControlExtender<UniGrid>
    {
        /// <inheritdoc/>
        public override void OnInit()
        {
            Control.OnExternalDataBound += Control_OnExternalDataBound;
        }


        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            switch (sourceName)
            {
                case "event":
                    var eventValue = ValidationHelper.GetInteger(parameter, -1);
                    var eventType = GetWebhookEventTypeEnum(eventValue);
                    return eventType.ToStringRepresentation();
            }

            return parameter;
        }


        private WebhookEventType GetWebhookEventTypeEnum(int value)
        {
            switch (value)
            {
                case 0:
                    return WebhookEventType.Create;
                case 1:
                    return WebhookEventType.Update;
                case 2:
                    return WebhookEventType.Delete;
                default:
                    return WebhookEventType.None;
            }
        }
    }
}