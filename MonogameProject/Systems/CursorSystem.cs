using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Config;
using MonogameProject.Core;
using MonogameProject.Entities;

namespace MonogameProject.Systems
{
    public static class CursorSystem
    {
        private static double _moveCooldown = 0;
        private static readonly double MoveDelay = 0.15;

        public static void Update(World world, GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GameSettings.CursorEntityId == -1) return;

            int cursorId = GameSettings.CursorEntityId;
            var cursorEnt = new Entity(cursorId);

            if (world.TryGetComponent<ActionMenuComponent>(cursorEnt) != null)
                return;

            var cursorPos = world.TryGetComponent<PositionComponent>(cursorEnt);
            if (!cursorPos.HasValue) return;

            int cx = cursorPos.Value.X;
            int cy = cursorPos.Value.Y;

            _moveCooldown -= dt;

            if (_moveCooldown <= 0)
            {
                bool moved = false;
                if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
                {
                    cy--;
                    moved = true;
                }
                else if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
                {
                    cy++;
                    moved = true;
                }
                else if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
                {
                    cx--;
                    moved = true;
                }
                else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
                {
                    cx++;
                    moved = true;
                }

                if (moved)
                {
                    cx = Math.Clamp(cx, 0, GameSettings.MapWidth - 1);
                    cy = Math.Clamp(cy, 0, GameSettings.MapHeight - 1);

                    world.AddComponent(cursorEnt, new PositionComponent(cx, cy));

                    _moveCooldown = MoveDelay;
                }
                else
                {
                    _moveCooldown = 0;
                }
            }
        }
    }
}