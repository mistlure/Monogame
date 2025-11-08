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
using MonogameProject.Enums;

namespace MonogameProject.Tests
{
    public static class TestMouseClick
    {
        public static void TestMouse(World world)
        {
            // Mouse input handling
            var mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                int tileX = mouse.X / GameSettings.TileSize;
                int tileY = mouse.Y / GameSettings.TileSize;

                // Iterate through all entities to find the one at the mouse position
                foreach (var entityId in world.GetAllEntityIds())
                {
                    var entity = new Entity(entityId);

                    var position = world.TryGetComponent<PositionComponent>(entity);
                    var tile = world.TryGetComponent<TileTypeComponent>(entity);

                    if (position.HasValue && tile.HasValue)
                    {
                        // Check if this entity is at the clicked tile position
                        if (position.Value.X == tileX && position.Value.Y == tileY)
                        {
                            // Change tile type on click (if it's water, change to grass)
                            if (tile.Value.Type == TileType.Water)
                            {
                                world.AddComponent(entity, new TileTypeComponent(TileType.Grass));
                            }
                        }
                    }
                }
            }
        }
    }
}
