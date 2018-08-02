using ScriptSDK.Attributes;
using ScriptSDK.Engines;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScriptSDK.SantiagoUO.RaillessLumberjacking
{
    public class RaillessLumberjacking
    {
        private static readonly int TILE_SCAN_DISTANCE = 75;
        private static readonly int LUMBERJACKING_HIT_TIMEOUT = 10000;

        public void Start()
        {
            var tiles = UltimaTileReader.GetLumberSpots(TILE_SCAN_DISTANCE);
         
            while (tiles.Count > 0)
            {
                Data.Point3D playerLocation = PlayerMobile.GetPlayer().Location;
                var nearestTreeTile = tiles.OrderBy(tile => Math.Sqrt(Math.Pow((tile.X - playerLocation.X), 2) + Math.Pow((tile.Y - playerLocation.Y), 2))).First();

                MovingHelper.GetMovingHelper().newMoveXY(nearestTreeTile.X, nearestTreeTile.Y, true, 1, true);
                
                var hatchet = ObjetsFinder.FindInBackpackOrPaperdoll<Item>(EasyUOItem.HATCHETS);
                if (hatchet.Count == 0)
                    return;

                if (ChopTree(hatchet.First(), nearestTreeTile) == ChopTreeResult.DONE)
                    tiles.Remove(nearestTreeTile);
            }
        }

        private ChopTreeResult ChopTree(Item hatchet, StealthAPI.StaticItemRealXY tile)
        {
            hatchet.DoubleClick();
            TargetHelper.GetTarget().WaitForTarget(5000);

            DateTime dateTime = DateTime.Now;
            DateTime maxDateTime = dateTime.AddMilliseconds(LUMBERJACKING_HIT_TIMEOUT);

            TargetHelper.GetTarget().TargetTo(tile.Tile, new Data.Point3D(tile.X, tile.Y, tile.Z));

            while (DateTime.Now < maxDateTime)
            {
                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You hack at the tree for a while, but fail to produce any useable wood", dateTime, DateTime.Now) >= 0)
                    return ChopTreeResult.CONTINUE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You put the logs in your pack", dateTime, DateTime.Now) >= 0)
                    return ChopTreeResult.CONTINUE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("There is nothing here to chop", dateTime, DateTime.Now) >= 0)
                    return ChopTreeResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("It appears immune to your blow", dateTime, DateTime.Now) >= 0)
                    return ChopTreeResult.DONE;

                Thread.Sleep(50);
            }

            return ChopTreeResult.DONE;
        }

        private enum ChopTreeResult
        {
            CONTINUE, DONE
        }
    }
}
