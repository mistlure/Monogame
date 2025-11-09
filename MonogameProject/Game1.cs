using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;
using MonogameProject.Systems;
using MonogameProject.Tests;

namespace MonogameProject
{
    public class Game1 : Game
    {
        // TEMPORARY
        private World _world;
        private Texture2D _pixelTexture;
        // TEMPORARY

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _world = new World();
            GenerateWorldSystem.GenerateWorld(_world);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // TEMPORARY
            _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
            // TEMPORARY
        }

        protected override void Update(GameTime gameTime)
        {
            // Exit on Back or Escape
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            ZoomSystem.UpdateZoom(gameTime);

            TilePurchaseSystem.HandlePurchase(_world);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            TestDrawMap.TestDraw(_spriteBatch, _pixelTexture, _world);

            base.Draw(gameTime);
        }
    }
}
