using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameProject.Components;
using MonogameProject.Config;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;

namespace MonogameProject.Tests
{
    public static class TestDrawMap
    {
        public static void TestDraw(SpriteBatch spriteBatch, Texture2D pixelTexture, World world)
        {
            //spriteBatch.Begin();

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
                        position.Value.X * GameSettings.TileSize, // X position in pixels
                        position.Value.Y * GameSettings.TileSize, // Y position in pixels
                        GameSettings.TileSize,                    // Width
                        GameSettings.TileSize                     // Height
                    );

                    spriteBatch.Draw(pixelTexture, rect, color);
                }
            }

            //spriteBatch.End();
        }
    }
}
