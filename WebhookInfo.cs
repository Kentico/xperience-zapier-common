using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Xperience.Zapier;

[assembly: RegisterObjectType(typeof(WebhookInfo), WebhookInfo.OBJECT_TYPE)]

namespace Xperience.Zapier
{
    /// <summary>
    /// Data container class for <see cref="WebhookInfo"/>.
    /// </summary>
    [Serializable]
    public partial class WebhookInfo : AbstractInfo<WebhookInfo, IWebhookInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "zapier.webhook";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(WebhookInfoProvider), OBJECT_TYPE, "Zapier.Webhook", "WebhookID", "WebhookLastModified", "WebhookGuid", "WebhookName", "WebhookName", null, "WebhookSiteID", null, null)
        {
            ModuleName = "Xperience.Zapier",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Webhook ID.
        /// </summary>
        [DatabaseField]
        public virtual int WebhookID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("WebhookID"), 0);
            }
            set
            {
                SetValue("WebhookID", value);
            }
        }


        /// <summary>
        /// Webhook created manually.
        /// </summary>
        [DatabaseField]
        public virtual bool WebhookCreatedManually
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("WebhookCreatedManually"), true);
            }
            set
            {
                SetValue("WebhookCreatedManually", value);
            }
        }


        /// <summary>
        /// Webhook site ID.
        /// </summary>
        [DatabaseField]
        public virtual int WebhookSiteID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("WebhookSiteID"), 0);
            }
            set
            {
                SetValue("WebhookSiteID", value, 0);
            }
        }


        /// <summary>
        /// Webhook name.
        /// </summary>
        [DatabaseField]
        public virtual string WebhookName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("WebhookName"), String.Empty);
            }
            set
            {
                SetValue("WebhookName", value);
            }
        }


        /// <summary>
        /// Webhook enabled.
        /// </summary>
        [DatabaseField]
        public virtual bool WebhookEnabled
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("WebhookEnabled"), true);
            }
            set
            {
                SetValue("WebhookEnabled", value);
            }
        }


        /// <summary>
        /// Webhook object type.
        /// </summary>
        [DatabaseField]
        public virtual string WebhookObjectType
        {
            get
            {
                return ValidationHelper.GetString(GetValue("WebhookObjectType"), String.Empty);
            }
            set
            {
                SetValue("WebhookObjectType", value);
            }
        }


        /// <summary>
        /// Webhook event type.
        /// </summary>
        [DatabaseField]
        public virtual int WebhookEventType
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("WebhookEventType"), 0);
            }
            set
            {
                SetValue("WebhookEventType", value);
            }
        }


        /// <summary>
        /// Webhook URL.
        /// </summary>
        [DatabaseField]
        public virtual string WebhookURL
        {
            get
            {
                return ValidationHelper.GetString(GetValue("WebhookURL"), String.Empty);
            }
            set
            {
                SetValue("WebhookURL", value, String.Empty);
            }
        }


        /// <summary>
        /// Webhook guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid WebhookGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("WebhookGuid"), Guid.Empty);
            }
            set
            {
                SetValue("WebhookGuid", value);
            }
        }


        /// <summary>
        /// Webhook last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime WebhookLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("WebhookLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("WebhookLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected WebhookInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="WebhookInfo"/> class.
        /// </summary>
        public WebhookInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="WebhookInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public WebhookInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}