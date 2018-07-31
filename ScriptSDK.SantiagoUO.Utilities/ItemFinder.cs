using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using System;
using System.Collections.Generic;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public static class ItemFinder
    {
        public static List<T> FindInBackpackOrPaperdoll<T>(ushort objectType) where T : UOEntity
        {
            List<Serial> serials = new List<Serial>();
            serials.Add(PlayerMobile.GetPlayer().Serial);
            serials.Add(PlayerMobile.GetPlayer().Backpack.Serial);

            return FindInContainers<T>(objectType, serials);
        }

        public static List<T> FindInContainers<T>(ushort objectType, List<Serial> containersSerials) where T : UOEntity
        {
            if (containersSerials == null)
                containersSerials = new List<Serial>();

            List<uint> containersIds = new List<uint>();
            foreach (Serial containerSerial in containersSerials)
            {
                containersIds.Add(containerSerial.Value);
            }

            var items = new List<T>();
            foreach (uint containerId in containersIds)
            {
                if (StealthAPI.Stealth.Client.FindType(objectType, containerId) < 1)
                    continue;

                foreach (var foundItemId in StealthAPI.Stealth.Client.GetFindList())
                {
                    items.Add(Activator.CreateInstance(typeof(T), new Serial(foundItemId)) as T);
                }
            }

            return items;
        }
    }
}
