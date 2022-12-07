using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Activities;
using CMS.Automation;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Marketing automation action which triggers a Zapier webhook with the processes <see cref="ContactInfo"/> and the
    /// activity which triggered the automation process.
    /// </summary>
    public class ZapierContactAutomationAction : ContactAutomationAction
    {
        /// <inheritdoc/>
        public override void Execute()
        {
            var url = GetResolvedParameter("WebhookURL", String.Empty);
            if (String.IsNullOrEmpty(url))
            {
                LogMessage(EventType.ERROR, nameof(ZapierContactAutomationAction), "Zapier webhook URL cannot be empty.", Contact);
                return;
            }

            if (Contact == null)
            {
                LogMessage(EventType.ERROR, nameof(ZapierContactAutomationAction), "Contact not found.", Contact);
                return;
            }

            // Try to get custom state data (activity)
            ActivityInfo activity = null;
            if (!DataHelper.IsEmpty(StateObject.StateCustomData))
            {
                var activityDetailItemID = StateObject.StateCustomData[TriggerDataConstants.TRIGGER_DATA_ACTIVITY_ITEM_DETAILID];
                var activityItemID = StateObject.StateCustomData[TriggerDataConstants.TRIGGER_DATA_ACTIVITY_ITEMID];
                var activityValue = StateObject.StateCustomData[TriggerDataConstants.TRIGGER_DATA_ACTIVITY_VALUE];
                var activitySiteId = StateObject.StateCustomData[TriggerDataConstants.TRIGGER_DATA_ACTIVITY_SITEID];
                activity = ActivityInfo.Provider.Get()
                    .TopN(1)
                    .WhereEquals(nameof(ActivityInfo.ActivityItemID), activityItemID)
                    .WhereEquals(nameof(ActivityInfo.ActivityItemDetailID), activityDetailItemID)
                    .WhereEquals(nameof(ActivityInfo.ActivitySiteID), activitySiteId)
                    .WhereEquals(nameof(ActivityInfo.ActivityContactID), Contact.ContactID)
                    .WhereEquals(nameof(ActivityInfo.ActivityValue), activityValue)
                    .WhereLessThan(nameof(ActivityInfo.ActivityCreated), StateObject.StateCreated.AddHours(1))
                    .WhereGreaterThan(nameof(ActivityInfo.ActivityCreated), StateObject.StateCreated.AddHours(-1))
                    .FirstOrDefault();
            }

            var data = new List<BaseInfo> { Contact };
            if (activity != null)
            {
                data.Add(activity);
            }

            Service.Resolve<IZapierClient>().TriggerWebhook(url, data.ToArray()).ConfigureAwait(false);
        }
    }
}