using ScriptSDK.Attributes;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using System;
using System.Linq;
using System.Threading;

namespace ScriptSDK.SantiagoUO.RaillessMining
{
    class RaillessMining
    {
        private static readonly int MINING_HIT_TIMEOUT = 10000;

        public void Start()
        {
            var tiles = UltimaTileReader.GetMineSpots(PlayerMobile.GetPlayer().Location.X, PlayerMobile.GetPlayer().Location.Y);

            while (tiles.Count > 0)
            {
                Data.Point3D playerLocation = PlayerMobile.GetPlayer().Location;
                var nearestMineableTile = tiles.FindAll(tile => Math.Sqrt(Math.Pow((tile.X - playerLocation.X), 2) + Math.Pow((tile.Y - playerLocation.Y), 2)) != 0).OrderBy(tile => Math.Sqrt(Math.Pow((tile.X - playerLocation.X), 2) + Math.Pow((tile.Y - playerLocation.Y), 2))).First();

                MovingHelper.GetMovingHelper().newMoveXY(nearestMineableTile.X, nearestMineableTile.Y, true, 0, true);

                var mineableTilesAroundPlayer = tiles.FindAll(tile => Math.Sqrt(Math.Pow((tile.X - nearestMineableTile.X), 2) + Math.Pow((tile.Y - nearestMineableTile.Y), 2)) == 1);

                if (mineableTilesAroundPlayer.Count == 0)
                {
                    tiles.Remove(nearestMineableTile);

                    continue;
                }
                
                while (mineableTilesAroundPlayer.Count > 0)
                {
                    var mineableTile = mineableTilesAroundPlayer[0];

                    var pickaxe = ObjetsFinder.FindInBackpackOrPaperdoll<Item>(EasyUOItem.PICKAXES);
                    if (pickaxe.Count == 0)
                        return;

                    if (MineTile(pickaxe.First(), mineableTile) == MineTileResult.DONE)
                    {
                        mineableTilesAroundPlayer.Remove(mineableTile);
                        tiles.Remove(mineableTile);
                    }
                }
            }
        }

        private MineTileResult MineTile(Item pickaxe, StealthAPI.StaticItemRealXY tile)
        {
            pickaxe.DoubleClick();
            TargetHelper.GetTarget().WaitForTarget(5000);

            DateTime dateTime = DateTime.Now;
            DateTime maxDateTime = dateTime.AddMilliseconds(MINING_HIT_TIMEOUT);

            TargetHelper.GetTarget().TargetTo(tile.Tile, new Data.Point3D(tile.X, tile.Y, tile.Z));

            while (DateTime.Now < maxDateTime)
            {
                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You loosen some rocks but fail to find any useable ore", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.CONTINUE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You put the", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.CONTINUE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("There is nothing here to mine", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("Try mining elsewhere", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("That is too far", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You have no line", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You cannot mine", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                Thread.Sleep(50);
            }

            return MineTileResult.DONE;
        }

        private enum MineTileResult
        {
            CONTINUE, DONE
        }
    }
}
