using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public static class WindowsRegistry
    {
        public static string GetValue(string key, string value)
        {
            var subKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, false);

            if (subKey != null)
                return subKey.GetValue(value).ToString();

            return string.Empty;
        }
    }
}
