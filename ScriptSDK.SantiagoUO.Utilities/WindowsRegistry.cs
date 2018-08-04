using System;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public static class WindowsRegistry
    {
        public static string GetValue(string key, string value)
        {
            var subKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, false);

            if (subKey != null)
            {
                var subKeyValue = subKey.GetValue(value);

                if (subKeyValue != null)
                    return subKey.GetValue(value).ToString();
            }

            return null;
        }

        public static bool GetValueOrDefault(string key, string value, bool defaultValue)
        {
            string stringValue = GetValue(key, value);
            if (stringValue == null)
                return defaultValue;

            return stringValue.ToLower().Equals("true");
        }

        public static int GetValueOrDefault(string key, string value, int defaultValue)
        {
            string stringValue = GetValue(key, value);
            if (stringValue == null)
                return defaultValue;

            return Int32.Parse(stringValue);
        }
    }
}
