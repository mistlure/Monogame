using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Config;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;

namespace MonogameProject.Systems
{
    public static class GrassDigSystem
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
                    TryOpenDigMenu(world, cursorEntity, pos.Value.X, pos.Value.Y);
                }
            }

            _previousState = keyboard;
        }

        private static void TryOpenDigMenu(World world, Entity cursorEntity, int x, int y)
        {
            var targetTileId = world.GetTileId(x, y);
            if (targetTileId == null) return;

            var targetEntity = new Entity(targetTileId.Value);
            var tile = world.TryGetComponent<TileTypeComponent>(targetEntity);

            if (!tile.HasValue || tile.Value.Type != TileType.Grass) return;

            world.AddComponent(cursorEntity, new ActionMenuComponent(targetEntity.Id, MenuMode.Dig));
        }
    }
}