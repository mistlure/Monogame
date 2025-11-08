using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameProject.Components;
using MonogameProject.Core;
using MonogameProject.Entities;
using MonogameProject.Enums;
using MonogameProject.Systems;

namespace MonogameProject
{
    public class Game1 : Game
    {
        //
        private World _world;
        private Texture2D _pixelTexture;
        //

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

            _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            foreach (var entityId in _world.GetAllEntityIds())
            {
                var entity = new Entity(entityId);
                
                var position = _world.TryGetComponent<PositionComponent>(entity);
                var tile = _world.TryGetComponent<TileTypeComponent>(entity);

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

                    _spriteBatch.Draw(_pixelTexture, rect, color);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
