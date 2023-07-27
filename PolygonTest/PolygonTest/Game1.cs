using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PolyLib;

namespace PolygonTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LineBatch lineBatch;
        Polygon2D polygonA = new Polygon2D();
        Polygon2D polygonB = new Polygon2D();
        private Texture2D uiAim;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            polygonA.Add(new Vector2(100, 100));
            polygonA.Add(new Vector2(150, 50));
            polygonA.Add(new Vector2(200, 100));

            polygonB = Polygon2D.ToPolygon(new Rectangle(100, 100, 100, 50));
            //polygonB.Add(new Vector2(100, 50));
            //polygonB.Add(new Vector2(100, 100));
            //polygonB.Add(new Vector2(150, 100));
            //polygonB.Add(new Vector2(150, 50));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            lineBatch = new LineBatch(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uiAim = Content.Load<Texture2D>(@"uiAim");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.D))
                polygonB.Translate(new Vector2(5, 0));
            if (keyboard.IsKeyDown(Keys.A))
                polygonB.Translate(new Vector2(-5, 0));
            if (keyboard.IsKeyDown(Keys.S))
                polygonB.Translate(new Vector2(0, 5));
            if (keyboard.IsKeyDown(Keys.W))
                polygonB.Translate(new Vector2(0, -5));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (polygonB.Intersects(polygonA))
                spriteBatch.Draw(uiAim, Vector2.Zero, Color.Red);
            else
                spriteBatch.Draw(uiAim, Vector2.Zero, Color.White);
            spriteBatch.End();

            lineBatch.Begin();
            lineBatch.DrawPolygon(polygonA, Color.White);
            lineBatch.DrawPolygon(polygonB, Color.Black);
            lineBatch.End();

            base.Draw(gameTime);
        }
    }
}
