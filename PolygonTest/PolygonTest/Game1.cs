using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PolyLib;

namespace PolygonTest
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private LineBatch _lineBatch;
        private Polygon2D _polygonA;
        private Polygon2D _polygonB;
        private Texture2D _marker;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _polygonA = Polygon2D.FromTriangle(100, 100, 70);
            _polygonB = Polygon2D.FromRectangle(new Rectangle(200, 200, 100, 50));
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _lineBatch = new LineBatch(GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _marker = Content.Load<Texture2D>(@"_marker");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.D))
                _polygonB.Translate(new Vector2(5, 0));
            if (keyboard.IsKeyDown(Keys.A))
                _polygonB.Translate(new Vector2(-5, 0));
            if (keyboard.IsKeyDown(Keys.S))
                _polygonB.Translate(new Vector2(0, 5));
            if (keyboard.IsKeyDown(Keys.W))
                _polygonB.Translate(new Vector2(0, -5));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (_polygonB.Intersects(_polygonA))
                _spriteBatch.Draw(_marker, Vector2.Zero, Color.Red);
            else
                _spriteBatch.Draw(_marker, Vector2.Zero, Color.White);
            _spriteBatch.End();

            _lineBatch.Begin();
            _lineBatch.DrawPolygon(_polygonA, Color.White);
            _lineBatch.DrawPolygon(_polygonB, Color.Black);
            _lineBatch.End();

            base.Draw(gameTime);
        }
    }
}
