using System;
using System.Collections.Generic;
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
        public static void TestDraw(SpriteBatch spriteBatch, Texture2D pixelTexture, World world, SpriteFont font)
        {
            foreach (var entityId in world.GetAllEntityIds())
            {
                var entity = new Entity(entityId);
                if (world.TryGetComponent<CursorComponent>(entity) != null) continue;

                var position = world.TryGetComponent<PositionComponent>(entity);
                var tile = world.TryGetComponent<TileTypeComponent>(entity);

                if (position.HasValue && tile.HasValue)
                {
                    Color color = tile.Value.Type switch
                    {
                        TileType.Water => Color.Blue,
                        TileType.Grass => Color.Green,
                        TileType.Farm => Color.Brown,
                        TileType.Sand => Color.Yellow,
                        _ => Color.Purple
                    };
                    Rectangle rect = new Rectangle(
                        position.Value.X * GameSettings.TileSize,
                        position.Value.Y * GameSettings.TileSize,
                        GameSettings.TileSize, GameSettings.TileSize);
                    spriteBatch.Draw(pixelTexture, rect, color);

                    var plant = world.TryGetComponent<PlantComponent>(entity);
                    if (plant.HasValue)
                    {
                        int pSize = 6;
                        Rectangle pRect = new Rectangle(rect.X + (16 - pSize) / 2, rect.Y + (16 - pSize) / 2, pSize, pSize);
                        spriteBatch.Draw(pixelTexture, pRect, Color.Magenta);
                    }
                }
            }

            foreach (var entityId in world.GetAllEntityIds())
            {
                var entity = new Entity(entityId);
                if (world.TryGetComponent<CursorComponent>(entity) != null)
                {
                    var pos = world.TryGetComponent<PositionComponent>(entity);
                    if (pos.HasValue)
                    {
                        Rectangle rect = new Rectangle(
                            pos.Value.X * GameSettings.TileSize,
                            pos.Value.Y * GameSettings.TileSize,
                            GameSettings.TileSize, GameSettings.TileSize);

                        spriteBatch.Draw(pixelTexture, rect, Color.White * 0.5f);

                        var menu = world.TryGetComponent<ActionMenuComponent>(entity);
                        if (menu.HasValue)
                        {
                            DrawMenu(spriteBatch, pixelTexture, font, rect, menu.Value);
                        }
                    }
                }
            }
        }

        private static void DrawMenu(SpriteBatch spriteBatch, Texture2D tex, SpriteFont font, Rectangle cursorRect, ActionMenuComponent menu)
        {
            var items = (menu.Mode == MenuMode.Buy) ? GameSettings.TilePurchaseOptions : GameSettings.GrassDigOptions;

            int itemSize = 40;
            int padding = 5;
            int count = items.Count;
            int totalWidth = (itemSize + padding) * count + padding;
            int totalHeight = itemSize + padding * 2;

            Vector2 menuPos = new Vector2(
                cursorRect.X - totalWidth / 2 + cursorRect.Width / 2,
                cursorRect.Y - totalHeight - 10
            );

            spriteBatch.Draw(tex, new Rectangle((int)menuPos.X, (int)menuPos.Y, totalWidth, totalHeight), Color.Black * 0.8f);

            for (int i = 0; i < count; i++)
            {
                var item = items[i];
                bool isSelected = (i == menu.CurrentIndex);

                int x = (int)menuPos.X + padding + i * (itemSize + padding);
                int y = (int)menuPos.Y + padding;

                Rectangle itemRect = new Rectangle(x, y, itemSize, itemSize);

                Color btnColor = isSelected ? Color.White : Color.Gray;
                if (item.IsCancelButton) btnColor = isSelected ? Color.Red : Color.DarkRed;

                spriteBatch.Draw(tex, itemRect, btnColor);

                if (isSelected)
                {
                    string priceText = item.IsCancelButton ? "No" : $"{item.Price}$";
                    spriteBatch.DrawString(font, priceText, new Vector2(x, y + itemSize), Color.White);
                    spriteBatch.DrawString(font, item.Name, new Vector2(x, y), Color.Black);
                }
            }
        }
    }
}