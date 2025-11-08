using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using MonogameProject.Components;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;
using Microsoft.Xna.Framework;

namespace MonogameProject.Tests
{
    public static class TestDrawMap
    {
        public static void TestDraw(SpriteBatch spriteBatch, Texture2D pixelTexture, World world)
        {
            spriteBatch.Begin();

            foreach (var entityId in world.GetAllEntityIds())
            {
                var entity = new Entity(entityId);

                var position = world.TryGetComponent<PositionComponent>(entity);
                var tile = world.TryGetComponent<TileTypeComponent>(entity);

                if (position.HasValue && tile.HasValue)
                {
                    // Choose color based on tile type
                    Color color = tile.Value.Type switch
                    {
                        TileType.Water => Color.Blue,
                        TileType.Grass => Color.Green,
                        TileType.Farm => Color.Brown,
                        TileType.Sand => Color.Yellow,
                        // Otherwise
                        _ => Color.Purple
                    };

                    // Define rectangle size and position
                    Rectangle rect = new Rectangle(
                        position.Value.X * 16, // X position in pixels
                        position.Value.Y * 16, // Y position in pixels
                        16,                    // Width
                        16                     // Height
                    );

                    spriteBatch.Draw(pixelTexture, rect, color);
                }
            }

            spriteBatch.End();
        }
    }
}
