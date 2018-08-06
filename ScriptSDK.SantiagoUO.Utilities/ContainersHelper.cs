using ScriptSDK.Engines;
using ScriptSDK.Gumps;
using ScriptSDK.Items;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public static class ContainersHelper
    {
        /// <summary>
        /// Wait for a container to be accessible
        /// </summary>
        /// <param name="container">Container to wait for</param>
        /// <param name="maximumDelay">Maximum delay</param>
        /// <returns>true if container is accessible</returns>
        public static bool WaitForContainer(Container container, TimeSpan maximumDelay)
        {
            DateTime timeout = DateTime.Now.Add(maximumDelay);

            while (DateTime.Now < timeout)
            {
                if (container.Valid)
                    return true;

                Thread.Sleep(250);
            }

            ScriptLogger.WriteLine("[ERROR] Failed to open container '" + container.Serial.Value + "'");

            return false;
        }

        /// <summary>
        /// Moves all items of types from a container to another
        /// </summary>
        /// <param name="easyUOTypes">types of items to move (EasyUO)</param>
        /// <param name="source">Source container</param>
        /// <param name="target">Target container</param>
        public static void EmptyContainer(string[] easyUOTypes, Container source, Container target)
        {
            var items = ObjectsFinder.FindInContainer<Item>(easyUOTypes, source);

            foreach (var item in items)
            {
                item.MoveItem(target);

                Thread.Sleep(1000);
            }
        }
    }
}
