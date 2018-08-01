using ScriptSDK.Engines;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using System;
using System.Collections.Generic;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public static class ItemFinder
    {
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

            return FindInContainer<T>(EasyUOHelper.ConvertToStealthType(easyUOObjectType), uint.MaxValue);
        }

        public static List<T> FindInBackpackOrPaperdoll<T>(string[] easyUOObjectTypes) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(PlayerMobile.GetPlayer().Serial);
            containersSerials.Add(PlayerMobile.GetPlayer().Backpack.Serial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials);
        }

        public static List<T> FindInContainer<T>(string[] easyUOObjectTypes, Serial containerSerial) where T : UOEntity
        {
            List<Serial> containersSerials = new List<Serial>();
            containersSerials.Add(containerSerial);

            return FindInContainers<T>(easyUOObjectTypes, containersSerials);
        }

        public static List<T> FindInContainer<T>(string easyUOObjectType, Serial containerSerial) where T : UOEntity
        {
            return FindInContainer<T>(EasyUOHelper.ConvertToStealthType(easyUOObjectType), containerSerial);
        }

        public static List<T> FindInContainers<T>(string[] easyUOObjectTypes, List<Serial> containersSerials) where T : UOEntity
        {
            var items = new List<T>();

            foreach (var easyUOObjectType in easyUOObjectTypes) // TODO: remove duplicate objectType, if any
            {
                items.AddRange(FindInContainers<T>(EasyUOHelper.ConvertToStealthType(easyUOObjectType), containersSerials));
            }

            return items;
        }

        public static List<T> FindInContainers<T>(ushort stealthObjectType, List<Serial> containersSerials) where T : UOEntity
        {
            if (containersSerials == null)
                containersSerials = new List<Serial>();

            var items = new List<T>();
            foreach (var containerSerial in containersSerials) // TODO: remove duplicate container, if any
            {
                items.AddRange(FindInContainer<T>(stealthObjectType, containerSerial));
            }

            return items;
        }

        public static List<T> FindInContainer<T>(ushort stealthObjectType, Serial containerSerial) where T : UOEntity
        {
            return FindInContainer<T>(stealthObjectType, containerSerial == null ? uint.MaxValue : containerSerial.Value);
        }

        public static List<T> FindInContainer<T>(ushort stealthObjectType, uint container) where T : UOEntity
        {
            var items = new List<T>();

            if (StealthAPI.Stealth.Client.FindType(stealthObjectType, container) < 1)
                return items;

            foreach (var foundItemId in StealthAPI.Stealth.Client.GetFindList())
            {
                items.Add(Activator.CreateInstance(typeof(T), new Serial(foundItemId)) as T);
            }

            return items;
        }
    }
}
