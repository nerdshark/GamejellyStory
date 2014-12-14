using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using xTile;
using xTile.Dimensions;
using xTile.Display;

namespace MyGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D squareTexture;
        private Texture2D boopTexture;
        private Texture2D skeletonTexture;
        private Random rng;

        private SpriteFont font;

        private StringBuilder leftStickInfo;

        private GameEntity entity;

        private Map map;
        private IDisplayDevice displayDevice;
        private xTile.Dimensions.Rectangle viewport;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rng = new Random(DateTime.UtcNow.Millisecond);
            leftStickInfo = new StringBuilder();
            mouseInfo = new StringBuilder();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            displayDevice = new XnaDisplayDevice(Content, GraphicsDevice);
            
            var windowBounds = Window.ClientBounds;
            viewport = new xTile.Dimensions.Rectangle(new Size(windowBounds.Width, windowBounds.Height));
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
            squareTexture = Content.Load<Texture2D>(@"Textures\square");
            boopTexture = Content.Load<Texture2D>(@"Textures\boop");
            skeletonTexture = Content.Load<Texture2D>(@"Textures\BODY_skeleton");
            font = Content.Load<SpriteFont>(@"Fonts\font");
            entity = new GameEntity(skeletonTexture);
            map = Content.Load<Map>(@"Maps\test");
            map.LoadTileSheets(displayDevice);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private StringBuilder mouseInfo;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                pressedKeys.Contains(Keys.Escape))
                this.Exit();
            
            entity.Update(gameTime);
            map.Update(gameTime.ElapsedGameTime.Milliseconds);

            var viewportX = entity.Position.X - (viewport.Width/2);
            var viewportY = entity.Position.Y - (viewport.Height / 2);

            if (viewportX < 0) viewportX = 0;
            if (viewportY < 0) viewportY = 0;
            if (viewportX >= 3200 - 800) viewportX = 3200 - 800;
            if (viewportY >= 3200 - 600) viewportY = 3200 - 600;
            viewport.X = (int)viewportX;
            viewport.Y = (int) viewportY;

            //if (viewport.X <= 0) viewportX = 0;
            //if (viewport.Y <= 0) viewportY = 0;
            
            leftStickInfo = new StringBuilder();
            leftStickInfo.Append("Left Stick X: ")
                         .Append(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)
                         .Append("  Y: ")
                         .Append(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //spriteBatch.Draw(entity.Texture, entity.Position, Color.White);
            map.Draw(displayDevice, viewport);

            var drawX = entity.Position.X - 32 - viewport.X;
            if (drawX < 0) drawX = 0;

            var drawY = entity.Position.Y - 32 - viewport.Y;
            if (drawY < 0) drawY = 0;

            var drawPos = new Vector2(drawX, drawY);
            
            entity.CurrentAnimation.CurrentFrame.Draw(spriteBatch, drawPos, Color.White);
            //spriteBatch.Draw(boopTexture, , Color.Orange);
            //spriteBatch.DrawString(font, leftStickInfo, new Vector2(50, 50), Color.Black);     
            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
