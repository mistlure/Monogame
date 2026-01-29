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
        private static double _moveCooldown = 0;
        private static readonly double MoveDelay = 0.15;

        // For detecting single key presses
        private static KeyboardState _previousState;

        public static void HandlePurchase(World world, GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Finding the cursor entity and its position
            int? cursorId = null;
            PositionComponent? cursorPos = null;

            foreach (var id in world.GetAllEntityIds())
            {
                var entity = new Entity(id);
                if (world.TryGetComponent<CursorComponent>(entity) != null)
                {
                    cursorId = id;
                    cursorPos = world.TryGetComponent<PositionComponent>(entity);
                    break;
                }
            }

            if (cursorId == null || cursorPos == null) return;

            // Unpack current cursor position
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
                    // Map bounds checking
                    cx = Math.Clamp(cx, 0, GameSettings.MapWidth - 1);
                    cy = Math.Clamp(cy, 0, GameSettings.MapHeight - 1);

                    // Update cursor position
                    var cursorEnt = new Entity(cursorId.Value);
                    world.AddComponent(cursorEnt, new PositionComponent(cx, cy));
                    
                    _moveCooldown = MoveDelay;
                }
                else
                {
                    // If no movement keys are pressed, reset cooldown for immediate response
                    _moveCooldown = 0;
                }
            }

            if (keyboard.IsKeyDown(Keys.Space) && !_previousState.IsKeyDown(Keys.Space))
            {
                AttemptPurchase(world, cx, cy);
            }

            _previousState = keyboard;
        }

        private static void AttemptPurchase(World world, int targetX, int targetY)
        {
            // Get tile ID directly by coordinates
            var targetTileId = world.GetTileId(targetX, targetY);
            
            if (targetTileId == null) return;

            var entity = new Entity(targetTileId.Value);
            
            // Получаем компоненты этого тайла
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
                    // Skip the center tile (self)
                    if (dx == 0 && dy == 0) continue;

                    int nx = targetX + dx;
                    int ny = targetY + dy;

                    // Direct lookup for neighbor ID
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