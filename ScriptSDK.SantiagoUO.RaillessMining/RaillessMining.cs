using ScriptSDK.Attributes;
using ScriptSDK.Engines;
using ScriptSDK.Gumps;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScriptSDK.SantiagoUO.RaillessMining
{
    class RaillessMining
    {
        private static readonly int MINING_HIT_TIMEOUT = 10000;

        private PlayerMobile playerMobile;
        private MovingHelper movingHelper;

        private readonly int smeltWeight;
        private readonly bool smelt1x1;
        private readonly int bankWeight;
        private readonly Container dropContainer;
        private readonly int pickaxesCount;

        public RaillessMining()
        {
            this.playerMobile = PlayerMobile.GetPlayer();
            this.movingHelper = MovingHelper.GetMovingHelper();

            this.smeltWeight = WindowsRegistry.GetValueOrDefault(@"Software\ScriptSDK.SantiagoUO\RaillessMining\" + playerMobile.Name, "SmeltWeight", 350);
            this.smelt1x1 = WindowsRegistry.GetValueOrDefault(@"Software\ScriptSDK.SantiagoUO\RaillessMining\" + playerMobile.Name, "Smelt1x1", false);
            this.bankWeight = WindowsRegistry.GetValueOrDefault(@"Software\ScriptSDK.SantiagoUO\RaillessMining\" + playerMobile.Name, "BankWeight", 100);
            this.dropContainer = new Container(EasyUOHelper.ConvertToStealthID(WindowsRegistry.GetValue(@"Software\ScriptSDK.SantiagoUO\RaillessMining\" + playerMobile.Name, "DropContainerId")));
            this.pickaxesCount = WindowsRegistry.GetValueOrDefault(@"Software\ScriptSDK.SantiagoUO\RaillessMining\" + playerMobile.Name, "PickaxesCount", 2);
        }

        public void MineCave()
        {
            var tiles = UltimaTileReader.GetMineSpots(this.playerMobile.Location.X, this.playerMobile.Location.Y);
            var forges = ObjectsFinder.Find<Item>(EasyUOItem.FORGE, 18);

            while (tiles.Count > 0)
            {
                Data.Point3D playerLocation = this.playerMobile.Location;
                var nearestMineableTile = tiles.FindAll(tile => Math.Sqrt(Math.Pow((tile.X - playerLocation.X), 2) + Math.Pow((tile.Y - playerLocation.Y), 2)) != 0).OrderBy(tile => Math.Sqrt(Math.Pow((tile.X - playerLocation.X), 2) + Math.Pow((tile.Y - playerLocation.Y), 2))).First();

                movingHelper.newMoveXY(nearestMineableTile.X, nearestMineableTile.Y, true, 0, true);

                var mineableTilesAroundPlayer = tiles.FindAll(tile => Math.Sqrt(Math.Pow((tile.X - nearestMineableTile.X), 2) + Math.Pow((tile.Y - nearestMineableTile.Y), 2)) == 1);

                if (mineableTilesAroundPlayer.Count == 0)
                {
                    tiles.Remove(nearestMineableTile);

                    continue;
                }
                
                while (mineableTilesAroundPlayer.Count > 0)
                {
                    var mineableTile = mineableTilesAroundPlayer[0];

                    var pickaxe = ObjectsFinder.FindInBackpackOrPaperdoll<Item>(EasyUOItem.PICKAXES);
                    if (pickaxe.Count == 0)
                    {
                        Bank();

                        continue;
                    }

                    if (MineTile(pickaxe.First(), mineableTile) == MineTileResult.DONE)
                    {
                        mineableTilesAroundPlayer.Remove(mineableTile);
                        tiles.Remove(mineableTile);
                    }

                    if (playerMobile.Weight >= smeltWeight)
                    {
                        Smelt(forges);

                        if (playerMobile.Weight >= bankWeight)
                            Bank();
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

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You have no line of sight to that location", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You cannot mine", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE; 

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("Try mining in rock", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You decide not to mine for now", dateTime, DateTime.Now) >= 0)
                    return MineTileResult.DONE;

                Thread.Sleep(50);
            }

            return MineTileResult.DONE;
        }

        private void Smelt(List<Item> forges)
        {
            if (forges.Count > 0)
            {
                var forge = forges.OrderBy(_forge => Math.Sqrt(Math.Pow((_forge.Location.X - playerMobile.Location.X), 2) + Math.Pow((_forge.Location.Y - playerMobile.Location.Y), 2))).First();

                movingHelper.newMoveXY((ushort)forge.Location.X, (ushort)forge.Location.Y, true, 1, true);

                if (smelt1x1)
                    SplitOres();

                var oresStacks = ObjectsFinder.FindInBackpack<Item>(EasyUOItem.ORES);
                while (oresStacks.Count > 0)
                {
                    var oresStack = oresStacks[0];

                    oresStack.DoubleClick();

                    Thread.Sleep(250);

                    oresStacks = ObjectsFinder.FindInBackpack<Item>(EasyUOItem.ORES);
                }
            }
        }

        private void SplitOres()
        {
            var oresStacks = ObjectsFinder.FindInBackpack<Item>(EasyUOItem.ORES_STACKS);

            while (oresStacks.Count > 0)
            {
                foreach (var oresStack in oresStacks)
                {
                    oresStack.MoveItem(playerMobile.Backpack, 1, new Data.Point3D(100, 100, 0));

                    Thread.Sleep(500);
                }

                oresStacks = ObjectsFinder.FindInBackpack<Item>(EasyUOItem.ORES_STACKS);
            }
        }

        private void Bank()
        {
            List<Data.Point3D> track = new List<Data.Point3D>();
            track.Add(new Data.Point3D(800, 1469, 0)); // Drop location
            track.Add(new Data.Point3D(807, 1469, 0));
            track.Add(new Data.Point3D(857, 1425, 0));
            track.Add(new Data.Point3D(882, 1480, 0));
            track.Add(new Data.Point3D(909, 1543, 0));
            track.Add(new Data.Point3D(995, 1622, 0));
            track.Add(new Data.Point3D(997, 1602, 0)); // Mine 

            for (var x = track.Count - 1; x >= 0; x--)
            {
                movingHelper.newMoveXY((ushort)track[x].X, (ushort)track[x].Y, true, 0, true);
            }

            ContainersHelper.EmptyContainer(EasyUOItem.INGOTS, playerMobile.Backpack, dropContainer);
            ContainersHelper.EmptyContainer(EasyUOItem.ORES, playerMobile.Backpack, dropContainer);

            RestockPickaxes();

            for (var x = 0; x < track.Count; x++)
            {
                movingHelper.newMoveXY((ushort)track[x].X, (ushort)track[x].Y, true, 0, true);
            }
        }

        private void RestockPickaxes()
        {
            var missingPickaxes = pickaxesCount - ObjectsFinder.FindInBackpackOrPaperdoll<Item>(EasyUOItem.PICKAXES).Count;

            if (missingPickaxes > 0 && dropContainer.DoubleClick())
            {
                if (ContainersHelper.WaitForContainer(dropContainer, TimeSpan.FromSeconds(10)))
                {
                    var pickaxes = ObjectsFinder.FindInContainer<Item>(EasyUOItem.PICKAXES, dropContainer);

                    foreach (var pickaxe in pickaxes)
                    {
                        pickaxe.MoveItem(playerMobile.Backpack);

                        if (--missingPickaxes == 0)
                            return;

                        Thread.Sleep(500);
                    }
                }
            }
        }

        private enum MineTileResult
        {
            CONTINUE, DONE
        }
    }
}
