using CMS.DataEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xperience.Zapier
{
    public static class ZapierExtensionMethods
    {
        public static Dictionary<string, string> ToZapierObject(this BaseInfo baseInfo)
        {
            if(baseInfo != null)
            {
                var obj = new Dictionary<string, string>();
                foreach (var col in baseInfo.ColumnNames)
                {
                    obj[col] = baseInfo.GetStringValue(col, string.Empty);
                }
                return obj;
            }

            return null;
        }

        public static string ToZapierString(this BaseInfo baseInfo)
        {
            if(baseInfo != null)
            {
                var obj = new Dictionary<string, string>();
                foreach (var col in baseInfo.ColumnNames)
                {
                    obj[col] = baseInfo.GetStringValue(col, string.Empty);
                }

                return JsonConvert.SerializeObject(obj);
            }

            return string.Empty;
        }
    }
}