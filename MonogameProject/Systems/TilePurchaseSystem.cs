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

        public static void Update(World world, GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (GameSettings.CursorEntityId == -1) return;
            var cursorEntity = new Entity(GameSettings.CursorEntityId);

            if (world.TryGetComponent<ActionMenuComponent>(cursorEntity) != null) return;

            if (keyboard.IsKeyDown(Keys.Space) && !_previousState.IsKeyDown(Keys.Space))
            {
                var pos = world.TryGetComponent<PositionComponent>(cursorEntity);
                if (pos.HasValue)
                {
                    TryOpenPurchaseMenu(world, cursorEntity, pos.Value.X, pos.Value.Y);
                }
            }

            _previousState = keyboard;
        }

        private static void TryOpenPurchaseMenu(World world, Entity cursorEntity, int x, int y)
        {
            var targetTileId = world.GetTileId(x, y);
            if (targetTileId == null) return;

            var targetEntity = new Entity(targetTileId.Value);
            var tile = world.TryGetComponent<TileTypeComponent>(targetEntity);
            var owned = world.TryGetComponent<OwnedComponent>(targetEntity);

            if (!tile.HasValue) return;

            if (tile.Value.Type != TileType.Water) return;

            if (owned.HasValue && owned.Value.isOwned) return;

            if (!HasOwnedNeighbor(world, x, y)) return;

            world.AddComponent(cursorEntity, new ActionMenuComponent(targetEntity.Id, MenuMode.Buy));
        }

        private static bool HasOwnedNeighbor(World world, int x, int y)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var nId = world.GetTileId(x + dx, y + dy);
                    if (nId != null)
                    {
                        var nEnt = new Entity(nId.Value);
                        var nOwned = world.TryGetComponent<OwnedComponent>(nEnt);
                        if (nOwned.HasValue && nOwned.Value.isOwned) return true;
                    }
                }
            }
            return false;
        }
    }
}