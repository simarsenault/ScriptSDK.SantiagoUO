using ScriptSDK.Engines;
using ScriptSDK.Mobiles;
using StealthAPI;
using System.Collections.Generic;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public class UltimaTileReader
    {
        public static List<StaticItemRealXY> GetLumberSpots(int distance)
        {
            var tiles = new List<StaticItemRealXY>();
            var playerLocation = PlayerMobile.GetPlayer().Location;

            for (var x = playerLocation.X - distance; x <= playerLocation.X + distance; x++)
            {
                for (var y = playerLocation.Y - distance; y <= playerLocation.Y + distance; y++)
                {
                    foreach (var tile in Ultima.Map.Felucca.Tiles.GetStaticTiles(x, y))
                    {
                        ushort fixedTileId = (ushort) (tile.ID - 16385); // TODO: but whyyyyy?

                        if (TileReader.TreeTiles.Contains(fixedTileId))
                        {
                            tiles.Add(new StaticItemRealXY { Tile = fixedTileId, X = (ushort)x, Y = (ushort)y, Z = (byte)tile.Z, Color = (ushort)tile.Hue });

                            break;
                        }
                    }
                }
            }

            return tiles;
        }
    }
}
