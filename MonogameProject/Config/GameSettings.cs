using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonogameProject.Enums;

namespace MonogameProject.Config
{
    public struct ShopItem
    {
        public string Name;
        public int Price;
        public TileType? ResultTile;
        public bool IsCancelButton;
    }

    public static class GameSettings
    {
        public const int MapWidth = 30;
        public const int MapHeight = 30;
        public static int TileSize = 16;
        public static int PlayerCoins = 120;

        public static int CursorEntityId = -1;

        public const int DefaultTileCost = 10;

        public static readonly List<ShopItem> TilePurchaseOptions = new List<ShopItem>
        {
            new ShopItem { Name = "X", Price = 0, ResultTile = null, IsCancelButton = true },
            new ShopItem { Name = "Grass", Price = 10, ResultTile = TileType.Grass, IsCancelButton = false },
            new ShopItem { Name = "Sand", Price = 15, ResultTile = TileType.Sand, IsCancelButton = false }
        };

        public static readonly List<ShopItem> GrassDigOptions = new List<ShopItem>
        {
            new ShopItem { Name = "X", Price = 0, ResultTile = null, IsCancelButton = true },
            new ShopItem { Name = "Dig", Price = 0, ResultTile = TileType.Farm, IsCancelButton = false }
        };
    }
}