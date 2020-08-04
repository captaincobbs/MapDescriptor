using MapDescriptorTest.Statics;
using MapDescriptorTest.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MapDescriptorTest.Sprite;
using MapDescriptorTest.World;
using System;

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
        
        /// <summary>
        /// Change in scroll wheel since last update
        /// </summary>
        public static int CurrentScrollWheelValue { get; private set; }
       
        /// <summary>
        /// Last recorded scroll wheel update
        /// </summary>
        public static int DeltaScrollWheelValue { get; private set; }
        
        // Main variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        WorldViewer worldViewer;
        readonly Player player;
        // Window properties
        int windowWidth = 1280;
        int windowHeight = 720;
        // Camera properties
        float camZoom = 0.5f;
        float camZoomDest = 0.5f;
        float maxCamZoom = 4f;
        float minCamZoom = 0.2f;
        int camX = 0;
        int camXDest = 0;
        int camY = 0;
        int camYDest = 0;

        /// <summary>
        /// Main Instance of the game that instantiates and creates all primary properties, generators, and managers.
        /// </summary>
        public Game1()
        {
            // Create main game variables
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player("Player");
            World.World world = WorldGenerator.Generate("world", player);
            worldViewer = new WorldViewer(world);

            // Set window dimensions
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Creates a new SpriteBatch, which is used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureIndex.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unloads content
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Camera scrolling each frame - subtracting (GameOptions.TileSize/2) centers the screen
            camXDest = (int)((player.ContainingTile.XPosition * GameOptions.TileSize) - (GameOptions.TileSize/2));
            camYDest = (int)((player.ContainingTile.YPosition * GameOptions.TileSize) - (GameOptions.TileSize/2));
            camX += (int)((camXDest - camX) * GameOptions.InertiaFactor);
            camY += (int)((camYDest - camY) * GameOptions.InertiaFactor);

            // Scrolling
            camZoom += ((camZoomDest - camZoom) * GameOptions.InertiaFactor);
            DeltaScrollWheelValue = InputManager.MouseState.ScrollWheelValue - CurrentScrollWheelValue;
            CurrentScrollWheelValue += DeltaScrollWheelValue;
            if (DeltaScrollWheelValue != CurrentScrollWheelValue)
            {
                camZoomDest += DeltaScrollWheelValue * (GameOptions.ScrollSensitivity / 1000f);
            }

            // Reset camera zoom
            if (InputManager.MouseState.MiddleButton == ButtonState.Pressed)
            {
                camZoomDest = 0.5f;
            }

            // Keep camera zoom within a specific range
            camZoom = MathUtilities.ContainInRange(camZoom, minCamZoom, maxCamZoom);

            // Change camera matrix properties with updated information
            Camera =
                Matrix.CreateTranslation(new Vector3(-camX, -camY, 0)) *
                Matrix.CreateScale(new Vector3(camZoom, camZoom, 1)) *
                Matrix.CreateTranslation(new Vector3(windowWidth * 0.5f,
                windowHeight * 0.5f, 0));

            // Main updates
            InputManager.Update(IsActive);
            worldViewer.UpdateChunkRange(player.ContainingTile.XPosition - 100, player.ContainingTile.YPosition - 100, player.ContainingTile.XPosition + 100, player.ContainingTile.YPosition + 100);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Render window
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera);
            GraphicsDevice.Clear(GameOptions.BackgroundColor);
            worldViewer.DrawChunkRange(spriteBatch, player.ContainingTile.XPosition - 100, player.ContainingTile.YPosition - 100, player.ContainingTile.XPosition + 100, player.ContainingTile.YPosition + 100);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
