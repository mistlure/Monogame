using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Config;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;

namespace MonogameProject.Systems
{
    public static class MenuSystem
    {
        private static KeyboardState _previousState;
        private static double _inputCooldown = 0;

        public static void Update(World world, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _inputCooldown -= dt;
            var keyboard = Keyboard.GetState();

            if (GameSettings.CursorEntityId == -1) return;
            var cursorEntity = new Entity(GameSettings.CursorEntityId);

            var menu = world.TryGetComponent<ActionMenuComponent>(cursorEntity);
            if (!menu.HasValue)
            {
                _previousState = keyboard;
                return;
            }

            var options = (menu.Value.Mode == MenuMode.Buy)
                ? GameSettings.TilePurchaseOptions
                : GameSettings.GrassDigOptions;

            var currentMenu = menu.Value;
            bool changed = false;

            if (_inputCooldown <= 0)
            {
                if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
                {
                    currentMenu.CurrentIndex--;
                    changed = true;
                }
                else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
                {
                    currentMenu.CurrentIndex++;
                    changed = true;
                }
            }

            if (changed)
            {
                if (currentMenu.CurrentIndex < 0) currentMenu.CurrentIndex = options.Count - 1;
                if (currentMenu.CurrentIndex >= options.Count) currentMenu.CurrentIndex = 0;

                world.AddComponent(cursorEntity, currentMenu);
                _inputCooldown = 0.15;
            }

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                world.RemoveComponent<ActionMenuComponent>(cursorEntity);
            }

            if ((keyboard.IsKeyDown(Keys.Space) && !_previousState.IsKeyDown(Keys.Space)) ||
                (keyboard.IsKeyDown(Keys.Enter) && !_previousState.IsKeyDown(Keys.Enter)))
            {
                ExecuteAction(world, cursorEntity, currentMenu, options);
            }

            _previousState = keyboard;
        }

        private static void ExecuteAction(World world, Entity cursorEntity, ActionMenuComponent menu, List<ShopItem> options)
        {
            var item = options[menu.CurrentIndex];

            if (item.IsCancelButton)
            {
                world.RemoveComponent<ActionMenuComponent>(cursorEntity);
                return;
            }

            if (GameSettings.PlayerCoins >= item.Price && item.ResultTile.HasValue)
            {
                GameSettings.PlayerCoins -= item.Price;

                var targetEntity = new Entity(menu.TargetEntityId);

                world.AddComponent(targetEntity, new TileTypeComponent(item.ResultTile.Value));
                world.AddComponent(targetEntity, new OwnedComponent(true));

                world.RemoveComponent<ActionMenuComponent>(cursorEntity);
            }
        }
    }
}