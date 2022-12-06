using System;

using CMS.Core;
using CMS.DocumentEngine;
using CMS.EventLog;

using Xperience.Zapier.Common;

namespace Xperience.Zapier
{
    public class ZapierWorkflowAction : DocumentWorkflowAction
    {
        public override void Execute()
        {
            var url = GetResolvedParameter("WebhookURL", String.Empty);
            if (String.IsNullOrEmpty(url))
            {
                LogMessage(EventType.ERROR, nameof(ZapierWorkflowAction), "Zapier webhook URL cannot be empty.", Node);
                return;
            }

            if (Node == null)
            {
                LogMessage(EventType.ERROR, nameof(ZapierWorkflowAction), "Node not found.", Node);
                return;
            }

            Service.Resolve<IZapierClient>().TriggerWebhook(url, Node).ConfigureAwait(false);
        }
    }
}