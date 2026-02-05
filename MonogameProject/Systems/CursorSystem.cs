using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            int? cursorId = null;
            PositionComponent? cursorPos = null;

            foreach (var id in world.GetAllEntityIds())
            {
                var ent = new Entity(id);
                if (world.TryGetComponent<CursorComponent>(ent) != null)
                {
                    if (world.TryGetComponent<PurchaseMenuComponent>(ent) != null)
                        return;

                    cursorId = id;
                    cursorPos = world.TryGetComponent<PositionComponent>(ent);
                    break;
                }
            }

            if (cursorId == null || cursorPos == null) return;

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

                    var cursorEnt = new Entity(cursorId.Value);
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
