using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.ProxyInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ON?=" + StealthAPI.Stealth.Client.GetUseProxy());
            Console.WriteLine("  IP=" + StealthAPI.Stealth.Client.GetProxyIP());
            Console.WriteLine("PORT=" + StealthAPI.Stealth.Client.GetProxyPort());

            Thread.Sleep(100000);
        }
    }
}
