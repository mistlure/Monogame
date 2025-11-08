using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonogameProject.Components;
using MonogameProject.Enums;
using MonogameProject.Entities;
using MonogameProject.Config;
using MonogameProject.Core;

namespace MonogameProject.Systems
{
    public static class GenerateWorldSystem
    {
        public static void GenerateWorld(World world)
        {
            int width = GameSettings.MapWidth;
            int height = GameSettings.MapHeight;

            // Create random generator
            var random = new Random();

            // Pick random positions for grass placement
            int grassX = random.Next(width);
            int grassY = random.Next(height);

            // Start entity ID counter (for every single tile)
            int entityId = 1;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    var tileEntity = world.CreateEntity(entityId++);

                    world.AddComponent(tileEntity, new PositionComponent(x, y));

                    // Determine tile type based on random grass position
                    // All tiles are water except the one at (grassX, grassY) that was randomly generated
                    var tileType = (x == grassX && y == grassY) ? TileType.Grass : TileType.Water;

                    world.AddComponent(tileEntity, new TileTypeComponent(tileType));
                }
            }
        }
    }
}
