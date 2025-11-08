using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Config;

namespace MonogameProject.Systems
{
    public static class ZoomSystem
    {
        // Stores the keyboard state from the previous frame
        private static KeyboardState _previousState;
        // Cooldown timer to prevent rapid zoom changes
        private static double _zoomCooldown = 0;

        public static void UpdateZoom(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            // Decrease cooldown timer based on elapsed time
            _zoomCooldown -= gameTime.ElapsedGameTime.TotalSeconds;



            // Zoom in
            if (keyboard.IsKeyDown(Keys.OemPlus))
            {
                // Check if key was just pressed this frame
                bool justPressed = !_previousState.IsKeyDown(Keys.OemPlus);
                // Check if cooldown has passed
                bool cooldownPassed = _zoomCooldown <= 0;

                // Apply zoom if either condition is true
                if (justPressed || cooldownPassed)
                {
                    GameSettings.TileSize += 4;
                    if (GameSettings.TileSize > 64)
                        GameSettings.TileSize = 64;

                    // Reset cooldown timer
                    _zoomCooldown = 0.2;
                }
            }

            // Zoom out
            if (keyboard.IsKeyDown(Keys.OemMinus))
            {
                bool justPressed = !_previousState.IsKeyDown(Keys.OemMinus);
                bool cooldownPassed = _zoomCooldown <= 0;

                if (justPressed || cooldownPassed)
                {
                    GameSettings.TileSize -= 4;
                    if (GameSettings.TileSize < 4)
                        GameSettings.TileSize = 4;

                    _zoomCooldown = 0.2;
                }
            }



            // Save current keyboard state for next frame comparison
            _previousState = keyboard;

        }
    }
}
