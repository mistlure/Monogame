using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Config;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;

namespace MonogameProject.Systems
{
    public static class TilePurchaseSystem
    {
        private static KeyboardState _previousState;

        public static void HandlePurchase(World world, GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (!keyboard.IsKeyDown(Keys.Space) || _previousState.IsKeyDown(Keys.Space))
            {
                _previousState = keyboard;
                return;
            }

            int? cursorX = null;
            int? cursorY = null;

            foreach (var id in world.GetAllEntityIds())
            {
                var ent = new Entity(id);
                if (world.TryGetComponent<CursorComponent>(ent) != null)
                {
                    var pos = world.TryGetComponent<PositionComponent>(ent);
                    if (pos.HasValue)
                    {
                        cursorX = pos.Value.X;
                        cursorY = pos.Value.Y;
                    }
                    break;
                }
            }

            if (cursorX.HasValue && cursorY.HasValue)
            {
                AttemptPurchase(world, cursorX.Value, cursorY.Value);
            }

            _previousState = keyboard;
        }

        private static void AttemptPurchase(World world, int targetX, int targetY)
        {

            var targetTileId = world.GetTileId(targetX, targetY);
            if (targetTileId == null) return;

            var entity = new Entity(targetTileId.Value);
            var tile = world.TryGetComponent<TileTypeComponent>(entity);
            var owned = world.TryGetComponent<OwnedComponent>(entity);

            if (!tile.HasValue) return;
            if (tile.Value.Type != TileType.Water) return;
            if (owned.HasValue && owned.Value.isOwned) return;
            if (GameSettings.PlayerCoins < GameSettings.TileCost) return;

            bool hasNeighbor = false;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int nx = targetX + dx;
                    int ny = targetY + dy;
                    var neighborId = world.GetTileId(nx, ny);
                    if (neighborId != null)
                    {
                        var neighborEnt = new Entity(neighborId.Value);
                        var neighborOwned = world.TryGetComponent<OwnedComponent>(neighborEnt);
                        if (neighborOwned.HasValue && neighborOwned.Value.isOwned)
                        {
                            hasNeighbor = true;
                            goto FoundNeighbor;
                        }
                    }
                }
            }
        FoundNeighbor:
            if (!hasNeighbor) return;

            GameSettings.PlayerCoins -= GameSettings.TileCost;
            world.AddComponent(entity, new OwnedComponent(true));
            world.AddComponent(entity, new TileTypeComponent(TileType.Grass));
        }
    }
}