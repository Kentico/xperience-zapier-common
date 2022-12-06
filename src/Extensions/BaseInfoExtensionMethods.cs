using System;
using System.Collections.Generic;

using CMS.DataEngine;

using Newtonsoft.Json;

namespace Xperience.Zapier.Common
{
    public static class BaseInfoExtensionMethods
    {
        public static Dictionary<string, string> ToZapierObject(this BaseInfo baseInfo)
        {
            if (baseInfo == null)
            {
                throw new ArgumentNullException(nameof(baseInfo));
            }

            var obj = new Dictionary<string, string>();
            foreach (var col in baseInfo.ColumnNames)
            {
                obj[col] = baseInfo.GetStringValue(col, String.Empty);
            }

            return obj;
        }


        public static string ToZapierString(this BaseInfo baseInfo)
        {
            if (baseInfo == null)
            {
                throw new ArgumentNullException(nameof(baseInfo));
            }

            var obj = new Dictionary<string, string>();
            foreach (var col in baseInfo.ColumnNames)
            {
                obj[col] = baseInfo.GetStringValue(col, String.Empty);
            }

            return JsonConvert.SerializeObject(obj);
        }
    }
}