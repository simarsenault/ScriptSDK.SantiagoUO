using ScriptSDK.Data;
using ScriptSDK.Engines;
using ScriptSDK.Mobiles;
using StealthAPI;
using System;
using System.Collections.Generic;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public class UltimaTileReader
    {
        /// <summary>
        /// Get all the choppable tiles within a distance of current player
        /// </summary>
        /// <param name="distance">Radius to scan</param>
        /// <returns>List of choppable tiles</returns>
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

        /// <summary>
        /// Get all the mineable tiles around (x,y).
        /// 
        /// Location (x,y) must be mineable.
        /// </summary>
        /// <param name="x">Location X</param>
        /// <param name="y">Location Y</param>
        /// <returns>List of mineable tiles</returns>
        public static List<StaticItemRealXY> GetMineSpots(int x, int y)
        {
            var tiles = new List<StaticItemRealXY>();
            var tilesCandidates = new List<Point2D>();
            var tilesScanned = new List<Point2D>();
            tilesCandidates.Add(new Point2D(x, y));

            while (tilesCandidates.Count > 0)
            {
                var tileCandidate = tilesCandidates[0];

                bool scanSurroundingTiles = false;
                foreach (var tile in Ultima.Map.Felucca.Tiles.GetStaticTiles(tileCandidate.X, tileCandidate.Y))
                {
                    ushort fixedTileId = (ushort)(tile.ID - 16384); // TODO: but whyyyyy?

                    if (TileReader.CaveTiles.Contains(fixedTileId))
                    {
                        tiles.Add(new StaticItemRealXY { Tile = fixedTileId, X = (ushort)tileCandidate.X, Y = (ushort)tileCandidate.Y, Z = (byte)tile.Z, Color = (ushort)tile.Hue });
                        scanSurroundingTiles = true;

                        break;
                    }
                }

                tilesCandidates.RemoveAt(0);
                tilesScanned.Add(new Point2D(tileCandidate.X, tileCandidate.Y));

                if (scanSurroundingTiles)
                {
                    for (var tileCandidateX = -1; tileCandidateX <= 1; tileCandidateX++)
                    {
                        for (var tileCandidateY = -1; tileCandidateY <= 1; tileCandidateY++)
                        {
                            Point2D tileCandidateOffset = new Point2D(tileCandidate.X + tileCandidateX, tileCandidate.Y + tileCandidateY);
                            if (!tilesScanned.Contains(tileCandidateOffset) && !tilesCandidates.Contains(tileCandidateOffset))
                                tilesCandidates.Add(tileCandidateOffset);
                        }
                    }
                }
            }

            return tiles;
        }
    }
}
