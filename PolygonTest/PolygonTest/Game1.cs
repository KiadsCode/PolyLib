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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LineBatch lineBatch;
        Polygon2D polygonA;
        Polygon2D polygonB;
        private Texture2D uiAim;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            polygonA = Polygon2D.FromTriangle(300, 300, 100);
            polygonB = Polygon2D.FromRectangle(new Rectangle(100, 100, 100, 50));
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            lineBatch = new LineBatch(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uiAim = Content.Load<Texture2D>(@"uiAim");
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

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
