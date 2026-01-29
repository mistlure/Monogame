using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameProject.Config
{
    public static class GameSettings
    {
        // Map dimensions in tiles
        // Change these values to adjust the map size
        public const int MapWidth = 30;
        public const int MapHeight = 30;

        // Size of each tile in pixels
        // Change this value to adjust the tile size
        public static int TileSize = 16;

        // Initial player coins
        // Change this value to set the starting amount of coins
        public static int PlayerCoins = 120;

        // Cost per tile
        public const int TileCost = 10;
        public const int CostGrass = 10;
        public const int CostSand = 50;
    }
}
