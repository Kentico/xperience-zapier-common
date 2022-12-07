using System;
using System.Collections.Generic;

using CMS.DataEngine;

namespace Xperience.Zapier.Common
{
    /// <summary>
    /// Extension methods for Xperience <see cref="BaseInfo"/> objects.
    /// </summary>
    public static class BaseInfoExtensionMethods
    {
        /// <summary>
        /// Converts a <see cref="BaseInfo"/> object into a <see cref="Dictionary{TKey, TValue}"/> containing the
        /// object's columns and values.
        /// </summary>
        /// <param name="baseInfo">The Xperience object to convert.</param>
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
    }
}