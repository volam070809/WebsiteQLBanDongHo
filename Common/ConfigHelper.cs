using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebsiteQLBanDongHo.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// Get value from AppSettings safely.
        /// (Project hay bị lỗi NullReferenceException khi thiếu key.)
        /// </summary>
        public static string GetByKey(string key, string defaultValue = "")
        {
            var v = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(v)) return defaultValue;
            return v;
        }
    }
}