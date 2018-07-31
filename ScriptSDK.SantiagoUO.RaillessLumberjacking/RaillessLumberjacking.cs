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
        private static readonly int TILE_SCAN_DISTANCE = 25;

        public void Start()
        {
            var tiles = UltimaTileReader.GetLumberSpots(TILE_SCAN_DISTANCE);
         
            while (tiles.Count > 0)
            {
                var nearestTreeTile = tiles.OrderBy(tile => Math.Sqrt(Math.Pow((tile.X - PlayerMobile.GetPlayer().Location.X), 2) + Math.Pow((tile.Y - PlayerMobile.GetPlayer().Location.Y), 2))).First();

                MovingHelper.GetMovingHelper().newMoveXY(nearestTreeTile.X, nearestTreeTile.Y, true, 1, true);
                
                var hatchet = ItemFinder.FindInBackpackOrPaperdoll<Item>(EasyUOHelper.ConvertToStealthType(EasyUOItem.HATCHET));
                if (hatchet.Count == 0)
                    return;

                hatchet.First().DoubleClick();
                TargetHelper.GetTarget().WaitForTarget(10000);
                TargetHelper.GetTarget().TargetTo(nearestTreeTile.Tile, new Data.Point3D(nearestTreeTile.X, nearestTreeTile.Y, nearestTreeTile.Z));

                Thread.Sleep(8000); // TODO scan journal

                tiles.Remove(nearestTreeTile);
            }
        }
    }
}
