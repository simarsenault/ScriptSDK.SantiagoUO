using ScriptSDK.Attributes;
using ScriptSDK.Data;
using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptSDK.SantiagoUO.RaillessMining
{
    class RaillessMining
    {
        private static readonly int MAXIMUM_MINE_DISTANCE = 1;
        private static readonly int MINING_HIT_TIMEOUT = 10000;

        public void Start()
        {
            var tiles = UltimaTileReader.GetMineSpots(PlayerMobile.GetPlayer().Location.X, PlayerMobile.GetPlayer().Location.Y);

            while (tiles.Count > 0)
            {
                Data.Point3D playerLocation = PlayerMobile.GetPlayer().Location;
                var nearestMineableTile = tiles.OrderBy(tile => Math.Sqrt(Math.Pow((tile.X - playerLocation.X), 2) + Math.Pow((tile.Y - playerLocation.Y), 2))).First();

                MovingHelper.GetMovingHelper().newMoveXY(nearestMineableTile.X, nearestMineableTile.Y, true, 1, true);

                tiles.Remove(nearestMineableTile);
            }
        }
    }
}
