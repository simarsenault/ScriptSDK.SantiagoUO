using ScriptSDK.Engines;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using System;
using System.Collections.Generic;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public static class ObjectsFinder
    {
        private static readonly uint NO_CONTAINER = uint.MaxValue;

        private static readonly Dictionary<string, List<uint>> IGNORE_LISTS = new Dictionary<string, List<uint>>();

        #region Ignore Lists
        public static void AddToIgnoreList(string ignoreList, Serial serial)
        {
            if (!IGNORE_LISTS.ContainsKey(ignoreList))
                IGNORE_LISTS.Add(ignoreList, new List<uint>());

            if (!IGNORE_LISTS[ignoreList].Contains(serial.Value))
                IGNORE_LISTS[ignoreList].Add(serial.Value);
        }

        public static void ClearIgnoreList(string ignoreList)
        {
            if (IGNORE_LISTS.ContainsKey(ignoreList))
                IGNORE_LISTS.Remove(ignoreList);
        }

        private static bool IsIgnored(string ignoreList, uint itemId)
        {
            if (!IGNORE_LISTS.ContainsKey(ignoreList))
                return false;

            return IGNORE_LISTS[ignoreList].Contains(itemId);
        }
        #endregion

        #region Finder
        public static List<T> Find<T>(string[] easyUOObjectTypes, uint distance) where T : UOEntity
        {
            var items = new List<T>();

            foreach (var easyUOObjectType in easyUOObjectTypes) // TODO: remove duplicate objectType, if any
            {
                items.AddRange(Find<T>(easyUOObjectType, distance));
            }

            return items;
        }

        public static List<T> Find<T>(string easyUOObjectType, uint distance) where T : UOEntity
        {
            StealthAPI.Stealth.Client.SetFindDistance(distance);

            return FindInContainer<T>(EasyUOHelper.ConvertToStealthType(easyUOObjectType), NO_CONTAINER, null);
        }

        public static List<T> FindInBackpack<T>(string[] easyUOObjectTypes) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(PlayerMobile.GetPlayer().Backpack.Serial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials, null);
        }

        public static List<T> FindInBackpack<T>(string[] easyUOObjectTypes, string ignoreList) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(PlayerMobile.GetPlayer().Backpack.Serial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials, ignoreList);
        }

        public static List<T> FindInBackpackOrPaperdoll<T>(string[] easyUOObjectTypes) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(PlayerMobile.GetPlayer().Serial);
            containersSerials.Add(PlayerMobile.GetPlayer().Backpack.Serial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials, null);
        }

        public static List<T> FindInContainer<T>(string[] easyUOObjectTypes, Container container) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(container.Serial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials, null);
        }

        public static List<T> FindInContainer<T>(string[] easyUOObjectTypes, Serial containerSerial) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(containerSerial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials, null);
        }

        public static List<T> FindInContainer<T>(string easyUOObjectType, Serial containerSerial) where T : UOEntity
        {
            return FindInContainer<T>(EasyUOHelper.ConvertToStealthType(easyUOObjectType), containerSerial, null);
        }

        public static List<T> FindInContainers<T>(string[] easyUOObjectTypes, List<Serial> containersSerials, string ignoreList) where T : UOEntity
        {
            var items = new List<T>();

            foreach (var easyUOObjectType in easyUOObjectTypes) // TODO: remove duplicate objectType, if any
            {
                items.AddRange(FindInContainers<T>(EasyUOHelper.ConvertToStealthType(easyUOObjectType), containersSerials, ignoreList));
            }

            return items;
        }

        public static List<T> FindInContainers<T>(ushort stealthObjectType, List<Serial> containersSerials, string ignoreList) where T : UOEntity
        {
            if (containersSerials == null)
                containersSerials = new List<Serial>();

            var items = new List<T>();
            foreach (var containerSerial in containersSerials) // TODO: remove duplicate container, if any
            {
                items.AddRange(FindInContainer<T>(stealthObjectType, containerSerial, ignoreList));
            }

            return items;
        }

        public static List<T> FindInContainer<T>(ushort stealthObjectType, Serial containerSerial, string ignoreList) where T : UOEntity
        {
            return FindInContainer<T>(stealthObjectType, containerSerial == null ? NO_CONTAINER : containerSerial.Value, ignoreList);
        }

        public static List<T> FindInContainer<T>(ushort stealthObjectType, uint container, string ignoreList) where T : UOEntity
        {
            var items = new List<T>();

            if (StealthAPI.Stealth.Client.FindType(stealthObjectType, container) < 1)
                return items;

            foreach (var foundItemId in StealthAPI.Stealth.Client.GetFindList())
            {
                if (ignoreList == null || !IsIgnored(ignoreList, foundItemId))
                    items.Add(Activator.CreateInstance(typeof(T), new Serial(foundItemId)) as T);
            }

            return items;
        }
        #endregion
    }
}
