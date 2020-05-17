using MapDescriptorTest.Terrain;
using MapDescriptorTest.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapDescriptorTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        /// Controls the position of the screen.
        /// </summary>
        public Matrix Camera { get; private set; }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TerrainGenerator terrainGenerator;
        EntityManager entityManager;
        Player player;
        int WindowWidth = 1280;
        int WindowHeight = 720;
        float CamZoom = 0.5f;
        int camX = 0;
        int camY = 0;
        int camXDest = 0;
        int camYDest = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            terrainGenerator = new TerrainGenerator();
            terrainGenerator.Generate();
            entityManager = new EntityManager();
            player = new Player("Player");

            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Terrain.Terrain.LoadContent(Content);
            Player.LoadContent(Content);
            entityManager.Entities.Add(player);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameOptions.FrameCounter++;
            if (GameOptions.FrameCounter == GameOptions.InputDelay)
            {
                GameOptions.FrameCounter = 0;
            }
            InputManager.Update();
            entityManager.Update();
            camXDest = (int)(player.GetEntityProperties().Coordinate.X * GameOptions.TileSize);
            camYDest = (int)(player.GetEntityProperties().Coordinate.Y * GameOptions.TileSize);
            camX += (int)((camXDest - camX) * GameOptions.InertiaFactor);
            camY += (int)((camYDest - camY) * GameOptions.InertiaFactor);
            Camera =
                Matrix.CreateTranslation(new Vector3(-camX, -camY, 0)) *
                Matrix.CreateScale(new Vector3(CamZoom, CamZoom, 1)) *
                Matrix.CreateTranslation(new Vector3(WindowWidth * 0.5f,
                WindowHeight * 0.5f, 0));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera);
            GraphicsDevice.Clear(Color.Black);
            terrainGenerator.Draw(spriteBatch);
            entityManager.Draw(spriteBatch);
            base.Draw(gameTime);
            spriteBatch.End();
        }

        public Vector2 GetCoordsMouse()
        {
            return Vector2.Transform(new Vector2(InputManager.MouseState.Position.X,
                InputManager.MouseState.Position.Y), Matrix.Invert(Camera));
        }
    }
}
